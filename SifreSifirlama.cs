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
    public partial class SifreSifirlama : Form
    {

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        private bool isUserValidated = false; // Kullanıcı doğrulama durumu

        public SifreSifirlama()
        {
            InitializeComponent();
            textEditNewPassword.Enabled = false; // Yeni şifre alanını devre dışı bırak
            btnResetPassword.Enabled = false; // Şifre sıfırla butonunu devre dışı bırak
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            string username = textEditUsername.Text.Trim();
            string phone = maskedTextBox1.Text.Trim();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM login WHERE K_Adi = @k_adi AND Telefon = @telefon", conn);
                cmd.Parameters.AddWithValue("@k_adi", username);
                cmd.Parameters.AddWithValue("@telefon", phone);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read()) // Eğer kayıt bulunursa
                {
                    isUserValidated = true;
                    MessageBox.Show("Kullanıcı doğrulandı! Yeni şifre alanı aktif.");
                    textEditNewPassword.Enabled = true;
                    btnResetPassword.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya telefon numarası hatalı!");
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        
    

        private void btnResetPassword_Click_1(object sender, EventArgs e)
        {
            if (!isUserValidated)
            {
                MessageBox.Show("Lütfen önce kullanıcı doğrulaması yapın!");
                return;
            }

            string username = textEditUsername.Text.Trim();
            string newPassword = textEditNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Yeni şifre alanı boş olamaz!");
                return;
            }

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE login SET Sifre = @sifre WHERE K_Adi = @k_adi", conn);
                cmd.Parameters.AddWithValue("@sifre", newPassword);
                cmd.Parameters.AddWithValue("@k_adi", username);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Şifre başarıyla güncellendi!");
                }
                else
                {
                    MessageBox.Show("Şifre güncellenemedi! Kullanıcı adı bulunamadı.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        
    }

 

}


