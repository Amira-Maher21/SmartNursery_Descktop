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
    public partial class TestMonthForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public TestMonthForm()
        {
            InitializeComponent();
        }

        private void TestMonthForm_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            TestMonth te = new TestMonth();
            te.TestMonthDesc = textEdit1.Text;
            te.TestMonthIsClosed = false;
            db.TestMonths.InsertOnSubmit(te);
            db.SubmitChanges();
            MessageBox.Show("تم الحفظ");

            textEdit1.Text = null;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            textEdit1.Text = null;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}