using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SecuretyModule.DB;

using SecuretyModule.Forms;



namespace SecuretyModule
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        // DataClassesSecurityMouleDataContext dbcon = new DataClassesSecurityMouleDataContext();
        // DBDataContext db = new DBDataContext();
        DataClassesSecurityMouleDataContext db = new DataClassesSecurityMouleDataContext();
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserInformation.UserName = null;
            System.Environment.Exit(1);
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //textEdit1.Focus();
            //checkEdit1.Checked = false;
            if (Properties.Settings.Default.RememberMe)
            {
                textEdit1.Text = Properties.Settings.Default.UserName;
                textEdit2.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = Properties.Settings.Default.RememberMe;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text==string.Empty || textEdit2.Text==string.Empty)
            {
                MessageBox.Show("Please Enter UserName & Password", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textEdit1.Focus();
            }
            else
            {
                CheckPassWord(); 
            }
           
        }
          private bool CheckPassWord()
        {

            //bool result = false;
            //try
            //{
            //    User user = dbcon.Users.SingleOrDefault(s => s.Name == textEdit1.Text.Trim());
            //    if (user.PassWord == UserInformation.CompHash(textEdit2.EditValue.ToString()))
            //    {
            //        result = true;
            //        UserInformation.UserName = textEdit1.Text;
            //        this.Close();
            //    }
            //    else
            //    {
            //        result = false;
            //        UserInformation.UserName = null;
            //        MessageBox.Show("من فضلك تأكد من البيانات" + "\n" + "ربما يكون الإسم أو الرقم السري غير صحيح");
            //    }

            //}
            //catch
            //{
            //    result = false;
            //    UserInformation.UserName = null;
            //    MessageBox.Show(" ربما يكون الإسم أو الرقم السري غير صحيح" + "\n" + "من فضلك تأكد من البيانات");
            //}
            //return result;
            bool result = false;
            try
            {
                User user = db.Users.SingleOrDefault(s => s.Name == textEdit1.Text.Trim());
                if (user.PassWord == UserInformation.CompHash(textEdit2.EditValue.ToString()))
                {
                    result = true;
                    UserInformation.UserName = textEdit1.Text;
                    if (chkRememberMe.Checked)
                    {
                        Properties.Settings.Default.UserName = textEdit1.Text;
                        Properties.Settings.Default.Password = textEdit2.Text;
                    }
                    else
                    {
                        Properties.Settings.Default.UserName = null;
                        Properties.Settings.Default.Password = null;
                    }
                    Properties.Settings.Default.RememberMe = chkRememberMe.Checked;
                    Properties.Settings.Default.Save();
                    this.Close();
                }
                else
                {
                    result = false;
                   UserInformation.UserName = null;
                   MessageBox.Show("من فضلك تأكد من البيانات" + "\n" + "ربما يكون الإسم أو الرقم السري غير صحيح");
                }

            }
            catch
            {
                result = false;
                UserInformation.UserName = null;
                MessageBox.Show(" ربما يكون الإسم أو الرقم السري غير صحيح" + "\n" + "من فضلك تأكد من البيانات");
            }
            return result;
        }
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                CheckPassWord();
            }
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                CheckPassWord();
            }
        }
    }
}
