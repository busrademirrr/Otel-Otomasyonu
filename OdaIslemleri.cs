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
using DevExpress.XtraEditors;  // Gelişmiş editörler için
using DevExpress.XtraGrid;     // GridView bileşeni için
using DevExpress.XtraBars;     // Ribbon ve menü araçları için


namespace OtelMaster11
{
    public partial class OdaIslemleri : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");

        public OdaIslemleri()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Ekle();
                MessageBox.Show("Oda başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Ekle()
        {
            // Veritabanı bağlantısını aç
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO Rooms (RoomNumber, RoomType, IsOccupied, IsClean, Price, Floor, Description) " +
                                            "VALUES (@roomnumber, @roomtype, @isoccupied, @isclean, @price, @floor, @description)", conn);

            // Parametreleri ekle
            cmd.Parameters.AddWithValue("@roomnumber", textEdit1.Text);
            cmd.Parameters.AddWithValue("@roomtype", comboBoxEdit3.Text); // SelectedItem yerine Text
            cmd.Parameters.AddWithValue("@isoccupied", comboBoxEdit1.Text);
            cmd.Parameters.AddWithValue("@isclean", comboBoxEdit2.Text);
            cmd.Parameters.AddWithValue("@price", decimal.TryParse(textEdit4.Text, out decimal price) ? price : 0); // Geçersiz sayı için 0 atanır
            cmd.Parameters.AddWithValue("@floor", int.TryParse(textEdit3.Text, out int floor) ? floor : 0);
            cmd.Parameters.AddWithValue("@description", richTextBox1.Text);

            // Sorguyu çalıştır
            cmd.ExecuteNonQuery();

            // Bağlantıyı kapat
            conn.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Listele();
        }

        public void Listele()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Rooms", conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                gridControl1.DataSource = ds.Tables[0];

                // Formu temizle
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            textEdit1.Text = string.Empty;
            comboBoxEdit3.SelectedIndex = -1; // ComboBox seçimlerini sıfırla
            comboBoxEdit1.SelectedIndex = -1;
            comboBoxEdit2.SelectedIndex = -1;
            textEdit4.Text = string.Empty;
            textEdit3.Text = string.Empty;
            richTextBox1.Text = string.Empty;
        }

        private void OdaIslemleri_Load(object sender, EventArgs e)
        {
            Listele();
        }
        private bool isUpdating = false; // Güncelleme modunu takip etmek için bir değişken
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Güncelle
            if (!isUpdating)
            {
                // Güncelleme moduna geçiş
                if (gridView1.GetFocusedRowCellValue("RoomID") == null)
                {
                    MessageBox.Show("Lütfen bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bilgileri doldur
                BilgiCek();
                simpleButton2.Text = "Seçili Veriyi Güncelle"; // Buton metnini değiştir
                isUpdating = true; // Güncelleme moduna geç
            }
            else
            {
                // Güncelleme işlemini gerçekleştir
                Guncelle();
                Listele(); // Güncellemeden sonra listeyi yenile
                simpleButton2.Text = "Güncelle"; // Buton metnini eski haline getir
                isUpdating = false; // Güncelleme modundan çık
            }
        }
        public void BilgiCek()
        {
            try
            {
                comboBoxEdit3.EditValue = gridView1.GetFocusedRowCellValue("RoomType")?.ToString();
                textEdit1.Text = gridView1.GetFocusedRowCellValue("RoomNumber")?.ToString();
                comboBoxEdit1.EditValue = gridView1.GetFocusedRowCellValue("IsOccupied")?.ToString();
                comboBoxEdit2.EditValue = gridView1.GetFocusedRowCellValue("IsClean")?.ToString();
                textEdit4.Text = gridView1.GetFocusedRowCellValue("Price")?.ToString();
                textEdit3.Text = gridView1.GetFocusedRowCellValue("Floor")?.ToString();
                richTextBox1.Text = gridView1.GetFocusedRowCellValue("Description")?.ToString();



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
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Rooms SET " +
                    "RoomNumber = @roomnumber, " +
                    "RoomType = @roomtype, " +
                    "IsOccupied = @isoccupied, " +
                    "IsClean = @isclean, " +
                    "Price = @price, " +
                    "Floor = @floor, " +
                    "Description = @description " +
                    "WHERE RoomID = @id", conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@roomnumber", textEdit1.Text);
                cmd.Parameters.AddWithValue("@roomtype", comboBoxEdit3.Text);
                cmd.Parameters.AddWithValue("@isoccupied", comboBoxEdit1.Text);
                cmd.Parameters.AddWithValue("@isclean", comboBoxEdit2.Text);
                cmd.Parameters.AddWithValue("@price", decimal.TryParse(textEdit4.Text, out decimal price) ? price : 0);
                cmd.Parameters.AddWithValue("@floor", int.TryParse(textEdit3.Text, out int floor) ? floor : 0);
                cmd.Parameters.AddWithValue("@description", richTextBox1.Text);
                cmd.Parameters.AddWithValue("@id", gridView1.GetFocusedRowCellValue("RoomID"));

                // Sorguyu çalıştır
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Güncelleme işlemi başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();
        }
        private void Sil()
        {
            try
            {
                if (gridView1.GetFocusedRowCellValue("RoomID") != null)
                {
                    string id = gridView1.GetFocusedRowCellValue("RoomID").ToString();
                    DialogResult result = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Rooms WHERE RoomID = @id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Oda başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Listele();
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir oda seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {

            string searchValue = searchControl1.Text;

            if (string.IsNullOrEmpty(searchValue))
            {
                gridView1.ActiveFilterString = string.Empty; // Filtreyi temizle
            }
            else
            {
                // Birden fazla sütunda arama yap
                gridView1.ActiveFilterString = string.Format("[RoomType] LIKE '%{0}%' OR [IsOccupied] LIKE '%{0}%' OR [Price] LIKE '%{0}%'", searchValue);
            }


        }

      
    }
}

