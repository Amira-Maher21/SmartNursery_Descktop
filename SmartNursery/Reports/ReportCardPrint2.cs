using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SmartNursery.Reports
{
    public partial class ReportCardPrint2 : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportCardPrint2()
        {
            InitializeComponent();
            dataTable1TableAdapter.Fill(dataSetCardPrint1.DataTable1);
        }

    }
}
