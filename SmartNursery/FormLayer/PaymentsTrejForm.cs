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
    public partial class PaymentsTrejForm : DevExpress.XtraEditors.XtraForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        public static int Id { get; set; }
        public static int Id2 { get; set; }

        public PaymentsTrejForm()
        {
           
            InitializeComponent();
            //accountsBindingSource.DataSource = db.Accounts.Where(o => o.AccountFlagId == 6);
            ////accountFlagsBindingSource.DataSource = db.AccountFlags.Where(o=>o.AccountFlagId != 8 && o.AccountFlagId != 11 && o.AccountFlagId != 12);
            ////searchLookUpEdit2.Properties.DataSource = db.Accounts.Where(o => o.AccountFlagId != 8 && o.AccountFlagId != 11 && o.AccountFlagId != 12).ToList();
            //searchLookUpEdit3.Properties.DataSource = db.AccountFlags.Where(o => o.AccountFlagId != 8 && o.AccountFlagId != 11 && o.AccountFlagId != 12).ToList();

        }


        private void searchLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtout_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            txtin.Text = "0";
        }

        private void txtin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            txtout.Text = "0";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;

            }
            //ممصروف من خزنة
            if (txtout.Text != "0")
            {
                Treasury tr2 = new Treasury();
                {

                    tr2.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue)));
                    tr2.TreasuryDate = InvoiceDate.DateTime;
                    tr2.TreasuryDebit = 0;
                    tr2.TreasuryCredit = Convert.ToDecimal(txtout.Text);
                    tr2.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))) - Convert.ToDecimal(txtout.Text);
                    tr2.DocTypeId = 29;
                   // tr2.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    tr2.Username = MainForm.user;
                   // tr2.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    tr2.TreasuryNote = txtNote.Text;
                }
                db.Treasuries.InsertOnSubmit(tr2);
                db.SubmitChanges();
            }
                // وارد خزنة
             if (txtin.Text != "0")
            {
                Treasury tr = new Treasury();
                {
                    tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue)));
                    tr.TreasuryDate = InvoiceDate.DateTime;
                    tr.TreasuryDebit = Convert.ToDecimal(txtin.Text);
                    tr.TreasuryCredit = 0;
                    tr.TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))) + Convert.ToDecimal(txtin.Text);
                    tr.DocTypeId = 28;
                   // tr.AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    tr.Username = MainForm.user;
                   // tr.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    tr.TreasuryNote = txtNote.Text;
                }
                db.Treasuries.InsertOnSubmit(tr);
                db.SubmitChanges();
            }
            
             
            MessageBox.Show("تم الحفظ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            searchLookUpEdit1.EditValue = null;
            searchLookUpEdit2.EditValue = null;
            searchLookUpEdit3.EditValue = null;
            txtin.Text = "0";
            txtout.Text = "0";
            txtNote.Text = string.Empty;
            InvoiceDate.DateTime = DateTime.Now;

            //if (txtin.Text != "0")
            //{
            //    Treasury tr = new Treasury()
            //    {
            //        SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))),
            //        TreasuryDate = InvoiceDate.DateTime,
            //        TreasuryDebit = 0,
            //        TreasuryCredit = Convert.ToDecimal(txtin.Text),
            //        TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))) + Convert.ToDecimal(txtin.Text),
            //        DocTypeId = 28,
            //        AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue),
            //        Username = MainForm.user,
            //        Account2Id = Convert.ToInt32(searchLookUpEdit2.EditValue),
            //        TreasuryNote = txtNote.Text
            //    };

            //    db.Treasuries.InsertOnSubmit(tr);
            //    db.SubmitChanges();



            //    if (Convert.ToInt32(searchLookUpEdit3.EditValue) != 6 || Convert.ToInt32(searchLookUpEdit3.EditValue) != 5)
            //    {
            //        AccountPayment payb = new AccountPayment();
            //        {
            //            payb.PaymentSerial = Convert.ToInt32(pay.serialAccount(Convert.ToInt32(searchLookUpEdit2.EditValue)));
            //            payb.PaymentDate = InvoiceDate.DateTime;
            //            payb.AccountId = Convert.ToInt32(searchLookUpEdit2.EditValue);
            //            payb.PaymentDebit = 0;
            //            payb.PaymentCredit = Convert.ToDecimal(txtin.Text);
            //            payb.PaymentBalance = Convert.ToDecimal(pay.BalanceAccount(Convert.ToInt32(searchLookUpEdit2.EditValue))) - Convert.ToDecimal(txtin.Text);
            //            payb.UserName = MainForm.user;
            //            payb.DocTypeId = 28;
            //            payb.InvoiceId = tr.TreasuryId;
            //            payb.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);
            //            // payb.CoinId = Convert.ToInt32(CoinSearchId.EditValue);

            //        }
            //        db.AccountPayments.InsertOnSubmit(payb);
            //        db.SubmitChanges();
            //    }



            //    if (Convert.ToInt32(searchLookUpEdit3.EditValue) == 6 || Convert.ToInt32(searchLookUpEdit3.EditValue) == 5)
            //    {

            //        Treasury tr2 = new Treasury()
            //        {
            //            SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit2.EditValue))),
            //            TreasuryDate = InvoiceDate.DateTime,
            //            TreasuryDebit = Convert.ToDecimal(txtin.Text),
            //            TreasuryCredit = 0,
            //            TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit2.EditValue))) - Convert.ToDecimal(txtin.Text),
            //            DocTypeId = 29,
            //            AccountId = Convert.ToInt32(searchLookUpEdit2.EditValue),
            //            Username = MainForm.user,
            //            Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue),
            //            TreasuryNote = txtNote.Text
            //        };

            //        db.Treasuries.InsertOnSubmit(tr2);
            //        db.SubmitChanges();

            //    }
            //}
            //else
            //{
            //    Treasury tr = new Treasury()
            //    {
            //        SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))),
            //        TreasuryDate = InvoiceDate.DateTime,
            //        TreasuryDebit = Convert.ToDecimal(txtout.Text),
            //        TreasuryCredit = 0,
            //        TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit1.EditValue))) - Convert.ToDecimal(txtout.Text),
            //        DocTypeId = 29,
            //        AccountId = Convert.ToInt32(searchLookUpEdit1.EditValue),
            //        Username = MainForm.user,
            //        Account2Id = Convert.ToInt32(searchLookUpEdit2.EditValue),
            //        TreasuryNote = txtNote.Text
            //    };

            //    db.Treasuries.InsertOnSubmit(tr);
            //    db.SubmitChanges();



            //    if (Convert.ToInt32(searchLookUpEdit3.EditValue) != 6 || Convert.ToInt32(searchLookUpEdit3.EditValue) != 5)
            //    {
            //        AccountPayment payb = new AccountPayment();
            //        {
            //            payb.PaymentSerial = Convert.ToInt32(pay.serialAccount(Convert.ToInt32(searchLookUpEdit2.EditValue)));
            //            payb.PaymentDate = InvoiceDate.DateTime;
            //            payb.AccountId = Convert.ToInt32(searchLookUpEdit2.EditValue);
            //            payb.PaymentDebit = Convert.ToDecimal(txtout.Text);
            //            payb.PaymentCredit = 0;
            //            payb.PaymentBalance = Convert.ToDecimal(pay.BalanceAccount(Convert.ToInt32(searchLookUpEdit2.EditValue))) + Convert.ToDecimal(txtout.Text);
            //            payb.UserName = MainForm.user;
            //            payb.DocTypeId = 29;
            //            payb.InvoiceId = tr.TreasuryId;
            //            payb.Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue);

            //            // payb.CoinId = Convert.ToInt32(CoinSearchId.EditValue);

            //        }
            //        db.AccountPayments.InsertOnSubmit(payb);
            //        db.SubmitChanges();
            //    }

            //    if (Convert.ToInt32(searchLookUpEdit3.EditValue) == 6 || Convert.ToInt32(searchLookUpEdit3.EditValue) == 5)
            //    {

            //        Treasury tr2 = new Treasury()
            //        {
            //            SerialNumber = Convert.ToInt32(pay.serialTreasury(Convert.ToInt32(searchLookUpEdit2.EditValue))),
            //            TreasuryDate = InvoiceDate.DateTime,
            //            TreasuryDebit = 0,
            //            TreasuryCredit = Convert.ToDecimal(txtout.Text),
            //            TreasuryBalance = Convert.ToDecimal(pay.BalanceTreasury(Convert.ToInt32(searchLookUpEdit2.EditValue))) + Convert.ToDecimal(txtout.Text),
            //            DocTypeId = 28,
            //            AccountId = Convert.ToInt32(searchLookUpEdit2.EditValue),
            //            Username = MainForm.user,
            //            Account2Id = Convert.ToInt32(searchLookUpEdit1.EditValue),
            //            TreasuryNote = txtNote.Text
            //        };

            //        db.Treasuries.InsertOnSubmit(tr2);
            //        db.SubmitChanges();

            //  }

            // }


        }

        private void PaymentsTrejForm_Load(object sender, EventArgs e)
        {
            InvoiceDate.DateTime = DateTime.Now;
            searchLookUpEdit3.EditValue = Id;
            searchLookUpEdit2.EditValue = Id2;
            //searchLookUpEdit1.EditValue = 6;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void searchLookUpEdit3_EditValueChanged(object sender, EventArgs e)
        {
            //accountsBindingSource1.DataSource =db.Accounts.Where(o => o.AccountFlagId == Convert.ToInt32(searchLookUpEdit3.EditValue));
            


        }
    }
}