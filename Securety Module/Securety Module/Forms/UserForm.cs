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
using SecuretyModule.DB;

using DevExpress.XtraEditors;

namespace SecuretyModule.Forms
{
    public partial class UserForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DB.DataClassesSecurityMouleDataContext db = new DB.DataClassesSecurityMouleDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public UserForm()
        {
            InitializeComponent();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!IsEdit)
            {
                try
                {
                    if (txtConfirmPassWord.EditValue.ToString() == txtPassWord.EditValue.ToString())
                    {
                        User user = new User();
                        user.Name = txtName.Text;
                        user.RankID = Convert.ToInt32(searchLookUpRank.EditValue);
                        user.IsActive = checkEdit1.Checked;
                        user.PassWord = UserInformation.CompHash(txtPassWord.Text);
                        user.Notes = txtNotes.Text;
                        db.Users.InsertOnSubmit(user);
                        db.SubmitChanges();
                    }
                    else
                    {
                        XtraMessageBox.Show("Please Confirm PassWord", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    MessageBox.Show("تم الحفظ بنجاح ");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("لم يتم الحفظ");
                    this.Close();
                }
            }
            else
            {
                try
                {
                    if (txtConfirmPassWord.EditValue.ToString() == txtPassWord.EditValue.ToString())
                    {
                        var edit = db.Users.Where(a => a.UserCode == Code).FirstOrDefault();
                        edit.Name = txtName.Text;
                        edit.RankID = Convert.ToInt32(searchLookUpRank.EditValue);
                        edit.IsActive = checkEdit1.Checked;
                        edit.PassWord = UserInformation.CompHash(txtPassWord.Text);
                        edit.Notes = txtNotes.Text;
                        db.SubmitChanges();
                    }
                    else
                    {
                        XtraMessageBox.Show("Please Confirm PassWord", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    MessageBox.Show("تم الحفظ بنجاح ");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("لم يتم الحفظ");
                    this.Close();
                }
            }

        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                var editt = db.Users.Where(a => a.UserCode == Code).FirstOrDefault();
                txtName.Text = editt.Name;
                searchLookUpRank.EditValue = editt.RankID;
                checkEdit1.EditValue = editt.IsActive;
            }
            else
            {
                searchLookUpRank.Properties.DataSource = db.Ranks;
                checkEdit1.Checked = true;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            txtName.Text = null;
            searchLookUpRank.EditValue = null;
            txtPassWord.Text = null;
            txtNotes.Text = null;
            txtConfirmPassWord.Text = null;
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
    }
}

namespace Securety_Module
{
    public class Login
    {
        public Login()
        {
        }
    }
}