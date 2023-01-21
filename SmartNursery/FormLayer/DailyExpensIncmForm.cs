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
    public partial class DailyExpensIncmForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        public DailyExpensIncmForm()
        {
            InitializeComponent();
        }

        private void DailyExpensIncmForm_Load(object sender, EventArgs e)
        {
            searchLookUpEdit1.Properties.DataSource = db.ExpensesAndIncomeItems.ToList();
            searchLookUpEdit2.Properties.DataSource = db.DiscountAndAdditions.ToList();
            searchLookUpEdit3.Properties.DataSource = db.Safes.ToList();
            dateEdit1.DateTime = DateTime.Now;
            searchLookUpEdit4.Properties.DataSource = db.Employees.ToList();
            searchLookUpEdit4.EditValue = MainForm.EmpId;
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(searchLookUpEdit1.EditValue);
            var data = db.ExpensesAndIncomeItems.Where(d => d.ExpensesAndIncomeItemsId == id).FirstOrDefault();
            searchLookUpEdit2.EditValue = data.DiscountAndAdditionId;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            try
            {
                ExpensesAndIncomeData data = new ExpensesAndIncomeData();
                data.Date = Convert.ToDateTime(dateEdit1.EditValue);
                data.DiscountAndAdditionId = Convert.ToInt32(searchLookUpEdit2.EditValue);
                data.EmployeeId = Convert.ToInt32(searchLookUpEdit4.EditValue);
                data.ExpensesAndIncomeDataValue = Convert.ToDecimal(calcEdit1.EditValue);
                data.ExpensesAndIncomeItemsId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                data.Note = textEdit1.Text;
                data.SafeId = Convert.ToInt32(searchLookUpEdit3.EditValue);
                db.ExpensesAndIncomeDatas.InsertOnSubmit(data);
                db.SubmitChanges();
                MessageBox.Show("تم الحفظ");

                if (Convert.ToInt32(searchLookUpEdit2.EditValue) == 1)
                {
                    Treasury tr = new Treasury();
                    {
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit3.EditValue)));
                        tr.TreasuryDate = dateEdit1.DateTime;
                        tr.TreasuryDebit = 0;
                        tr.TreasuryCredit = Convert.ToDecimal(calcEdit1.EditValue);
                        tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit3.EditValue))) - Convert.ToDecimal(calcEdit1.EditValue);
                        tr.DocTypeId = 1;
                        // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        tr.Username = MainForm.user;
                        // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        //tr.TreasuryNote = txtNote.Text;
                    }
                    db.Treasuries.InsertOnSubmit(tr);
                    db.SubmitChanges();
                }
                else if(Convert.ToInt32(searchLookUpEdit2.EditValue) == 2)
                {

                    Treasury tr = new Treasury();
                    {
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit3.EditValue)));
                        tr.TreasuryDate = dateEdit1.DateTime;
                        tr.TreasuryDebit = Convert.ToDecimal(calcEdit1.EditValue);
                        tr.TreasuryCredit = 0;
                        tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit3.EditValue))) + Convert.ToDecimal(calcEdit1.EditValue);
                        tr.DocTypeId = 2;
                        // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        tr.Username = MainForm.user;
                        // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        //tr.TreasuryNote = txtNote.Text;
                    }
                    db.Treasuries.InsertOnSubmit(tr);
                    db.SubmitChanges();
                }
            }
            catch
            {
                MessageBox.Show("لم يتم الحفظ");
            }
        }
        private void searchLookUpEdit4_EditValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}