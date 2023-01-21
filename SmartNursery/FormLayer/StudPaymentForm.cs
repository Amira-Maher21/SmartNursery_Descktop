using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SmartNursery.DatabaseLayer;

namespace SmartNursery.FormLayer
{
    public partial class StudPaymentForm : DevExpress.XtraEditors.XtraForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        DataTable StudentDataTable = new DataTable();
        public StudPaymentForm()
        {
            InitializeComponent();
            DataTableCreate();
            Fill(0);
        }
        void DataTableCreate()
        {
            StudentDataTable = new DataTable();
            StudentDataTable.Columns.Add("StudentPaymentId");
            StudentDataTable.Columns.Add("StudentBarCode");
            StudentDataTable.Columns.Add("StudentId");
            StudentDataTable.Columns.Add("ClassId");
            StudentDataTable.Columns.Add("TotalPaidAfterDeduction");
            StudentDataTable.Columns.Add("IsPaid", typeof(Boolean));

        }
        void Fill(int patch)
        {
            var data = from s in db.Students
                       join p in db.StudentPayments on s.StudentId equals p.StudentId
                       where p.SalaryPatchId == patch
                       select new
                       {
                           p.StudentPaymentId,
                           s.StudentId,
                           s.StudentName,
                           s.StudentBarCode,
                           s.ClassId,
                           s.TotalPaidAfterDeduction,
                           p.IsPaid
                       };
            if (!data.Any())
            {
                var studentt = db.Students.ToList();
                foreach (var item in studentt)
                {
                    StudentDataTable.Rows.Add(0, item.StudentBarCode, item.StudentId, item.ClassId, item.TotalPaidAfterDeduction, false);
                }
            }
            else
            {
                foreach (var itemm in data)
                {
                    StudentDataTable.Rows.Add(itemm.StudentPaymentId ,itemm.StudentBarCode, itemm.StudentId, itemm.ClassId, itemm.TotalPaidAfterDeduction, itemm.IsPaid);
                }
               
            }
            gridControl1.DataSource = StudentDataTable;
        }
        private void simpleButtSave_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            else
            {
                SalaryPatch pat = new SalaryPatch();
                pat.DateFrom = dateEditFrom.DateTime;
                pat.DateTo = dateEditTo.DateTime;
                pat.DocTypeId = 8;
                pat.MonthId = Convert.ToInt32(searchLookUpMonth.EditValue);
                pat.PatchDesc = txtPatch.Text;
                pat.SalaryPatchIsClosed = false;
                db.SalaryPatches.InsertOnSubmit(pat);
                db.SubmitChanges();
                Fill(pat.SalaryPatchId);

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    StudentPayment pay = new StudentPayment();
                    pay.DocTypeId = 8;
                    pay.IsPaid = false;
                    pay.StudentId = Convert.ToInt32(gridView1.GetRowCellValue(i, "StudentId"));
                    pay.SalaryPatchId = pat.SalaryPatchId;
                    pay.UserName = MainForm.user;
                    db.StudentPayments.InsertOnSubmit(pay);
                    db.SubmitChanges();
                    
                    gridView1.SetRowCellValue(i, "StudentPaymentId", pay.StudentPaymentId);
                }
                MessageBox.Show("تم الحفظ");
                searchLookUpPatch.Properties.DataSource = db.SalaryPatches.ToList();
            }
        }
        
        private void StudPaymentForm_Load(object sender, EventArgs e)
        {
            searchLookUpMonth.Properties.DataSource = db.Months.ToList();
            searchLookUpPatch.Properties.DataSource = db.SalaryPatches.Where(a => a.DocTypeId == 8 && a.SalaryPatchIsClosed == false).ToList();
            searchLookUpSafe.Properties.DataSource = db.Safes.Where(d => d.SafeFlagId == 1).ToList();
            dateEditFrom.DateTime = DateTime.Now;
            dateEditTo.DateTime = DateTime.Now;
            repositoryItemSearchLookUpClass.DataSource = db.ClassTablees.ToList();
            repositoryItemSearchLookUpStudent.DataSource = db.Students.ToList();
        }

        private void repositoryItemHyperLinkPay_Click(object sender, EventArgs e)
        {
            if (searchLookUpPatch.EditValue == null)
            {
                MessageBox.Show("من فضلك اختر الباتش أولا");
            }
            else if (searchLookUpSafe.EditValue == null)
            {
                MessageBox.Show("من فضلك اختر الخزنة أولا");
            }
            else
            {
                int patchIDD = Convert.ToInt32(searchLookUpPatch.EditValue);
                int stuPayId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentPaymentId"));
                string barCode = (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentBarCode")).ToString();
                int studentID = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentId"));
                int classID = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ClassId"));
                decimal PayedValue = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TotalPaidAfterDeduction"));
                bool isPayed = Convert.ToBoolean(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "IsPaid"));
                if (isPayed == false)
                {
                    StudentPayment edit = db.StudentPayments.Where(a => a.StudentPaymentId == stuPayId).FirstOrDefault();
                    edit.StuPaymentDate = DateTime.Now;
                    edit.StuPaymentValue = PayedValue;
                    edit.IsPaid = true;
                    db.SubmitChanges();
                    MessageBox.Show("تم الدفع");

                    Treasury tr = new Treasury();
                    {
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpSafe.EditValue)));
                        tr.TreasuryDate = DateTime.Now;
                        tr.TreasuryDebit = PayedValue;
                        tr.TreasuryCredit = 0;
                        tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpSafe.EditValue))) + PayedValue;
                        tr.DocTypeId = 8;
                        // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        tr.Username = MainForm.user;
                        // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        //tr.TreasuryNote = txtNote.Text;
                    }
                    db.Treasuries.InsertOnSubmit(tr);
                    db.SubmitChanges();
                }
                else
                {
                    MessageBox.Show("تم الدفع من قبل");
                }
            }
        }

        private void searchLookUpPatch_EditValueChanged(object sender, EventArgs e)
        {
            int patch = Convert.ToInt32(searchLookUpPatch.EditValue);
            Fill(patch);
        }

        private void checkEditClosePatch_CheckedChanged(object sender, EventArgs e)
        {
            if (searchLookUpPatch.EditValue == null)
            {
                MessageBox.Show("اختر الباتش أولا");
            }
            if (checkEditClosePatch.Checked == true)
            {
                DialogResult result = MessageBox.Show("تأكيد إغلاق الباتش؟", "تنبيه", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int patchIDD = Convert.ToInt32(searchLookUpPatch.EditValue);
                    SalaryPatch edit = db.SalaryPatches.Where(a => a.SalaryPatchId == patchIDD).FirstOrDefault();
                    edit.SalaryPatchIsClosed = true;
                    db.SubmitChanges();

                    searchLookUpPatch.EditValue = null;
                    searchLookUpPatch.Properties.DataSource = db.SalaryPatches.Where(a => a.DocTypeId == 8 && a.SalaryPatchIsClosed == false).ToList();
                    
                }
            }
        }

        private void simpleButNew_Click(object sender, EventArgs e)
        {
            searchLookUpMonth.EditValue = null;
            searchLookUpPatch.EditValue = null;
            searchLookUpSafe.EditValue = null;
            txtPatch.Text = null;
            dateEditFrom.DateTime = DateTime.Now;
            dateEditTo.DateTime = DateTime.Now;
        }
    }
}