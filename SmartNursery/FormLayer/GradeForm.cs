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
    public partial class GradeForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public GradeForm()
        {
            InitializeComponent();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            try
            {
                if (IsEdit)
                {
                    var getdata = db.Grades.Where(a => a.GradeId == Code).FirstOrDefault();
                    getdata.GradeDec = Convert.ToString(textEditGradeDec.Text);
                    getdata.GradeIsActive = Convert.ToBoolean(checkEdit1.Checked);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");

                    textEditGradeDec.EditValue = "";
                    checkEdit1.Checked = true;

                }


                else
                {
                    Grade gr = new Grade();
                    gr.GradeDec = Convert.ToString(textEditGradeDec.EditValue);
                    gr.GradeIsActive = Convert.ToBoolean(checkEdit1.Checked);
                    db.Grades.InsertOnSubmit(gr);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");

                    textEditGradeDec.EditValue = "";
                    checkEdit1.Checked = true;
                }

            }
            catch 
            {

                MessageBox.Show("لم يتم الحفظ");
                return;
            }               
            
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void Grade_Load(object sender, EventArgs e)
        {
            checkEdit1.Checked = true;
            if (IsEdit)
            {
                var Edit = db.Grades.Where(a => a.GradeId == Code).FirstOrDefault();
                textEditGradeDec.Text = Edit.GradeDec;
                checkEdit1.Checked =Convert.ToBoolean (Edit.GradeIsActive);
            }
      
          
            }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            textEditGradeDec.EditValue = "";
            checkEdit1.Checked = true;
        }
    }
}