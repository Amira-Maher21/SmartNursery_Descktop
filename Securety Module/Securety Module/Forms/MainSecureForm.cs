using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SecuretyModule.Forms
{
    public partial class MainSecureForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        DB.DataClassesSecurityMouleDataContext db = new DB.DataClassesSecurityMouleDataContext();
        enum FormMode
        {
            userFrm,updatePass,mainPerm, subPermession, rank
        }
        FormMode formMode;
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public MainSecureForm()
        {
            InitializeComponent();
        }
        public object userForm()
        {
            db = new DB.DataClassesSecurityMouleDataContext();
            return from U in db.Users
                   join r in db.Ranks on U.RankID equals r.RankID
                   select new {U.UserCode, U.Name, U.RankID, r.RankName, U.IsActive, U.Notes };
        }
        public object updatePassword()
        {
            db = new DB.DataClassesSecurityMouleDataContext();
            return from U in db.Users
                   select new { U.Name, U.RankID, U.IsActive, U.Notes };
        }
        public object mainPermession()
        {
            db = new DB.DataClassesSecurityMouleDataContext();
            return from U in db.Actions
                   select new { U.ActionID, U.ActionName,U.IsActive, U.Notes};
        }
        public object subbPermession()
        {
            db = new DB.DataClassesSecurityMouleDataContext();
            return from U in db.ActionDetails
                   join r in db.Actions on U.ActionID equals r.ActionID
                   select new { U.ActionDetailName, U.ActionID, r.ActionName, U.IsActive, U.Notes };
        }
        public object Ranks()
        {
            db = new DB.DataClassesSecurityMouleDataContext();
            return from U in db.Ranks
                   select new { U.RankID, U.RankName, U.IsActive };
        }
        private void Fill()
        {
            gridView1.Columns.Clear();

            switch (formMode)
            {
                case FormMode.userFrm:
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled = true;
                    gridControl1.DataSource = userForm();
                    gridView1.GroupPanelText = "المستخدم";
                    break;
                case FormMode.updatePass:
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled = false;
                    gridControl1.DataSource = updatePassword();
                    gridView1.GroupPanelText = "تغيير كلمة المرور";
                    break;
                case FormMode.mainPerm:
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled = false;
                    gridControl1.DataSource = mainPermession();
                    gridView1.GroupPanelText = "الصلاحيات الأساسية";
                    break;
                case FormMode.subPermession:
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled =true;
                    gridControl1.DataSource = subbPermession();
                    gridView1.GroupPanelText = "الصلاحيات الفرعية";
                    break;
                case FormMode.rank:
                    gridControl1.DataSource = Ranks();
                    gridView1.GroupPanelText = "رتب الأمان";
                    break;
            }
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView1.Columns)
            {
                column.Caption = Properties.Resources.ResourceManager.GetString(column.FieldName);
                column.OptionsColumn.AllowEdit = false;
            }
        }
        private void accordionControlElement8_Click(object sender, EventArgs e)
        {
            formMode = FormMode.rank;
            Fill();
            //new Ranks().Show();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            formMode = FormMode.userFrm;
            Fill();
            // new UserForm().Show();
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            formMode = FormMode.updatePass;
            Fill();
            // new UpdatePassword().Show();
        }

        private void accordionControlElement5_Click(object sender, EventArgs e)
        {
            formMode = FormMode.mainPerm;
            Fill();
            //new MainPermessions().Show();
        }

        private void accordionControlElement6_Click(object sender, EventArgs e)
        {
            formMode = FormMode.subPermession;
            Fill();
            //new SubPermessions().Show();
        }

        private void accordionControlElement9_Click(object sender, EventArgs e)
        {
            new RanksPermessions().Show();
        }

        private void MainSecureForm_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridControl1.DataSource != null)
            {
                switch (formMode)
                {
                    case FormMode.userFrm:
                        new UserForm().ShowDialog();
                        break;
                    case FormMode.updatePass:
                        new UpdatePassword().ShowDialog();
                        break;
                    case FormMode.mainPerm:
                        new MainPermessions().ShowDialog();
                        break;
                    case FormMode.subPermession:
                        new SubPermessions().ShowDialog();
                        break;
                    case FormMode.rank:
                        new Ranks().ShowDialog();
                        break;

                }
            }
            Fill();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            int code = 0;
            switch (formMode)
            {
                case FormMode.userFrm:
                    code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "UserCode"));
                    new UserForm() { IsEdit = true, Code = code }.ShowDialog();
                    break;
                case FormMode.subPermession:
                    code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ActionDetailId"));
                    new SubPermessions() { IsEdit = true, Code = code }.ShowDialog();
                    break;
                case FormMode.rank:
                    code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "RankID"));
                    new Ranks() { IsEdit = true, Code = code }.ShowDialog();
                    break;
            }
            Fill();
        }

        private void accordionControlElement4_Click(object sender, EventArgs e)
        {

        }

        private void accordionControlElement7_Click(object sender, EventArgs e)
        {

        }
    }
}

