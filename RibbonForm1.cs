using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace OtelMaster11
{
    public partial class AnaForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public AnaForm()
        {
            InitializeComponent();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            HotelExpenses hotel = new HotelExpenses();
            hotel.MdiParent = this;
            hotel.Show();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.MdiParent = this;
            frm2.Show();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            OdaIslemleri oda1 = new OdaIslemleri();
            oda1.MdiParent = this;
            oda1.Show();
        }

     

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            OdaIslemleri oda1 = new OdaIslemleri();
            oda1.MdiParent = this;
            oda1.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            OdaIslemleri oda1 = new OdaIslemleri();
            oda1.MdiParent = this;
            oda1.Show();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            CleaningStatusForm2 csf = new CleaningStatusForm2();
            csf.MdiParent = this;
            csf.Show();
        }

        private void x1_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewReservationForm reservationForm = new NewReservationForm();
            reservationForm.MdiParent = this;
            reservationForm.Show();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            MusteriEkleme müsteri = new MusteriEkleme();
            müsteri.MdiParent = this;
            müsteri.Show();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            MusteriEkleme müsteri = new MusteriEkleme();
            müsteri.MdiParent = this;
            müsteri.Show();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            MusteriEkleme müsteri = new MusteriEkleme();
            müsteri.MdiParent = this;
            müsteri.Show();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            MusteriEkleme müsteri = new MusteriEkleme();
            müsteri.MdiParent = this;
            müsteri.Show();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            Personel p1 = new Personel();
            p1.MdiParent = this;
            p1.Show();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {

            Personel p1 = new Personel();
            p1.MdiParent = this;
            p1.Show();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {

            Personel p1 = new Personel();
            p1.MdiParent = this;
            p1.Show();
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {

            Personel p1 = new Personel();
            p1.MdiParent = this;
            p1.Show();
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            DestekTalepleri destek = new DestekTalepleri();
            destek.MdiParent = this;
            destek.Show();
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewReservationForm reservationForm = new NewReservationForm();
            reservationForm.MdiParent = this;
            reservationForm.Show();
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewReservationForm reservationForm = new NewReservationForm();
            reservationForm.MdiParent = this;
            reservationForm.Show();
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewReservationForm reservationForm = new NewReservationForm();
            reservationForm.MdiParent = this;
            reservationForm.Show();
        }
    }
}