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
    public partial class TestForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public TestForm()
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
                    var getdata = db.Tests.Where(a => a.TestId == Code).FirstOrDefault();
                    getdata.TestDesc = Convert.ToString(textEditTest.Text);
                    getdata.GradeId = Convert.ToInt32(searchLookUpEditGrade.EditValue);
                    getdata.SubjectId = Convert.ToInt32(searchLookUpEditSubject.EditValue);
                    getdata.MajorDegree = Convert.ToString(textEditMajorDegree.EditValue);
                    getdata.MinorDegree = Convert.ToString(textEditMinorDegree.EditValue);

                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Fill();

                }

                else
                {

                    Test te = new Test();
                    te.TestDesc = Convert.ToString(textEditTest.Text);
                    te.GradeId = Convert.ToInt32(searchLookUpEditGrade.EditValue);
                    te.SubjectId = Convert.ToInt32(searchLookUpEditSubject.EditValue);
                    te.MajorDegree = Convert.ToString(textEditMajorDegree.EditValue);
                    te.MinorDegree =  Convert.ToString(textEditMinorDegree.EditValue);

                    db.Tests.InsertOnSubmit(te);
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


        void Fill()
        {
            searchLookUpEditGrade.Properties.DataSource = db.Grades.Where(a=>a.GradeIsActive==true).ToList();
            searchLookUpEditSubject.Properties.DataSource = db.Subjects.Where(o=>o.SubjectIsActive==true).ToList();
            
        }


        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
        void Clear()
        {
            textEditTest.Text = null;
            searchLookUpEditGrade.EditValue = null;
            searchLookUpEditSubject.EditValue = null;
            textEditMajorDegree.Text = "0";
            textEditMinorDegree.Text = "0";
            Fill();
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
           
            InsertOrUpdate();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            Fill();
            if (IsEdit)
            {
                var Edit = db.Tests.Where(a => a.TestId == Code).FirstOrDefault();

                textEditTest.Text = Edit.TestDesc;
                searchLookUpEditGrade.EditValue = Edit.GradeId.ToString();
                searchLookUpEditSubject.EditValue = Edit.SubjectId.ToString();
                textEditMajorDegree.EditValue = Edit.MajorDegree;
                textEditMinorDegree.EditValue = Edit.MinorDegree;
            
            }
        }
    }
}