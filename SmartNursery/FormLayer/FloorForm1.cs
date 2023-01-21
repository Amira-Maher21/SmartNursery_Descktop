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
    public partial class FloorForm1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public FloorForm1()
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
                    var getdata = db.Floors.Where(a => a.FloorId == Code).FirstOrDefault();
                    getdata.FloorDesc = Convert.ToString(textEditFloor.Text);
                    getdata.FloorIsActive = Convert.ToBoolean(checkEdit.Checked);
                    getdata.EmplyeeId = Convert.ToInt32(searchLookUpEditEmplyee.EditValue);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    Clear();
                }
                else
                {
                    Floor fl = new Floor();
                    fl.FloorDesc = Convert.ToString(textEditFloor.Text);
                    fl.FloorIsActive = Convert.ToBoolean(checkEdit.Checked);
                    fl.EmplyeeId = Convert.ToInt32(searchLookUpEditEmplyee.EditValue);
                    db.Floors.InsertOnSubmit(fl);
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
        void Clear()
        {
            textEditFloor.Text = null;
            checkEdit.Checked = true;
            Fill();

        }
        void Fill()
        {
            searchLookUpEditEmplyee.Properties.DataSource = db.Employees.Where(g => g.EmpoyeeIsActive == true && g.EmployeeFlagId==2).ToList();

            
        }

        private void FloorForm1_Load(object sender, EventArgs e)
        {
            Fill();

            if (IsEdit)
            {
                var Edit = db.Floors.Where(a => a.FloorId == Code).FirstOrDefault();
                textEditFloor.Text = Edit.FloorDesc;
                searchLookUpEditEmplyee.EditValue = Edit.EmplyeeId;
                checkEdit.Checked = true;
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}