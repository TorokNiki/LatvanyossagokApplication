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
                    listBoxVarosNevek.Items.Add(nev);
                    listBoxVarosok.Items.Add(nev+"– Lakosság: "+lakossag+ " fő");
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

        private void Form1_Load(object sender, EventArgs e)
        {
            VarosListazas();
        }
    }
}
