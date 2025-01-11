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
    public partial class DestekTalepleri : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public DestekTalepleri()
        {
            InitializeComponent();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            

            // Kullanıcıdan alınan veriler
            string adSoyad = textEdit1.Text;
            string email = textEdit2.Text;
            string telefon = maskedTextBox1.Text;
            string sorunBaslik = textEdit3.Text;
            string sorunDetay = richTextBox1.Text;
            string kategori = comboBoxEdit2.SelectedItem?.ToString(); // ComboBox'dan seçili değer
            DateTime tarih = dateTimePicker1.Value; // Tarih picker'dan seçilen değer

            // Hata kontrolü (örnek)
            if (string.IsNullOrEmpty(adSoyad) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sorunBaslik))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Veritabanı işlemi
            try
            {
                conn.Open(); // Bağlantıyı aç

                string query = "INSERT INTO Support (AdSoyad, Email, Telefon, SorunBaslik, SorunDetay, Kategori, Tarih) " +
                               "VALUES (@AdSoyad, @Email, @Telefon, @SorunBaslik, @SorunDetay, @Kategori, @Tarih)";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    // Parametreler ekleniyor
                    command.Parameters.AddWithValue("@AdSoyad", adSoyad);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Telefon", telefon);
                    command.Parameters.AddWithValue("@SorunBaslik", sorunBaslik);
                    command.Parameters.AddWithValue("@SorunDetay", sorunDetay);
                    command.Parameters.AddWithValue("@Kategori", kategori ?? (object)DBNull.Value); // Null kontrol
                    command.Parameters.AddWithValue("@Tarih", tarih);

                    int result = command.ExecuteNonQuery(); // Sorguyu çalıştır

                    if (result > 0)
                    {
                        MessageBox.Show("Destek talebiniz başarıyla gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TemizleForm(); // Form alanlarını temizle
                    }
                    else
                    {
                        MessageBox.Show("Destek talebi gönderilirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close(); // Bağlantıyı kapat
            }
            GridViewVerileriGoster();
        }
        private void GridViewVerileriGoster()
        {
            try
            {
                // Veritabanı bağlantısını aç
                conn.Open();

                // Verileri getir
                string query = "SELECT * FROM Support";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // GridView'e bağla
                gridControl1.DataSource = dt; // Eğer DevExpress kullanıyorsanız
                                              // gridView1.RefreshData(); // DevExpress için gerekli olabilir (isteğe bağlı)

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Formdaki alanları temizleme metodu
        private void TemizleForm()
        {
            textEdit1.Text = string.Empty; // Ad Soyad alanını temizle
            textEdit2.Text = string.Empty; // E-posta alanını temizle
            maskedTextBox1.Text = string.Empty; // Telefon alanını temizle
            textEdit3.Text = string.Empty; // Sorun Başlığı alanını temizle
            richTextBox1.Clear(); // Sorun Detay alanını temizle
            comboBoxEdit2.SelectedIndex = -1; // Kategori seçimini temizle
            dateTimePicker1.Value = DateTime.Now; // Tarih alanını sıfırla
        }

        private void DestekTalepleri_Load(object sender, EventArgs e)
        {
            GridViewVerileriGoster();
        }
    }
}

