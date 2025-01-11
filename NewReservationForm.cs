using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraEditors; // DevExpress bileşenleri için gerekli
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace OtelMaster11
{
    public partial class NewReservationForm : Form
    {

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");

        public NewReservationForm()
        {
            InitializeComponent();

        }
        private void NewReservationForm_Load(object sender, EventArgs e)
        {
            Listele();
            LoadCustomers();
            LoadRooms();
            comboBoxEdit4.SelectedIndexChanged += comboBoxEdit4_SelectedIndexChanged;
        }
        

        private void btnEkle_Click(object sender, EventArgs e)
        {
            //Ekle
            // Ekleme işlemini butona bağlayın
            try
            {
                Ekle();
                MessageBox.Show("Müşteri başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Yeni veriyi listeye eklemek için tekrar listeleyin
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Ekle()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Reservations (CustomerID, RoomID, CheckInDate, CheckOutDate, NumberOfGuests, ReservationStatus, PaymentStatus,TotalPrice,CreatedDate) " +
                "VALUES (@customerid, @roomid, @checkindate, @checkoutdate, @numberofguests, @reservationstatus, @paymentstatus, @totalprice, @createddate)", conn);
            
            cmd.Parameters.AddWithValue("@customerid", comboBoxEdit1.SelectedItem);
            cmd.Parameters.AddWithValue("@roomid",comboBoxEdit4.SelectedItem);
            cmd.Parameters.AddWithValue("@checkindate", dtStartDate.Value);
            cmd.Parameters.AddWithValue("@checkoutdate", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@numberofguests", misafirSayisi.Text);
            cmd.Parameters.AddWithValue("@reservationstatus", comboBoxEdit2.SelectedItem);
            cmd.Parameters.AddWithValue("@paymentstatus", comboBoxEdit3.SelectedItem);
            cmd.Parameters.AddWithValue("@totalprice", toplamFiyat.Text);
            cmd.Parameters.AddWithValue("@createddate", dateTimePicker1.Value);

            // Sorguyu çalıştırın
            cmd.ExecuteNonQuery();
            conn.Close(); // Bağlantıyı kapatmayı unutmayın
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {

            // NewReservations formunda toplam fiyatı al
            string toplamFiyat1 = toplamFiyat.Text; // toplam fiyatı tutan kontrol

            // Ödeme formunu aç ve toplam fiyat bilgisini aktar
            Odeme odemeFormu = new Odeme(toplamFiyat1);
            odemeFormu.Owner = this; // Parent formu ayarla
            odemeFormu.ShowDialog();
        }
        private bool isUpdating = false; // Güncelleme modunu takip etmek için bir değişken
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            //Güncelle

            if (!isUpdating)
            {
                // Güncelleme moduna geçiş
                if (gridView1.GetFocusedRowCellValue("ReservationID") == null)
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
                comboBoxEdit1.SelectedItem= gridView1.GetFocusedRowCellValue("CustomerID")?.ToString();
                comboBoxEdit4.SelectedItem = gridView1.GetFocusedRowCellValue("RoomID")?.ToString();
                misafirSayisi.Text = gridView1.GetFocusedRowCellValue("NumberOfGuests")?.ToString();
                comboBoxEdit2.SelectedItem = gridView1.GetFocusedRowCellValue("ReservationStatus")?.ToString();
                comboBoxEdit3.SelectedItem = gridView1.GetFocusedRowCellValue("PaymentStatus")?.ToString();
                toplamFiyat.Text = gridView1.GetFocusedRowCellValue("TotalPrice")?.ToString();

                // DateTimePicker için
                if (gridView1.GetFocusedRowCellValue("CheckInDate") != null)
                {
                    dtStartDate.Value = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("CheckInDate"));
                }
                if (gridView1.GetFocusedRowCellValue("CheckOutDate") != null)
                {
                    dateTimePicker2.Value = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("CheckOutDate"));
                }
                if (gridView1.GetFocusedRowCellValue("CreatedDate") != null)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("CreatedDate"));
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
        if (string.IsNullOrEmpty(comboBoxEdit4.Text) || comboBoxEdit4.SelectedItem == null)
        {
            MessageBox.Show("Lütfen bir oda seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(comboBoxEdit1.Text) || comboBoxEdit1.SelectedItem == null)
        {
            MessageBox.Show("Lütfen bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        conn.Open();
        SqlCommand cmd = new SqlCommand(
            "UPDATE Reservations SET " +
            "CustomerID = @customerid, " +
            "RoomID = @roomid, " +
            "CheckInDate = @checkindate, " +
            "CheckOutDate = @checkoutdate, " +
            "NumberOfGuests = @numberofguests, " +
            "ReservationStatus = @reservationstatus, " +
            "PaymentStatus = @paymentstatus, " +
            "TotalPrice = @totalprice, " +
            "CreatedDate = @createddate " +
            "WHERE ReservationID = @id", conn);

        cmd.Parameters.AddWithValue("@customerid", comboBoxEdit1.SelectedItem);
        cmd.Parameters.AddWithValue("@roomid", comboBoxEdit4.SelectedItem);
        cmd.Parameters.AddWithValue("@checkindate", dtStartDate.Value);
        cmd.Parameters.AddWithValue("@checkoutdate", dateTimePicker2.Value);
        cmd.Parameters.AddWithValue("@numberofguests", misafirSayisi.Text);
        cmd.Parameters.AddWithValue("@reservationstatus", comboBoxEdit2.SelectedItem);
        cmd.Parameters.AddWithValue("@paymentstatus", comboBoxEdit3.SelectedItem);
        cmd.Parameters.AddWithValue("@totalprice", toplamFiyat.Text);
        cmd.Parameters.AddWithValue("@createddate", dateTimePicker1.Value);
        cmd.Parameters.AddWithValue("@id", gridView1.GetFocusedRowCellValue("ReservationID"));

        cmd.ExecuteNonQuery();
        conn.Close();

        MessageBox.Show("Güncelleme işlemi başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Listele();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        conn.Close();
    }
}





        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();
        }
        private void Sil()
        {
            try
            {
                // Seçili satır ve sütun kontrolü
                if (gridView1.GetFocusedRowCellValue("ReservationID") != null)
                {
                    string id = gridView1.GetFocusedRowCellValue("ReservationID").ToString();

                    // Kullanıcıdan onay al
                    DialogResult result = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes) // Eğer kullanıcı 'Evet' derse silme işlemini yap
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Reservations WHERE ReservationID = @id", conn);
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
        public void Listele()
        {
            string komut = "select* from Reservations";
            SqlDataAdapter da = new SqlDataAdapter(komut, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
            comboBoxEdit1.SelectedIndex = -1;
            comboBoxEdit4.SelectedIndex = -1;
            misafirSayisi.Text = " ";
            comboBoxEdit2.SelectedIndex = 0;
            comboBoxEdit3.SelectedIndex = 0;
            toplamFiyat.Text = " ";
            dtStartDate.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Müşteri ekle
            MusteriEkleme müsteri = new MusteriEkleme();
            müsteri.Show();
        }
        // Müşterileri yükleme
        private void LoadCustomers()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CustomerID FROM Customers", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBoxEdit1.Properties.Items.Add(dr["CustomerID"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Dolu olmayan oda ID'lerini comboBoxEdit4'e yükleme
        private void LoadRooms()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT RoomID FROM Rooms WHERE IsOccupied = @isoccupied", conn);
                cmd.Parameters.AddWithValue("@isoccupied", "Boş"); // Boş olan odaları arıyoruz

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBoxEdit4.Properties.Items.Add(dr["RoomID"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Odalar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReservations()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;"))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Reservations", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl1.DataSource = dt;
            }
        }
        public void UpdatePaymentStatus(string reservationID)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Reservations SET PaymentStatus = @paymentstatus WHERE ReservationID = @reservationid", conn);
                cmd.Parameters.AddWithValue("@paymentstatus", "Ödendi");
                cmd.Parameters.AddWithValue("@reservationid", reservationID);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("PaymentStatus 'Ödendi' olarak güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PaymentStatus güncellenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }



        private void comboBoxEdit4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEdit4.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir oda seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedRoomID = comboBoxEdit4.SelectedItem.ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Price FROM Rooms WHERE RoomID = @roomid", conn);
                cmd.Parameters.AddWithValue("@roomid", selectedRoomID);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    toplamFiyat.Text = dr["Price"].ToString();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oda fiyatı yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}\nHata Detayı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                gridView1.ActiveFilterString = string.Format("[PaymentStatus] LIKE '%{0}%'", searchValue);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                // Seçili satırdan RoomID değerini al
                var roomIdValue = gridView1.GetFocusedRowCellValue("RoomID");

                if (roomIdValue != null)
                {
                    string roomId = roomIdValue.ToString();

                    // comboBoxEdit4'e RoomID'yi yükle
                    if (comboBoxEdit4.Properties.Items.Contains(roomId))
                    {
                        comboBoxEdit4.SelectedItem = roomId; // Eğer combobox'ta RoomID varsa seçili yap
                    }
                    else
                    {
                        comboBoxEdit4.SelectedIndex = -1; // Eğer RoomID combobox'ta yoksa seçim yapma
                    }
                }
                else
                {
                    // Eğer RoomID null gelirse combobox'ı sıfırla
                    comboBoxEdit4.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
