using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Data;
using System.Text;
using System.IO;
using System.Globalization;
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

            ecldat1.SelectedDate = DateTime.UtcNow;
            ecldat2.SelectedDate = DateTime.UtcNow.AddYears(1);
            //SwissEphNet.SwissEph.SE_EPHE_PATH = "0";
            
            using (var sweph = new SwissEphNet.SwissEph())
            {
                sweph.swe_set_ephe_path("C:\\SWEPH\\EPHE\\");
                
                sweph.swe_set_jpl_file("E:\\EPHE\\");
                //MessageBox.Show( sweph.swe_get_library_path());
                
                //swe_set_ephe_path(NULL);
            }

            string sql = "SELECT rd_users.id, rd_users.fname, rd_users_data.birthday, " +
                "rd_users_data.birthtime, rd_users_data.remark, rd_users_data.dateupd, " +
                "rd_city.cityname, rd_city.longitude, rd_city.latitude, rd_city.gmt " +
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
                //adapter.Update(dtUsers);
                

                //int t = dtUsers.Rows.Count;
                recCount.Content += " " + dtUsers.Rows.Count.ToString();


                //DataRow dr = dtUsers.Rows[0];
                //MessageBox.Show(dr["fname"].ToString());
                //dr["birthday"] = DateTime.ParseExact(tmp, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                
                //dr["birthday"] = "01/11/1978";
                //MessageBox.Show(dr["birthday"].ToString());


                usersGrid.ItemsSource = dtUsers.DefaultView;
                

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
            //if(usersGrid.SelectedItems.Count == 0) return;
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (usersGrid.SelectedItems.Count == 0) return;
                string clientID;
                //clientID = (DataRowView)(usersGrid.SelectedItems[0]).Row["id"];
                clientID = ((DataRowView)usersGrid.SelectedItems[0]).Row["id"].ToString();
                //MessageBox.Show(clientID.ToString());

                CliData cliwin = new CliData(clientID);
                cliwin.Owner = this;
                cliwin.ShowDialog();
                //cliwin.Close();

                //frmMain.Focus();
                //usersGrid.ItemsSource = null;
                //usersGrid.ItemsSource = dtUsers.DefaultView;
                //usersGrid.Items.Refresh();

                CollectionViewSource.GetDefaultView(usersGrid.ItemsSource).Refresh();
                //usersGrid.Focus();

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

                //var y = usersGrid.Items.IndexOf(usersGrid.CurrentItem);
                //DataRow dr = dtUsers.Rows[y];
                
                DateTime beginUtc, endUtc, ReJul;

                //DateTime.UtcNow.AddYears(1);

                beginUtc = (DateTime)ecldat1.SelectedDate;
                endUtc = (DateTime)ecldat2.SelectedDate;

                var jUTbegin = Julie(beginUtc);
                var jUTend = Julie(endUtc);

                double[] tret = new double[10];
                double cliJulday;
                double cliLat, cliLon;
                string clientName, tmp, merr = "";
                int cliGMT, iftype = 0;
                short iOrbis = Convert.ToInt16(inpOrbis.Text);

                // ******** Посчитаем данные выбранного клиента ********

                if (usersGrid.SelectedItems.Count == 0) return;

                //DataRow dr = dtUsers.Rows[usersGrid.SelectedItems[0]];
                var cri = usersGrid.Items.IndexOf(usersGrid.CurrentItem);
                var dr = dtUsers.Rows[cri];
                //MessageBox.Show(dr["birthday"].ToString());
                //MessageBox.Show(dr["date"].ToString());

                clientName = ((DataRowView)usersGrid.SelectedItems[0]).Row[1].ToString();
                //MessageBox.Show(((DataRowView)usersGrid.SelectedItems[0]).Row[5].ToString());
                //cliLon = double.Parse(((DataRowView)usersGrid.SelectedItems[0]).Row[5].ToString());
                //cliLat = double.Parse(((DataRowView)usersGrid.SelectedItems[0]).Row[6].ToString());
                DateTime clientBDate;
                //clientBDate = (DateTime)((DataRowView)usersGrid.SelectedItems[0]).Row[2];
                clientBDate = Convert.ToDateTime(dr["birthday"]);


                tmp = clientBDate.ToString("g", CultureInfo.GetCultureInfo("ru-RU"));
                MessageBox.Show(tmp);
                //tmp = (DateTime)((DataRowView)usersGrid.SelectedItems[0]).Row[3];
                //clientBDate += TimeSpan.Parse(((DataRowView)usersGrid.SelectedItems[0]).Row[3].ToString());
                //clientBDate = Convert.ToDateTime( dr["birthtime"]);
                tmp = dr["birthtime"].ToString();
                clientBDate += TimeSpan.Parse( tmp);

                

                cliGMT = Convert.ToInt32(dr["gmt"]);

                cliLat = Convert.ToDouble(dr["latitude"]);
                cliLon = Convert.ToDouble(dr["longitude"]);

                //MessageBox.Show(cliLat.ToString());

                txtIndiEcl.Clear();

                // ******** Отнимем поправку на gmt ********
                //clientBDate += cliGMT;
                
                MessageBox.Show(clientBDate.ToString());
                clientBDate.AddHours(-6);
                MessageBox.Show(clientBDate.ToString());

                cliJulday = Julie(clientBDate);

                // ******** Расчитываем все планеты и дома клиента ********
                double[] cliPlPos = new double[100];
                string plnName;
                int i = SwissEphNet.SwissEph.SE_SUN;

                using (var swephcli = new SwissEphNet.SwissEph())
                {
                    swephcli.swe_set_ephe_path("e:\\ephe");
                    swephcli.OnLoadFile += Swephcli_OnLoadFile;
                    
                    // ******** Планетки ********

                    while (i < SwissEphNet.SwissEph.SE_PHOLUS)
                    {
                        if (!(i == SwissEphNet.SwissEph.SE_EARTH))
                        {
                            var clipl = swephcli.swe_calc_ut(cliJulday, i, SwissEphNet.SwissEph.SEFLG_SPEED | SwissEphNet.SwissEph.SEFLG_SWIEPH, tret, ref merr);
                            cliPlPos[i] = tret[0];
                            plnName = swephcli.swe_get_planet_name(i);
                            txtIndiEcl.AppendText(plnName + " - " + tret[0].ToString() + "\n");
                            //txtIndiEcl.AppendText(merr + "\n");
                            i++;
                        }
                        else i++;

                    }
                    
                    // ******** Домишки ********

                    double[] hcusp = new double[13];
                    double[] ascmc = new double[10];
                    char hsys = char.Parse("P");
                    int z;
                    cliLat = 54.58;
                    cliLon = 73.23;
                    txtIndiEcl.AppendText(cliJulday.ToString() + "\n");
                    z = swephcli.swe_houses(cliJulday, cliLat, cliLon, 'P', hcusp, ascmc);
                    i = 0;
                    for (i = 1; i <= 12; i++)
                    {
                        txtIndiEcl.AppendText("House №:" + i.ToString() + " " + hcusp[i].ToString() + "\n");
                    }
                    
                }


                //tmp = dr["birthday"].ToString();
                //tmp = string.Format("{dd-MM-yyyy}", tmp);

                //txtGlobalEcl.AppendText(tmp + "\n");
                //+ " " + ((DataRowView)usersGrid.SelectedItems[0]).Row[3];

                //clientBDate = (DateTime)tmp;

                //clientBDate = DateTime.ParseExact(tmp, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                //txtGlobalEcl.AppendText(clientBDate.ToString() + "\n");
                //txtGlobalEcl.AppendText(clientBDate.ToString() + "\n");



                // ******** Перебираем все солнечные затмения ********

                txtIndiEcl.AppendText("Солнечные затмения:\n");

                while (jUTbegin < jUTend)
                {

                    using (var sweph = new SwissEphNet.SwissEph())
                    {
                        // ******** Считаем затмение ********
                        var ef = sweph.swe_sol_eclipse_when_glob(jUTbegin, 0, iftype, tret, false, ref merr);

                        //txtGlobalEcl.AppendText(jUTbegin.ToString() + "\n");

                        jUTbegin = tret[0];

                        ReJul = ReJulie(jUTbegin);
                        txtIndiEcl.AppendText(ReJul.ToString() + "\n");

                        //txtGlobalEcl.AppendText("****************\n");
                        // ******** Считаем положение солнца в этот день ********
                        var ef_sun = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_SUN, 0, tret, ref merr);
                        //txtGlobalEcl.AppendText("Солнце:" + tret[0].ToString() + "\n");

                        // ******** Считаем положение луны в этот день ********
                        var ef_moon = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_MOON, 0, tret, ref merr);
                        //txtGlobalEcl.AppendText("Луна:" + tret[0].ToString() + "\n");
                        //txtGlobalEcl.AppendText("****************\n");
                        //SwissEphNet.SwissEph.SEFLG_SPEED
                    }

                }
                
                txtIndiEcl.AppendText("Лунные затмения:\n");
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
                        txtIndiEcl.AppendText(ReJul.ToString() + "\n");

                        // ******** Считаем положение солнца в этот день ********
                        var ef_sun = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_SUN, 0, tret, ref merr);
                        //txtGlobalEcl.AppendText("Солнце:" + tret[0].ToString() + "\n");

                        // ******** Считаем положение луны в этот день ********
                        var ef_moon = sweph.swe_calc_ut(jUTbegin, SwissEphNet.SwissEph.SE_MOON, 0, tret, ref merr);
                        //txtGlobalEcl.AppendText("Луна:" + tret[0].ToString() + "\n");
                        //txtGlobalEcl.AppendText("****************\n");

                    }

                }


                tbsMain.SelectedItem = tbsMain.Items[3];
            }
        }

        private void Swephcli_OnLoadFile(object sender, SwissEphNet.LoadFileEventArgs e)
        {
            if (e.FileName.StartsWith("[ephe]"))
            {
                e.File = SearchFile(e.FileName.Replace("[ephe]", string.Empty));
            }
            else
            {
                var f = e.FileName;
                if (System.IO.File.Exists(f))
                    e.File = new System.IO.FileStream(f, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            }
            //Encoding enc = e.Encoding;
            //e.File = Provider.LoadFile(e.FileName, out enc);
            //e.Encoding = enc;
            //Provider.Debug.WriteLine($"Required file:{e.FileName} => {(e.File != null ? "OK" : "Not found")}");

            //sweph.OnLoadFile += (s, e) => {
            // Loading file
            //};
        }

        private Stream SearchFile(string v)
        {
            throw new NotImplementedException();
        }

        private List<double> EcclipseList(DateTime startDate, DateTime endDate, List<double> eclDates)
        {

            var jUTbegin = Julie(startDate);
            var jUTend = Julie(endDate);

            //List<double> eclDates = new List<double>(20);

            double[] tret = new double[10];
            string merr = "";
            int iftype = 0;
            DateTime ReJul;

            // ******** Перебираем все солнечные затмения ********

            while (jUTbegin <= jUTend)
            {

                using (var sweph = new SwissEphNet.SwissEph())
                {
                    // ******** Считаем затмение ********
                    var ef = sweph.swe_sol_eclipse_when_glob(jUTbegin, 0, iftype, tret, false, ref merr);

                    //txtGlobalEcl.AppendText(jUTbegin.ToString() + "\n");
                    if (tret[0] < jUTend)
                    {
                        jUTbegin = tret[0];

                        ReJul = ReJulie(jUTbegin);
                        eclDates.Add(jUTbegin);
                    }
                    else jUTbegin = tret[0];
                }

            }

            //jUTbegin = Julie((DateTime)ecldat1.SelectedDate);
            //jUTend = Julie((DateTime)ecldat2.SelectedDate);
            //eclDates.Add(-1);

            // ******** Перебираем все лунные затмения ********

            while (jUTbegin <= jUTend)
            {

                using (var sweph = new SwissEphNet.SwissEph())
                {
                    // ******** Считаем затмение ********
                    var ef = sweph.swe_lun_eclipse_when(jUTbegin, 0, iftype, tret, false, ref merr);

                    if (tret[0] < jUTend)
                    {
                        jUTbegin = tret[0];

                        ReJul = ReJulie(jUTbegin);
                        eclDates.Add(jUTbegin);
                    }
                    else jUTbegin = tret[0];

                }

            }


            return eclDates;
        }


        private double JulieUT(DateTime jUT)
        {
            using (var sweph = new SwissEphNet.SwissEph())
            {
                double[] tret = new double[2];
                string merr = "";

                var jday = Convert.ToInt16(jUT.Day);
                var jmon = Convert.ToInt16(jUT.Month);
                var jyear = Convert.ToInt16(jUT.Year);
                var jhour = Convert.ToInt16(jUT.Hour);
                var jmin = Convert.ToInt16(jUT.Minute);

                //var jsec = 1;
                //var jut = jhour + (jmin / 60.0) + (jsec / 3600.0);

                //var JulianDateUT = sweph.swe_julday(jyear, jmon, jday, jut, 1);
                int JulianDateUT = sweph.swe_utc_to_jd(jyear, jmon, jday, jhour, jmin, 0, SwissEphNet.SwissEph.SE_GREG_CAL, tret, ref merr);
                //MessageBox.Show(merr);
                return tret[1];
                //return sweph.swe_julday
            }
        }

        private double Julie(DateTime jUT)
        {
            using (var sweph = new SwissEphNet.SwissEph())
            {
                //double[] tret = new double[2];
                //string merr = "";

                var jday = Convert.ToInt16(jUT.Day);
                var jmon = Convert.ToInt16(jUT.Month);
                var jyear = Convert.ToInt16(jUT.Year);
                var jhour = Convert.ToInt16(jUT.Hour);
                var jmin = Convert.ToInt16(jUT.Minute);

                var jsec = 1;
                var jut = jhour + (jmin / 60.0) + (jsec / 3600.0);
                //jut -= GMT;

                //var JulianDateUT = sweph.swe_julday(jyear, jmon, jday, jut, 1);
                //int JulianDateUT = sweph.swe_utc_to_jd(jyear, jmon, jday, jhour, jmin, 0, SwissEphNet.SwissEph.SE_GREG_CAL, tret, ref merr);
                //MessageBox.Show(jut.ToString());
                return sweph.swe_julday(jyear, jmon, jday, jut, 1);
                //return sweph.swe_julday
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

        private void usersGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            //messagebox.show(header);

            //MessageBox.Show(header);

            if (header == "birthday")
            {
                e.Column.Header = "Дата";
                e.Column.ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                //e.Column.HeaderStyle.
                //dataGridView1.Columns[0].Header = "Number";
                //usersGrid.Columns[2].HeaderStyle.Re
            }

        }

        private void usersGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            //MessageBox.Show("Сука ебанная !!!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<double> lstEcclipses = new List<double>(20);
            EcclipseList((DateTime)ecldat1.SelectedDate, (DateTime)ecldat2.SelectedDate, lstEcclipses);
            DateTime dt;

            txtGlobalEcl.Clear();
            txtGlobalEcl.AppendText("Солнечные затмения:\n");

            foreach (var dEcclipse in lstEcclipses)
            {
                if (dEcclipse > 0)
                {
                    dt = ReJulie(dEcclipse);
                    txtGlobalEcl.AppendText(dt.ToString() + "\n");
                }
                else
                {
                    txtGlobalEcl.AppendText("Лунные затмения:\n");
                }
                
            }
        }
    }
}
