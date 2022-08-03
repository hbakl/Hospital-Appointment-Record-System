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
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }

        public string tc;
        sqlbaglantisi bgl = new sqlbaglantisi();
        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            //TC ve Ad ve soyadı hasta detay ekranında gösteren kısım
            LblTC.Text = tc;
            SqlCommand komut = new SqlCommand("Select HastaAd,HastaSoyad From Tbl_Hastalar Where HastaTC=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTC.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                LblAdSoyad.Text = dr[0] +" "+ dr[1];
            }
            bgl.baglanti().Close();

            //TC Kimlik numarasına göre randevu geçmişini getiren kod kısmı
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular where HastaTC='" + LblTC.Text + "'" , bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //Branşları combobox'a getiren kod kısmı
            SqlCommand komut2 = new SqlCommand("Select BransAd From Tbl_Branslar",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while(dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

        }

        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            //branşı seçince doktor isimlerini getiren kod kısmıhf
            CmbDoktor.Items.Clear();
            SqlCommand komut3 = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar where DoktorBrans=@p1", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", CmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while(dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();
        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //doktorun aktif randevularını gösteren kod kısmı
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular where RandevuBrans='" + CmbBrans.Text +"'" + "and RandevuDoktor ='" + CmbDoktor.Text + "'and RandevuDurum=0", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void LnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDuzenle fr = new FrmBilgiDuzenle();
            fr.TCno = LblTC.Text;
            fr.Show();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            textBox1.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }

        private void BtnRandevuAl_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            { MessageBox.Show("Lütfen branş ve doktor seçtikten sonra, randevu tablosunda müsait tarih varsa tıkladıktan sonra randevu kayıt butonuna basın. Müsait randevu yoksa başka doktorun randevusunu kontrol ediniz.", "Randevu seçilmedi!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else
            {

                //id numarasına göre randevuyu kayıt eden kısım
                SqlCommand komut = new SqlCommand("Update Tbl_Randevular set RandevuDurum=1,HastaTC=@p1,HastaSikayet=@p2 where RandevuID=@p3", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", tc);
                komut.Parameters.AddWithValue("@p2", RchSikayet.Text);
                komut.Parameters.AddWithValue("@p3", textBox1.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Randevu başarı ile alındı", "Randevu alındı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
                CmbBrans.Text = "";
                CmbDoktor.Text = "";
                RchSikayet.Text = "";

                //DataGridView ler temizleniyor.
                dataGridView1.DataSource = "";
                dataGridView2.DataSource = "";

                //randevu alındıktan sonra doktorun aktif randevularını gösteren kod kısmı
                DataTable dt3 = new DataTable();
                SqlDataAdapter da3 = new SqlDataAdapter("Select * From Tbl_Randevular where RandevuBrans='" + CmbBrans.Text + "'" + "and RandevuDoktor ='" + CmbDoktor.Text + "'and RandevuDurum=0", bgl.baglanti());
                da3.Fill(dt3);
                dataGridView2.DataSource = dt3;

                //randevu alındıktan sonra TC Kimlik numarasına göre randevu geçmişini getiren kod kısmı
                DataTable dt4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter("Select * From Tbl_Randevular where HastaTC='" + tc+"'", bgl.baglanti());
                da4.Fill(dt4);
                dataGridView1.DataSource = dt4;
            }


        }

        private void CmbBrans_SelectedValueChanged(object sender, EventArgs e)
        {
            CmbDoktor.Text = "";
            //branşı seçince doktor isimlerini getiren kod kısmıhf
            CmbDoktor.Items.Clear();
            SqlCommand komut3 = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar where DoktorBrans=@p1", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", CmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();
            dataGridView2.DataSource = "";
        }

        private void FrmHastaDetay_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
