using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using SmartNursery.DatabaseLayer;

namespace SmartNursery.FormLayer
{
    public partial class EmpSalaryForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        DataTable EmpDataTable = new DataTable();
        public EmpSalaryForm()
        {
            InitializeComponent();
            CreateDataTable();
            Fill(0);
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }
        void CreateDataTable()
        {
            EmpDataTable = new DataTable();
            EmpDataTable.Columns.Add("EmployeeSalaryDetailId");
            EmpDataTable.Columns.Add("EmployeeId");
            EmpDataTable.Columns.Add("EmployeeMainSalary");
            EmpDataTable.Columns.Add("EmployeeAddingValue");
            EmpDataTable.Columns.Add("EmployeeDeductValue");
            EmpDataTable.Columns.Add("EmployeeFinalTotalSalary");
            EmpDataTable.Columns.Add("EmployeeSalaryIsPaid");
        }
        void Fill(int patch)
        {
            gridControl1.DataSource = null;

            var data = from s in db.Employees
                       join p in db.EmployeeSalaryDetails on s.EmplyeeId equals p.EmployeeId
                       join ms in db.EmpSalaryMasters on p.EmployeeSalaryMasterId equals ms.EmployeeSalaryMasterId
                      
                       where ms.SalaryPatchId == patch
                       select new
                       {
                           p.EmployeeSalaryDetailId,
                           p.EmployeeId,
                           s.EmplyeeName,
                           p.EmployeeMainSalary,
                           p.EmployeeAddingValue,
                           p.EmployeeDeductValue,
                           p.EmployeeFinalTotalSalary,
                           p.EmployeeSalaryIsPaid
                       };
            if (!data.Any())
            {
                var empl = db.Employees.ToList();
                foreach (var item in empl)
                {
                    EmpDataTable.Rows.Add(0, item.EmplyeeId, item.EmplyeelSalary, 0, 0, item.EmplyeelSalary, false);
                }
            }
            else
            {
                foreach (var itemm in data)
                {
                    EmpDataTable.Rows.Add(itemm.EmployeeSalaryDetailId, itemm.EmployeeId, itemm.EmployeeMainSalary, itemm.EmployeeAddingValue, itemm.EmployeeDeductValue, itemm.EmployeeFinalTotalSalary, itemm.EmployeeSalaryIsPaid);
                }

            }
            gridControl1.DataSource = EmpDataTable;
            repositoryItemSearchLookUpEmp.DataSource = db.Employees.Where(a => a.EmpoyeeIsActive == true).ToList();
        }
        decimal TotSalary()
        {
            decimal tot = 0;
            var data = db.Employees.Where(a=> a.EmpoyeeIsActive == true).ToList();
            foreach (var item in data)
            {
                tot += Convert.ToDecimal(item.EmplyeelSalary);
            }
            return tot;
        }
        private void simpleButtonSavePatch_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            else
            {
                SalaryPatch pat = new SalaryPatch();
                pat.DateFrom = Convert.ToDateTime(dateEditFrom.EditValue);
                pat.DateTo = Convert.ToDateTime(dateEditTo.EditValue);
                pat.DocTypeId = 9;
                pat.MonthId = Convert.ToInt32(searchLookUpMonth.EditValue);
                pat.PatchDesc = txtPatch.Text;
                db.SalaryPatches.InsertOnSubmit(pat);
                db.SubmitChanges();
                MessageBox.Show("تم حفظ الباتش");

                searchLookUpPatch.Properties.DataSource = db.SalaryPatches.ToList();
                searchLookUpPatch.EditValue = pat.SalaryPatchId;
                Fill(pat.SalaryPatchId);

                EmpSalaryMaster sal = new EmpSalaryMaster();
                sal.SalaryPatchId = pat.SalaryPatchId;
                sal.DateFrom = Convert.ToDateTime(dateEditFrom.EditValue);
                sal.DateTo = Convert.ToDateTime(dateEditTo.EditValue);
                sal.EmployeeLoginAccountId = MainForm.EmpId;
                sal.EmployeeSalaryMasterIsClosed = false;
                sal.EmpUser = MainForm.user;
                sal.SafeId = Convert.ToInt32(searchLookUpTreasury.EditValue);
                sal.TotalSalary = TotSalary();
                db.EmpSalaryMasters.InsertOnSubmit(sal);
                db.SubmitChanges();

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    EmployeeSalaryDetail det = new EmployeeSalaryDetail();
                    det.EmployeeSalaryMasterId = sal.EmployeeSalaryMasterId;
                    det.EmployeeId = Convert.ToInt32(gridView1.GetRowCellValue(i, "EmployeeId"));
                    det.EmployeeMainSalary = Convert.ToDecimal(gridView1.GetRowCellValue(i, "EmployeeMainSalary"));
                    det.EmpAddAndDeductId = 0;
                    det.EmployeeAddingValue = Convert.ToDecimal(gridView1.GetRowCellValue(i, "EmployeeAddingValue"));
                    det.EmployeeDeductValue = Convert.ToDecimal(gridView1.GetRowCellValue(i, "EmployeeDeductValue"));
                    det.EmployeeFinalTotalSalary = Convert.ToDecimal(gridView1.GetRowCellValue(i, "EmployeeFinalTotalSalary"));
                    det.EmployeeSalaryIsPaid = false;
                    db.EmployeeSalaryDetails.InsertOnSubmit(det);
                    db.SubmitChanges();
                    gridView1.SetRowCellValue(i, "EmployeeSalaryDetailId", det.EmployeeSalaryDetailId);
                }
                MessageBox.Show("تم الحفظ");
                
            }
        }

        private void EmpSalaryForm_Load(object sender, EventArgs e)
        {
            searchLookUpPatch.Properties.DataSource = db.SalaryPatches.ToList();
            searchLookUpMonth.Properties.DataSource = db.Months.ToList();
            searchLookUpTreasury.Properties.DataSource = db.Safes.ToList();
            dateEditFrom.DateTime = DateTime.Now;
            dateEditTo.DateTime = DateTime.Now;
        }

        private void dateEditTo_EditValueChanged(object sender, EventArgs e)
        {

        }
        public decimal EmpDeduct(int patch, int empID)
        {
            decimal val = 0;
            var data = db.EmpAddAndDeducts.Where(a => a.SalaryPatchId == patch && a.EmplyeeId == empID && a.DiscountAndAdditionId == 3).ToList();
            foreach (var item in data)
            {
                val += Convert.ToDecimal(item.EmpAddAndDeductValue);
            }
            return val;
        }
        public decimal EmpAdd(int patch, int empID)
        {
            decimal val = 0;
            var data = db.EmpAddAndDeducts.Where(a => a.SalaryPatchId == patch && a.EmplyeeId == empID && a.DiscountAndAdditionId == 4).ToList();
            foreach (var item in data)
            {
                val += Convert.ToDecimal(item.EmpAddAndDeductValue);
            }
            return val;
        }
        private void searchLookUpPatch_EditValueChanged(object sender, EventArgs e)
        {
            int patch = Convert.ToInt32(searchLookUpPatch.EditValue);
            Fill(patch);
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                int empId = Convert.ToInt32(gridView1.GetRowCellValue(i, "EmployeeId"));
                decimal salary = Convert.ToDecimal(gridView1.GetRowCellValue(i, "EmployeeMainSalary"));
                decimal deductVal = EmpDeduct(patch, empId);
                decimal addVal = EmpAdd(patch, empId);
                gridView1.SetRowCellValue(i, "EmployeeDeductValue", deductVal);
                gridView1.SetRowCellValue(i, "EmployeeAddingValue", addVal);
                decimal finalSalary = salary + addVal - deductVal;
                gridView1.SetRowCellValue(i, "EmployeeFinalTotalSalary", finalSalary);

            }
        }

        private void repositoryItemHyperLinkIsPaid_Click(object sender, EventArgs e)
        {
            bool isPaid = Convert.ToBoolean(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeSalaryIsPaid"));
            if (isPaid)
            {
                MessageBox.Show("تم دفع المرتب للموظف من قبل");
            }
            else
            {

                if (searchLookUpTreasury.EditValue == null)
                {
                    MessageBox.Show("من فضلك اختر الخزنة أولا");
                }
                else
                {
                    int safeId = Convert.ToInt32(searchLookUpTreasury.EditValue);
                    int id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeSalaryDetailId"));
                    if (id == 0)
                    {
                        MessageBox.Show("من فضلك تأكد من حفظ الماستر أولا");
                    }
                    else
                    {
                        var edit = db.EmployeeSalaryDetails.Where(a => a.EmployeeSalaryDetailId == id).FirstOrDefault();
                        edit.EmployeeAddingValue = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeAddingValue"));
                        edit.EmployeeDeductValue = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeDeductValue"));
                        edit.EmployeeFinalTotalSalary = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeFinalTotalSalary"));
                        edit.EmployeeSalaryIsPaid = true;
                        db.SubmitChanges();
                        MessageBox.Show("تم الدفع");
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EmployeeSalaryIsPaid", true);

                        decimal finalSal = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeFinalTotalSalary"));
                        Treasury tr = new Treasury();
                        {
                            tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpTreasury.EditValue)));
                            tr.TreasuryDate = DateTime.Now;
                            tr.TreasuryDebit = 0;
                            tr.TreasuryCredit = finalSal;
                            tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpTreasury.EditValue))) - finalSal;
                            tr.DocTypeId = 9;
                            tr.EmplyeeId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeId"));
                            tr.Username = MainForm.user;
                        }
                        db.Treasuries.InsertOnSubmit(tr);
                        db.SubmitChanges();
                    }
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            txtPatch.Text = null;
            searchLookUpMonth.EditValue = null;
            searchLookUpPatch.EditValue = null;
            searchLookUpTreasury.EditValue = null;
        }
    }
}