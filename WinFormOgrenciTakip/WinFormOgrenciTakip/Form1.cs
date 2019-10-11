using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WinFormOgrenciTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection con;
        OleDbCommand cmd;
        string connectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + Application.StartupPath + "/ogrenciler.accdb";
        int ogrenciId = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            ogrenciLoad(); 
        }

        #region "GİZLE"

        /*
        void addWithValue()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("insert into ogrenci (adi, soyadi, cinsiyeti, sinifi, numarasi) values (@adi, @soyadi, @cinsiyeti, @sinifi, @numarasi)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@adi", txtAdi.Text);
                cmd.Parameters.AddWithValue("@soyadi", txtSoyadi.Text);
                string cinsiyet = "";
                if (radioBtnErkek.Checked)
                {
                    cinsiyet = "Erkek";
                }
                else if (radioBtnKiz.Checked)
                {
                    cinsiyet = "kız";
                }
                cmd.Parameters.AddWithValue("@cinsiyeti", cinsiyet);
                cmd.Parameters.AddWithValue("@sinifi", cbbSinif.Text);
                cmd.Parameters.AddWithValue("@numarasi", Convert.ToInt32(txtNumara.Text));
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        */

        #endregion

        void ekle()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("insert into ogrenci (adi, soyadi, cinsiyeti, sinifi, numarasi) values (@adi, @soyadi, @cinsiyeti, @sinifi, @numarasi)", con);
                con.Open();
                cmd.Parameters.Add("@adi", OleDbType.VarChar).Value = txtAdi.Text;
                cmd.Parameters.Add("@soyadi", OleDbType.VarChar).Value = txtSoyadi.Text;
                string cinsiyet = "";
                if (radioBtnErkek.Checked)
                {
                    cinsiyet = radioBtnErkek.Text;
                }
                else if (radioBtnKiz.Checked)
                {
                    cinsiyet = radioBtnKiz.Text;
                }
                cmd.Parameters.Add("@cinsiyeti", OleDbType.VarChar).Value = cinsiyet;
                cmd.Parameters.Add("@sinifi", OleDbType.VarChar).Value = cbbSinif.Text;
                cmd.Parameters.Add("@numarasi", OleDbType.Integer).Value = txtNumara.Text;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            ogrenciLoad();
        }

        void guncelle()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("update ogrenci set adi = @adi, soyadi = @soyadi, cinsiyeti = @cinsiyeti, sinifi = @sinifi, numarasi = @numarasi where id = @id", con);
                con.Open();
                cmd.Parameters.Add("@adi", OleDbType.VarChar).Value = txtAdi.Text;
                cmd.Parameters.Add("@soyadi", OleDbType.VarChar).Value = txtSoyadi.Text;
                string cinsiyet = "";
                if (radioBtnErkek.Checked)
                {
                    cinsiyet = radioBtnErkek.Text;
                }
                else if (radioBtnKiz.Checked)
                {
                    cinsiyet = radioBtnKiz.Text;
                }
                cmd.Parameters.Add("@cinsiyeti", OleDbType.VarChar).Value = cinsiyet;
                cmd.Parameters.Add("@sinifi", OleDbType.VarChar).Value = cbbSinif.Text;
                cmd.Parameters.Add("@numarasi", OleDbType.Integer).Value = txtNumara.Text;
                cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            ogrenciLoad();
        }

        void sil()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("delete from ogrenci where id = @id", con);
                con.Open();
                cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            ogrenciLoad();
        }


        void ogrenciLoad()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("select * from ogrenci", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dg.DataSource = dt;
                dg.Columns["id"].Visible = false;
            }
            catch (OleDbException ex)
            {
                throw ex;
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (ogrenciId == 0)
            {
                ekle();
            }
            else
            {
                MessageBox.Show("Seçili öğrenci var");
            }
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                ogrenciId = Convert.ToInt32(dg.Rows[e.RowIndex].Cells["id"].Value);

                try
                {
                    con = new OleDbConnection(connectionString);
                    cmd = new OleDbCommand("select * from ogrenci where id = @id", con);
                    cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    radioBtnErkek.Checked = false;
                    radioBtnKiz.Checked = false;

                    foreach (DataRow row in dt.Rows)
                    {
                        txtAdi.Text = row["adi"].ToString();
                        txtSoyadi.Text = row["soyadi"].ToString();
                        string cinsiyet = row["cinsiyeti"].ToString();
                        if (cinsiyet == "Erkek")
                        {
                            radioBtnErkek.Checked = true;
                        }
                        else if (cinsiyet == "Kız")
                        {
                            radioBtnKiz.Checked = true;
                        }

                        cbbSinif.Text = row["sinifi"].ToString();
                        txtNumara.Text = row["numarasi"].ToString();
                    }
                }
                catch (OleDbException ex)
                {
                    throw ex;
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (ogrenciId > 0)
            {
                guncelle();
            }
            else
            {
                MessageBox.Show("Öğrenci Seçiniz");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (ogrenciId > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Seçili öğrenciyi silmek istediğinize emin misiniz?", "Öğrenci Sil", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    sil();
                    temizle();
                }
                else
                {
                    MessageBox.Show("İşlem İptal Edildi");
                }
            }
            else
            {
                MessageBox.Show("Öğrenci Seçiniz");
            }
        }

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            temizle();
        }

        void temizle()
        {
            ogrenciId = 0;
            txtAdi.Text = "";
            txtSoyadi.Text = "";
            radioBtnErkek.Checked = false;
            radioBtnKiz.Checked = false;
            cbbSinif.Text = "";
            txtNumara.Text = "";

            /* foreach (var item in tableLayoutPanel1.Controls)
             {
                 if (item is TextBox)
                 {
                     ((TextBox)item).Text = "";
                 }

                 if (item is ComboBox)
                 {
                     ((ComboBox)item).Text = "";
                 }
             }*/

        }
    }
}
