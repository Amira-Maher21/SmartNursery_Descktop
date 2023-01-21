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
    public partial class SubjectForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public SubjectForm()
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
                    var getdata = db.Subjects.Where(a => a.SubjectId == Code).FirstOrDefault();
                    getdata.SubjectName = Convert.ToString(textEdiSubject.Text);
                    getdata.SubjectIsActive = Convert.ToBoolean(checkEdit.Checked);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Clear();
                }
                else
                {
                    Subject su = new Subject();
                    su.SubjectName = Convert.ToString(textEdiSubject.Text);
                    su.SubjectIsActive = Convert.ToBoolean(checkEdit.Checked);
                    db.Subjects.InsertOnSubmit(su);
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

        private void SubjectForm_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                var Edit = db.Subjects.Where(a => a.SubjectId == Code).FirstOrDefault();

                textEdiSubject.Text = Edit.SubjectName;
                checkEdit.Checked = Convert.ToBoolean(Edit.SubjectIsActive);


            }
        }
        void Clear()
        {

            textEdiSubject.Text = null;
            checkEdit.Checked = true;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}