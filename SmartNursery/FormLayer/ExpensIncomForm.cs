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
    public partial class ExpensIncomForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public ExpensIncomForm()
        {
            InitializeComponent();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
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
                    var data= db.ExpensesAndIncomeItems.Where(a => a.ExpensesAndIncomeItemsId == Code).FirstOrDefault();
                    data.ExpensesAndIncomeItemsDesc = textEdit1.Text;
                    data.DiscountAndAdditionId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    data.ExpensesAndIncomeItemsIsActive = Convert.ToBoolean(checkEdit1.Checked);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");

                    textEdit1.Text = "";
                    searchLookUpEdit1.EditValue = null;
                    checkEdit1.Checked = true;
                }
                
                else
                {
                    ExpensesAndIncomeItem ex = new ExpensesAndIncomeItem();
                    ex.ExpensesAndIncomeItemsDesc = textEdit1.Text;
                    ex.DiscountAndAdditionId = Convert.ToInt32(searchLookUpEdit1.EditValue);
                    ex.ExpensesAndIncomeItemsIsActive = Convert.ToBoolean(checkEdit1.Checked);
                    db.ExpensesAndIncomeItems.InsertOnSubmit(ex);
                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");

                    textEdit1.Text = "";
                    searchLookUpEdit1.EditValue = null;
                    checkEdit1.Checked = true;
                }

            }
            catch
            {
                MessageBox.Show("لم يتم الحفظ");
                return;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            textEdit1.Text = "";
            searchLookUpEdit1.EditValue = null;
            checkEdit1.Checked = true;
        }

        private void ExpensIncomForm_Load(object sender, EventArgs e)
        {
            searchLookUpEdit1.Properties.DataSource = db.DiscountAndAdditions.ToList();
            if (IsEdit)
            {
                var Edit = db.ExpensesAndIncomeItems.Where(a => a.ExpensesAndIncomeItemsId == Code).FirstOrDefault();
                textEdit1.Text = Edit.ExpensesAndIncomeItemsDesc;
                searchLookUpEdit1.EditValue = Edit.DiscountAndAdditionId;
                checkEdit1.Checked = Convert.ToBoolean(Edit.ExpensesAndIncomeItemsIsActive);
            }
        }
    }
}