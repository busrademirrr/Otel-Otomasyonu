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
using DevExpress.XtraEditors; 
using DevExpress.XtraTab;

namespace OtelMaster11
{
    public partial class Personel : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public Personel()
        {
            InitializeComponent();
        }

        private void Personel_Load(object sender, EventArgs e)
        {
            Listele(); // Form yüklendiğinde sadece listeleme yapılır
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Ekle
            try
            {
                Ekle();
                MessageBox.Show("Personel başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele(); // Yeni veriyi listeye eklemek için tekrar listeleyin
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Ekle()
        {
            try
            {
                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
                    string.IsNullOrWhiteSpace(textEdit2.Text) ||
                    string.IsNullOrWhiteSpace(maskedTextBox1.Text) ||
                    string.IsNullOrWhiteSpace(textEdit3.Text) ||
                    comboBoxEdit1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Employees (FirstName, LastName, PhoneNumber, Email, BirthDate, Position, HireDate) " +
                    "VALUES (@firstname, @lastname, @phonenumber, @email, @birthdate, @position, @hiredate)", conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@firstname", textEdit1.Text.Trim());
                cmd.Parameters.AddWithValue("@lastname", textEdit2.Text.Trim());
                cmd.Parameters.AddWithValue("@phonenumber", maskedTextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@email", textEdit3.Text.Trim());
                cmd.Parameters.AddWithValue("@birthdate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@position", comboBoxEdit1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@hiredate", dateTimePicker2.Value);

                // Sorguyu çalıştır
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Personel başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele(); // Listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();
        }
        private void Sil()
        {
            try
            {
                if (gridView1.GetFocusedRowCellValue("EmployeeID") != null)
                {
                    string id = gridView1.GetFocusedRowCellValue("EmployeeID").ToString();

                    DialogResult result = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Personel başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Listele(); // Listeyi güncelle
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private bool isUpdating = false; // Güncelleme modunu takip etmek için bir değişken

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Güncelle
            if (!isUpdating)
            {
                // Güncelleme moduna geçiş
                if (gridView1.GetFocusedRowCellValue("EmployeeID") == null)
                {
                    MessageBox.Show("Lütfen bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bilgileri doldur
                BilgiCek();
                simpleButton3.Text = "Seçili Veriyi Güncelle"; // Buton metnini değiştir
                isUpdating = true; // Güncelleme moduna geç
            }
            else
            {
                // Güncelleme işlemini gerçekleştir
                Guncelle();
                Listele(); // Güncellemeden sonra listeyi yenile
                simpleButton3.Text = "Güncelle"; // Buton metnini eski haline getir
                isUpdating = false; // Güncelleme modundan çık
            }
        }
        public void BilgiCek()
        {
            try
            {
                textEdit1.Text = gridView1.GetFocusedRowCellValue("FirstName")?.ToString();
                textEdit2.Text = gridView1.GetFocusedRowCellValue("LastName")?.ToString();
                maskedTextBox1.Text = gridView1.GetFocusedRowCellValue("PhoneNumber")?.ToString();
                textEdit3.Text = gridView1.GetFocusedRowCellValue("Email")?.ToString();
                comboBoxEdit1.EditValue = gridView1.GetFocusedRowCellValue("Position")?.ToString();

                // BirthDate kontrolü
                if (DateTime.TryParse(gridView1.GetFocusedRowCellValue("BirthDate")?.ToString(), out DateTime birthDate))
                {
                    dateTimePicker1.Value = birthDate;
                }
                else
                {
                    dateTimePicker1.Value = DateTimePicker.MinimumDateTime; // Veya istediğiniz varsayılan bir tarih
                }

                // HireDate kontrolü
                if (DateTime.TryParse(gridView1.GetFocusedRowCellValue("HireDate")?.ToString(), out DateTime hireDate))
                {
                    dateTimePicker2.Value = hireDate;
                }
                else
                {
                    dateTimePicker2.Value = DateTimePicker.MinimumDateTime; // Veya istediğiniz varsayılan bir tarih
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bilgiler alınırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Guncelle()
        {
            try
            {
                if (gridView1.GetFocusedRowCellValue("EmployeeID") == null)
                {
                    MessageBox.Show("Lütfen güncellemek için bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
                    string.IsNullOrWhiteSpace(textEdit2.Text) ||
                    string.IsNullOrWhiteSpace(maskedTextBox1.Text) ||
                    string.IsNullOrWhiteSpace(textEdit3.Text) ||
                    comboBoxEdit1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Employees SET " +
                    "FirstName = @firstname, " +
                    "LastName = @lastname, " +
                    "PhoneNumber = @phonenumber, " +
                    "Email = @email, " +
                    "BirthDate = @birthdate, " +
                    "Position = @position, " +
                    "HireDate = @hiredate " +
                    "WHERE EmployeeID = @id", conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@firstname", textEdit1.Text.Trim());
                cmd.Parameters.AddWithValue("@lastname", textEdit2.Text.Trim());
                cmd.Parameters.AddWithValue("@phonenumber", maskedTextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@email", textEdit3.Text.Trim());
                cmd.Parameters.AddWithValue("@birthdate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@position", comboBoxEdit1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@hiredate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@id", gridView1.GetFocusedRowCellValue("EmployeeID"));

                // Sorguyu çalıştır
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Güncelleme işlemi başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele(); // Listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }



        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Listele
            Listele();
        }
        public void Listele()
        {
            try
            {
                string komut = "SELECT * FROM Employees";
                SqlDataAdapter da = new SqlDataAdapter(komut, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                gridControl1.DataSource = ds.Tables[0];

                // Alanları sıfırla
                textEdit1.Text = string.Empty;
                textEdit2.Text = string.Empty;
                textEdit3.Text = string.Empty;
                maskedTextBox1.Text = string.Empty;
                comboBoxEdit1.SelectedIndex = -1; // Seçimi sıfırla
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
