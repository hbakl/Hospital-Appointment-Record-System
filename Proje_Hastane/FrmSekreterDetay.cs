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

namespace Proje_Hastane
{
    public partial class FrmSekreterDetay : Form
    {
        public FrmSekreterDetay()
        {
            InitializeComponent();
        }



        public string sekreterTC;

        public string RndId;
        public string RndTarih;
        public string RndSaat;
        public string RndBrans;
        public string RndDoktor;
        public bool RndDurum;
        public string RndTC;

        sqlbaglantisi bgl = new sqlbaglantisi();

        public void DisableButton()
        {
            BtnKaydet.Enabled = false;
            BtnGuncelle.Enabled = true;
        }



        private void FrmSekreterDetay_Load(object sender, EventArgs e)
        {
            //sekreter detay ekranında sekreter bilgilerierini veritabanından getiren kod kısmı
            LblTC.Text = sekreterTC;
            SqlCommand komut = new SqlCommand("select SekreterAdSoyad from Tbl_Sekreter where SekreterTC=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTC.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                LblAdSoyad.Text = dr[0].ToString();
            }
            bgl.baglanti().Close();


            //branşları  datagridview1'e aktarma
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select BransID as 'Branş No', BransAd as 'Branş Adı' from Tbl_Branslar", bgl.baglanti());
            da.Fill(dt1);
            dataGridView1.DataSource = dt1;


            //Doktorları listeye aktaran kodlar
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("Select (DoktorAd+' '+DoktorSoyad) as 'Doktor Adı',DoktorBrans as 'Branşı' From Tbl_Doktorlar", bgl.baglanti());
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;

            //Branşları combobox'a aktaran kodlar
            SqlCommand komut2 = new SqlCommand("Select BransAd From Tbl_Branslar", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while(dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();


            //randevu listesindeki verileri rendevu panelindeki kısımlara ekleyen kodlar
            txtId.Text = RndId;
            mskTarih.Text = RndTarih;
            mskSaat.Text = RndSaat;
            cmbBrans.Text = RndBrans;
            cmbDoktor.Text = RndDoktor;
            ChkDurum.Checked = RndDurum;
            mskTc.Text = RndTC;
    }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //randevu kaydetme kısmı
            SqlCommand komutKaydet = new SqlCommand("insert into Tbl_Randevular (RandevuTarih,RandevuSaat,RandevuBrans,RandevuDoktor) values (@r1,@r2,@r3,@r4)", bgl.baglanti());
            komutKaydet.Parameters.AddWithValue("@r1",mskTarih.Text);
            komutKaydet.Parameters.AddWithValue("@r2",mskSaat.Text);
            komutKaydet.Parameters.AddWithValue("@r3",cmbBrans.Text);
            komutKaydet.Parameters.AddWithValue("@r4",cmbDoktor.Text);
            komutKaydet.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu başarıyla kaydedildi.");

            mskTarih.Text = "";
            mskSaat.Text = "";
            cmbBrans.Text = "";
            cmbDoktor.Text = "";
            txtId.Text = "";
            mskTc.Text = "";


        }

        private void cmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Doktorları combobox'a aktaran kodlar
            cmbDoktor.Items.Clear();
            SqlCommand komut3 = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar where DoktorBrans=@p1", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", cmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                cmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();
        }

        private void BtnDuyuruOlustur_Click(object sender, EventArgs e)
        {

            //duyuru ekleme kısmı
            SqlCommand komut = new SqlCommand("insert into Tbl_Duyurular (Duyuru) values (@d1)", bgl.baglanti());
            komut.Parameters.AddWithValue("@d1", RchDuyuru.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Duyuru başarıyla oluşturuldu.");
        }

        private void BtnDoktorPanel_Click(object sender, EventArgs e)
        {
            //Doktor paneline geçiş yapacak kodlar
            FrmDoktorPaneli drp = new FrmDoktorPaneli();
            drp.Show();

        }

        private void BtnBransPanel_Click(object sender, EventArgs e)
        {
            FrmBrans frb = new FrmBrans();
            frb.Show();
        }

        private void BtnListe_Click(object sender, EventArgs e)
        {
            FrmRandevuListesi frr = new FrmRandevuListesi();
            frr.NAd = LblAdSoyad.Text;
            frr.NTC = LblTC.Text;
            frr.Show();
            this.Hide();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            //randevu güncelleme kısmı
            SqlCommand komut = new SqlCommand("update Tbl_Randevular set RandevuTarih=@r1,RandevuSaat=@r2,RandevuBrans=@r3,RandevuDoktor=@r4,RandevuDurum=@r5,HastaTC=@r6 where RandevuId=@r7", bgl.baglanti());
            komut.Parameters.AddWithValue("@r1", mskTarih.Text);
            komut.Parameters.AddWithValue("@r2", mskSaat.Text);
            komut.Parameters.AddWithValue("@r3", cmbBrans.Text);
            komut.Parameters.AddWithValue("@r4", cmbDoktor.Text);
            komut.Parameters.AddWithValue("@r7", txtId.Text);
            komut.Parameters.AddWithValue("@r6", mskTc.Text);


            komut.Parameters.AddWithValue("@r5", ChkDurum.Checked.ToString());


            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            mskTarih.Text="";
            mskSaat.Text = "";
            cmbBrans.Text = "";
            cmbDoktor.Text = "";
            txtId.Text = "";
            mskTc.Text = "";

            BtnKaydet.Enabled = true;
            BtnGuncelle.Enabled = false;

        }

        private void btnDuyuru_Click(object sender, EventArgs e)
        {
            FrmDuyurular frd = new FrmDuyurular();
            frd.Show();
        }

        private void cmbBrans_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbDoktor.Text = "";
        }

        private void FrmSekreterDetay_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
