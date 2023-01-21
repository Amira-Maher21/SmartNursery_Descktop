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
    public partial class StudentAttendanceForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        DataTable AttendanceData = new DataTable();
        
        public StudentAttendanceForm()
        {
            InitializeComponent();
            createdatatable();
        }
       
        
        protected void createdatatable()
        {
            AttendanceData = new DataTable();
            AttendanceData.Columns.Add("StudentId", typeof(int));
            AttendanceData.Columns.Add("GradeId", typeof(int));
            AttendanceData.Columns.Add("ClassId", typeof(int));
            AttendanceData.Columns.Add("IsAttend", typeof(Boolean));
            AttendanceData.Columns.Add("Date", typeof(DateTime));
            gridControl1.DataSource = AttendanceData;
        }
        private void Fill()
        {
            DayOfWeekId.Properties.DataSource = db.DayOfWeeks;
            EmplyeeId.Properties.DataSource = db.Employees.Where(a => a.EmpoyeeIsActive == true);
            AttendanceDate.DateTime = DateTime.Now;
            repositoryClassId.DataSource = db.ClassTablees.ToList();
            repositoryGradeId.DataSource = db.Grades.ToList();
            repositoryStudentId.DataSource = db.Students.ToList();
        }
        void GetDate(DateTime dateValue)
        {
            dateValue = AttendanceDate.DateTime;
            if (dateValue.ToString("ddd") == "Sat")
            { DayOfWeekId.EditValue = 1; }
            if (dateValue.ToString("ddd") == "Sun")
            { DayOfWeekId.EditValue = 2; }
            if (dateValue.ToString("ddd") == "Mon")
            { DayOfWeekId.EditValue = 3; }
            if (dateValue.ToString("ddd") == "Tue")
            { DayOfWeekId.EditValue = 4; }
            if (dateValue.ToString("ddd") == "Wed")
            { DayOfWeekId.EditValue = 5; }
            if (dateValue.ToString("ddd") == "Thu")
            { DayOfWeekId.EditValue = 6; }
            if (dateValue.ToString("ddd") == "Fri")
            { DayOfWeekId.EditValue = 7; }
        }
        private void AttendanceDate_EditValueChanged(object sender, EventArgs e)
        {
            GetDate(AttendanceDate.DateTime);
        }

        private void StudentAttendanceForm_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            gridView1.FocusedRowHandle = 1;
            if (e.KeyCode == Keys.Enter)
            {
                  int Student;
                  int Class;
                  int Gread;
                  DateTime date;
                  Boolean IsAttendd;
                  string barCode = BarCode.Text;
                  var data = db.Students.Where(a => a.StudentBarCode == barCode).FirstOrDefault();
                  try
                  {
                      var check = db.StudentAttendanceDetails.Where(u => u.DateOfAttend.Value.Date == AttendanceDate.DateTime.Date && u.IsAttend == true).ToList();
                      if (check.Any())
                      {
                          DialogResult result = MessageBox.Show("تم تسجيل حضور الطا من قبل هل تريد التعديل على التسجيل؟", "تنبيه", MessageBoxButtons.YesNo);
                          if (result == DialogResult.Yes)
                          {
                              var Update = db.StudentAttendanceDetails.Where(u => u.DateOfAttend.Value.Date == AttendanceDate.DateTime.Date && u.StudentId == Convert.ToInt32(data.StudentId)).First();
                               Update.IsAttend = false;
                               db.SubmitChanges();

                            if (data != null)
                            {
                                Student = Convert.ToInt32(data.StudentId);
                                Class = Convert.ToInt32(data.ClassId);
                                date = DateTime.Now;
                                IsAttendd = true;
                                var GreadId1 = db.ClassTablees.Where(a => a.ClassId == Class).FirstOrDefault();
                                Gread = Convert.ToInt32(GreadId1.GradeId);
                                AttendanceData.Rows.Add(Student, Class, Gread, IsAttendd, date);
                                gridView1.FocusedRowHandle = -1;
                                BarCode.Text = "";
                                BarCode.Focus();
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
                    IsAttendd = true;
                    var GreadId1 = db.ClassTablees.Where(a => a.ClassId == Class).FirstOrDefault();
                    Gread = Convert.ToInt32(GreadId1.GradeId);
                    AttendanceData.Rows.Add(Student, Class, Gread, IsAttendd, date);
                    BarCode.Text = "";
                    BarCode.Focus();
                    //DateTime date2 = Convert.ToDateTime(AttendanceDate.DateTime);
                }
            }
        }



        private void Insert()
        {
            StudentAttendanceMaster STM = new StudentAttendanceMaster();
            {
                STM.DayOfWeekId = Convert.ToInt32(DayOfWeekId.EditValue);
                STM.EmplyeeId = Convert.ToInt32(EmplyeeId.EditValue);
                STM.StudentAttendanceDate = Convert.ToDateTime(AttendanceDate.EditValue);
                STM.EmpUser = MainForm.user;

            } 
            
            db.StudentAttendanceMasters.InsertOnSubmit(STM);
            db.SubmitChanges();

            
            foreach (DataRow row in AttendanceData.Rows)
            {

                StudentAttendanceDetail STD = new StudentAttendanceDetail();
                {
                    STD.StudentAttendanceMasterId = STM.StudentAttendanceMasterId;
                    STD.StudentId = Convert.ToInt32(row["StudentId"]);
                    STD.ClassId = Convert.ToInt32(row["ClassId"]);
                    STD.GradeId = Convert.ToInt32(row["GradeId"]);
                    STD.IsAttend = Convert.ToBoolean(row["IsAttend"]);
                    STD.DateOfAttend = Convert.ToDateTime(row["Date"]);
                }
                db.StudentAttendanceDetails.InsertOnSubmit(STD);
                db.SubmitChanges();
            }

            MessageBox.Show("تم حفظ حضور اليوم", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void butDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            gridView1.DeleteRow(gridView1.FocusedRowHandle);
        }

        private void BarCode_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}