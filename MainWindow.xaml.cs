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
            StreamReader sr = new StreamReader("adatok-utf8.txt");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                adatok.Add(new Adat(sr.ReadLine()));
            }
            sr.Close();
        }

        private void Save(object sender, RoutedEventArgs e)
        {

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
