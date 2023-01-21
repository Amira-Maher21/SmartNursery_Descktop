using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SmartNursery.DatabaseLayer;
using SecuretyModule;
using SmartNursery.FormLayer;
using DevExpress.XtraNavBar;
using DevExpress.XtraReports.UI;

namespace SmartNursery
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        DataClassesDataContext db = new DataClassesDataContext ();
        MainClass objMain = new MainClass();

        public static int EmpId = 0;
        public static string user = string.Empty;
        enum FormMode
        {
            classf, grad, Student, Test, floor, Subject,safe,bank, EmployeeFlag, Employee, expenIncm, dailyExp,
            deductAndAdd

        }
        FormMode formMode;
        

        public MainForm()
        {
            InitializeComponent();

            new SecuretyModule.Login().ShowDialog();
            if (SecuretyModule.UserInformation.UserName == null)
            {
                System.Environment.Exit(1);
            }
            EmpId = objMain.UserLogin(SecuretyModule.UserInformation.UserName);
            var us = db.Employees.Where(a => a.EmplyeeId == EmpId).FirstOrDefault();
            user = Convert.ToString(us.EmployeeUserName);
            gridView1.Columns.Clear();
            navBarGroup1.Expanded = false;
            navBarGroup2.Expanded = false;
            navBarGroup3.Expanded = false;
            navBarGroup4.Expanded = false;
            navBarGroup5.Expanded = false;
            barButtonItem2.Enabled = false;
            if (user=="admin")
            {
                barButtonItem5.Enabled = true;
            }
            else
            {
                barButtonItem5.Enabled = false;
            }
            Security();
        }

        public void Security()
        {

            if (UserInformation.CheckAction("البيانات الأساسية"))
            {
                navBarGroup1.Visible = true;

                if (UserInformation.CheckAction("الفصول"))
                {
                    navBarItem1.Visible = true;
                }
                else
                {
                    navBarItem1.Visible = false;
                }
                if (UserInformation.CheckAction("الدور"))
                {
                    navBarItem19.Visible = true;
                }
                else
                {
                    navBarItem19.Visible = false;
                }
                if (UserInformation.CheckAction("المواد الدراسية"))
                {
                    navBarItem20.Visible = true;
                }
                else
                {
                    navBarItem20.Visible = false;
                }
                if (UserInformation.CheckAction("المراحل الدراسيه"))
                {
                    navBarItem2.Visible = true;
                }
                else
                {
                    navBarItem2.Visible = false;
                }
                if (UserInformation.CheckAction("الاختبارات"))
                {
                    navBarItem3.Visible = true;
                }
                else
                {
                    navBarItem3.Visible = false;
                }

            }
            else
            {
                navBarGroup1.Visible = false;
            }
            if (UserInformation.CheckAction("الطلاب"))
            {
                navBarGroup2.Visible = true;

                if (UserInformation.CheckAction("بيانات الطلاب"))
                {
                    navBarItem4.Visible = true;
                }
                else
                {
                    navBarItem4.Visible = false;
                }
                if (UserInformation.CheckAction("تسكين الطلاب للفصول "))
                {
                    navBarItem5.Visible = true;
                }
                else
                {
                    navBarItem5.Visible = false;
                }
                if (UserInformation.CheckAction("مدفوعات الطلاب"))
                {
                    navBarItem6.Visible = true;
                }
                else
                {
                    navBarItem6.Visible = false;
                }
                if (UserInformation.CheckAction("تسجيل حضور الطلاب"))
                {
                    navBarItem7.Visible = true;
                }
                else
                {
                    navBarItem7.Visible = false;
                }
                if (UserInformation.CheckAction("تسجيل درجات الطلاب"))
                {
                    navBarItem8.Visible = true;
                }
                else
                {
                    navBarItem8.Visible = false;
                }

            }
            else
            {
                navBarGroup2.Visible = false;
            }
            if (UserInformation.CheckAction("شئون العاملين"))
            {
                navBarGroup3.Visible = true;

                if (UserInformation.CheckAction("بيانات العاملين"))
                {
                    navBarItem9.Visible = true;
                }
                else
                {
                    navBarItem9.Visible = false;
                }
                if (UserInformation.CheckAction("تصنيف العاملين"))
                {
                    navBarItem10.Visible = true;
                }
                else
                {
                    navBarItem10.Visible = false;
                }
                if (UserInformation.CheckAction("الباتش"))
                {
                    navBarItem11.Visible = true;
                }
                else
                {
                    navBarItem11.Visible = false;
                }
                if (UserInformation.CheckAction("مرتبات العاملين"))
                {
                    navBarItem12.Visible = true;
                }
                else
                {
                    navBarItem12.Visible = false;
                }
                if (UserInformation.CheckAction("بنود الإستقطاع والإضافي"))
                {
                    navBarItem22.Visible = true;
                }
                else
                {
                    navBarItem22.Visible = false;
                }
                if (UserInformation.CheckAction("إضافات وإستقطاعات المرتب"))
                {
                    navBarItem23.Visible = true;
                }
                else
                {
                    navBarItem23.Visible = false;
                }

            }
            else
            {
                navBarGroup3.Visible = false;
            }
            if (UserInformation.CheckAction("الخزائن والبنوك"))
            {
                navBarGroup4.Visible = true;

                if (UserInformation.CheckAction("الخزائن"))
                {
                    navBarItem13.Visible = true;
                }
                else
                {
                    navBarItem13.Visible = false;
                }
                if (UserInformation.CheckAction("البنوك"))
                {
                    navBarItem14.Visible = true;
                }
                else
                {
                    navBarItem14.Visible = false;
                }
                if (UserInformation.CheckAction("حسابات الخزائن والبنوك"))
                {
                    navBarItem15.Visible = true;
                }
                else
                {
                    navBarItem15.Visible = false;
                }
                if (UserInformation.CheckAction("تحويلات الخزائن"))
                {
                    navBarItem16.Visible = true;
                }
                else
                {
                    navBarItem16.Visible = false;
                }

            }
            else
            {
                navBarGroup4.Visible = false;
            }

            if (UserInformation.CheckAction("المصاريف والايرادات"))
            {
                navBarGroup5.Visible = true;

                if (UserInformation.CheckAction("المصاريف والإيرادات اليومية"))
                {
                    navBarItem17.Visible = true;
                }
                else
                {
                    navBarItem17.Visible = false;
                }
                if (UserInformation.CheckAction("بنود المصاريف والإيرادات"))
                {
                    navBarItem18.Visible = true;
                }
                else
                {
                    navBarItem18.Visible = false;
                }
                if (UserInformation.CheckAction("الامان"))
                {
                    barButtonItem5.Enabled = true;
                }
                else
                {
                    barButtonItem5.Enabled = false;
                }
                if (UserInformation.CheckAction("التقارير"))
                {
                    barButtonItem1.Enabled = true;
                }
                else
                {
                    barButtonItem1.Enabled = false;
                }

            }
            else
            {
                navBarGroup5.Visible = false;
            }
        }
        private void Fill()
        {
            gridView1.Columns.Clear();

            switch (formMode)
            {
                case FormMode.classf:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.classf();
                    gridView1.GroupPanelText = "الفصول";
                    break;
                case FormMode.grad:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.GradeData();
                    gridView1.GroupPanelText = "المراحل الدراسيه";
                    break;
                case FormMode.Student:
                    barButtonItem2.Enabled = true;
                    gridControl1.DataSource = objMain.Student();
                    gridView1.GroupPanelText = "بيانات الطلاب";
                    break;
                case FormMode.Test:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.Test();
                    gridView1.GroupPanelText = "الاختبارات";
                    break;
                case FormMode.floor:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.floor();
                    gridView1.GroupPanelText = "الدور";
                    break;
                case FormMode.Subject:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.Subject();
                    gridView1.GroupPanelText = "المواد الدراسية";
                    break;
                case FormMode.safe:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.safe();
                    gridView1.GroupPanelText = "الخزن";
                    break;
                case FormMode.bank:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.bank();
                    gridView1.GroupPanelText = "البنوك";
                    break;
                case FormMode.EmployeeFlag:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.EmployeeFlag();
                    gridView1.GroupPanelText = " بيانات العاملين";
                    break;
                case FormMode.Employee:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.Employee();
                    gridView1.GroupPanelText = "تصنيف العاملين";
                    break;
                case FormMode.expenIncm:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.ExpenIncome();
                    gridView1.GroupPanelText = "بنود المصروفات والإيرادات";
                    break;
                case FormMode.dailyExp:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.DailyExpenIncome();
                    gridView1.GroupPanelText = "المصروفات والإيرادات اليومية";
                    break;
                case FormMode.deductAndAdd:
                    barButtonItem2.Enabled = false;
                    gridControl1.DataSource = objMain.EmpDeductAndAdd();
                    gridView1.GroupPanelText = "بنود الاستقطاعات والإضافات";
                    break;
            }

        }

        

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

           
                switch (formMode)
                {
                    case FormMode.classf:
                        new ClassForm().ShowDialog();
                        break;
                     case FormMode.grad:
                         new GradeForm().ShowDialog();
                         break;
                     case FormMode.Student:
                         new StudentForm().ShowDialog();
                         break;
                     case FormMode.Test:
                         new TestForm().ShowDialog();
                         break;
                     case FormMode.floor:
                         new FloorForm1().ShowDialog();
                         break;
                case FormMode.Subject:
                    new SubjectForm().ShowDialog();
                    break;
                case FormMode.safe:
                    new SafesForm() { Flag = 1 }.ShowDialog();
                    break;
                case FormMode.bank:
                    new SafesForm() { Flag = 2 }.ShowDialog();
                    break;
                case FormMode.EmployeeFlag:
                    new EmployeeFlagForm().ShowDialog();
                    break;
                case FormMode.Employee:
                    new EmployeeForm().ShowDialog();
                    break;
                case FormMode.expenIncm:
                    new ExpensIncomForm().ShowDialog();
                    break;
                case FormMode.dailyExp:
                    new DailyExpensIncmForm().ShowDialog();
                    break;
                case FormMode.deductAndAdd:
                    new EmpAddDeductForm().ShowDialog();
                    break;
            }
            
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void BtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int code = 0;

                switch (formMode)
                {
                    case FormMode.classf:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ClassId"));
                        new ClassForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.grad:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "GradeId"));
                        new GradeForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.Student:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentId"));
                        new StudentForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.Test:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TestId"));
                        new TestForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.floor:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "FloorId"));
                        new FloorForm1() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.Subject:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SubjectId"));
                        new SubjectForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.safe:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SafeId"));
                        new SafesForm() { IsEdit = true, Code = code , Flag = 1 }.ShowDialog();
                        break;
                    case FormMode.bank:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SafeId"));
                        new SafesForm() { IsEdit = true, Code = code, Flag = 2 }.ShowDialog();
                        break;
                    case FormMode.EmployeeFlag:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmployeeFlagId"));
                        new EmployeeFlagForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.Employee:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmplyeeId"));
                        new EmployeeForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.expenIncm:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ExpensesAndIncomeItemsId"));
                        new ExpensIncomForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                    case FormMode.deductAndAdd:
                        code = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EmpAddAndDeductItemsId"));
                        new EmpAddDeductForm() { IsEdit = true, Code = code }.ShowDialog();
                        break;
                }
            }
           
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.classf;
            Fill();
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
            formMode = FormMode.Student;
            Fill();
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.grad;
            Fill();
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.Test;
            Fill();
        }

        private void navBarItem19_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            formMode = FormMode.floor;
            Fill();
        }

        private void navBarItem20_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.Subject;
            Fill();
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.safe;
            Fill();
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formMode = FormMode.bank;
            Fill();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                db.HaveBackup();

                MessageBox.Show("تم أخذ نسخة من البيانات بنجاح");
            }
            catch
            {
                MessageBox.Show("خطأ برجاء مراجعة الدعم الفنى");

            }
        }
          
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SecuretyModule.Forms.MainSecureForm sec = new SecuretyModule.Forms.MainSecureForm();
            sec.ShowDialog();
        }

        private void BtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.ShowPrintPreview();
        }

        private void navBarControl1_GroupExpanded(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            foreach (var item in navBarControl1.Groups)
            {
                if ((item as NavBarGroup).GetHashCode() != e.Group.GetHashCode())
                    (item as NavBarGroup).Expanded = false;
            }
        }

        private void navBarItem9_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            formMode = FormMode.Employee;
            Fill();
        }

        private void navBarItem10_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            formMode = FormMode.EmployeeFlag;
            Fill();
        }

        private void navBarItem5_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new StudentClassForm().ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // int id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentId"));
            new Reports.ReportCardPrint2().ShowPreview();
        }

        private void navBarItem7_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new StudentAttendanceForm().ShowDialog();
        }

        private void navBarItem18_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            formMode = FormMode.expenIncm;
            Fill();
        }

        private void navBarItem17_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            formMode = FormMode.dailyExp;
            Fill();
        }

        private void navBarItem8_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new StuLeavingForm().ShowDialog();
        }

        private void navBarItem21_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new StuDegreeForm().ShowDialog();
        }

        private void navBarItem22_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            formMode = FormMode.deductAndAdd;
            Fill();
        }

        private void navBarItem5_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            new EmpAttendanceForm().ShowDialog();
        }

        private void navBarItem11_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new EmpLeaveForm().ShowDialog();
        }

        private void navBarItem6_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new StudPaymentForm().ShowDialog();
        }

        private void navBarItem12_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new EmpSalaryForm().ShowDialog();
        }

        private void navBarItem16_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new SafeTransferredForm().ShowDialog();
        }

        private void navBarItem15_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            new TreasuryPaymentForm().ShowDialog();
        }
    }
}