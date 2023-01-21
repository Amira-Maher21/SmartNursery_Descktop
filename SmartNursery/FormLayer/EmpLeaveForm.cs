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
    public partial class EmpLeaveForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        DataTable LeavingData = new DataTable();
        public EmpLeaveForm()
        {
            InitializeComponent();
        }
        protected void createdatatable()
        {
            LeavingData = new DataTable();
            LeavingData.Columns.Add("EmplyeeId", typeof(int));
            LeavingData.Columns.Add("IsLeaving", typeof(Boolean));
            LeavingData.Columns.Add("DateOfLeaving", typeof(DateTime));
            gridControl1.DataSource = LeavingData;
        }
        private void Fill()
        {
            searchLookUpEditDay.Properties.DataSource = db.DayOfWeeks;
            searchLookUpEmp.Properties.DataSource = db.Employees.Where(a => a.EmpoyeeIsActive == true);
            dateEdit1.DateTime = DateTime.Now;
            repositoryItemSearchLookUpEmp.DataSource = db.Employees.Where(a => a.EmpoyeeIsActive == true).ToList();
        }
        void GetDate(DateTime dateValue)
        {
            dateValue = dateEdit1.DateTime;
            if (dateValue.ToString("ddd") == "Sat")
            { searchLookUpEditDay.EditValue = 1; }
            if (dateValue.ToString("ddd") == "Sun")
            { searchLookUpEditDay.EditValue = 2; }
            if (dateValue.ToString("ddd") == "Mon")
            { searchLookUpEditDay.EditValue = 3; }
            if (dateValue.ToString("ddd") == "Tue")
            { searchLookUpEditDay.EditValue = 4; }
            if (dateValue.ToString("ddd") == "Wed")
            { searchLookUpEditDay.EditValue = 5; }
            if (dateValue.ToString("ddd") == "Thu")
            { searchLookUpEditDay.EditValue = 6; }
            if (dateValue.ToString("ddd") == "Fri")
            { searchLookUpEditDay.EditValue = 7; }
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            GetDate(dateEdit1.DateTime);
        }

        private void EmpLeaveForm_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            gridView1.FocusedRowHandle = 1;
            if (e.KeyCode == Keys.Enter)
            {
                int Employee;
                DateTime date;
                Boolean IsLeave;
                string barCode = txtBarCode.Text;
                var data = db.Employees.Where(a => a.EmplyeeBarCode == barCode).FirstOrDefault();
                try
                {
                    var check = db.EmployeeAttendanceDetails.Where(u => u.DateOfLeaving.Value.Date == dateEdit1.DateTime.Date && u.IsLeaving == true).ToList();
                    if (check.Any())
                    {
                        DialogResult result = MessageBox.Show("تم تسجيل الإنصراف من قبل هل تريد التعديل على التسجيل؟", "تنبيه", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            var Update = db.EmployeeAttendanceDetails.Where(u => u.DateOfLeaving.Value.Date == dateEdit1.DateTime.Date && u.EmplyeeId == Convert.ToInt32(data.EmplyeeId)).First();
                            Update.IsLeaving = false;
                            db.SubmitChanges();

                            if (data != null)
                            {
                                Employee = Convert.ToInt32(data.EmplyeeId);
                                date = DateTime.Now;
                                IsLeave = true;
                                LeavingData.Rows.Add(Employee, IsLeave, date);
                                gridView1.FocusedRowHandle = -1;
                                txtBarCode.Text = "";
                                txtBarCode.Focus();
                                //DateTime date2 = Convert.ToDateTime(AttendanceDate.DateTime);
                            }

                            MessageBox.Show("تم اضافتة مرة اخرى برجاء حفظ التسجيل");
                        }
                        else if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                catch
                {

                }

                if (data != null)
                {
                    Employee = Convert.ToInt32(data.EmplyeeId);
                    date = DateTime.Now;
                    IsLeave = true;
                    LeavingData.Rows.Add(Employee, IsLeave, date);
                    txtBarCode.Text = "";
                    txtBarCode.Focus();
                    //DateTime date2 = Convert.ToDateTime(AttendanceDate.DateTime);
                }
            }
        }
        private void Insert()
        {
            EmployeeAttendanceMaster STM = new EmployeeAttendanceMaster();
            {
                STM.DayOfWeekId = Convert.ToInt32(dateEdit1.EditValue);
                STM.EmplyeeId = Convert.ToInt32(searchLookUpEmp.EditValue);
                STM.StudentAttendanceDate = Convert.ToDateTime(dateEdit1.EditValue);
                STM.EmpUser = MainForm.user;

            }

            db.EmployeeAttendanceMasters.InsertOnSubmit(STM);
            db.SubmitChanges();

            foreach (DataRow row in LeavingData.Rows)
            {

                EmployeeAttendanceDetail STD = new EmployeeAttendanceDetail();
                {
                    STD.EmployeeAttendanceMasterId = STM.EmployeeAttendanceMasterId;
                    STD.EmplyeeId = Convert.ToInt32(row["EmplyeeId"]);
                    STD.IsLeaving = Convert.ToBoolean(row["IsLeaving"]);
                    STD.DateOfLeaving = Convert.ToDateTime(row["DateOfLeaving"]);
                }
                db.EmployeeAttendanceDetails.InsertOnSubmit(STD);
                db.SubmitChanges();
            }

            MessageBox.Show("تم حفظ إنصراف اليوم", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void barButtonSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            else
            {
                Insert();
            }
        }

        private void repositoryItemHyperLinkDelete_Click(object sender, EventArgs e)
        {
            gridView1.DeleteRow(gridView1.FocusedRowHandle);
        }

        private void barButtonClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}