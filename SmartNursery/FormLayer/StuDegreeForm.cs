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
    public partial class StuDegreeForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public int Code { get; set; }
        public bool IsEdit { get; set; }
        public DataTable subjectDataTable = new DataTable();
        public StuDegreeForm()
        {
            InitializeComponent();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void StuDegreeForm_Load(object sender, EventArgs e)
        {
            searchLookUpUser.Properties.DataSource = db.Employees.ToList();
            searchLookUpUser.EditValue = MainForm.EmpId;
            searchLookUpClass.Properties.DataSource = db.ClassTablees.ToList();
            searchLookUpTest.Properties.DataSource = db.Tests.ToList();
            searchLookUpMonth.Properties.DataSource = db.TestMonths.Where(a=> a.TestMonthIsClosed == false).ToList();
        }
        void GridData(int masterId)
        {
            var data = from a in db.DegreeDetails
                       join st in db.Students on a.StudentId equals st.StudentId
                       where a.DegreeMasterId == masterId
                       select new { a.DegreeDetailId, a.DegreeMasterId, a.StudentId, st.StudentName, a.Mark };
            gridControl1.DataSource = data;
        }
        void SubjectDataTableCreate()
        {
            subjectDataTable = new DataTable();
            subjectDataTable.Columns.Add("StudentId");
            subjectDataTable.Columns.Add("TestId");

            subjectDataTable.Columns.Add("SubjectId1");
            subjectDataTable.Columns.Add("SubjectId2");
            subjectDataTable.Columns.Add("SubjectId3");
            subjectDataTable.Columns.Add("SubjectId4");
            subjectDataTable.Columns.Add("SubjectId5");
            subjectDataTable.Columns.Add("SubjectId6");
            subjectDataTable.Columns.Add("SubjectId7");
            subjectDataTable.Columns.Add("SubjectId8");
            subjectDataTable.Columns.Add("SubjectId9");
            subjectDataTable.Columns.Add("SubjectId10");
            subjectDataTable.Columns.Add("SubjectId11");
            subjectDataTable.Columns.Add("SubjectId12");
            subjectDataTable.Columns.Add("SubjectId13");
            subjectDataTable.Columns.Add("SubjectId14");
            subjectDataTable.Columns.Add("SubjectId15");
            subjectDataTable.Columns.Add("SubjectId16");
            subjectDataTable.Columns.Add("SubjectId17");
            subjectDataTable.Columns.Add("SubjectId18");
            subjectDataTable.Columns.Add("SubjectId19");
            subjectDataTable.Columns.Add("SubjectId20");
            subjectDataTable.Columns.Add("SubjectId21");
            subjectDataTable.Columns.Add("SubjectId22");
            subjectDataTable.Columns.Add("SubjectId23");
            subjectDataTable.Columns.Add("SubjectId24");
            subjectDataTable.Columns.Add("SubjectId25");
            subjectDataTable.Columns.Add("SubjectId26");
            subjectDataTable.Columns.Add("SubjectId27");

            subjectDataTable.Columns.Add("Mark");
            subjectDataTable.Columns.Add("EmpUser");
        }
        private void searchLookUpClass_EditValueChanged(object sender, EventArgs e)
        {
            int classID = Convert.ToInt32(searchLookUpClass.EditValue);
            searchLookUpStud.Properties.DataSource = db.Students.Where(a => a.ClassId == classID).ToList();
        }

        private void searchLookUpTest_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpTest.EditValue != null)
            {
                int testID = Convert.ToInt32(searchLookUpTest.EditValue);
                var degree = db.Tests.Where(a => a.TestId == testID).FirstOrDefault();
                textBigDegree.Text = degree.MajorDegree;
                textSmallDegree.Text = degree.MinorDegree;
            }
           
        }
        int degreeMasterId = 0;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            try
            {
                DegreeMaster mas = new DegreeMaster();
                mas.EmpUser = Convert.ToInt32(searchLookUpUser.EditValue);
                mas.TestId = Convert.ToInt32(searchLookUpTest.EditValue);
                mas.TestMonthId = Convert.ToInt32(searchLookUpMonth.EditValue);
                db.DegreeMasters.InsertOnSubmit(mas);
                db.SubmitChanges();
                MessageBox.Show("تم الحفظ");
                degreeMasterId = mas.DegreeMasterId;
                GridData(degreeMasterId);
                if (checkEditCloseMonth.Checked)
                {
                    TestMonth edit = db.TestMonths.Where(a => a.TestMonthId == Convert.ToInt32(searchLookUpMonth.EditValue)).FirstOrDefault();
                    edit.TestMonthIsClosed = true;
                    db.SubmitChanges();
                }
                simpleButton2.Enabled = false;
            }
            catch
            {
                MessageBox.Show("لم يتم الحفظ");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (degreeMasterId > 0)
            {
                if (!dxValidationProvider2.Validate())
                {
                    return;
                }
                if (!IsEdit)
                {
                    try
                    {
                        DegreeDetail det = new DegreeDetail();
                        det.DegreeMasterId = degreeMasterId;
                        det.Mark = textMainDegree.Text;
                        det.StudentId = Convert.ToInt32(searchLookUpStud.EditValue);
                        Image img = pictureEdit1.Image;
                        ImageConverter imgCo = new ImageConverter();
                        byte[] img2Arry = (byte[])imgCo.ConvertTo(img, typeof(byte[]));
                        det.TestPicture = img2Arry;
                        db.DegreeDetails.InsertOnSubmit(det);
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");

                    }
                    catch
                    {
                        MessageBox.Show("لم يتم الحفظ");
                    }
                }
                else
                {
                    try
                    {
                        DegreeDetail edit = db.DegreeDetails.Where(a => a.DegreeDetailId == Code).FirstOrDefault();
                        edit.Mark = textMainDegree.Text;
                        edit.StudentId = Convert.ToInt32(searchLookUpStud.EditValue);
                        Image img = pictureEdit1.Image;
                        ImageConverter imgCo = new ImageConverter();
                        byte[] img2Arry = (byte[])imgCo.ConvertTo(img, typeof(byte[]));
                        edit.TestPicture = img2Arry;
                        db.SubmitChanges();
                        MessageBox.Show("تم الحفظ");

                    }
                    catch
                    {
                        MessageBox.Show("لم يتم الحفظ");
                    }
                }
                Code = 0;
                IsEdit = false;
            }
            else
            {
                MessageBox.Show("من فضلك تأكد من حفظ بيانات الاختبار أولا");
            }
        }
        string loadedPath;
        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;*.png;*.ico)|*.jpg; *.jpeg; *.gif; *.bmp;*.png;*.ico";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image loadedImage = Image.FromFile(ofd.FileName);
                loadedPath = ofd.FileName;
                pictureEdit1.Image = loadedImage;
                pictureEdit1.Tag = ofd.FileName;
            }
        }

        private void searchLookUpMonth_EditValueChanged(object sender, EventArgs e)
        {
            int test = Convert.ToInt32(searchLookUpTest.EditValue);
            int month = Convert.ToInt32(searchLookUpMonth.EditValue);
            var data = db.DegreeMasters.Where(a => a.TestId == test && a.TestMonthId == month).FirstOrDefault();
            if (data!= null)
            {
                GridData(data.DegreeMasterId);
                degreeMasterId = data.DegreeMasterId;
            }
        }
        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            int detId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DegreeDetailId"));
            int stuId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "StudentId"));
            string mark = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Mark"));
            var details = db.DegreeDetails.Where(d => d.DegreeDetailId == detId).FirstOrDefault();
            var data = db.Students.Where(a => a.StudentId == stuId).FirstOrDefault();
            int classId = Convert.ToInt32(data.ClassId);

            searchLookUpClass.EditValue = classId;
            searchLookUpStud.EditValue = stuId;
            textMainDegree.Text = mark;
            pictureEdit1.EditValue = details.TestPicture;
            IsEdit = true;
            Code = detId;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            IsEdit = false;
            Code = 0;
            simpleButton2.Enabled = true;
            searchLookUpClass.EditValue = null;
            searchLookUpMonth.EditValue = null;
            searchLookUpStud.EditValue = null;
            searchLookUpTest.EditValue = null;
            textBigDegree.Text = null;
            textMainDegree.Text = null;
            textSmallDegree.Text = null;
            checkEditCloseMonth.Checked = false;
            pictureEdit1.EditValue = null;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            new TestMonthForm().ShowDialog();
            searchLookUpMonth.Properties.DataSource = db.TestMonths.ToList();
        }
    }
}