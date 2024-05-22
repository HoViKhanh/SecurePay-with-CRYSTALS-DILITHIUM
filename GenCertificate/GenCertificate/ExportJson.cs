using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Text.RegularExpressions;
namespace GenCertificate
{
    public partial class ExportJson : Form
    {
        public ExportJson()
        {
            InitializeComponent();
        }
        public class Invoice
        {
            public Person BuyerInfo { get; set; }
            public Person SellerInfo { get; set; }
            public Order OrderInfo { get; set; }
        }
        public class Person
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
        public class Item
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice => Quantity * UnitPrice;
        }
        public class Order
        {
            public string OrderId { get; set; }
            public string Date { get; set; }
            public List<Item> Items { get; set; }
            public decimal TotalAmount { get; private set; }

            public void CalculateTotalAmount()
            {
                foreach (var item in Items)
                {
                    TotalAmount += item.TotalPrice;
                }
            }
        }
        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
        public static bool IsPhoneNumber(string number)
        {
            return Regex.IsMatch(number, @"^[0-9]{6,12}$");
        }

        private void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuyerName.Text) ||
                string.IsNullOrWhiteSpace(txtBuyerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtBuyerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtBuyerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSellerName.Text) ||
                string.IsNullOrWhiteSpace(txtSellerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtSellerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtSellerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem ít nhất một sản phẩm đã được nhập vào DataGridView chưa
            if (dataGridViewItems.Rows.Count == 0 || dataGridViewItems.Rows[0].Cells["ProductName"].Value == null)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TimeSpan vietnamOffset = TimeSpan.FromHours(7);
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            DateTime vietnamTime = utcNow.ToOffset(vietnamOffset).DateTime;
            string formattedTime = vietnamTime.ToString("dd-MM-yyyy HH:mm");
            //bool allRowsValid = true;
            if (!IsEmail(txtSellerEmail.Text) & !IsEmail(txtSellerEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsPhoneNumber(txtBuyerPhone.Text) || !IsPhoneNumber(txtSellerPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var invoice = new Invoice
            {

                BuyerInfo = new Person
                {
                    Name = txtBuyerName.Text,
                    Address = txtBuyerAddress.Text,
                    Phone = txtBuyerPhone.Text,
                    Email = txtBuyerEmail.Text
                },
                SellerInfo = new Person
                {
                    Name = txtSellerName.Text,
                    Address = txtSellerAddress.Text,
                    Phone = txtSellerPhone.Text,
                    Email = txtSellerEmail.Text
                },
                OrderInfo = new Order
                {
                    OrderId = txtOrderId.Text,
                    Date = formattedTime,
                    Items = new List<Item>()
                }
            };

            foreach (DataGridViewRow row in dataGridViewItems.Rows)
            {
                if (row.IsNewRow) continue;
                //        if (row.IsNewRow || row.Cells["ProductName"].Value == null ||
                //row.Cells["Quantity"].Value == null || row.Cells["UnitPrice"].Value == null)
                //        {
                //            allRowsValid = false;
                //            break;
                //        }
                //if (row.Cells["ProductName"].Value != null /*& row.Cells["Quantity"].Value != null & row.Cells["UnitPrice"].Value != null*/)
                //{
                //    var item = new Item
                //    {
                //        ProductName = row.Cells["ProductName"].Value.ToString(),
                //        Quantity = int.Parse(row.Cells["Quantity"].Value.ToString()),
                //        UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value.ToString())
                //    };
                //    invoice.OrderInfo.Items.Add(item);
                //} else
                //{
                //    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm bao gồm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                if (row.Cells["ProductName"].Value == null || string.IsNullOrEmpty(row.Cells["ProductName"].Value.ToString()) ||
        row.Cells["Quantity"].Value == null || string.IsNullOrEmpty(row.Cells["Quantity"].Value.ToString()) ||
        row.Cells["UnitPrice"].Value == null || string.IsNullOrEmpty(row.Cells["UnitPrice"].Value.ToString()))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm gồm Tên,Số lượng và Giá sản phẩm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var item = new Item
                {
                    ProductName = row.Cells["ProductName"].Value.ToString(),
                    Quantity = int.Parse(row.Cells["Quantity"].Value.ToString()),
                    UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value.ToString())
                };
                invoice.OrderInfo.Items.Add(item);
            }
            invoice.OrderInfo.CalculateTotalAmount();

            // Xuất hóa đơn thành file JSON
            string json = JsonConvert.SerializeObject(invoice, Formatting.Indented);
            File.WriteAllText("information.json", json);

            MessageBox.Show("Hóa đơn đã được lưu thành công vào file information.json", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGenerateInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGenerateInvoice.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }

        private void ExportJson_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += btnGenerateInvoice_KeyDown;
        }
    }
}
