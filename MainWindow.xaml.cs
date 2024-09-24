using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Nepesseg_GRF_HF
{
    public partial class MainWindow : Window
    {
        static List<Adat> adatok = new List<Adat>();
        public MainWindow()
        {
            InitializeComponent();
            beolvasas();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public static void beolvasas()
        {
            
            StreamReader sr = new StreamReader(File.Exists("ujadat.txt") ? "ujadat.txt":"adatok-utf8.txt");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                adatok.Add(new Adat(sr.ReadLine()));
            }
            sr.Close();
        }
        public static void mentes(string newData)
        {
            Adat adat = new Adat(newData);
            adatok.Add(adat);
            StreamWriter sw = new StreamWriter("ujadat.txt");
            foreach (var item in adatok)
            {
                sw.WriteLine($"{item.Orszag};{item.Terulet};{item.Nepesseg};{item.FoVaros};{item.FoVarosNepesseg}");
            }
            sw.Close();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            hibauzenet.Text = "";
            if (orszag_text.Text == "" || fovaros_text.Text == "" || terulet_text.Text == "" || nepesseg_text.Text == "" || fovaros_text.Text == "")
            {
                hibauzenet.Text = "Adjon meg adatokat";
            }
            else
            {

                if (int.Parse(fovaros_lakosssag_text.Text) > int.Parse(nepesseg_text.Text))
                {
                    hibauzenet.Text = "A főváros lakossága nem lehet nagyobb az orszag lakossaganal";
                    nepesseg_text.Text = fovaros_lakosssag_text.Text;
                }
                else
                {
                    mentes($"{orszag_text.Text};{terulet_text.Text};{nepesseg_text.Text};{fovaros_text.Text};{fovaros_lakosssag_text.Text}");
                    hibauzenet.Text = "mentés sikeres";
                }

            }

        }
    }

    class Adat
    {
        public string Orszag;
        public int Terulet;
        public int Nepesseg;
        public string FoVaros;
        public int FoVarosNepesseg;

        public Adat(string sor)
        {
            string[] tomb = sor.Split(';');
            Orszag = tomb[0];
            Terulet = int.Parse(tomb[1]);
            Nepesseg = int.Parse(tomb[2].Contains("g") ? tomb[2].Replace("g", "0000") : tomb[2]);
            FoVaros = tomb[3];
            FoVarosNepesseg = int.Parse(tomb[4]) * 1000;
        }

        public double nepsuruseg()
        {
            return Math.Round(Convert.ToDouble(Nepesseg) / Convert.ToDouble(Terulet), 0);
        }

        public bool FoVaros30_koncentracio()
        {
            return Convert.ToDouble(Nepesseg) * 0.3 < FoVarosNepesseg;
        }
    }
}
