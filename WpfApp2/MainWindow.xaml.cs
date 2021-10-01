using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        string cs = "";
        DataTable table;
        SqlDataReader reader;
        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from Authors";
                command.Connection = conn;
                table = new DataTable();

                bool hasColumnAdded = false;
                using (reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (!hasColumnAdded)
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            hasColumnAdded = true;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                    mydatagrid.ItemsSource = table.DefaultView;

                }



            }
        }
        DataSet set;
        SqlDataAdapter da;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {



            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();
                set = new DataSet();

                da = new SqlDataAdapter("select* from Authors;select * from Books", conn);
              //  da = new SqlDataAdapter(txtbox1.Text, conn);
                mydatagrid.ItemsSource = null;
                da.Fill(set, "mybook");
                mydatagrid.ItemsSource = set.Tables[0].DefaultView;
               
                mydatagrid2.ItemsSource = set.Tables[1].DefaultView;

            }



        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();

                ////WITH STORED PROCEDUR
                ///
                SqlCommand sqlCommand = new SqlCommand("sp_UpdateBook", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var param1 = new SqlParameter();
                param1.SqlDbType = SqlDbType.Int;
                param1.ParameterName = "@page";
                param1.Value = 0;
                sqlCommand.Parameters.Add(param1);

                var param2 = new SqlParameter();
                param2.SqlDbType = SqlDbType.Int;
                param2.ParameterName = "@bookId";
                param2.Value = 2;
                sqlCommand.Parameters.Add(param2);

                sqlCommand.ExecuteNonQuery();

                //                SqlCommand UpdateCommand = new SqlCommand(@"
                //        UPDATE Books
                //        SET Pages = @pPage where Id=@pId
                //", conn);
                //                var param1 = new SqlParameter();
                //                param1.SqlDbType = SqlDbType.Int;
                //                param1.ParameterName = "@pPage";
                //                param1.Value = 1111;
                //                UpdateCommand.Parameters.Add(param1);

                //                var param2 = new SqlParameter();
                //                param2.SqlDbType = SqlDbType.Int;
                //                param2.ParameterName = "@pId";
                //                param2.Value = 2;
                //                UpdateCommand.Parameters.Add(param2);

                //                UpdateCommand.ExecuteNonQuery();

            }
        }

        private void mydatagrid2_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
