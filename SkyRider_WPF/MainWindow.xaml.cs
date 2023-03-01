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
            using (var sweph = new SwissEphNet.SwissEph())
            {
                sweph.swe_set_ephe_path(null);
                //swe_set_ephe_path(NULL);
            }

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

                DateTime beginUtc, endUtc, ReJul;

                ecldat1.SelectedDate = DateTime.UtcNow;
                beginUtc = (DateTime)ecldat1.SelectedDate;
                ecldat2.SelectedDate = beginUtc.AddYears(1);
                
                endUtc = (DateTime)ecldat2.SelectedDate;

                //nowUtc2 = nowUtc.AddYears(1);
                //ecldat2.SelectedDate = nowUtc2;
                var jUTbegin = Julie(beginUtc);
                var jUTend = Julie(endUtc);

                double[] tret = new double[10];
                string tmp, clientName, merr = "";
                int iftype = 0;

                // ******** Посчитаем данные выбранного клиента ********

                if (usersGrid.SelectedItems.Count == 0) return;
                clientName = ((DataRowView)usersGrid.SelectedItems[0]).Row[1].ToString();
                DateTime clientBDate;
                tmp = ((DataRowView)usersGrid.SelectedItems[0]).Row[2] + " " + ((DataRowView)usersGrid.SelectedItems[0]).Row[3];

                //clientBDate = (DateTime)tmp;

                //clientBDate = DateTime.ParseExact(tmp, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                //txtGlobalEcl.AppendText(clientBDate.ToString() + "\n");
                txtGlobalEcl.AppendText(tmp + "\n");

                // ******** Перебираем все солнечные затмения ********

                while (jUTbegin < jUTend)
                {

                    using (var sweph = new SwissEphNet.SwissEph())
                    {
                        // ******** Считаем затмение ********
                        var ef = sweph.swe_sol_eclipse_when_glob(jUTbegin, 0, iftype, tret, false, ref merr);

                        //txtGlobalEcl.AppendText(jUTbegin.ToString() + "\n");

                        jUTbegin = tret[0];

                        ReJul = ReJulie(jUTbegin);
                        txtGlobalEcl.AppendText(ReJul.ToString() + "\n");

                        txtGlobalEcl.AppendText("****************\n");
                        // ******** Считаем положение солнца в этот день ********
                        var ef_sun = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_SUN, 0, tret, ref merr);
                        txtGlobalEcl.AppendText("Солнце:" + tret[0].ToString() + "\n");

                        // ******** Считаем положение луны в этот день ********
                        var ef_moon = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_MOON, 0, tret, ref merr);
                        txtGlobalEcl.AppendText("Луна:" + tret[0].ToString() + "\n");
                        txtGlobalEcl.AppendText("****************\n");
                        //SwissEphNet.SwissEph.SEFLG_SPEED
                    }

                }
                
                txtGlobalEcl.AppendText("Лунные затмения:\n");
                jUTbegin = Julie(beginUtc);
                jUTend = Julie(endUtc);

                // ******** Перебираем все лунные затмения ********

                while (jUTbegin < jUTend)
                {

                    using (var sweph = new SwissEphNet.SwissEph())
                    {
                        // ******** Считаем затмение ********
                        var ef = sweph.swe_lun_eclipse_when(jUTbegin, 0, iftype, tret, false, ref merr);

                        //txtGlobalEcl.AppendText(jUTbegin.ToString() + "\n");

                        jUTbegin = tret[0];

                        ReJul = ReJulie(jUTbegin);
                        txtGlobalEcl.AppendText(ReJul.ToString() + "\n");

                        // ******** Считаем положение солнца в этот день ********
                        var ef_sun = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_SUN, 0, tret, ref merr);
                        txtGlobalEcl.AppendText("Солнце:" + tret[0].ToString() + "\n");

                        // ******** Считаем положение луны в этот день ********
                        var ef_moon = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_MOON, 0, tret, ref merr);
                        txtGlobalEcl.AppendText("Луна:" + tret[0].ToString() + "\n");
                        txtGlobalEcl.AppendText("****************\n");

                    }

                }


                tbsMain.SelectedItem = tbsMain.Items[3];
            }
        }

        private double Julie(DateTime jUT)
        {
            using (var sweph = new SwissEphNet.SwissEph())
            {

                var jday = Convert.ToInt16(jUT.Day);
                var jmon = Convert.ToInt16(jUT.Month);
                var jyear = Convert.ToInt16(jUT.Year);
                var jhour = Convert.ToInt16(jUT.Hour);
                var jmin = Convert.ToInt16(jUT.Minute);

                var jsec = 1;
                var jut = jhour + (jmin / 60.0) + (jsec / 3600.0);

                //var JulianDateUT = sweph.swe_julday(jyear, jmon, jday, jut, 1);
                return sweph.swe_julday(jyear, jmon, jday, jut, 1);
            }
        }

        private DateTime ReJulie(double jUT)
        {
            double jh = 0, jh2 = 0;
            int year, mon, day, jh3;
            year = mon = day = 0;
            DateTime result;

            using (var sweph = new SwissEphNet.SwissEph())
            {
                // ******** Преобразуем в обычную дату и время ********

                sweph.swe_revjul(jUT, 1, ref year, ref mon, ref day, ref jh);

                //txtGlobalEcl.AppendText("----------------------\n");
                //txtGlobalEcl.AppendText(eday.ToString() + "." + emon.ToString() + "." + eyear.ToString() + "\n");
                //txtGlobalEcl.AppendText(jh.ToString()+"\n");

                jh2 = Math.Truncate((jh - Math.Truncate(jh)) * 60);
                jh3 = (int)Math.Truncate(jh);

                //txtGlobalEcl.AppendText(jh3.ToString() + ":" + jh2.ToString());
                result = new DateTime(year, mon, day, jh3, (int)jh2, 0);
                return result;
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

        private void frmMain_Closed(object sender, EventArgs e)
        {
            using (var sweph = new SwissEphNet.SwissEph())
            {
                sweph.swe_close();
                //swe_set_ephe_path(NULL);
            }
        }
    }
}
