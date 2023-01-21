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

using System.Configuration;

using DevExpress.XtraEditors;

namespace SecuretyModule.Forms
{
    public partial class UpdatePassword : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DB.DataClassesSecurityMouleDataContext db = new DB.DataClassesSecurityMouleDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public UpdatePassword()
        {
            InitializeComponent();
        }

        private void UpdatePassword_Load(object sender, EventArgs e)
        {
            searchLookUpUser.Properties.DataSource = db.Users;
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (dxValidationProvider1.Validate())
            {
                User User = db.Users.Single(us => us.UserCode == Convert.ToInt32(searchLookUpUser.EditValue));
                if (UserInformation.CompHash(textEdit1.Text) == User.PassWord)
                {
                    if (textEdit2.Text == textEdit3.Text)
                    {
                        User.PassWord = UserInformation.CompHash(textEdit2.Text);
                        db.SubmitChanges();
                        XtraMessageBox.Show("تم الحفظ", "تغيير كلمة مرور المستخدم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();

                    }
                    else
                    {
                        XtraMessageBox.Show("أكد كلمة المرور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Clear();
                    }

                }
                else
                {
                    XtraMessageBox.Show("كلمة المرور القديمة خطأ أعد إدخال كلمة المرور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();


                }
            }
        }
        private void Clear()
        {
            searchLookUpUser.EditValue = null;
            textEdit1.Text = "";
            textEdit2.Text = "";
            textEdit3.Text = "";
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }
    }
}
    