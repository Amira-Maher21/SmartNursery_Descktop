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
    public partial class StudentClassForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public StudentClassForm()
        {
            InitializeComponent();
        }

        private void StudentClassIdForm_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void Fill()
        {
            repositoryItemSearchStudentId.DataSource = db.Students.Where(a => a.ISActive == true);
            ClassId.Properties.DataSource = db.ClassTablees.Where(a => a.ClassIsActive == true).ToList();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
    }
}