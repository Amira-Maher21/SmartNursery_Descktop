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
using DevExpress.XtraEditors;

namespace SecuretyModule.Forms
{
    public partial class RanksPermessions : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DB.DataClassesSecurityMouleDataContext db = new DB.DataClassesSecurityMouleDataContext();
        public bool IsEdit { get; set; }
        public int Code { get; set; }
        public RanksPermessions()
        {
            InitializeComponent();
        }
        private void FillGrid(string name)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ActionID");
            dt.Columns.Add("ActionName");
            dt.Columns.Add("ActionDetailName");
            dt.Columns.Add("isSelect", typeof(bool));
            var all = (from ac in db.ActionDetails
                       join c in db.Actions on ac.ActionID equals c.ActionID
                       where c.ActionName == name
                       select new { ac.ActionID, c.ActionName, ac.ActionDetailName }).ToList();


            foreach (var item in all)
            {
                dt.Rows.Add(item.ActionID, item.ActionName, item.ActionDetailName, false);

            }
            gridControl1.DataSource = dt;
            var useAction = from ua in db.UserActios where ua.Name == searchLookUpEdit1.EditValue.ToString() select ua;
            foreach (var item in useAction)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (item.ActionName == gridView1.GetRowCellValue(i, "ActionDetailName").ToString())
                    {
                        gridView1.SetRowCellValue(i, "isSelect", true);
                    }
                }
            }
            fill();
        }
        private void fill()
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView1.Columns)
            {
                //column.Caption = Resource1.ResourceManager.GetString(column.FieldName);

                //column.Caption = CarService.Security.Resource1.ResourceManager.GetString(column.FieldName);
            }
        }
        private void radioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEdit1.EditValue == null)
            {
                MessageBox.Show("من فضلك اختر المستخدم أولا");
            }
            else
            {
                switch (Convert.ToInt32(radioGroup1.EditValue))
                {
                    case 1:
                        FillGrid("البيانات الأساسية");
                        break;
                    case 2:
                        FillGrid("الطلاب");
                        break;
                    case 3:
                        FillGrid("شئون العاملين");
                        break;
                    case 4:
                        FillGrid("الخزائن والبنوك");
                        break;
                    case 5:
                        FillGrid("المصاريف والايرادات");
                        break;
                    case 6:
                        FillGrid("التقارير");
                        break;
                    case 7:
                        FillGrid("الامان");
                        break;
                    default:
                        break;
                }
            }
        }

        private void RanksPermessions_Load(object sender, EventArgs e)
        {

            searchLookUpEdit1.Properties.DataSource = db.Ranks; 
          
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (Convert.ToBoolean(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "isSelect")))
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "isSelect", false);
            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "isSelect", true);
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            gridView1.FocusedRowHandle = -2;
            try
            {

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(gridView1.GetRowCellValue(i, "isSelect")))
                    {
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionName").ToString());
                        db.UserActioInser(searchLookUpEdit1.EditValue.ToString(), gridView1.GetRowCellValue(i, "ActionName").ToString());
                        db.UserActioDetailInser(searchLookUpEdit1.EditValue.ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                    }
                    else
                    {
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());

                    }

                }
                XtraMessageBox.Show("تم الحفظ بنجاح", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {

                XtraMessageBox.Show("لم يتم الحفظ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (searchLookUpEdit1.EditValue == null)
            {
                MessageBox.Show("من فضلك اختر المستخدم أولا");
            }
            else
            {
                switch (Convert.ToInt32(radioGroup1.EditValue))
                {
                    case 1:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "البيانات الأساسية");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 2:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "الطلاب");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 3:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "شئون العاملين");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 4:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "الخزائن والبنوك");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 5:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "المصاريف والايرادات");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 6:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "التقارير");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;
                    case 7:
                        db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), "الامان");
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            db.UserActioDelete((searchLookUpEdit1.EditValue).ToString(), gridView1.GetRowCellValue(i, "ActionDetailName").ToString());
                            gridView1.SetRowCellValue(i, "isSelect", false);
                        }
                        break;

                    default:
                        break;
                }
                XtraMessageBox.Show("تم  الحذف", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
    }
