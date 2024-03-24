using DevExpress.XtraEditors;
using System;
using System.Collections;
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
    public partial class frmSinhVien : DevExpress.XtraEditors.XtraForm
    {

        string macn = "";
        int vitri = 0;
        private string _flagOptionSinhVien;
        private string _oldMaSV = "";
        private string _oldTenLop = "";
        System.Collections.Stack undoList = new Stack();
        private bool dangthemmoi = false;
        private bool dangsua = false;
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void lOPBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsLOP.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            dataSet.EnforceConstraints = false;
            this.LOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.LOPTableAdapter.Fill(this.dataSet.LOP);

            this.SINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.SINHVIENTableAdapter.Fill(this.dataSet.SINHVIEN);

            this.DANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.DANGKYTableAdapter.Fill(this.dataSet.DANGKY);

            macn = ((DataRowView)bdsLOP[0])["MAKHOA"].ToString();
            cbKhoa.DataSource = Program.bds_dspm;
            cbKhoa.DisplayMember = "TENKHOA";
            cbKhoa.ValueMember = "TENSERVER";
            cbKhoa.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "KHOA")
            {
                cbKhoa.Enabled = false;
            }
            panel2.Enabled = false;
            btnHuy.Enabled = false;
            txbMaLop.Enabled = false;
            btnGhi.Enabled = false; 


        }

        private bool validatorSinhVien()
        {
            if (txbMaSV.Text.Trim() == "")
            {
                MessageBox.Show("Mã sinh viên không được thiếu!", "", MessageBoxButtons.OK);
                txbMaLop.Focus();
                return false;
            }
            if (txbHo.Text.Trim() == "")
            {
                MessageBox.Show("họ không được thiếu!", "", MessageBoxButtons.OK);
                txbHo.Focus();
                return false;
            }
            if (txbTen.Text.Trim() == "")
            {
                MessageBox.Show("Tên không được thiếu!", "", MessageBoxButtons.OK);
                txbTen.Focus();
                return false;
            }
            if (txbDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("Địa chỉ không được thiếu!", "", MessageBoxButtons.OK);
                txbDiaChi.Focus();
                return false;
            }
           
           
            if (_flagOptionSinhVien == "ADD")
            {
                string query1 = " DECLARE @return_value INT " +

                            " EXEC @return_value = [dbo].[SP_CHECKID] " +

                            " @Code = N'" + txbMaSV.Text.Trim() + "',  " +

                            " @Type = N'MASV' " +

                            " SELECT  'Return Value' = @return_value ";

                int resultMa = Program.CheckDataHelper(query1);
                if (resultMa == -1)
                {
                    XtraMessageBox.Show("Lỗi kết nối với database. Mời bạn xem lại", "", MessageBoxButtons.OK);
                    this.Close();
                }
                if (resultMa == 1)
                {
                    XtraMessageBox.Show("Mã Sinh Viên đã tồn tại. Mời bạn nhập mã khác !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (resultMa == 2)
                {
                    XtraMessageBox.Show("Mã Sinh Viên đã tồn tại ở Khoa khác. Mời bạn nhập lại !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (_flagOptionSinhVien == "UPDATE")
            {
                if (!this.txbMaSV.Text.Trim().ToString().Equals(_oldMaSV))
                {
                    string query2 = " DECLARE @return_value INT " +

                            " EXEC @return_value = [dbo].[SP_CHECKID] " +

                            " @Code = N'" + txbMaSV.Text.Trim() + "',  " +

                            " @Type = N'MASV' " +

                            " SELECT  'Return Value' = @return_value ";

                    int resultMa = Program.CheckDataHelper(query2);
                    if (resultMa == -1)
                    {
                        XtraMessageBox.Show("Lỗi kết nối với database. Mời bạn xem lại", "", MessageBoxButtons.OK);
                        this.Close();
                    }
                    if (resultMa == 1)
                    {
                        XtraMessageBox.Show("Mã Sinh Viên đã tồn tại. Mời bạn nhập mã khác !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (resultMa == 2)
                    {
                        XtraMessageBox.Show("Mã Sinh Viên đã tồn tại ở Khoa khác. Mời bạn nhập lại !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

            }
            return true;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            dangthemmoi = true;
            vitri = bdsLOP.Position;
            _flagOptionSinhVien = "ADD";
            panel2.Enabled = true;
            
         
            this.txbMaLop.Text = ((DataRowView)bdsLOP[bdsLOP.Position])["MALOP"].ToString();
            bdsSINHVIEN.AddNew();
    
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnLamMoi.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnHuy.Enabled = true;
            gcLOP.Enabled = cbKhoa.Enabled = gcSINHVIEN.Enabled = false;


        }


        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {


            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnPhucHoi.Enabled = false;
                return;
            }


            else
            {
                String undoQuery = undoList.Pop().ToString();
                Program.ExecSqlNonQuery(undoQuery);
            }

            this.SINHVIENTableAdapter.Fill(this.dataSet.SINHVIEN);
        }
        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }
        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                this.SINHVIENTableAdapter.Fill(this.dataSet.SINHVIEN);
                gcSINHVIEN.Enabled = gcLOP.Enabled = true;            
                btnPhucHoi.Enabled = btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = true;
                panel2.Enabled = btnSua.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }


        private void cbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
          if (cbKhoa.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cbKhoa.SelectedValue.ToString();
            if (cbKhoa.SelectedIndex != Program.mChinhanh && cbKhoa.SelectedIndex != 2)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.KetNoi() == 0)
            {
                if (Program.mGroup == "PGV")
                {
                    MessageBox.Show("PGV ko đc vào PKT", "", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
            }
            else if (cbKhoa.SelectedIndex != 2)
            {
                this.LOPTableAdapter.Connection.ConnectionString = Program.connstr;
                this.LOPTableAdapter.Fill(this.dataSet.LOP);

                if (bdsLOP.Count > 0)
                    macn = ((DataRowView)bdsLOP[0])["MAKHOA"].ToString();
            }
        }


        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            string masv = "";
            if (bdsDANGKY.Count > 0)
            {
                MessageBox.Show("Không thể xóa lớp học này vì đã đăng ký lớp tín chỉ", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsSINHVIEN.Count == 0)
            {
                MessageBox.Show("Chưa có sinh viên để xoá", "", MessageBoxButtons.OK);
                return;
            }


            if (MessageBox.Show("Bạn có thật sự muốn xóa sinh viên này ?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    DateTime NGAYSINH = (DateTime)((DataRowView)bdsSINHVIEN[bdsSINHVIEN.Position])["NGAYSINH"];
            
                    bool checkPhai = cbPhai.Checked;
                    int phai = checkPhai ? 1 : 0;

                    bool isChecked = cbDangNghiHoc.Checked;
                    int nghihoc = isChecked ? 1 : 0;
                    string undoQuery =
              string.Format("INSERT INTO DBO.SINHVIEN(MASV,HO,TEN,PHAI,DIACHI,NGAYSINH,MALOP,DANGHIHOC)" +
                "VALUES(N'{0}',N'{1}',N'{2}', '{3}',N'{4}',CAST('{5}' AS DATE),'{6}','{7}')", txbMaSV.Text.Trim(),txbHo.Text.Trim()
                ,txbTen.Text.Trim(),phai,txbDiaChi.Text.Trim(),NGAYSINH.ToString("yyyy-MM-dd"),txbMaLop.Text.Trim(),nghihoc);

                    Console.WriteLine(undoQuery);
                    undoList.Push(undoQuery);
                    masv = ((DataRowView)bdsSINHVIEN[bdsSINHVIEN.Position])["MASV"].ToString();
                    bdsSINHVIEN.RemoveCurrent();
                    this.SINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.SINHVIENTableAdapter.Update(this.dataSet.SINHVIEN);
                    btnPhucHoi.Enabled = btnLamMoi.Enabled = btnGhi.Enabled = true;
                }
                catch (Exception ex)
                {
                           
                    undoList.Pop();
                    MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message, "", MessageBoxButtons.OK);
                    this.LOPTableAdapter.Fill(this.dataSet.LOP);
                    bdsLOP.Position = bdsLOP.Find("MASV", masv);
                    return;
                }
            }


            if (bdsSINHVIEN.Count == 0) btnXoa.Enabled = false;
        }


        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (validatorSinhVien() == true)
            {

                DialogResult dr = MessageBox.Show("Ghi thông tin vào database?", "Thông báo",
                  MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {

                        bdsSINHVIEN.EndEdit();
                        bdsSINHVIEN.ResetCurrentItem();
                        this.SINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.SINHVIENTableAdapter.Update(this.dataSet.SINHVIEN);
                        vitri = bdsLOP.Position;
                        dangsua = false;
                        btnPhucHoi.Enabled = true;

                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi lớp học: " + ex.Message, "", MessageBoxButtons.OK);
                        Console.WriteLine(ex.Message);
                        return;
                    }
                    this.bdsSINHVIEN.Position = vitri;
                    gcSINHVIEN.Enabled =gcLOP.Enabled = true;
                    btnGhi.Enabled = btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnLamMoi.Enabled = btnPhucHoi.Enabled = true;
                    btnPhucHoi.Enabled = btnHuy.Enabled = false;
                    panel2.Enabled = dangthemmoi = false;
                }


            }
            else
            {
                return;
            }

        }


        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dangsua == true)
            {
                undoList.Pop();
                dangsua = false;
            }
            vitri = bdsSINHVIEN.Position;
            bdsSINHVIEN.CancelEdit();
            //   bdsLOP.RemoveCurrent();
            btnThem.Enabled = btnXoa.Enabled = btnLamMoi.Enabled = gcSINHVIEN.Enabled = gcLOP.Enabled = btnSua.Enabled = btnPhucHoi.Enabled = true;
            btnHuy.Enabled = panel2.Enabled = false;

            bdsSINHVIEN.Position = vitri;

        }


        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dangsua = true;

            vitri = bdsSINHVIEN.Position;
            _flagOptionSinhVien = "UPDATE";
            _oldMaSV = this.txbMaLop.Text.Trim();
            DateTime NGAYSINH = (DateTime)((DataRowView)bdsSINHVIEN[bdsSINHVIEN.Position])["NGAYSINH"];
            

            bool isChecked = cbDangNghiHoc.Checked;
            int nghihoc = isChecked ? 1 : 0;

            bool checkPhai = cbPhai.Checked;
            int phai = checkPhai ? 1 : 0;
            String undoQuery = "";
            undoQuery = "UPDATE DBO.SINHVIEN SET HO = N'" + txbHo.Text.Trim()
                +"',TEN = N'"+txbTen.Text.Trim()
                +"',PHAI ="+phai+",DIACHI =N'"+txbDiaChi.Text.Trim()
                + "',NGAYSINH = CAST('" + NGAYSINH.ToString("yyyy-MM-dd") + "' AS DATE),"+
                "MALOP='"+txbMaLop.Text.Trim()+"',DANGHIHOC = "+ nghihoc + " where masv ='" + txbMaSV + "'";
            Console.WriteLine("query" + undoQuery);
            undoList.Push(undoQuery);

            txbMaLop.Enabled = false;
            panel2.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnLamMoi.Enabled = cbKhoa.Enabled = false;
            btnGhi.Enabled = btnHuy.Enabled = true;
            gcLOP.Enabled = gcSINHVIEN.Enabled = false;
        }




    }
}