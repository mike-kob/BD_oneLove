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