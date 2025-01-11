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
using DevExpress.XtraBars;     // Ribbon ve menü araçları için
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;


namespace OtelMaster11
{
    public partial class MusteriEkleme : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public MusteriEkleme()
        {
            InitializeComponent();
        }

        private void MusteriEkleme_Load(object sender, EventArgs e)
        {
            Listele(); // Form yüklendiğinde sadece listeleme yapılır
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // Ekleme işlemini butona bağlayın
            try
            {
                Ekle();
                MessageBox.Show("Müşteri başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele(); // Yeni veriyi listeye eklemek için tekrar listeleyin
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Ekle()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Customers (FirstName, LastName, Phone, Email, DateOfBirth, Address) " +
                "VALUES (@firstname, @lastname, @phone, @email, @dateofbirth, @address)", conn);

            // Parametreleri ekleyin
            cmd.Parameters.AddWithValue("@firstname", textEdit1.Text);
            cmd.Parameters.AddWithValue("@lastname", textEdit2.Text);
            cmd.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
            cmd.Parameters.AddWithValue("@email", textEdit4.Text);
            cmd.Parameters.AddWithValue("@dateofbirth", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@address", textEdit5.Text);

            // Sorguyu çalıştırın
            cmd.ExecuteNonQuery();
            conn.Close(); // Bağlantıyı kapatmayı unutmayın
        }


        private bool isUpdating = false; // Güncelleme modunu takip etmek için bir değişken

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                // Güncelleme moduna geçiş
                if (gridView1.GetFocusedRowCellValue("CustomerID") == null)
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
                textEdit1.Text = gridView1.GetFocusedRowCellValue("FirstName")?.ToString();
                textEdit2.Text = gridView1.GetFocusedRowCellValue("LastName")?.ToString();
                maskedTextBox1.Text = gridView1.GetFocusedRowCellValue("Phone")?.ToString();
                textEdit4.Text = gridView1.GetFocusedRowCellValue("Email")?.ToString();
                textEdit5.Text = gridView1.GetFocusedRowCellValue("Address")?.ToString();

                // DateTimePicker için
                if (gridView1.GetFocusedRowCellValue("DateOfBirth") != null)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("DateOfBirth"));
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
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Customers SET " +
                    "FirstName = @firstname, " +
                    "LastName = @lastname, " +
                    "Phone = @phone, " +
                    "Email = @email, " +
                    "DateOfBirth = @dateofbirth, " +
                    "Address = @address " +
                    "WHERE CustomerID = @id", conn);

                // Parametreleri ekle
                cmd.Parameters.AddWithValue("@firstname", textEdit1.Text);
                cmd.Parameters.AddWithValue("@lastname", textEdit2.Text);
                cmd.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                cmd.Parameters.AddWithValue("@email", textEdit4.Text);
                cmd.Parameters.AddWithValue("@dateofbirth", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@address", textEdit5.Text);
                cmd.Parameters.AddWithValue("@id", gridView1.GetFocusedRowCellValue("CustomerID"));

                // Sorguyu çalıştır
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Güncelleme işlemi başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close(); // Hata durumunda bağlantıyı kapatmayı unutmayın
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
                // Seçili satır ve sütun kontrolü
                if (gridView1.GetFocusedRowCellValue("CustomerID") != null)
                {
                    string id = gridView1.GetFocusedRowCellValue("CustomerID").ToString();

                    // Kullanıcıdan onay al
                    DialogResult result = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes) // Eğer kullanıcı 'Evet' derse silme işlemini yap
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Müşteri başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Listele(); // Güncel listeyi yükleyin
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Listele
            Listele();
        }
        public void Listele()
        {
            string komut = "select* from Customers";
            SqlDataAdapter da = new SqlDataAdapter(komut, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
            textEdit1.Text = " ";
            textEdit2.Text = " ";
            maskedTextBox1.Text = " ";
            textEdit4.Text = " ";
            textEdit5.Text = " ";
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            Listele();
        }
    }
}
