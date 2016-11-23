using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Shipsoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connetionString = null;
        SqlConnection cnn;

        public MainWindow()
        {
            InitializeComponent();
            string[] lines = System.IO.File.ReadAllLines(@"yhdistys.txt");
            connetionString = "Data Source=" + lines[0] + ";  Initial Catalog=" + lines[1] + ";User ID=" + lines[2] + ";Password=" + lines[3]; // yhteyden tiedot
            cnn = new SqlConnection(connetionString);
            try   // yhteyden avaus yritys
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");
            }
            catch
            {
                MessageBox.Show("Can not open connection ! "); // yhteys virhe viesti
            }
        }
       
            SqlDataReader Reader;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
                
                SqlCommand cmd = new SqlCommand("select ShipName, ShipID from Ships", cnn);
                try
                {
                    Reader = cmd.ExecuteReader();
                }
                catch
                {
                    MessageBox.Show("ei toimi");
                    return;
                }
                while (Reader.Read())
                {
                    //  lstLaivat.Items.Add(Reader["ShipName"]);

                    ListBoxItem Item = new ListBoxItem();
                    Item.Content = Reader["ShipName"];
                    Item.Tag = Reader["ShipID"];
                    lstLaivat.Items.Add(Item);
                    Item.MouseLeftButtonUp += ListBoxItem_MouseLeftButtonUp;

                
            }
            cnn.Close();
        }
        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cnn.Open();

            lblLaivaID.Content = ((ListBoxItem)lstLaivat.SelectedItem).Tag;
            SqlCommand cmd = new SqlCommand("select Course from Ships WHERE ShipID=" + ((ListBoxItem)lstLaivat.SelectedItem).Tag, cnn);
            Reader = cmd.ExecuteReader();
            Reader.Read();
            lblKompassiAste.Content = Reader["Course"];



            cnn.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
