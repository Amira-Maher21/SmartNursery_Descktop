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
    public partial class SafeTransferredForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        public SafeTransferredForm()
        {
            InitializeComponent();
        }

        private void SafeTransferredForm_Load(object sender, EventArgs e)
        {
            searchLookUpEmp.Properties.DataSource = db.Employees.ToList();
            searchLookUpEmp.EditValue = MainForm.EmpId;
            searchLookUpSafeFrom.Properties.DataSource = db.Safes.Where(a=> a.SafeFlagId == 1).ToList();
            searchLookUpSafeTo.Properties.DataSource = db.Safes.Where(a => a.SafeFlagId == 1).ToList();
            dateEditTransfer.DateTime = DateTime.Now;
        }
        decimal safeFromBalance = 0;
        decimal safeToBalance = 0;
        private void searchLookUpSafeFrom_EditValueChanged(object sender, EventArgs e)
        {
            int safeIDFrom = Convert.ToInt32(searchLookUpSafeFrom.EditValue);
            safeFromBalance = Convert.ToDecimal(pay.BalanceTreasury(safeIDFrom));
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            else
            {
                decimal val = Convert.ToDecimal(calcEditValTransfer.EditValue);
                if (val > safeFromBalance)
                {
                    MessageBox.Show("القيمة المحولة أكبر من رصيد الخزنة");
                }
                else
                {

                    SafeTransfferred s = new SafeTransfferred();
                    s.SafeIdFrom = Convert.ToInt32(searchLookUpSafeFrom.EditValue);
                    s.SafeIdTo = Convert.ToInt32(searchLookUpSafeTo.EditValue);
                    s.TransfferredDate = dateEditTransfer.DateTime;
                    s.TransfferredValue = Convert.ToDecimal(calcEditValTransfer.EditValue);
                    s.DocNum = Convert.ToInt32(textEdit1txtDocNo.Text);
                    db.SafeTransfferreds.InsertOnSubmit(s);
                    db.SubmitChanges();
                    MessageBox.Show("تم التحويل");

                    Treasury tr = new Treasury();
                    {
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpSafeFrom.EditValue)));
                        tr.TreasuryDate = dateEditTransfer.DateTime;
                        tr.TreasuryDebit = 0;
                        tr.TreasuryCredit = Convert.ToDecimal(calcEditValTransfer.EditValue);
                        tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpSafeFrom.EditValue))) - Convert.ToDecimal(calcEditValTransfer.EditValue);
                        tr.DocTypeId = 6;
                        // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        tr.Username = MainForm.user;
                        // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        //tr.TreasuryNote = txtNote.Text;
                    }
                    db.Treasuries.InsertOnSubmit(tr);
                    db.SubmitChanges();

                    Treasury tre = new Treasury();
                    {
                        tre.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpSafeTo.EditValue)));
                        tre.TreasuryDate = dateEditTransfer.DateTime;
                        tre.TreasuryDebit = Convert.ToDecimal(calcEditValTransfer.EditValue);
                        tre.TreasuryCredit = 0;
                        tre.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpSafeTo.EditValue))) + Convert.ToDecimal(calcEditValTransfer.EditValue);
                        tre.DocTypeId = 7;
                        // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        tre.Username = MainForm.user;
                        // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                        //tr.TreasuryNote = txtNote.Text;
                    }
                    db.Treasuries.InsertOnSubmit(tr);
                    db.SubmitChanges();
                }
            }
        }

        private void searchLookUpSafeTo_EditValueChanged(object sender, EventArgs e)
        {
            int safeIDTo = Convert.ToInt32(searchLookUpSafeTo.EditValue);
            safeToBalance = Convert.ToDecimal(pay.BalanceTreasury(safeIDTo));
        }
    }
}