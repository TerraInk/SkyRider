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
    /// 
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

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
                SaveToBase();
        }

        private void CliData1_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dtUserEdit = new DataTable();
            string query;
            //query = (string)Application.Current.Resources["mainquery"];

            query = "SELECT rd_users.id, rd_users.fname, rd_users_data.birthday, " +
                "rd_users_data.birthtime, rd_users_data.remark, " +
                "rd_city.cityname, rd_city.longitude, rd_city.latitude, rd_city.gmt " +
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
                clilat.Text = string.Format("{0:00.00}", dtUserEdit.Rows[0]["latitude"]);
                //temp = dtUserEdit.Rows[0]["longitude"].ToString();
                clilon.Text = string.Format("{0:00.00}", dtUserEdit.Rows[0]["longitude"]);
                cliremark.Text = dtUserEdit.Rows[0]["remark"].ToString();

                cliname.Focus();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveToBase();
        }

        private void SaveToBase()
        {
            string birthday, btime;

            string rd_users = string.Format("Update rd_users SET fname='{0}' WHERE id={1}", cliname.Text, clientIdEdit);
            MySqlConnection skycon = (MySqlConnection)Application.Current.Resources["skyconn"];
            skycon.Open();

            //MessageBox.Show(rd_users);

            MySqlCommand cmd = skycon.CreateCommand();
            //MySqlCommand cmd = new MySqlCommand(rd_users, skycon);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            cmd.CommandText = rd_users;
            cmd.ExecuteNonQuery();

            birthday = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(clidate.Text));
            btime    = string.Format("{0:HH:mm:ss}",   Convert.ToDateTime(clitime.Text));

            string rd_users_data = string.Format("Update rd_users_data SET birthday='{0}', birthtime='{1}', remark='{2}' " +
                "WHERE user_id={3}", birthday, btime, cliremark.Text, clientIdEdit);
            cmd.CommandText = rd_users_data;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (skycon != null)
                    skycon.Close();
            }
            //long id = cmd.LastInsertedId;

            //string rd_users_data = string.Format(
            //    "Insert into rd_users_data (user_id, birthday, birthtime, julday, city_id) " +
            //                               "values ({0}, '{1}', '{2}', '{3}', {4})",
            //                                id, NEDate, NETime, NE_JD, NE_CityId
            //    );

            //cmd.CommandText = rd_users_data;
            //cmd.ExecuteNonQuery();
            MessageBox.Show("Записано !");
            //this.Focusable = true;
            cliname.Focus();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Close();
        }
    }
}
