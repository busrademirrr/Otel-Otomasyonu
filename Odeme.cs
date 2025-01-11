using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace OtelMaster11
{
    public partial class Odeme : Form
    {
        
        public string ToplamTutar { get; set; } // Toplam fiyatı NewReservations formundan alacak
        public string ReservationID { get; set; }
        
        
        private void Odeme_Load(object sender, EventArgs e)
        {
            // Toplam tutarı ödeme formundaki ilgili alana aktar
            textEdit1.Text = ToplamTutar; // Değeri textEdit1'e yazdır
        }
        public Odeme(string toplamFiyat1)
        {
            InitializeComponent();
            ToplamTutar = toplamFiyat1; // Toplam fiyatı sınıf değişkenine aktar
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            // MaskedTextBox'lardan alınan verileri temizle (sadece rakamları al)
            string cardNumber = maskedTextBox1.Text.Replace(" ", "").Replace("-", "").Replace("_", "");
            string expiryDate = maskedTextBox2.Text.Replace(" ", "").Replace("-", "").Replace("_", "");
            string cvc = maskedTextBox3.Text.Replace(" ", "").Replace("-", "").Replace("_", "");

            // Kart numarası, son kullanma tarihi ve CVC doğrulaması
            if (cardNumber.Length != 16 || !Regex.IsMatch(cardNumber, @"^\d+$"))
            {
                MessageBox.Show("Kart numarası 16 haneli ve sadece rakamlardan oluşmalıdır.", "Hatalı Kart Numarası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (expiryDate.Length != 4 || !Regex.IsMatch(expiryDate, @"^\d+$"))
            {
                MessageBox.Show("Son kullanma tarihi 4 haneli (MMYY formatında) ve sadece rakamlardan oluşmalıdır.", "Hatalı Son Kullanma Tarihi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cvc.Length != 3 || !Regex.IsMatch(cvc, @"^\d+$"))
            {
                MessageBox.Show("CVC 3 haneli ve sadece rakamlardan oluşmalıdır.", "Hatalı CVC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ödeme başarılı mesajı
            MessageBox.Show("Ödeme başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // PaymentStatus'u güncelle
            NewReservationForm parentForm = (NewReservationForm)this.Owner;
            string selectedReservationID = parentForm.gridView1.GetFocusedRowCellValue("ReservationID")?.ToString();

            if (!string.IsNullOrEmpty(selectedReservationID))
            {
                parentForm.UpdatePaymentStatus(selectedReservationID);
                parentForm.Listele(); // Güncellenen veriyi yenile
            }

            this.DialogResult = DialogResult.OK; // Ödeme başarılı sonucu döndür
            this.Close();
        }

        // İptal butonuna tıklama işlemi
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            // "İptal" butonuna tıklanınca
            MessageBox.Show("Ödeme iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.Cancel; // Ödeme iptal sonucu döndür
            this.Close();
        }

      
    }
}
