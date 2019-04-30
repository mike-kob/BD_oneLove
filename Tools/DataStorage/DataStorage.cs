using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BD_oneLove.Models;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.Tools.DataStorage
{
    internal class DataStorage : IDataStorage
    {
        public bool UserExists(string login, string password)
        {
            string sql = $"SELECT COUNT(*) FROM \"User\" WHERE login='{login}' AND password='{password}'";
            try
            {
                SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);

                myConn.Open();
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    count = (int)command.ExecuteScalar();
                  
                }
                myConn.Close();
                return count != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection! " + ex.Message);
            }

            return false;
        }

        public User GetUser(string login, string password)
        {
            string sql = $"SELECT rights FROM \"User\" WHERE login='{login}' AND password='{password}'";
            try
            {
                SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
                myConn.Open();
                User user = null;
                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new User(login, password, reader.GetString(0));
                    }
                    
                    reader.Close();
                }
                myConn.Close();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection! " + ex.Message);
            }

            return null;
        }
    }
}
