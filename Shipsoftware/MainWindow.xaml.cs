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
        public MainWindow()
        {
            InitializeComponent();
      
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string connetionString = null;
            SqlConnection cnn;
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

            SqlDataReader Reader;
            SqlCommand cmd = new SqlCommand("select ShipName from Ships", cnn);
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
                lstLaivat.Items.Add(Reader["ShipName"]);
            }

            cnn.Close();
        }
    }
}
