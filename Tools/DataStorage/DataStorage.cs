using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
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
                         "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam " +
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
                        Student cur = new Student();
                        cur.NumAlphBook = reader.GetString(9);
                        cur.Id = reader.GetInt64(0).ToString();

                        cur.TypeDoc = reader.GetString(1);
                        cur.SerDoc = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.NumDoc = reader.GetString(3);
                        cur.StName = reader.GetString(4);
                        cur.Patronymic = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.Surname = reader.GetString(6);
                        cur.Sex = reader.GetString(7);
                        cur.Birthday = reader.GetDateTime(8);
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

        public Student SaveStudent(Student s)
        {
            string sql1 =
                "INSERT INTO students (type_doc, ser_doc, num_doc, st_name, patronymic,surname, sex, birthday, num_alph_book, [index], city, street, house, apart, home_phone, gpd_attendance, exam) " +
                $"VALUES ('{s.TypeDoc}', @serdoc, '{s.NumDoc}'," +
                        $"'{s.StName}', @patr, '{s.Surname}'," +
                        $"'{s.Sex}','{s.Birthday}','{s.NumAlphBook}'," +
                        $"@ind, @city, @street, @house, @apart," +
                        $"@phone, '{s.GpdAttendance}', '{s.ExamAllowedToPass}')";
            string sql2 = $"SELECT student_id FROM students WHERE num_alph_book='{s.NumAlphBook}'";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.Add("@serdoc", SqlDbType.VarChar);
                    command.Parameters.Add("@patr", SqlDbType.VarChar);
                    command.Parameters.Add("@ind", SqlDbType.Char);
                    command.Parameters.Add("@city", SqlDbType.VarChar);
                    command.Parameters.Add("@street", SqlDbType.VarChar);
                    command.Parameters.Add("@house", SqlDbType.VarChar);
                    command.Parameters.Add("@apart", SqlDbType.VarChar);
                    command.Parameters.Add("@phone", SqlDbType.VarChar);

                    command.Parameters["@serdoc"].Value = (object)s.SerDoc ?? DBNull.Value;
                    command.Parameters["@patr"].Value = (object)s.Patronymic ?? DBNull.Value;
                    command.Parameters["@ind"].Value = (object)s.Index ?? DBNull.Value;
                    command.Parameters["@city"].Value = (object)s.City ?? DBNull.Value;
                    command.Parameters["@street"].Value = (object)s.Street ?? DBNull.Value;
                    command.Parameters["@house"].Value = (object)s.House ?? DBNull.Value;
                    command.Parameters["@apart"].Value = (object)s.Apart ?? DBNull.Value;
                    command.Parameters["@phone"].Value = (object)s.HomePhone ?? DBNull.Value;
                   
                    res = command.ExecuteNonQuery();
                }

                if (res == 0)
                    return null;

                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        s.Id = reader.GetInt64(0).ToString();
                    }

                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return null;
            }
            finally
            {
                myConn?.Close();
            }

            return s;
        }

        public Student UpdateStudent(Student s)
        {
            string sql1 =
                "UPDATE students " +
                $"SET type_doc = '{s.TypeDoc}', " +
                    $"ser_doc = @serdoc, " +
                    $"num_doc = '{s.NumDoc}', " +
                    $"st_name = '{s.StName}', " +
                    $"patronymic = @patr, " +
                    $"surname = '{s.Surname}', " +
                    $"sex = '{s.Sex}', " +
                    $"birthday = @date, " +
                    $"num_alph_book = '{s.NumAlphBook}', " +
                    $"[index] = @ind, city=@city, street=@street, house=@house, apart=@apart, " +
                    $"home_phone = @phone, " +
                    $"gpd_attendance = '{s.GpdAttendance}', " +
                    $"exam = '{s.ExamAllowedToPass}' " +
                $"WHERE student_id={s.Id}"; 
  
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            int res = 0;
            try
            {
                myConn.Open();
                
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.Add("@serdoc", SqlDbType.VarChar);
                    command.Parameters.Add("@patr", SqlDbType.VarChar);
                    command.Parameters.Add("@date", SqlDbType.Date);
                    command.Parameters.Add("@ind", SqlDbType.Char);
                    command.Parameters.Add("@city", SqlDbType.VarChar);
                    command.Parameters.Add("@street", SqlDbType.VarChar);
                    command.Parameters.Add("@house", SqlDbType.VarChar);
                    command.Parameters.Add("@apart", SqlDbType.VarChar);
                    command.Parameters.Add("@phone", SqlDbType.VarChar);

                    command.Parameters["@serdoc"].Value = (object)s.SerDoc ?? DBNull.Value;
                    command.Parameters["@patr"].Value = (object)s.Patronymic ?? DBNull.Value;
                    command.Parameters["@date"].Value = s.Birthday;
                    command.Parameters["@ind"].Value = (object)s.Index ?? DBNull.Value;
                    command.Parameters["@city"].Value = (object)s.City ?? DBNull.Value;
                    command.Parameters["@street"].Value = (object)s.Street ?? DBNull.Value;
                    command.Parameters["@house"].Value = (object)s.House ?? DBNull.Value;
                    command.Parameters["@apart"].Value = (object)s.Apart ?? DBNull.Value;
                    command.Parameters["@phone"].Value = (object)s.HomePhone ?? DBNull.Value;

                    res = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return null;
            }
            finally
            {
                myConn?.Close();
            }
            if (res == 0)
                return null;
            return s;
        }

        public bool AssignStudentToClass(Student s, Class c)
        {
            string sql1 =
                "INSERT INTO classes_students (student_id, class_id)" +
                $"VALUES ('{s.Id}', '{c.ClassId}')";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    res = command.ExecuteNonQuery();
                }

                return res == 0;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return false;
            }
            finally
            {
                myConn?.Close();
            }
        }

        public bool ExpelStudent(Student s, Class c)
        {
            string sql1 =
                "DELETE FROM classes_students " +
                $"WHERE student_id='{s.Id}' AND class_id='{c.ClassId}'";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    res = command.ExecuteNonQuery();
                }

                return res == 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return false;
            }
            finally
            {
                myConn?.Close();
            }
        }

        public Parent GetFather(Student s)
        {
            string sql1 = $"SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                          $"FROM parents WHERE parent_id IN (SELECT father_id FROM students WHERE student_id={s.Id})";
            
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            Parent father = new Parent();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        father.Id = reader.GetInt64(0).ToString();
                        father.PName = reader.GetString(1);
                        father.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        father.Surname = reader.GetString(3);

                        father.Sex = reader.GetString(4);
                        father.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        father.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        father.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        father.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        father.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        father.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        father.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        father.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        father.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        father.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        


                    }

                    reader.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return null;
            }
            finally
            {
                myConn?.Close();
            }

            return father;
        }

        public Parent GetMother(Student s)
        {
            string sql1 = $"SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                          $"FROM parents WHERE parent_id IN (SELECT mother_id FROM students WHERE student_id={s.Id})";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            Parent mother = new Parent();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        mother.Id = reader.GetInt64(0).ToString();
                        mother.PName = reader.GetString(1);
                        mother.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        mother.Surname = reader.GetString(3);

                        mother.Sex = reader.GetString(4);
                        mother.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        mother.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        mother.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        mother.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        mother.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        mother.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        mother.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        mother.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        mother.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        mother.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);



                    }

                    reader.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return null;
            }
            finally
            {
                myConn?.Close();
            }

            return mother;
        }

    }
}