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
using SecuretyModule;

namespace SmartNursery.FormLayer
{
    public partial class EmployeeForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }


        public EmployeeForm()
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
                    var getdata = db.Employees.Where(a => a.EmplyeeId == Code).FirstOrDefault();
                    getdata.EmplyeeName = Convert.ToString(textEditEmplyee.EditValue);
                    getdata.EmplyeeAddress = Convert.ToString(textEditEmplyeeAddress.EditValue);
                    getdata.EmplyeeEmail = Convert.ToString(textEditEmplyeeEmail.EditValue);
                    getdata.EmplyeePhone = Convert.ToString(textEditEmplyeePhone.EditValue);
                    getdata.EmployeeFlagId = Convert.ToInt32(searchLookUpEditEmployeeFlagId.EditValue);
                    getdata.EmplyeelSalary = Convert.ToDecimal(textEditEmplyeelSalary.EditValue);
                    getdata.EmplyeeNote = Convert.ToString(textEditEmplyeeNote.EditValue);
                    getdata.EmployeeHireDate = Convert.ToDateTime(dateEditEmployeeHireDate.EditValue);
                    getdata.EmpoyeeIsActive = Convert.ToBoolean(checkEdit.EditValue);
                    getdata.UserCode = Convert.ToInt32(searchLookUpUser.EditValue);
                    getdata.EmployeeUserName = searchLookUpUser.Text;
                    getdata.EmplyeeBarCode = EmplyeeBarCode.Text;
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Clear();
                }
                else
                {
                    Employee em = new Employee();
                    em.EmplyeeName = Convert.ToString(textEditEmplyee.EditValue);
                    em.EmplyeeAddress = Convert.ToString(textEditEmplyeeAddress.EditValue);
                    em.EmplyeeEmail = Convert.ToString(textEditEmplyeeEmail.EditValue);
                    em.EmplyeePhone = Convert.ToString(textEditEmplyeePhone.EditValue);
                    em.EmployeeFlagId = Convert.ToInt32(searchLookUpEditEmployeeFlagId.EditValue);
                    em.EmplyeelSalary = Convert.ToDecimal(textEditEmplyeelSalary.EditValue);
                    em.EmplyeeNote = Convert.ToString(textEditEmplyeeNote.EditValue);
                    em.EmployeeHireDate = Convert.ToDateTime(dateEditEmployeeHireDate.EditValue);
                    em.EmpoyeeIsActive = Convert.ToBoolean(checkEdit.EditValue);
                    em.UserCode = Convert.ToInt32(searchLookUpUser.EditValue);
                    em.EmployeeUserName = searchLookUpUser.Text;
                    em.EmplyeeBarCode = EmplyeeBarCode.Text;
                    db.Employees.InsertOnSubmit(em);
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
        void Clear()
        {
            textEditEmplyee.EditValue = null;
            textEditEmplyeeAddress.EditValue = null;
            textEditEmplyeeEmail.EditValue = null;
            textEditEmplyeePhone.EditValue = null;
            searchLookUpEditEmployeeFlagId.EditValue = null;
            textEditEmplyeelSalary.EditValue = 0;
            textEditEmplyeeNote.EditValue = null;
            dateEditEmployeeHireDate.EditValue = DateTime.Now;
            checkEdit.Checked = true;
            searchLookUpUser.EditValue = null;
           
            Fill();
        }
        void Fill()
        {
            searchLookUpEditEmployeeFlagId.Properties.DataSource = db.EmployeeFlags.Where(a => a.EmployeeFlagIsActive == true).ToList();
            //searchLookUpUser.Properties.DataSource = db.Employees.Where(o => o.EmpoyeeIsActive == true).ToList();
            searchLookUpUser.Properties.DataSource = UserInformation.Useres();
            checkEdit.Checked = true;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            Fill();
            

            if (IsEdit)
            {
                var Edit = db.Employees.Where(a => a.EmplyeeId == Code).FirstOrDefault();
                textEditEmplyee.EditValue = Edit.EmplyeeName;
                textEditEmplyeeAddress.EditValue = Edit.EmplyeeAddress;
                textEditEmplyeeEmail.EditValue = Edit.EmplyeeEmail;
                textEditEmplyeePhone.EditValue = Edit.EmplyeePhone;
                searchLookUpEditEmployeeFlagId.EditValue = Edit.EmployeeFlagId;
                textEditEmplyeelSalary.EditValue = Edit.EmplyeelSalary.ToString();
                textEditEmplyeeNote.EditValue = Edit.EmplyeeNote;
                dateEditEmployeeHireDate.EditValue = Edit.EmployeeHireDate.ToString(); 
                searchLookUpUser.EditValue = Edit.UserCode ;
                EmplyeeBarCode.Text = Edit.EmplyeeBarCode;


            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();
        }

        private void textEditEmployeeFlagId_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}