using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ontap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(@"D:\tích hợp\ontap\ontap\congty.xml");
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void btnDocFile_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MaNV");
            table.Columns.Add("HoTen");
            table.Columns.Add("Tuoi");
            table.Columns.Add("Luong");
            table.Columns.Add("Xa");
            table.Columns.Add("Huyen");
            table.Columns.Add("Tinh");
            table.Columns.Add("DienThoai");

            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\tích hợp\ontap\ontap\congty.xml");

            XmlNodeList nhanvienList = doc.SelectNodes("/congty/nhanvien");
            foreach (XmlNode node in nhanvienList)
            {
                string manv = node.Attributes["manv"].Value;
                string hoten = node["hoten"].InnerText;
                string tuoi = node["tuoi"].InnerText;
                string luong = node["luong"].InnerText;

                XmlNode diachiNode = node["diachi"];
                string xa = diachiNode["xa"].InnerText;
                string huyen = diachiNode["huyen"].InnerText;
                string tinh = diachiNode["tinh"].InnerText;

                string dienthoai = node["dienthoai"].InnerText;

                table.Rows.Add(manv, hoten, tuoi, luong, xa, huyen, tinh, dienthoai);
            }

            dataGridView1.DataSource = table;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\tích hợp\ontap\ontap\congty.xml");

            XmlElement nv = doc.CreateElement("nhanvien");
            nv.SetAttribute("manv", txtMaNV.Text);

            nv.InnerXml = $@"
                <hoten>{txtHoTen.Text}</hoten>
                <tuoi>{txtTuoi.Text}</tuoi>
                <luong>{txtLuong.Text}</luong>
                <diachi>
                    <xa>{txtXa.Text}</xa>
                    <huyen>{txtHuyen.Text}</huyen>
                    <tinh>{txtTinh.Text}</tinh>
                </diachi>
                <dienthoai>{txtDienThoai.Text}</dienthoai>";

            doc.DocumentElement.AppendChild(nv);
            doc.Save(@"D:\tích hợp\ontap\ontap\congty.xml");
            btnDocFile_Click(null, null);
            MessageBox.Show("Đã thêm nhân viên!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\tích hợp\ontap\ontap\congty.xml");

            XmlNode node = doc.SelectSingleNode($"/congty/nhanvien[@manv='{txtMaNV.Text}']");
            if (node != null)
            {
                node.SelectSingleNode("hoten").InnerText = txtHoTen.Text;
                node.SelectSingleNode("tuoi").InnerText = txtTuoi.Text;
                node.SelectSingleNode("luong").InnerText = txtLuong.Text;
                node.SelectSingleNode("diachi/xa").InnerText = txtXa.Text;
                node.SelectSingleNode("diachi/huyen").InnerText = txtHuyen.Text;
                node.SelectSingleNode("diachi/tinh").InnerText = txtTinh.Text;
                node.SelectSingleNode("dienthoai").InnerText = txtDienThoai.Text;

                doc.Save(@"D:\tích hợp\ontap\ontap\congty.xml");
                LoadData();
                MessageBox.Show("Đã sửa nhân viên!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xoá dòng này không?",
                    "Xác nhận xoá",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Nếu chọn Yes thì xoá
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\tích hợp\ontap\ontap\congty.xml");

            XmlNode node = doc.SelectSingleNode($"/congty/nhanvien[@manv='{txtMaNV.Text}']");
            if (node != null)
            {
                doc.DocumentElement.RemoveChild(node);
                doc.Save(@"D:\tích hợp\ontap\ontap\congty.xml");
                LoadData();
                MessageBox.Show("Đã xóa nhân viên!");
            }
        }

        private void btnTimLuong_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(@"D:\tích hợp\ontap\ontap\congty.xml");

            var filtered = ds.Tables[0].AsEnumerable()
                .Where(r => Convert.ToInt32(r["luong"]) > 1000);

            if (filtered.Any())
            {
                DataTable result = filtered.CopyToDataTable();
                dataGridView1.DataSource = result;

                int tongLuong = filtered.Sum(r => Convert.ToInt32(r["luong"]));
                MessageBox.Show($"Tổng lương: {tongLuong}");
            }
        }

        private void btnTimTinh_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\tích hợp\ontap\ontap\congty.xml");

            XmlNodeList nodes = doc.SelectNodes("/congty/nhanvien[diachi/tinh='Hà Nội']");
            int count = nodes.Count;
            MessageBox.Show($"Có {count} nhân viên ở tỉnh Hà Nội");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
