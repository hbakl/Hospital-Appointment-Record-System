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

        sqlbaglantisi bgl = new sqlbaglantisi();

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
            
        }
    }
}
