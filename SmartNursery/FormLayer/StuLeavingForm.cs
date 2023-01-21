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
    public partial class StuLeavingForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        DataTable LeavingData = new DataTable();
        public StuLeavingForm()
        {
            InitializeComponent();
            createdatatable();
        }
        protected void createdatatable()
        {
            LeavingData = new DataTable();
            LeavingData.Columns.Add("StudentId", typeof(int));
            LeavingData.Columns.Add("GradeId", typeof(int));
            LeavingData.Columns.Add("ClassId", typeof(int));
            LeavingData.Columns.Add("IsLeaving", typeof(Boolean));
            LeavingData.Columns.Add("DateOfLeaving", typeof(DateTime));
            gridControl1.DataSource = LeavingData;
        }
        private void Fill()
        {
            searchLookUpDay.Properties.DataSource = db.DayOfWeeks;
            searchLookUpUser.Properties.DataSource = db.Employees.Where(a => a.EmpoyeeIsActive == true);
            dateEditLeave.DateTime = DateTime.Now;
            repositoryItemSearchLookUpClass.DataSource = db.ClassTablees.ToList();
            repositoryItemSearchLookUpGrade.DataSource = db.Grades.ToList();
            repositoryItemSearchLookUpStudent.DataSource = db.Students.ToList();
        }
        void GetDate(DateTime dateValue)
        {
            dateValue = dateEditLeave.DateTime;
            if (dateValue.ToString("ddd") == "Sat")
            { searchLookUpDay.EditValue = 1; }
            if (dateValue.ToString("ddd") == "Sun")
            { searchLookUpDay.EditValue = 2; }
            if (dateValue.ToString("ddd") == "Mon")
            { searchLookUpDay.EditValue = 3; }
            if (dateValue.ToString("ddd") == "Tue")
            { searchLookUpDay.EditValue = 4; }
            if (dateValue.ToString("ddd") == "Wed")
            { searchLookUpDay.EditValue = 5; }
            if (dateValue.ToString("ddd") == "Thu")
            { searchLookUpDay.EditValue = 6; }
            if (dateValue.ToString("ddd") == "Fri")
            { searchLookUpDay.EditValue = 7; }
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void dateEditLeave_EditValueChanged(object sender, EventArgs e)
        {
            GetDate(dateEditLeave.DateTime);
        }

        private void StuLeavingForm_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void textEditBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            gridView1.FocusedRowHandle = 1;
            if (e.KeyCode == Keys.Enter)
            {
                int Student;
                int Class;
                int Gread;
                DateTime date;
                Boolean IsLeave;
                string barCode = textEditBarCode.Text;
                var data = db.Students.Where(a => a.StudentBarCode == barCode).FirstOrDefault();
                try
                {
                    var check = db.StudentAttendanceDetails.Where(u => u.DateOfLeaving.Value.Date == dateEditLeave.DateTime.Date && u.IsLeaving == true).ToList();
                    if (check.Any())
                    {
                        DialogResult result = MessageBox.Show("تم تسجيل إنصراف الطالب من قبل هل تريد التعديل على التسجيل؟", "تنبيه", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            var Update = db.StudentAttendanceDetails.Where(u => u.DateOfLeaving.Value.Date == dateEditLeave.DateTime.Date && u.StudentId == Convert.ToInt32(data.StudentId)).First();
                            Update.IsLeaving = false;
                            db.SubmitChanges();

                            if (data != null)
                            {
                                Student = Convert.ToInt32(data.StudentId);
                                Class = Convert.ToInt32(data.ClassId);
                                date = DateTime.Now;
                                IsLeave = true;
                                var GreadId1 = db.ClassTablees.Where(a => a.ClassId == Class).FirstOrDefault();
                                Gread = Convert.ToInt32(GreadId1.GradeId);
                                LeavingData.Rows.Add(Student, Class, Gread, IsLeave, date);
                                gridView1.FocusedRowHandle = -1;
                                textEditBarCode.Text = "";
                                textEditBarCode.Focus();
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
                    Student = Convert.ToInt32(data.StudentId);
                    Class = Convert.ToInt32(data.ClassId);
                    date = DateTime.Now;
                    IsLeave = true;
                    var GreadId1 = db.ClassTablees.Where(a => a.ClassId == Class).FirstOrDefault();
                    Gread = Convert.ToInt32(GreadId1.GradeId);
                    LeavingData.Rows.Add(Student, Class, Gread, IsLeave, date);
                    textEditBarCode.Text = "";
                    textEditBarCode.Focus();
                    //DateTime date2 = Convert.ToDateTime(AttendanceDate.DateTime);
                }
            }
        }
        private void Insert()
        {
            StudentAttendanceMaster STM = new StudentAttendanceMaster();
            {
                STM.DayOfWeekId = Convert.ToInt32(searchLookUpDay.EditValue);
                STM.EmplyeeId = Convert.ToInt32(searchLookUpUser.EditValue);
                STM.StudentAttendanceDate = Convert.ToDateTime(dateEditLeave.EditValue);
                STM.EmpUser = MainForm.user;

            }

            db.StudentAttendanceMasters.InsertOnSubmit(STM);
            db.SubmitChanges();


            foreach (DataRow row in LeavingData.Rows)
            {

                StudentAttendanceDetail STD = new StudentAttendanceDetail();
                {
                    STD.StudentAttendanceMasterId = STM.StudentAttendanceMasterId;
                    STD.StudentId = Convert.ToInt32(row["StudentId"]);
                    STD.ClassId = Convert.ToInt32(row["ClassId"]);
                    STD.GradeId = Convert.ToInt32(row["GradeId"]);
                    STD.IsLeaving = Convert.ToBoolean(row["IsLeaving"]);
                    STD.DateOfLeaving = Convert.ToDateTime(row["DateOfLeaving"]);
                }
                db.StudentAttendanceDetails.InsertOnSubmit(STD);
                db.SubmitChanges();
            }

            MessageBox.Show("تم حفظ إنصراف اليوم", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
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
    }
}