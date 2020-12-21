using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
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

namespace iluoKonwerter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IluoTowar> listIluoTowar = new List<IluoTowar>();
        ObservableCollection<Kolektor> listKolektor = new ObservableCollection<Kolektor>();
        ObservableCollection<Kolektor> listSubiektBrak = new ObservableCollection<Kolektor>();
        ObservableCollection<IluoKontrah> listKontrah = new ObservableCollection<IluoKontrah>();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void importTowaryFirma_Click(object sender, RoutedEventArgs e)
        {
            listIluoTowar.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                string nazwa_pliku = ofd.FileName;
                string textLine = string.Empty;
                string[] splitLine;

                int i = 0;
                //int i = 1;
                try
                {
                    StreamReader sr = new StreamReader(nazwa_pliku, Encoding.Default);
                    while (!sr.EndOfStream)
                    {

                        textLine = sr.ReadLine();
                        splitLine = textLine.Split(';');
                        if (i > 0)
                        {   //Pominięcie nagłówka
                            listIluoTowar.Add(new IluoTowar()
                            {
                                //Indeks = splitLine[0],
                                //Indeks = i.ToString(),
                                Indeks = utworzIndex(i),
                                Nazwa = splitLine[1].ToUpper(),
                                //Nazwa = splitLine[1].ToUpper().Trim('"'),
                                //TypTowaru = splitLine[3]
                                //TypTowaru = "Towar",
                                //TypTowaru = "Produkt",
                                TypTowaru=czyProdukt(splitLine[2]),
                                Grupa = "Ogólna",
                                //JednostkaPodstawowa = splitLine[4],
                                JednostkaPodstawowa = jednostkaMiary(splitLine[4]),
                                //JednostkaPodzielna = "1",
                                JednostkaPodzielna = czyPodzielna(splitLine[4]),
                                StawkaVatZakupu = splitLine[9] + "%",
                                StawkaVatSprzedazy = splitLine[9] + "%",
                                PKWiU = splitLine[10],
                                SWW = "",
                                Opis = "",
                                StanMinimalny = "0",
                                StanMaksymalny = "0",
                                CenaBazowa = round_2(splitLine[7]),
                                Dostawca = "",
                                IndeksDostawcy = "",
                                WwwDostawcy = "",
                                Producent = "",
                                IndeksProducenta = "",
                                WwwProducenta = "",
                                MetodaWydawaniaTowaru = "FIFO",
                                CzyVATMarza = "0",
                                Etykiety = "",
                                WagaNetto = "",
                                WagaBrutto = "",
                                Szerokosc = "",
                                Wysokosc = "",
                                Glebokosc = "",
                                //Cena_Detaliczna = round_2(splitLine[5]),
                                Cena_Detaliczna = splitLine[5],
                                Cena_Hurtowa = round_2(splitLine[6]),
                                //Cena_Hurtowa = splitLine[6],
                                Cena_Specjalna = "",
                                //KodKreskowy_1 = utworzKodKreskowy(splitLine[???],i),
                                KodKreskowy_1=utworzEan13(i),
                                //KodKreskowy_1 = i.ToString(),
                                KodKreskowy_2 = "",
                                KodKreskowy_3 = "",
                                KodKreskowy_4 = "",
                                KodKreskowy_5 = "",
                                KodKreskowy_6 = "",
                                KodKreskowy_7 = "",
                                KodKreskowy_8 = "",

                            }
                        );
                        }
                        i++;
                    }
                    dg.DataContext = listIluoTowar;
                    //dg.ItemsSource = listIluoTowar;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd " + ex);
                    return;
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Nie wybrano pliku");
            }
        }

        private void importStanyFirma_Click(object sender, RoutedEventArgs e)
        {
            listKolektor.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                string nazwa_pliku = ofd.FileName;
                string textLine = string.Empty;
                string[] splitLine;
                string split;
                string iloscOk;
                int i = 0;
                try
                {
                    StreamReader sr = new StreamReader(nazwa_pliku, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        textLine = sr.ReadLine();
                        splitLine = textLine.Split(';');
                        split = splitLine[3].Replace(",", ".");
                        if (czyPodzielna(splitLine[4]) == "0")
                        {
                            iloscOk = Math.Round(Double.Parse(splitLine[3]),0).ToString();
                        }
                        else
                        {
                            iloscOk = splitLine[3].Replace(",", ".");
                        }
                        if (i > 0)
                        {
                            if (Double.Parse(splitLine[3]) > 0)
                            {
                                listKolektor.Add(new Kolektor()
                                {
                                    KodKreskowy = utworzEan13(i),
                                    //Ilosc = splitLine[3].Replace(",", "."),
                                    Ilosc=iloscOk,
                                    Cena = round_2(splitLine[7]).Replace(",", "."),
                                }
                                );
                            }
                        }
                        i++;

                    }
                    dg.DataContext = listKolektor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd " + ex);
                    return;
                }
            }
        }

        private void importKontrahent_Click(object sender, RoutedEventArgs e)
        {
            listKontrah.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                string nazwa_pliku = ofd.FileName;
                string textLine = string.Empty;
                string[] splitLine;

                int i = 0;
                //int i = 1;
                try
                {
                    StreamReader sr = new StreamReader(nazwa_pliku, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        textLine = sr.ReadLine();
                        splitLine = textLine.Split(';');
                        if (i > 0)  // Pominięcie nagłówka
                        {
                            listKontrah.Add(new IluoKontrah()
                            {
                                Nazwa = splitLine[0] + " " + splitLine[1],
                                NazwaKrotka = splitLine[0],
                                NIP = splitLine[9],
                                Regon = "",
                                PESEL = "",
                                NumerGLN = "",
                                KRS = "",
                                Grupa = "Ogólna",
                                UlicaLokal = splitLine[3],
                                KodPocztowy = splitLine[4],
                                Miejscowosc = splitLine[2],
                                Wojewodztwo = "",
                                KodKraju = "PL",
                                DomyslnyRabat = splitLine[12],
                                KredytKupiecki = "10000",
                                KredytDlaPrzeterminowanych = "0",
                                KredytBlokada = "0",
                                PlatnoscGotowka = "1",
                                PlatnoscKarta = "1",
                                PlatnoscOdroczona = "1",
                                PlatnoscDomyslna = "1",
                                TerminNaleznosci = "14",
                                DomyslnaCena = "Detaliczna",
                                Branza = "",
                                AdresKorespondencyjnyUlicaLokal = "",
                                AdresKorespondencyjnyKodPocztowy = "",
                                AdresKorespondencyjnyMiejscowosc = "",
                                AdresKorespondencyjnyWojewodztwo = "",
                                AdresKorespondencyjnyKodKraju = "",
                                AdresWysylkiUlicaLokal = "",
                                AdresWysylkiKodPocztowy = "",
                                AdresWysylkiMiejscowosc = "",
                                AdresWysylkiWojewodztwo = "",
                                AdresWysylkiKodKraju = "",
                                IloscPracownikow = "0",
                                DataPozyskania = "",
                                ZrodloPozyskania = "",
                                AdresEmail = "",
                                NrTelefonu = "",
                                KodKreskowy = "",
                                Etykiety = "",
                                RachunekBankowy_1 = "",
                                RachunekBankowy_2 = "",
                            }
                            );
                        }
                        i++;
                    }
                    dg.DataContext = listKontrah;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd " + ex);
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Nie wybrano pliku");
            }
        }


        private void importStanySubiekt_Click(object sender, RoutedEventArgs e)
        {
            listKolektor.Clear();
            listSubiektBrak.Clear();
            string nazwa_pliku = @"c:\iluoimport\nazwa_ilosc_csv.csv";
            string textLine = string.Empty;
            string[] splitLine;
            //string split;
            string kodKreskowy = "";
            string cenaBazowa = "";

            int i = 0;
            try
            {
                StreamReader sr = new StreamReader(nazwa_pliku, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    textLine = sr.ReadLine();
                    splitLine = textLine.Split(';');

                    if (i > 0)
                    {
                        bool bExist = listIluoTowar.Exists(oElement => oElement.Nazwa.Equals(splitLine[0].Trim('"')));
                        if (bExist)
                        {
                            IluoTowar towar = listIluoTowar.Find(oElement => oElement.Nazwa == splitLine[0].Trim('"'));
                            kodKreskowy = towar.KodKreskowy_1;
                            cenaBazowa = towar.CenaBazowa;
                            listKolektor.Add(new Kolektor()
                            {
                                KodKreskowy = kodKreskowy,
                                Ilosc = splitLine[1].Replace(",", "."),
                                Cena = cenaBazowa.Replace(",", "."),
                            }
                            );
                        }
                        else
                        {
                            listSubiektBrak.Add(new Kolektor()
                            {
                                KodKreskowy =splitLine[0],
                                Ilosc = splitLine[1].Replace(",", "."),
                                Cena = "",
                            }
                            );
                        }

                    }
                    i++;

                }
                dg.DataContext = listKolektor;
                MessageBox.Show("Ilość odnalezionych pozycji: "+listKolektor.Count().ToString());
                dg.DataContext = listSubiektBrak;
                MessageBox.Show("Ilość zagubionych pozycji: " + listSubiektBrak.Count().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd " + ex);
                return;
            }
        }

        private void eksportTowaryFirma_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ToDataTable(listIluoTowar);
            CreateCSVFile(dt, @"c:\IluoImport\towary_csv.csv", true);
            MessageBox.Show("Dane wyeksportowanoa do pliku towary_csv.csv;  Ilość pozycji = " + listIluoTowar.Count.ToString());
        }

        private void eksportStanyFirma_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ToDataTable(listKolektor);
            CreateCSVFile(dt, @"c:\IluoImport\dane.txt", false);
            CreateCSVFile(dt, @"c:\IluoImport\dane_csv.csv", false);
            MessageBox.Show("Wyeksportowanoa do pliku dane.txt   Ilość pozycji = " + listKolektor.Count.ToString() );
        }

        private void eksportKontrahent_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ToDataTable(listKontrah);
            CreateCSVFile(dt, @"c:\IluoImport\kontrahenci_csv.csv", true);
            MessageBox.Show("Dane wyeksportowanoa do pliku kontrahenci_csv.csv; Ilość pozycji = " + listKontrah.Count.ToString() );
        }

        private void eksportStanySubiekt_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ToDataTable(listKolektor);
            CreateCSVFile(dt, @"c:\IluoImport\dane.txt", false);
            DataTable dt1 = ToDataTable(listSubiektBrak);
            CreateCSVFile(dt1, @"c:\IluoImport\dane_csv.csv", true);
            MessageBox.Show("Wyeksportowanoa do pliku dane.txt;  Ilość pozycji odnalezionych = " + listKolektor.Count.ToString() + "nie odnalezionych ( zapisane w pliku dane_csv.csv) = " +listSubiektBrak.Count.ToString());
        }


        private void eksportXmlTowaryFirma_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ToDataTable(listIluoTowar);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt);
            // Save to disk
            dataSet.WriteXml(@"C:\test\iluoxml.xml");
            MessageBox.Show("Dane wyeksportowano do pliku c:_test_iluoxml.xml");
            // Read from disk
            //dataSet.ReadXml(@"C:\MyDataset.xml");
        }


        private void eksportXmlStanyFirma_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void eksportXmlKontrahent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void eksportXmlStanySubiekt_Click(object sender, RoutedEventArgs e)
        {
            
        }




