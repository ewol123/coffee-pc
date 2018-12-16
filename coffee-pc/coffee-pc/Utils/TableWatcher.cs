using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace coffee_pc.Utils
{
    class TableWatcher
    {
        public void WatchOrders()
        {

            var connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=testapi123;Integrated Security=True;MultipleActiveResultSets=True";
            var tableName = "Orders";
            var tableDependency = new SqlTableDependency<OrderEntity>(connectionString, tableName);

            tableDependency.OnChanged += OnNotificationReceived;
            tableDependency.Start();
        }

        private void OnNotificationReceived(object sender,
                          RecordChangedEventArgs<OrderEntity> e)
        {
            switch (e.ChangeType)
            {
                case ChangeType.Delete:
                    System.Diagnostics.Debug.WriteLine($"{e.Entity} has been deleted");
                    break;

                case ChangeType.Update:
                    System.Diagnostics.Debug.WriteLine($"{e.Entity} has been updated");
                    break;

                case ChangeType.Insert:
                    System.Diagnostics.Debug.WriteLine($"{e.Entity} has been inserted");
                    break;
            }
        }


        public DataTable Watch() {
            var connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=testapi123;Integrated Security=True;MultipleActiveResultSets=True";

            SqlDependency.Start(connectionString);

            DataTable dt = new DataTable();

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                SqlCommand cmd =
                    new SqlCommand("SELECT Status FROM dbo.Orders", connection);
                // Clear any existing notifications
                cmd.Notification = null;

                // Create the dependency for this command
                SqlDependency dependency = new SqlDependency(cmd);
                
                // Add the event handler
                dependency.OnChange +=
                       new OnChangeEventHandler(OnChange);

                // Open the connection if necessary
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Get the messages
                dt.Load(cmd.ExecuteReader(
                          CommandBehavior.CloseConnection));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        private void OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;

            // Notices are only a one shot deal
            // so remove the existing one so a new 
            // one can be added
            System.Diagnostics.Debug.WriteLine("changed");

                dependency.OnChange -= OnChange;

            // Fire the event
            dependency.OnChange +=
                      new OnChangeEventHandler(OnChange);
        }
    }
}
