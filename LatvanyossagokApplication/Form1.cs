using System;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatvanyossagokApplication
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection("Server=localhost; Database=latvanyossagokdb; Uid=root; Pwd=;");
            conn.Open();   
            varosokTablaLetrehozas();
            latvanyosagTablaLetrehozas();




        }
        private void adatbazisLetrehiz()
        {
            /*
            CREATE DATABASE IF NOT EXISTS `latvanyossagokdb` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
            USE `latvanyossagokdb`;*/
        }
        void latvanyosagTablaLetrehozas()
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `latvanyossagok` (
                                  `id` int(11) NOT NULL AUTO_INCREMENT,
                                  `nev` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
                                  `leiras` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
                                  `ar` int(11) NOT NULL DEFAULT '0',
                                  `varos_id` int(11) NOT NULL,
                                  PRIMARY KEY (`id`),
                                  KEY `varos_id` (`varos_id`),
                                FOREIGN KEY(varos_id) REFERENCES varosok (id)
                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;";
            cmd.ExecuteNonQuery();
        }
        void varosokTablaLetrehozas()
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `varosok` (
                                  `id` int(11) NOT NULL AUTO_INCREMENT,
                                  `nev` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
                                  `lakossag` int(11) NOT NULL,
                                  PRIMARY KEY (`id`),
                                  UNIQUE KEY `nev_indx` (`nev`)
                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;";
            cmd.ExecuteNonQuery();
        }
       
        void VarosListazas()
        {
            listBoxVarosNevek.Items.Clear();
            listBoxVarosok.Items.Clear();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `nev`, `lakossag` FROM `varosok`";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("id");
                    var nev = reader.GetString("nev");
                    var lakossag = reader.GetInt32("lakossag");
                    var varos = new Varosok(id, nev, lakossag);
                    listBoxVarosNevek.Items.Add(varos);
                    listBoxVarosok.Items.Add(varos);
                }
            }
        }
        void latvanyossagLista()
        {
            listBoxLatvanyossagok.Items.Clear();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `nev`, `leiras`, `ar`, `varos_id` FROM `latvanyossagok`";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("id");
                    var nev = reader.GetString("nev");
                    var ar = reader.GetInt32("ar");
                    var leiras = reader.GetString("leiras");
                    var vid = reader.GetInt32("varos_id");
                    var latvanyossag = new Latvanyossag(id,nev,leiras,ar,vid);
                    listBoxLatvanyossagok.Items.Add(latvanyossag);
                }
            }
        }
        private void buttonVarosFeltolt_Click(object sender, EventArgs e)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO varosok (nev, lakossag) VALUES (@nev, @lakossag)";
            cmd.Parameters.AddWithValue("@nev", textBoxVarosNev.Text);
            cmd.Parameters.AddWithValue("@lakossag", numericUpDownVarosLakossag.Value);
            cmd.ExecuteNonQuery();
            VarosListazas();
        }
        private void buttonLFeltolt_Click(object sender, EventArgs e)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO `latvanyossagok`(`nev`, `leiras`, `ar`, `varos_id`) VALUES (@nev,@leiras,@ar,@varosId)";
            cmd.Parameters.AddWithValue("@nev", textBoxLNev.Text);
            cmd.Parameters.AddWithValue("@leiras", textBoxLeiras.Text);
            cmd.Parameters.AddWithValue("@ar", numericUpDownLAr.Value);
            var varos = (Varosok)listBoxVarosNevek.SelectedItem;
            cmd.Parameters.AddWithValue("@varosId", varos.Id);
            cmd.ExecuteNonQuery();
            latvanyossagLista();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VarosListazas();
            latvanyossagLista();
        }

        private void listBoxVarosok_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonVTorles.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = true;
            textBoxVarosNevModosit.Enabled = true;
            numericUpDownVarosLakossagModosit.Enabled = true;
            var varos = (Varosok)listBoxVarosok.SelectedItem;
            textBoxVarosNevModosit.Text = varos.Nev;
            numericUpDownVarosLakossagModosit.Value = varos.Lakossag;
        }


        private void buttonVTorles_Click(object sender, EventArgs e)
        {
            if (listBoxVarosok.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs város kiválasztva!");
                return;
            }
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE 
                            FROM varosok 
                            WHERE id = @id";

            var varos = (Varosok)listBoxVarosok.SelectedItem;
            cmd.Parameters.AddWithValue("@id", varos.Id);

            cmd.ExecuteNonQuery();

            VarosListazas();
        }

        private void buttonLTorles_Click(object sender, EventArgs e)
        {
            if (listBoxLatvanyossagok.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs Látványosság kiválasztva!");
                return;
            }
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE 
                            FROM latvanyossagok 
                            WHERE id = @id";

            var latvanyossag = (Latvanyossag)listBoxLatvanyossagok.SelectedItem;
            cmd.Parameters.AddWithValue("@id", latvanyossag.LatvanyossagId);

            cmd.ExecuteNonQuery();

            latvanyossagLista();
        }

        private void listBoxLatvanyossagok_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonLTorles.Enabled = true;
            label10.Enabled = true;
            label9.Enabled = true;
            label8.Enabled = true;
            textBoxLatvanyNevModosit.Enabled = true;
            textBoxLatvanyosLeirasModosit.Enabled = true;
            numericUpDownLatvanyArModosit.Enabled = true;
            var latavnyossag = (Latvanyossag)listBoxLatvanyossagok.SelectedItem;
            textBoxLatvanyNevModosit.Text = latavnyossag.LatvanyossagNev;
            textBoxLatvanyosLeirasModosit.Text = latavnyossag.LatvanyossagLeiras;
            numericUpDownLatvanyArModosit.Value = latavnyossag.LatvanyossagAr;
        }
    }
}
