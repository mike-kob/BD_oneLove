using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using BD_oneLove.Models;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.Tools.DataStorage
{
    internal class DataStorage : IDataStorage
    {

       public void AddUser(User t)
        {
            string sql = $"INSERT INTO [user] (password, login, rights) VALUES ('{t.Password}', " +
                 $"'{t.Username}', '{t.AccessType}'); ";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {

                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }


                using (SqlCommand command = new SqlCommand(sql, myConn))
                {

                    res = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);

            }
            finally
            {
                myConn?.Close();
            }
        }




        public void AddTeacher(Teacher t)
        {
            //мб добавить проверку на существование учителя
            string sql1 = $"INSERT INTO [user] (password, login, rights) VALUES ('{t.User.Password}', " +
               $"'{t.User.Username}', 'Классный руководитель'); ";

            string sql3 =  $"INSERT INTO head_teachers " +
                $"(tab_number, h_name, patronymic, surname, login) VALUES ('{t.TabNumber}', " +
                $"'{t.HName}', '{t.Patronymiс}', '{t.Surname}', '{t.User.Username}');";

           

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            User u = null;
            int res = 0;

            try
            {
            
                myConn.Open();
               
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    
                    command.ExecuteNonQuery();
                }

                if (!UserExistsUseless(t.User.Username))
                {
                    using (SqlCommand command = new SqlCommand(sql1, myConn))
                    {
                        res = command.ExecuteNonQuery();
                    }
                }

                using (SqlCommand command = new SqlCommand(sql3, myConn))
                {
                    res = command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }
        }


       public bool UserExistsUseless(string login)
        {
            
            string sql1 = $"SELECT COUNT(*) FROM [User] WHERE login='{login}'";
            string sql2 = $"SELECT COUNT(*) FROM [User] u INNER JOIN head_teachers ht ON u.login=ht.login" +
                $" WHERE u.login='{login}'";
            try
            {
                SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);

                myConn.Open();
                int count1 = 0;
                int count2 = 0;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    count1 = (int)command.ExecuteScalar();
                }
                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    count2 = (int)command.ExecuteScalar();
                }

                myConn.Close();
                return count1!=0 && count2==0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }

            return false;
        }

  
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
                    count = (int) command.ExecuteScalar();
                }

                myConn.Close();
                return count != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
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
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }

            return null;
        }

        public Class GetClass(string classId)
        {
            string sql1 = $"SELECT number, letter, st_year FROM classes WHERE class_id='{classId}'";

            Class res = new Class(classId);
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Number = reader.GetString(0);
                        res.Letter = reader.GetString(1);
                        res.StYear = reader.GetString(2);
                    }

                    reader.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return res;
        }

        public List<string> GetYears()
        {
            string sql = "SELECT st_year FROM Classes";
            List<string> list = new List<string>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string cur = reader.GetString(0).ToString();
                        list.Add(cur);
                    }

                    reader.Close();
                }

                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return list;

        }

        public List<Teacher> GetTeachers(string year)
        {
            string sql = "SELECT ht.tab_number, ht.h_name, ht.patronymic, c.class_id, c.number, c.letter, c.st_year, ht.surname" +
                         " FROM (head_teachers ht INNER JOIN head_teachers_classes htc ON ht.tab_number=htc.tab_number) " +
                         "INNER JOIN classes c ON c.class_id = htc.class_id "+
                         $"WHERE c.st_year='{year}'";

            List<Teacher> res = new List<Teacher>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Teacher cur = new Teacher(reader.GetString(0));
                        cur.HName = reader.GetString(1);
                        cur.Patronymiс = reader.GetString(2);
                        cur.Surname = reader.GetString(7);
                        Class c = new Class(reader.GetInt64(3).ToString());
                        c.Number = reader.GetString(4);
                        c.Letter = reader.GetString(5);
                        c.StYear = reader.GetString(6);
                        cur.Class = c;
                   
                        res.Add(cur);
                    }

                    reader.Close();
                }

                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return res;
        }

        public List<User> GetUsers()
        {
            string sql = "SELECT password, login, rights" +
                                  " FROM [user] ";

            List<User> res = new List<User>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        User cur = new User(reader.GetString(1), reader.GetString(0), reader.GetString(2));

                        res.Add(cur);
                    }

                    reader.Close();
                }

                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return res;
        }

        public List<Teacher> GetTeachers()
        {
            string sql = "SELECT ht.tab_number, ht.h_name, ht.patronymic, ht.surname, ht.login, u.password, u.rights" +
                         " FROM head_teachers ht INNER JOIN [user] u ON u.login=ht.login; "; 
           
            List<Teacher> res = new List<Teacher>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Teacher cur = new Teacher(reader.GetString(0));
                        cur.HName = reader.GetString(1);
                        cur.Patronymiс = reader.GetString(2);
                        cur.Surname = reader.GetString(3);
                        cur.User.Username = reader.GetString(4); 
                        cur.User.Password = reader.GetString(5);
                        cur.User.AccessType = reader.GetString(6);

                        res.Add(cur);
                    }

                    reader.Close();
                }

                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return res;
        }



        public List<Student> GetStudents(Class c)
        {
            string sql = "SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic," +
                         "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city," +
                         "s.street, s.house, s.apart, s.home_phone, s.gdp_attendance, s.exam " +
                         "FROM Students s INNER JOIN classes_students sc ON s.student_id=sc.student_id " +
                         $"WHERE sc.class_id='{c.ClassId}'";

            List<Student> res = new List<Student>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Student cur = new Student(reader.GetString(9), reader.GetInt64(0).ToString());
                        cur.TypeDoc = reader.GetString(1);
                        cur.SerDoc = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.NumDoc = reader.GetString(3);
                        cur.StName = reader.GetString(4);
                        cur.Patronymic = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.Surname = reader.GetString(6);
                        cur.Sex = reader.GetString(7);
                        cur.Birthday = reader.GetDateTime(8).ToShortDateString();
                        cur.Index = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        cur.City = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.Street = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.House = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        cur.Apart = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        cur.HomePhone = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        cur.GpdAttendance = !reader.IsDBNull(16) && reader.GetBoolean(16);
                        cur.ExamAllowedToPass = !reader.IsDBNull(17) && reader.GetBoolean(17);

                        res.Add(cur);
                    }

                    reader.Close();
                }

                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return res;
        }
    }



}