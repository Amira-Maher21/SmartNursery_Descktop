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
    public partial class ExpensPrintForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        DataTable GridData = new DataTable();
        public ExpensPrintForm()
        {
            InitializeComponent();
            createdatatable();
        }

         void createdatatable()
        {
            GridData = new DataTable();
            GridData.Columns.Add("StudentBarCode");
            GridData.Columns.Add("StudentName");
            GridData.Columns.Add("TotalPaidAfterDeduction", typeof(decimal));
            GridData.Columns.Add("PrintNumber", typeof(int));
            
        }
        void FillGrid()
        {
            var data = db.Students.ToList();
            foreach (var item in data)
            {
                GridData.Rows.Add(item.StudentBarCode, item.StudentName, item.TotalPaidAfterDeduction, 0);
            }
            gridControl1.DataSource = GridData;
        }

        private void ExpensPrintForm_Load(object sender, EventArgs e)
        {
            searchLookUpEdit1.Properties.DataSource = db.SalaryPatches.Where(a => a.DocTypeId == 8).ToList();
            FillGrid();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}