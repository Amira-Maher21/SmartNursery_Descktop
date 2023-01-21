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
    public partial class SafesForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        PaymentClass pay = new PaymentClass();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public int Flag { get; set; }
        public SafesForm()
        {
            InitializeComponent();
        }



        void InsertOrUpdate()
        { //خزنة
            if (Flag==1)
            {
                if (!dxValidationProvider1.Validate())
                {
                    return;
                }
                try
                {
                    if (IsEdit)
                    {
                        var getdata = db.Safes.Where(a => a.SafeId == Code).FirstOrDefault();
                        getdata.SafeName = Convert.ToString(textEditSafe.Text);
                        getdata.FirstPeriodBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        getdata.SafeIsActive = Convert.ToBoolean(checkEdit.Checked);
                        getdata.SafeFlagId = Convert.ToInt32(searchLookUpEditSafeFlag.EditValue);
                       // getdata.AccountNumber = Convert.ToString(textEditAccount.EditValue);
                        db.SubmitChanges();

                        var TrEdit= db.Treasuries.Where(a => a.SafeId == Code).FirstOrDefault();
                        TrEdit.TreasuryDebit = Convert.ToDecimal(textEditFirstPeriod.Text);
                        TrEdit.TreasuryBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");
                        Clear();
                    }
                    else
                    {
                        Safe sa = new Safe();

                        sa.SafeName = Convert.ToString(textEditSafe.Text);
                        sa.FirstPeriodBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        sa.SafeIsActive = Convert.ToBoolean(checkEdit.Checked);
                        sa.SafeFlagId = Convert.ToInt32(searchLookUpEditSafeFlag.EditValue);
                        //sa.AccountNumber = Convert.ToString(textEditAccount.EditValue);
                        db.Safes.InsertOnSubmit(sa);
                        db.SubmitChanges();

                        Treasury tr = new Treasury();
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(sa.SafeId));
                        tr.TreasuryDate = DateTime.Now;
                        tr.TreasuryDebit = Convert.ToDecimal(textEditFirstPeriod.Text);
                        tr.TreasuryCredit = 0;
                        tr.TreasuryBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        tr.SafeId = sa.SafeId;
                        tr.Username = MainForm.user;
                        tr.DocTypeId = 5;
                        db.Treasuries.InsertOnSubmit(tr);
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");
                        Clear();
                    }

                }
                catch (Exception)
                {


                    MessageBox.Show("لم يتم الحفظ ");
                }
            }
            //بنك
            else if (Flag == 2)
            {
                if (!dxValidationProvider2.Validate())
                {
                    return;
                }
                try
                {
                    if (IsEdit)
                    {
                        var getdata = db.Safes.Where(a => a.SafeId == Code).FirstOrDefault();
                        getdata.SafeName = Convert.ToString(textEditSafe.Text);
                        getdata.FirstPeriodBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        getdata.SafeIsActive = Convert.ToBoolean(checkEdit.Checked);
                        getdata.SafeFlagId = Convert.ToInt32(searchLookUpEditSafeFlag.EditValue);
                        getdata.AccountNumber = Convert.ToString(textEditAccount.EditValue);
                        db.SubmitChanges();

                        var TrEdit = db.Treasuries.Where(a => a.SafeId == Code).FirstOrDefault();
                        TrEdit.TreasuryDebit = Convert.ToDecimal(textEditFirstPeriod.Text);
                        TrEdit.TreasuryBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");
                        Clear();
                    }
                    else
                    {
                        Safe sa = new Safe();

                        sa.SafeName = Convert.ToString(textEditSafe.Text);
                        sa.FirstPeriodBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        sa.SafeIsActive = Convert.ToBoolean(checkEdit.Checked);
                        sa.SafeFlagId = Convert.ToInt32(searchLookUpEditSafeFlag.EditValue);
                        sa.AccountNumber = Convert.ToString(textEditAccount.EditValue);
                        db.Safes.InsertOnSubmit(sa);
                        db.SubmitChanges();

                        Treasury tr = new Treasury();
                        tr.SerialNumber = Convert.ToInt32(pay.serialTreasury(sa.SafeId));
                        tr.TreasuryDate = DateTime.Now;
                        tr.TreasuryDebit = Convert.ToDecimal(textEditFirstPeriod.Text);
                        tr.TreasuryCredit = 0;
                        tr.TreasuryBalance = Convert.ToDecimal(textEditFirstPeriod.Text);
                        tr.SafeId = sa.SafeId;
                        tr.Username = MainForm.user;
                        tr.DocTypeId = 5;
                        db.Treasuries.InsertOnSubmit(tr);
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");
                        Clear();
                    }

                }
                catch (Exception)
                {


                    MessageBox.Show("لم يتم الحفظ ");
                }
            }
            
        }
        void Clear()
        {

            textEditSafe.Text = null;
            textEditFirstPeriod.Text = null;
            checkEdit.Checked = true;
            searchLookUpEditSafeFlag.EditValue = null;
            textEditAccount.EditValue = null;
            Fill();
        }
        void Fill()
        {
            searchLookUpEditSafeFlag.Properties.DataSource = db.SafeFlags.Where(g => g.IsActive == true ).ToList();
            searchLookUpEditSafeFlag.EditValue = Flag;
            checkEdit.Checked = true;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void SafesForm_Load(object sender, EventArgs e)
        {
            Fill();

            if (IsEdit)
            {

                if (Flag == 1)
                {
                    var Edit = db.Safes.Where(a => a.SafeId == Code).FirstOrDefault();
                    textEditSafe.Text = Edit.SafeName;
                    textEditFirstPeriod.EditValue = Edit.FirstPeriodBalance;
                    searchLookUpEditSafeFlag.EditValue = Edit.SafeFlagId;
                    
                }
                else if(Flag == 2)
                {
                    var Edit = db.Safes.Where(a => a.SafeId == Code).FirstOrDefault();
                    textEditSafe.Text = Edit.SafeName;
                    textEditFirstPeriod.EditValue = Edit.FirstPeriodBalance;
                    searchLookUpEditSafeFlag.EditValue = Edit.SafeFlagId;
                    textEditAccount.EditValue = Edit.AccountNumber;
                }
                
            }

            if (Flag==1)
            {
                layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
               
            }
            
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();

        }
    }
}