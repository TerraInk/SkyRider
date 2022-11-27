using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace SkyRider_WPF
{
    /// <summary>
    /// Логика взаимодействия для CliData.xaml
    /// </summary>
    public partial class CliData : Window
    {
        string clientIdEdit;
        public CliData(string cliendId)
        {
            InitializeComponent();
            clientIdEdit = cliendId;
            //MessageBox.Show(clientIdEdit);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                //frmMain.Close();
                e.Handled = true;
                this.Close();

            }
        }

        private void CliData1_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dtUserEdit = new DataTable();
            string query;
            //query = (string)Application.Current.Resources["mainquery"];

            query = "SELECT rd_users.id, rd_users.fname, rd_users_data.birthday, " +
                "rd_users_data.birthtime, rd_city.cityname, rd_city.longitude, rd_city.latitude, rd_city.gmt " +
                "FROM rd_users " +
                "JOIN rd_users_data ON rd_users.id=rd_users_data.user_id " +
                "JOIN rd_city ON rd_users_data.city_id=rd_city.id WHERE rd_users.id=" + clientIdEdit;

            MySqlConnection skycon = (MySqlConnection)Application.Current.Resources["skyconn"];
            
            //query += " WHERE rd_users.id=";
            //query += clientIdEdit;
            //MessageBox.Show(query);

            //MySqlCommand command = new MySqlCommand(query, skycon);

            //MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            //adapter.Fill(dtUserEdit);
            //MessageBox.Show(dtUserEdit.Rows[0]["birthtime"].ToString());

            

            try
            {
                MySqlCommand command = new MySqlCommand(query, skycon);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dtUserEdit);

                //cliname.DataContext.
                //dtUserEdit
                cliname.Text = (string)dtUserEdit.Rows[0]["fname"];
                clidate.Text = string.Format("{0:dd.MM.yyyy}", dtUserEdit.Rows[0]["birthday"]);
                //clitime.Text = string.Format("{0:HH:mm:ss}", dtUserEdit.Rows[0]["birthtime"]);
                //cliname.Text = (string)dtUserEdit.Rows[0]["birthtime"];
                cligmt.Text = dtUserEdit.Rows[0]["gmt"].ToString();
                clitime.Text = dtUserEdit.Rows[0]["birthtime"].ToString();
                clicity.Text = dtUserEdit.Rows[0]["cityname"].ToString();
                clilat.Text = dtUserEdit.Rows[0]["latitude"].ToString();
                //temp = dtUserEdit.Rows[0]["longitude"].ToString();
                clilon.Text = string.Format("{0:00.00}", dtUserEdit.Rows[0]["longitude"]);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (skycon != null)
                    skycon.Close();
            }
            
        }
    }
}
