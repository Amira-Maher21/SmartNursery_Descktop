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
    public partial class EmpAddDeductForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public EmpAddDeductForm()
        {
            InitializeComponent();
        }

        private void EmpAddDeductForm_Load(object sender, EventArgs e)
        {
            searchLookUpEdit1.Properties.DataSource = db.DiscountAndAdditions.ToList();
            if (IsEdit)
            {
                var data = db.EmpAddAndDeductItems.Where(a => a.EmpAddAndDeductItemsId == Code).FirstOrDefault();
                textEdit1.Text = data.EmpAddAndDeductItemsDesc;
                searchLookUpEdit1.EditValue = data.DiscountAndAdditionId;
                checkEdit1.EditValue = data.IsActive;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            else
            {
                if (IsEdit)
                {
                    var edit = db.EmpAddAndDeductItems.Where(a => a.EmpAddAndDeductItemsId == Code).FirstOrDefault();
                    edit.EmpAddAndDeductItemsDesc = textEdit1.Text;
                    edit.DiscountAndAdditionId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    edit.IsActive = checkEdit1.Checked;
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                }
                else
                {

                    EmpAddAndDeductItem em = new EmpAddAndDeductItem();
                    em.DiscountAndAdditionId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    em.EmpAddAndDeductItemsDesc = textEdit1.Text;
                    em.IsActive = checkEdit1.Checked;
                    db.EmpAddAndDeductItems.InsertOnSubmit(em);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                }
                IsEdit = false;
                Code = 0;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            IsEdit = false;
            Code = 0;
            textEdit1.Text = null;
            searchLookUpEdit1.EditValue = null;
            checkEdit1.Checked = true;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}