﻿using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLDSV_TC
{
    public partial class rptDanhSachLopTinChi : DevExpress.XtraReports.UI.XtraReport
    {
        public rptDanhSachLopTinChi(string nienkhoa, int hocky)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill(nienkhoa, hocky.ToString());
        }
       

    }
}
