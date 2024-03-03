using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLDSV_TC
{
    public partial class frmChinh : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmChinh()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            
            
            this.MAGV.Text = "MÃ GV: " + Program.username;
            this.HOTEN.Text ="HỌ TÊN: " +Program.mHoten;
            this.NHOM.Text ="NHÓM: "  + Program.mGroup;
            Console.WriteLine(this.NHOM.Text);

            if (Program.mGroup == "SV")
            {
                rpQuanLy.Visible = false;
                rbBaoCao.Visible = false;
                this.MAGV.Text = "MÃ SV: " + Program.username;
            }
            
        }
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(frmDangKyMon));
            if (frm != null) frm.Activate();
            else
            {
                frmDangKyMon f = new frmDangKyMon();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(frmLopHoc));
            if (frm != null) frm.Activate();
            else
            {
                frmLopHoc f = new frmLopHoc();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(frmSinhVien));
            if (frm != null) frm.Activate();
            else
            {
                frmSinhVien f = new frmSinhVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(frmMonHoc));
            if (frm != null) frm.Activate();
            else
            {
                frmMonHoc f = new frmMonHoc();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}