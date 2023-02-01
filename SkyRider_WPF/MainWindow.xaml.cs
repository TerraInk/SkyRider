using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Windows.Documents;
//using System.Windows.Forms;
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
            //connectionString = ConfigurationManager.ConnectionStrings["terra"].ConnectionString;
            
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT rd_users.id, rd_users.fname, rd_users_data.birthday, " +
                "rd_users_data.birthtime, rd_users_data.remark, rd_city.cityname, rd_city.longitude, rd_city.latitude " +
                "FROM rd_users JOIN rd_users_data ON rd_users.id=rd_users_data.user_id " +
                "JOIN rd_city ON rd_users_data.city_id=rd_city.id WHERE rd_users.del<1";
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

                //int t = dtUsers.Rows.Count;
                recCount.Content += " " + dtUsers.Rows.Count.ToString();

                //usersGrid2.ItemsSource = dtUsers.DefaultView;
                //usersGrid.SelectedIndex = 0;

                //tbsMain.Focusable = true;
                //tbsMain.Focus();
                //usersGrid.ColumnWidth = System.Windows.Controls.DataGridLength.Auto;

                //SelectRowByIndex(usersGrid, 2);

                //usersGrid.CanUserResizeColumns = false;
                usersGrid.Focusable = true;
                usersGrid.SelectedIndex = 0;
                //usersGrid.TabIndex = 0;
                //usersGrid.SelectionUnit = System.Windows.Controls.DataGridSelectionUnit.Cell;
                
                usersGrid.Focus();
                usersGrid.Columns[0].Visibility = Visibility.Collapsed;


                //SelectRowByIndex(usersGrid, 0);


                System.Windows.Input.Keyboard.Focus(usersGrid);

                //tr=new trav
                //usersGrid.MoveFocus();
                //usersGrid.SelectedIndex = 0;
                //usersGrid.Focus();
                //usersGrid.setf
                Application.Current.Resources["skyconn"] = skycon;
                Application.Current.Resources["mainquery"] = sql;
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

        private void frmMain_Activated(object sender, EventArgs e)
        {
            //tbsMain.Focus();
            //usersGrid.Focus();
        }

        private void frmMain_GotFocus(object sender, RoutedEventArgs e)
        {
            //usersGrid.Focus();
        }

        public static void SelectRowByIndex(System.Windows.Controls.DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(System.Windows.Controls.DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            System.Windows.Controls.DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as System.Windows.Controls.DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as System.Windows.Controls.DataGridRow;
            }
            //TODO: Retrieve and focus a DataGridCell object
        }

        private void usersGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //MessageBox.Show(e.ToString());
        }

        private void frmMain_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Escape)
            {
                //frmMain.Close();
                e.Handled = true;
                this.Close();
               
            }
        }

        private void frmMain_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string clientID;
                //clientID = (DataRowView)(usersGrid.SelectedItems[0]).Row["id"];
                clientID = ((DataRowView)usersGrid.SelectedItems[0]).Row["id"].ToString();
                //MessageBox.Show(clientID.ToString());

                CliData cliwin = new CliData(clientID);
                cliwin.Owner = this;
                cliwin.ShowDialog();
                //cliwin.Close();
                e.Handled = true;
            }

            if (e.Key == System.Windows.Input.Key.F3)
            
                // Открываем диалог выбора базы данных
            {
                BaseSelect base_select = new BaseSelect();
                base_select.Owner = this;
                base_select.ShowDialog();
                //cliwin.Close();
            }
            
            if (e.Key == System.Windows.Input.Key.F4)
            
                // Открываем функции по затмениям
            {
                ///rtbGlobalEcl.AppendText("Солнечные затмения");
                //tabControl1.SelectedItem = tabControl1.Items[0];

                //FlowDocument document = new FlowDocument();
                //Paragraph paragraph = new Paragraph();
                //List list1 = new List();
                //ListItem li1 = new ListItem();
                //document.Blocks.Add(list1);

                //paragraph.Inlines.Add(new Run("текст\n работает" + "И прибавления тоже" + "Удачи"));
                //document.Blocks.Add(paragraph);
                //rtbGlobalEcl.Document = document;

                txtGlobalEcl.AppendText("Солнечные затмения:\n");
                txtGlobalEcl.AppendText("В 2023 году\n");
                DateTime nowUtc = DateTime.Now;

                using (var sweph = new SwissEphNet.SwissEph())
                {
                    var jday = Convert.ToInt16(nowUtc.Day);
                    var jmon = Convert.ToInt16(nowUtc.Month);
                    var jyear = Convert.ToInt16(nowUtc.Year);
                    var jhour = Convert.ToInt16(nowUtc.Hour);
                    var jmin = Convert.ToInt16(nowUtc.Minute);
                    var jsec = 1;
                    var jut = jhour + (jmin / 60.0) + (jsec / 3600.0);
                    var JulianDateUT = sweph.swe_julday(jyear, jmon, jday, jut, 1);
                    txtGlobalEcl.AppendText(nowUtc.ToString() + "\n");
                    txtGlobalEcl.AppendText(JulianDateUT.ToString() + "\n");
                    //MessageBox.Show(JulianDateUT.ToString());
                    //JulianDateUT1 = (double)JulianDateUT;
                    double[] tret = new double[10];
                    string merr = "";
                    int iftype;
                    //iftype = SwissEphNet.SwissEph.SE_ECL_TOTAL | SwissEphNet.SwissEph.SE_ECL_CENTRAL| SwissEphNet.SwissEph.SE_ECL_NONCENTRAL;
                    //iftype = SwissEphNet.SwissEph.SE_ECL_PARTIAL;
                    iftype = 0;
                    var ef = sweph.swe_sol_eclipse_when_glob(
                        JulianDateUT,
                        0,
                        iftype,
                        tret,
                        false,
                        ref merr
                        );
                    double jh=0;
                    int eyear, emon, eday;
                    eyear = emon = eday = 0;
                    sweph.swe_revjul(tret[0], 0, ref eyear, ref emon, ref eday, ref jh);
                    txtGlobalEcl.AppendText("----------------------\n");
                    txtGlobalEcl.AppendText(eday.ToString() + "." + emon.ToString() + "." + eyear.ToString() + "\n");
                    for (int i = 0; i < 10; i++)
                     {
                        //Console.WriteLine(i);
                        txtGlobalEcl.AppendText(tret[i].ToString() + "\n");
                    }
                    
                }

                tbsMain.SelectedItem = tbsMain.Items[3];
            }
        }

        private void usersGrid_Initialized(object sender, EventArgs e)
        {
            //this.Focus();
            
        }

        private void frmMain_Initialized(object sender, EventArgs e)
        {
            //usersGrid.Focus();
        }

        private void TabItem_Initialized(object sender, EventArgs e)
        {
            //usersGrid.Focus();
        }

        private void usersGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Focus();
            //
            //usersGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void tbsMain_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (itmMain !=null && itmMain.IsSelected)
            //{
            //usersGrid.Focus();
            //e.Handled = true;
            //}
            
            var tc = sender as System.Windows.Controls.TabControl; //The sender is a type of TabControl...

            if (tc != null)
            {
                var item = tc.SelectedItem;
               
                //Do Stuff ...
            }
        }
    }
}