//********        
//********        //Procedury moje
//******** Funkcja zwraca index na podstawie numeru wiersza
        private string utworzIndex(int i)
        {
            int k = i;
            string lindex = "#0000";
            if (k < 10)
            {
                lindex = lindex.Substring(0, 4) + k.ToString();
            }
            else if (k >= 10 && k < 100)
            {
                lindex = lindex.Substring(0, 3) + k.ToString();
            }
            else if (k >= 100 && k < 1000)
            {
                lindex = lindex.Substring(0, 2) + k.ToString();
            }
            else if (k >= 1000 && k < 10000)
            {
                lindex = lindex.Substring(0, 1) + k.ToString();
            }
            else
            {
                lindex = "!!!!!";
            }

            return lindex;
        }


        //******** Funkcja zwraca kod kreskowy na podstawie numeru wiersza
        private string utworzEan13(int i)
        {
            int k = i;
            string lindex = utworzIndex(k).Substring(1); // liczba i w formacie XXXX
            string kodEan13 = "20000000" + lindex;
            int[] kod=new int[12];
            for (int j=0;  j<12; j++)
            {
                kod[j] = Int32.Parse(kodEan13.Substring(j, 1));
            }
            int suma = kod[0] + kod[2] + kod[4] + kod[6] + kod[8] + kod[10] +
                3 * (kod[1] + kod[3] + kod[5] + kod[7] + kod[9] + kod[11]);
            int reszta = suma % 10;
            string liczbaKontrolna = "0";
            if (reszta != 0)
            {
                liczbaKontrolna = (10 - reszta).ToString();
            }
            kodEan13 = kodEan13 + liczbaKontrolna;
            
            return kodEan13;
        }


        //********* Jeśli mkod=01 to produkt
        private string czyProdukt(string textLine)
        {
            string typ = "Towar";
            string mkod = textLine;
            if (mkod == "01")
            {
                typ = "Produkt";
            }
            return typ;
        }


        //********* Porządkuje jednostki miary
        private string jednostkaMiary(string textLine)
        {
            string jm = textLine;
            if (jm == "SZ" || jm == "ZES" || jm == "BLI" || jm == "KPL" || jm == "SZ.")
            {
                jm = "szt";
            }
            else if (jm == "OP" || jm == "OP0" || jm == "0P" || jm == "5SZ" || jm == "50" || jm == "5szt." || jm == "25M")
            {
                jm = "opak";
            }
            else if (jm == "MB")
            {
                jm = "m";
            }
            
            else if (jm == "100")
            {
                jm = "100szt.";
            }
            else if (jm == "TYS" || jm == "TYŚ")
            {
                jm = "1000szt.";
            }
            
            else if (jm == "KG")
            {
                jm = "kg";
            }
            return jm;
        }

