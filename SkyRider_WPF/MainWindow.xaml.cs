using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
//using MySqlConnector;



/*
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Common;
using System.IO;
using System.Globalization;
*/



namespace SkyRider_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        DataTable dtUsers;
        //SqlDataAdapter adapter;

        public MainWindow()
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT rd_users.id, rd_users.fname, rd_users_data.birthday, rd_users_data.birthtime, rd_city.cityname, rd_city.longitude, rd_city.latitude FROM rd_users JOIN rd_users_data ON rd_users.id=rd_users_data.user_id JOIN rd_city ON rd_users_data.city_id=rd_city.id";
            dtUsers = new DataTable();
            //SqlConnection connection = null;
            MySqlConnection skycon = new MySqlConnection(connectionString);

            try
            {
                skycon.Open();
                
                MySqlCommand command = new MySqlCommand(sql, skycon);
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dtUsers);
                usersGrid.ItemsSource = dtUsers.DefaultView;
                usersGrid2.ItemsSource = dtUsers.DefaultView;
            }


            //try
            //{
            //connection = new SqlConnection(connectionString);
            //SqlCommand command = new SqlCommand(sql, connection);
            //adapter = new SqlDataAdapter(command);

            // установка команды на добавление для вызова хранимой процедуры
            /*
            adapter.InsertCommand = new SqlCommand("sp_InsertPhone", connection);
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar, 50, "Title"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@company", SqlDbType.NVarChar, 50, "Company"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Int, 0, "Price"));
            SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            parameter.Direction = ParameterDirection.Output;
            */

            //connection.Open(connectionString);
            //    connection.Open();
            //    adapter.Fill(dtUsers);
            //    usersGrid.ItemsSource = dtUsers.DefaultView;
            //}
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
