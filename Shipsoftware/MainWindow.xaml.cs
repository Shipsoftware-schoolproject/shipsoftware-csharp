using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shipsoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connetionString = null;
        SqlConnection cnn;
        SqlDataReader Reader;

        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"yhdistys.txt");
                connetionString = "Data Source=" + lines[0] + ";  Initial Catalog=" + lines[1] + ";User ID=" + lines[2] + ";Password=" + lines[3]; // yhteyden tiedot
                cnn = new SqlConnection(connetionString);
                try   // yhteyden avaus yritys
                {
                    cnn.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open connection ! "); // yhteys virhe viesti
                }
            }
            catch
            {
                MessageBox.Show("Could not load configuration");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select ShipName, Ships.ShipID, North, East from Ships inner join GPS on ships.ShipID = GPS.ShipID ", cnn);
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
				// TODO: Add pushpins to map
            }
            cnn.Close();
        }
        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cnn.Open();

            lblLaivaID.Content = ((ListBoxItem)lstLaivat.SelectedItem).Tag;
            SqlCommand cmd = new SqlCommand("select Course, ShipTypes.Name, North,East from Ships inner join GPS on Ships.ShipID = GPS.ShipID Inner Join ShipTypes on ships.ShipTypeID = ShipTypes.ShipTypeID  WHERE Ships.ShipID = " + ((ListBoxItem)lstLaivat.SelectedItem).Tag, cnn);
            Reader = cmd.ExecuteReader();
            Reader.Read();
            lblKompassiAste.Content = Reader["Course"];
            lblLaivanTyyppi.Content = Reader["Name"];
            // TODO: Update location to map

            cnn.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
