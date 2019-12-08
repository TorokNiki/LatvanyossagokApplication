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
using System.Text.RegularExpressions;

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
                                FOREIGN KEY(varos_id) REFERENCES varosok (id) ON DELETE CASCADE ON UPDATE CASCADE
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
            if (textBoxVarosNev.TextLength > 0)
            {
                if (Regex.Match(textBoxVarosNev.Text, @"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").Success)
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"INSERT INTO varosok (nev, lakossag) VALUES (@nev, @lakossag)";
                    cmd.Parameters.AddWithValue("@nev", textBoxVarosNev.Text);
                    cmd.Parameters.AddWithValue("@lakossag", numericUpDownVarosLakossag.Value);
                    cmd.ExecuteNonQuery();
                    label1.ForeColor = Color.Black;

                }
                else
                {
                    MessageBox.Show("Nem jó név formátumot adtál meg!");
                    label1.ForeColor = Color.Red;
                    // return;
                }
            }
            else
            {
                MessageBox.Show("Nem mindent töltöttél ki!");
                label1.ForeColor = Color.Red;
            }
                VarosListazas();
            textBoxVarosNev.Text = "";
            numericUpDownVarosLakossag.Value = 1;

        }
        private void buttonLFeltolt_Click(object sender, EventArgs e)
        {
            if (textBoxLNev.TextLength > 0 && textBoxLeiras.TextLength > 0 && listBoxVarosNevek.SelectedIndex >= 0)
            {
                if (Regex.Match(textBoxLNev.Text, @"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").Success)
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"INSERT INTO `latvanyossagok`(`nev`, `leiras`, `ar`, `varos_id`) VALUES (@nev,@leiras,@ar,@varosId)";
                    cmd.Parameters.AddWithValue("@nev", textBoxLNev.Text);
                    cmd.Parameters.AddWithValue("@leiras", textBoxLeiras.Text);
                    cmd.Parameters.AddWithValue("@ar", numericUpDownLAr.Value);
                    var varos = (Varosok)listBoxVarosNevek.SelectedItem;
                    cmd.Parameters.AddWithValue("@varosId", varos.Id);
                    cmd.ExecuteNonQuery();
                    label4.ForeColor = Color.Black;
                    label5.ForeColor = Color.Black;
                    label11.ForeColor = Color.Black;

                }
                else
                {
                    MessageBox.Show("Nem jó név formátumot adtál meg!");
                    label4.ForeColor = Color.Red;
                    // return;
                }
            }
            else
            {
                if (textBoxLNev.TextLength <= 0)
                {
                    label4.ForeColor = Color.Red;
                }
                if (textBoxLeiras.TextLength <= 0)
                {
                    label11.ForeColor = Color.Red;
                }
                if (listBoxVarosNevek.SelectedIndex < 0)
                {
                    label5.ForeColor = Color.Red;
                }
                MessageBox.Show("Nem mindent töltöttél ki!");
            }
            latvanyossagLista();
            textBoxLNev.Text = "";
            textBoxLeiras.Text = "";
            numericUpDownLAr.Value = 0;
            listBoxVarosNevek.ClearSelected();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            label11.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            
            desabledLatvanyossag();
            desabledVarosok();
            VarosListazas();
            latvanyossagLista();
        }

        private void listBoxVarosok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxVarosok.SelectedIndex >= 0)
            {
                buttonVTorles.Enabled = true;
                label6.Enabled = true;
                label7.Enabled = true;
                buttonVModosit.Enabled = true;
                textBoxVarosNevModosit.Enabled = true;
                numericUpDownVarosLakossagModosit.Enabled = true;
                var varos = (Varosok)listBoxVarosok.SelectedItem;
                textBoxVarosNevModosit.Text = varos.Nev;
                numericUpDownVarosLakossagModosit.Value = varos.Lakossag;
            }
            else
            {
                desabledVarosok();
            }
        }
        private void desabledVarosok()
        {
            buttonVTorles.Enabled = false;
            buttonVModosit.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            textBoxVarosNevModosit.Enabled = false;
            numericUpDownVarosLakossagModosit.Enabled = false;
            textBoxVarosNevModosit.Text = "";
            numericUpDownVarosLakossagModosit.Value = 1;
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
            latvanyossagLista();
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
            VarosListazas();
            latvanyossagLista();
        }

        private void listBoxLatvanyossagok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLatvanyossagok.SelectedIndex>=0)
            {
            buttonLTorles.Enabled = true;
            buttonLatvanyossagModosit.Enabled = true;
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
            else
            {
                desabledLatvanyossag();
            }

            
        }
        private void desabledLatvanyossag()
        {

            buttonLTorles.Enabled = false;
            buttonLatvanyossagModosit.Enabled = false;
            label10.Enabled = false;
            label9.Enabled = false;
            label8.Enabled = false;
            textBoxLatvanyNevModosit.Enabled = false;
            textBoxLatvanyosLeirasModosit.Enabled = false;
            numericUpDownLatvanyArModosit.Enabled = false;
            textBoxLatvanyNevModosit.Text = "";
            textBoxLatvanyosLeirasModosit.Text = "";
            numericUpDownLatvanyArModosit.Value = 0;
        }

        private void buttonVModosit_Click(object sender, EventArgs e)
        {
            
                if (Regex.Match(textBoxVarosNevModosit.Text, @"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").Success)
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"UPDATE varosok SET nev= @nev,lakossag= @lakossag WHERE id= @id";
                    cmd.Parameters.AddWithValue("@nev", textBoxVarosNevModosit.Text);
                    cmd.Parameters.AddWithValue("@lakossag", numericUpDownVarosLakossagModosit.Value);
                    var varos = (Varosok)listBoxVarosok.SelectedItem;
                    cmd.Parameters.AddWithValue("@id", varos.Id);
                    cmd.ExecuteNonQuery();
                    
                }
                else
                {
                    MessageBox.Show("Nem jó név formátumot adtál meg!");
                   // return;
                }
                VarosListazas();
                listBoxVarosok.ClearSelected();
                desabledVarosok();
            
        }

        private void buttonLatvanyossagModosit_Click(object sender, EventArgs e)
        {
            
                if (Regex.Match(textBoxLatvanyNevModosit.Text, @"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").Success)
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"UPDATE latvanyossagok SET nev = @nev, leiras = @leiras, ar = @ar WHERE id = @id";
                    cmd.Parameters.AddWithValue("@nev", textBoxLatvanyNevModosit.Text);
                    cmd.Parameters.AddWithValue("@leiras", textBoxLatvanyosLeirasModosit.Text);
                    cmd.Parameters.AddWithValue("@ar", numericUpDownLatvanyArModosit.Value);
                    var latvanyossag = (Latvanyossag)listBoxLatvanyossagok.SelectedItem;
                    cmd.Parameters.AddWithValue("@id", latvanyossag.LatvanyossagId);
                    cmd.ExecuteNonQuery();
                    
                }
                else
                {
                    MessageBox.Show("Nem jó név formátumot adtál meg!");
                   // return;
                }
                latvanyossagLista();
                listBoxLatvanyossagok.ClearSelected();
                desabledLatvanyossag();
            
            
        }
    }
}