//********** Tworzy kod kreskowy na podstawie numera wiersza
        private string utworzKodKreskowy(string textLine, int i)
        {
            string lindex = textLine;
            if (lindex != string.Empty)
            {
                int k = i;
                lindex = "1" + k.ToString();
            }
            return lindex;
        }

//********* Określa podzielność jednostki miary
        private string czyPodzielna(string textLine)
        {
            string ljednostka = textLine;
            string lpodzielna = "1";
            if (ljednostka == "SZ" || ljednostka == "ZES" || ljednostka == "BLI" || ljednostka == "KPL" || ljednostka == "SZT" || ljednostka == "SZ.")
            {
                lpodzielna = "0";
            }

            return lpodzielna;
        }


//********* Zaokrągla do 2 znaków poprzecinku
        private string round_2(string cena_m)
        {

            string cena = cena_m;
            if (cena == string.Empty)
            {
                cena = "0";
            }
            double d_cena = double.Parse(cena);
            d_cena = Math.Round(d_cena, 2);
            cena = d_cena.ToString();
            return cena;
        }




//********        //Procedury obce
//******** Zamienia kolekcję na tabelę
        public DataTable ToDataTable<T>(IList<T> data)// T is any generic type
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

//******** Eksport do csv
        public void CreateCSVFile(DataTable dtDataTablesList, string strFilePath, bool nazwyKolumn)

        {
            // Create the CSV file to which grid data will be exported.

             StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.Default);

            //First we will write the headers.

            int iColCount = dtDataTablesList.Columns.Count;

            if (nazwyKolumn)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dtDataTablesList.Columns[i]);
                    if (i < iColCount - 1)
                    {
                        //sw.Write(",");
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
            }

            // Now write all the rows.

            foreach (DataRow dr in dtDataTablesList.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)

                    {
                        //sw.Write(",");
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }


//**********

//**********
    }
}
