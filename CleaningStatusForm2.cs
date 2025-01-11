using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OtelMaster11
{
    public partial class CleaningStatusForm2 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public CleaningStatusForm2()
        {
            InitializeComponent();
        }

        private void CleaningStatusForm2_Load(object sender, EventArgs e)
        {
            ListeleOdalar();
            LoadEmployees();

            // "Temiz" ve "Kirli" öğelerini comboBoxEdit2'ye ekliyoruz
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.Add("Temiz");
            comboBoxEdit2.Properties.Items.Add("Kirli");

        }
        private void InitializeComboBox()
        {
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.Add("Temiz");
            comboBoxEdit2.Properties.Items.Add("Kirli");
        }
        private void LoadEmployees()
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT EmployeeID FROM Employees", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxEdit3.Properties.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBoxEdit3.Properties.Items.Add(row["EmployeeID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Çalışanlar yüklenirken bir hata oluştu.", ex);
            }
        }

        private void ListeleOdalar(string filter = null)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Rooms WHERE IsClean = 'Kirli'", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                ShowError("Kirli odalar listelenirken bir hata oluştu.", ex);
            }
        }

        private void BilgiCek()
        {
            try
            {
                if (gridView1.GetFocusedRowCellValue("RoomID") == null)
                {
                    MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                comboBoxEdit2.Text = gridView1.GetFocusedRowCellValue("IsClean")?.ToString(); // "Temiz" veya "Kirli"
                comboBoxEdit3.Text = gridView1.GetFocusedRowCellValue("AssignedEmployeeID")?.ToString();
                dateTimePicker1.Value = DateTime.TryParse(gridView1.GetFocusedRowCellValue("LastCleanedDate")?.ToString(), out DateTime dateValue)
                    ? dateValue
                    : DateTime.Now;
            }
            catch (Exception ex)
            {
                ShowError("Bilgiler alınırken bir hata oluştu.", ex);
            }
        }

        private void Guncelle()
        {
            try
            {
                // Seçili satır kontrolü
                if (gridView1.GetFocusedRowCellValue("RoomID") == null)
                {
                    MessageBox.Show("Lütfen güncellemek için bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // RoomID alınması
                int roomID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("RoomID"));

                // "IsClean" bilgisi "Temiz" veya "Kirli" olarak belirleniyor
                string isClean = comboBoxEdit2.Text;

                // Görevli kontrolü
                if (!int.TryParse(comboBoxEdit3.Text, out int assignedEmployeeID))
                {
                    MessageBox.Show("Geçerli bir görevli seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // "LastCleanedDate" alınması
                DateTime lastCleanedDate = dateTimePicker1.Value;

                // SQL UPDATE işlemi
                using (SqlCommand cmd = new SqlCommand("UPDATE Rooms SET IsClean = @isClean, LastCleanedDate = @lastCleanedDate, AssignedEmployeeID = @assignedEmployeeID WHERE RoomID = @roomID", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@isClean", isClean); // "Temiz" veya "Kirli" değerini yazıyoruz
                    cmd.Parameters.AddWithValue("@lastCleanedDate", lastCleanedDate); // Temizlik tarihi
                    cmd.Parameters.AddWithValue("@assignedEmployeeID", assignedEmployeeID); // Görevli ID
                    cmd.Parameters.AddWithValue("@roomID", roomID); // Oda ID

                    cmd.ExecuteNonQuery();
                }

                // Bilgileri mesaj kutusunda gösterme
                string message = $"Oda Güncellendi!\n" +
                                 $"- Oda ID: {roomID}\n" +
                                 $"- Temizlik Durumu: {isClean}\n" +
                                 $"- Temizlik Tarihi: {lastCleanedDate.ToShortDateString()}\n" +
                                 $"- Görevli ID: {assignedEmployeeID}";

                MessageBox.Show(message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi yeniden yükleyerek güncellenen veriyi listeden çıkarıyoruz
                ListeleOdalar();
                Temizle();
            }
            catch (Exception ex)
            {
                ShowError("Güncelleme sırasında bir hata oluştu.", ex);
            }
            finally
            {
                conn.Close();
            }
        }


        private void UpdateButtonText()
        {
            if (simpleButton1.Text == "Güncelle")
            {
                simpleButton1.Text = "Seçili Veriyi Güncelle";
            }
            else
            {
                simpleButton1.Text = "Güncelle";
            }
        }

        private void Temizle()
        {
            comboBoxEdit2.SelectedIndex = -1;
            comboBoxEdit3.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}\nHata Detayı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void btnListeleKirliOdalar_Click(object sender, EventArgs e)
        {
            ListeleOdalar();
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit2.SelectedItem != null)
            {
                string selectedStatus = comboBoxEdit2.SelectedItem.ToString();
                ListeleOdalar(selectedStatus);
            }
        }
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            BilgiCek();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Guncelle();
        }
    }

       
}
