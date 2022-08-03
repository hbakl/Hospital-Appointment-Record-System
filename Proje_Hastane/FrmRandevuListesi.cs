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
    public partial class FrmRandevuListesi : Form
    {
        public FrmRandevuListesi()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();
        public string NTC;
        public string NAd;
        private void FrmRandevuListesi_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from Tbl_Randevular", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;


        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            FrmSekreterDetay frs = new FrmSekreterDetay();

            frs.RndId = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            frs.RndTarih = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            frs.RndSaat = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            frs.RndBrans = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            frs.RndDoktor = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            frs.RndTC = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            //bool değişkeni durumuna göre bir yerde veri girmek için bu şekilde kodu yazıyoruz.
            if ((bool)dataGridView1.Rows[secilen].Cells[5].Value)
            {
                frs.RndDurum = true;
            }
            else
            {
                frs.RndDurum = false;
            }
            frs.sekreterTC = NTC;
            frs.DisableButton();
            frs.Show();
            this.Hide();
        }

        private void FrmRandevuListesi_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmRandevuListesi_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmSekreterDetay frs = new FrmSekreterDetay();
            frs.sekreterTC = NTC;
            frs.Show();
            this.Hide();
        }
    }
}
