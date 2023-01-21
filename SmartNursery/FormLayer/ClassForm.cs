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
    public partial class ClassForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public ClassForm()
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
                    var getdata = db.ClassTablees.Where(a => a.ClassId == Code).FirstOrDefault();
                    getdata.ClassDisc = Convert.ToString(textClassDisc.Text);
                    getdata.GradeId = Convert.ToInt32(searchLookUpEditGrade.EditValue);
                    getdata.EmplyeeId = Convert.ToInt32(searchLookUpEditEmplyee.EditValue);
                    getdata.FloorId = Convert.ToInt32(searchLookUpEditFloor.EditValue);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Fill();

                }

                else
                {

                    ClassTablee cl = new ClassTablee();
                    cl.ClassDisc = textClassDisc.Text;
                    cl.GradeId = Convert.ToInt32(searchLookUpEditGrade.EditValue);
                    cl.EmplyeeId = Convert.ToInt32(searchLookUpEditEmplyee.EditValue);
                    cl.FloorId = Convert.ToInt32(searchLookUpEditFloor.EditValue);
                    db.ClassTablees.InsertOnSubmit(cl);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Fill();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("لم يتم الحفظ ");

            }
        }

        void Clear()
        {
            textClassDisc.EditValue = null;
            searchLookUpEditGrade.EditValue = null;
            searchLookUpEditEmplyee.EditValue = null;
            searchLookUpEditFloor.EditValue = null;
            checkEdit.Checked = true;
            Fill();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
        void Fill ()
        {
            searchLookUpEditGrade.Properties.DataSource = db.Grades.Where(g=>g.GradeIsActive==true).ToList();
            searchLookUpEditEmplyee.Properties.DataSource = db.Employees.Where(a=>a.EmployeeFlagId==2&&a.EmpoyeeIsActive==true).ToList();
            searchLookUpEditFloor.Properties.DataSource = db.Floors.Where(a=>a.FloorIsActive==true).ToList();
            checkEdit.Checked = true;
        }
        

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            Fill();

            if (IsEdit)
            {
                var Edit = db.ClassTablees.Where(a => a.ClassId == Code).FirstOrDefault();
                textClassDisc.Text = Edit.ClassDisc;
                searchLookUpEditEmplyee.EditValue = Edit.EmplyeeId.ToString();
                searchLookUpEditGrade.EditValue = Edit.GradeId.ToString();
                searchLookUpEditFloor.EditValue = Edit.FloorId.ToString();
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}