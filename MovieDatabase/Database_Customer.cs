using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase
{
    class Database_Customer
    {
        #region Set up SQL Connection

        //Create Connection and Command,and an Adapter.
        private SqlConnection Connection = new SqlConnection();

        private SqlCommand Command = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();

        //THE CONSTRUCTOR SETS THE DEFAULTS UPON LOADING THE CLASS


        #endregion

        #region database information
        public Database_Customer()
        {
            //change the connection string to run from your own music db
            string connectionString = @"Data Source=WT135-826LSW\SQLEXPRESS;Initial Catalog=VideoRental;Integrated Security=True";
            Connection.ConnectionString = connectionString;
            Command.Connection = Connection;
        }
        #endregion


        #region customers tab

        public DataTable FillDGVCustomersWithCustomers()
        {
            //create a datatable as we only have one table, the Owner
            DataTable dt = new DataTable();
            using (da = new SqlDataAdapter("select * from Customer ", Connection))
            {
                //connect in to the DB and get the SQL
                Connection.Open();
                //open a connection to the DB
                da.Fill(dt);
                //fill the datatable from the SQL
                Connection.Close(); //close the connection
            }
            return dt; //pass the datatable data to the DataGridView
        }

        #endregion

        #region fill form with database information

        public DataTable FillDGVTopCustomersWithTopCustomers()
        {
            //create a datatable as we only have one table, the Owner
            DataTable dt = new DataTable();
            using (da = new SqlDataAdapter("select * from TopCustomers ORDER BY MoviesRented DESC ", Connection))

            {
                //connect in to the DB and get the SQL
                Connection.Open();
                //open a connection to the DB
                da.Fill(dt);
                //fill the datatable from the SQL
                Connection.Close(); //close the connection
            }
            return dt; //pass the datatable data to the DataGridView
        }

        #endregion

        #region add customer
        //Add customer to database when data entered in form
        public string AddCustomer(string FirstName, string LastName, string Address, string Phone)
        {

            // this puts the parameters into the code so that the data in the text boxes is added to the database
            string NewEntry =
            "INSERT INTO Customer (FirstName, LastName, Address, Phone) VALUES ( @FirstName, @LastName, @Address, @Phone)";

            try
            {
                var newdata = new SqlCommand(NewEntry, Connection);
                newdata.Parameters.AddWithValue("@FirstName", FirstName);
                newdata.Parameters.AddWithValue("@LastName", LastName);
                newdata.Parameters.AddWithValue("@Address", Address);
                newdata.Parameters.AddWithValue("@Phone", Phone);

                Connection.Open(); //open a connection to the database

                newdata.ExecuteNonQuery(); //Run the Query

                //a happy message box
                Connection.Close();
                return "Data has been Inserted  !! ";
                
            }
            catch (Exception ex)
            {
                // need to get it to close a second time as it jumps the first connection.close when ExecuteNonQuery fails.
                Connection.Close();
                return " has Failed with " + ex;
            }
        }

        #endregion

        #region Delete Customer
        //Delete Customer from database
        public void DeleteCustomer(string CustomerID)
        {
            
                var myCommand = new SqlCommand("Delete Customer where CustID = @CustID");

                myCommand.Connection = Connection;
                myCommand.Parameters.AddWithValue("@CustID", CustomerID);
                Connection.Open();
                myCommand.ExecuteNonQuery();
                Connection.Close();
          
            
        }

        #endregion

        #region Update Customer
        //Update customer information from form
        public void UpdateCustomer(string CustID, string firstName, string lastName, string address, string phone)
        {
            //this updates existing data in the database where the ID of the data equals the ID in the text box

            var myCommand = new SqlCommand("Update Customer set FirstName=@FirstName, LastName=@LastName, Address=@Address, Phone=@Phone where CustID = @CustID", Connection);

            // create the parameters and pass the data from the textboxes 
            myCommand.Parameters.AddWithValue("@CustID", CustID);
            myCommand.Parameters.AddWithValue("@FirstName", firstName);
            myCommand.Parameters.AddWithValue("@LastName", lastName);
            myCommand.Parameters.AddWithValue("@Address", address);
            myCommand.Parameters.AddWithValue("@Phone", phone);

            Connection.Open();
            //it's NonQuery as data is only going up

            myCommand.ExecuteNonQuery();
            Connection.Close();
        }

        #endregion
    }
}

