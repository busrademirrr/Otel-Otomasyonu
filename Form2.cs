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
    public partial class Form2 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-49KRHBB\\SQLEXPRESS01; Initial Catalog=OtelMasterVT; Integrated Security=True;");
        public Form2()
        {
            InitializeComponent();
        }

        private void navBarControl1_Click(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            OdaIslemleri oda= new OdaIslemleri();
            oda.ShowDialog();
        }

        private void OdaTemizlikTakibi(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            CleaningStatusForm2 cs = new CleaningStatusForm2();
            cs.ShowDialog();
        }

        private void NewReservations(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            NewReservationForm rs = new NewReservationForm();
            rs.ShowDialog();
        }

        private void MusteriEkleme(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            MusteriEkleme müsteri34 = new MusteriEkleme();
            müsteri34.ShowDialog();
        }

        

        private void GelirGider1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            HotelExpenses gelir = new HotelExpenses();
            gelir.ShowDialog();
        }

        private void PersonelYönetimi(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Personel pr = new Personel();
            pr.ShowDialog();
        }

        private void Destek1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            DestekTalepleri destek = new DestekTalepleri();
            destek.ShowDialog();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

       
    }
}
