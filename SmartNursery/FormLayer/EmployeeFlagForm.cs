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
    public partial class EmployeeFlagForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public EmployeeFlagForm()
        {
            InitializeComponent();
        }
        void InsertOrUpdate()
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            try
            {
                if (IsEdit)
                {
                    var getdata = db.EmployeeFlags.Where(a => a.EmployeeFlagId == Code).FirstOrDefault();
                    getdata.EmployeeFlagDescr = Convert.ToString(textEditEmployeeFlag.Text);
                    getdata.EmployeeFlagIsActive = Convert.ToBoolean(checkEdit.Checked);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
             
                }
                else
                {
                    EmployeeFlag ep = new EmployeeFlag();
                    ep.EmployeeFlagDescr = Convert.ToString(textEditEmployeeFlag.Text);
              ep.EmployeeFlagIsActive = Convert.ToBoolean(checkEdit.Checked);
                    db.EmployeeFlags.InsertOnSubmit(ep);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("لم يتم الحفظ ");
            }
        }
         void Clear()
        {
            textEditEmployeeFlag.Text = null;
            checkEdit.Checked = true;
        }
     

        private void EmployeeFlagForm_Load(object sender, EventArgs e)
        {
            checkEdit.Checked = true;

            if (IsEdit)
            {
                var Edit = db.EmployeeFlags.Where(a => a.EmployeeFlagId == Code).FirstOrDefault();
                textEditEmployeeFlag.Text = Edit.EmployeeFlagDescr;
                checkEdit.Checked = Convert.ToBoolean(Edit.EmployeeFlagIsActive);
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

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}