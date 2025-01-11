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
    public partial class HotelExpenses : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public HotelExpenses()
        {
            InitializeComponent();
        }

        private void HotelExpenses_Load_1(object sender, EventArgs e)
        {
            LoadExpenses();
        }
        // Giderleri listeleme metodu
        void LoadExpenses()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM GelirGider", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl1.DataSource = dt; // DataGridView adı
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
       

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // Seçili satırdaki ID'yi al
                object selectedIdObj = gridView1.GetFocusedRowCellValue("ID");

                if (selectedIdObj == null)
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.");
                    return;
                }

                int selectedId = Convert.ToInt32(selectedIdObj);

                // Veritabanına bağlan ve silme işlemini gerçekleştir
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM GelirGider WHERE ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", selectedId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Gider başarıyla silindi!");
                LoadExpenses(); // Tabloyu yeniden yükleme işlemi
            }
            catch (FormatException)
            {
                MessageBox.Show("Geçerli bir ID seçilemedi. Lütfen doğru bir satır seçin.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //Ekle
            // Gider Ekle
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO GelirGider (Tarih, GiderTuru, Tutar, Aciklama) VALUES (@Tarih, @GiderTuru, @Tutar, @Aciklama)", conn);
                cmd.Parameters.AddWithValue("@Tarih", dateTimePicker1.Value); // Tarih için
                cmd.Parameters.AddWithValue("@GiderTuru", comboBoxEdit1.Text); // Gider Türü için
                cmd.Parameters.AddWithValue("@Tutar", decimal.Parse(textEdit1.Text)); // Tutar için
                cmd.Parameters.AddWithValue("@Aciklama", richTextBox1.Text); // Açıklama için
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Gider başarıyla eklendi!");
                LoadExpenses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
