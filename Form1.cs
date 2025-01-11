using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OtelMaster11
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string k_adi = textEdit1.Text.Trim();
            string sifre = textEdit2.Text.Trim();
            bool kayitliMi = false;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM login", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (k_adi == dr["K_Adi"].ToString() && sifre == dr["Sifre"].ToString())
                    {
                        kayitliMi = true;
                        break;
                    }
                }
                dr.Close(); // DataReader'ı kapatmayı unutmayın.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return; // Hata oluşursa işlem burada durur.    
            }
            finally
            {
                conn.Close(); // Bağlantıyı kapatmayı unutmayın.
            }

            // Giriş kontrolü
            if (kayitliMi)
            {
                AnaForm anf = new AnaForm();
                anf.Show();
                this.Hide();
                MessageBox.Show("Giriş Başarılı");
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
 
        }


        private void btnForgotPassword_Click_1(object sender, EventArgs e)
        {

            // SifreSifirla formunu aç
            SifreSifirlama sifreSifirlaForm = new SifreSifirlama();
            sifreSifirlaForm.Show();
            this.Hide(); // Login formunu gizle
        }
    }
}
