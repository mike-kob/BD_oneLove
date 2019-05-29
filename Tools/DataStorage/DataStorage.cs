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

        public List<Class> GetClassesStatistics(string year, string type)
        {
                    string sql = "WITH Temp AS( " +
            "SELECT s.student_id, c.class_id, MIN(grade) AS min_riven " +
            "FROM(marks AS m INNER JOIN classes AS c ON m.class_id = c.class_id) INNER JOIN Students s ON s.student_id = m.student_id " +
            $"WHERE mark_type = '{type}' AND st_year = '{year}' " +
            "GROUP BY s.student_id, c.class_id) " +
            " SELECT class_id,[1] AS CN,[2] AS BN,[3] AS MN,[4]  AS GN,[5] AS HN, [1]+[2]+[3]+[4]+[5] AS Summ " +
            "FROM Temp " +
            "PIVOT(COUNT(student_id) FOR min_riven IN([1],[2],[3],[4],[5])) AS NumTable; ";

            List<Class> res = new List<Class>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                
                    while (reader.Read())
                    {
                        Class cur = GetClass(reader.GetInt64(0).ToString());
                        cur.Sum = reader.GetInt32(6);
                        cur.HighNumber = reader.GetInt32(5);
                        cur.GoodNumber = reader.GetInt32(4);
                        cur.MiddleNumber = reader.GetInt32(3);
                        cur.BeginNumber = reader.GetInt32(2);
                        cur.CriticalNumber = reader.GetInt32(1);
                        res.Add(cur);
                      
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
        public List<Student> GetStudentsStatistics(Class c, string type)
        {
            string sql = $"WITH Temp AS( " +
                            $"SELECT m.grade, m.student_id, m.subject " +
                            $"FROM Marks AS m INNER JOIN Classes AS c ON c.class_id= m.class_id " +
                            $"WHERE c.class_id= '{c.ClassId}' AND m.mark_type = '{type}') " +
                         $"SELECT student_id, [5] AS HN, [4] AS GN,[3] AS MN,[2] AS BN,[1] AS CN,[1]+[2]+[3]+[4]+[5] AS Summ, " +
                         $"CAST((5*[5]+4*[4]+3*[3]+2*[2]+1*[1]) AS float)/([1]+[2]+[3]+[4]+[5]) AS Middle " +
                         $"FROM Temp " +
                         $"PIVOT(COUNT(subject) FOR grade IN([1],[2],[3],[4],[5])) AS NumTable  " +
                         $"UNION " +
                         $"SELECT s.student_id, '0' AS HN, '0' AS GN,'0' AS MN,'0' AS BN,'0' AS CN,'0' AS Summ, " +
                         $"CAST('0' AS float) AS Middle " +
                         $"FROM students s INNER JOIN classes_students cs ON s.student_id=cs.student_id " +
                         $"WHERE cs.class_id= '{c.ClassId}' AND  s.student_id NOT IN (" +
                         $"                                                     SELECT student_id " +
                         $"                                                     FROM Temp) " +
                         $"ORDER BY  Middle DESC;  ";



            List <Student> res = new List<Student>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    int i = 1;
                    while (reader.Read())
                    {
                        Student cur = GetStudent(reader.GetInt64(0).ToString());
                        cur.Sum = reader.GetInt32(6);
                        cur.HighNumber = reader.GetInt32(1);
                        cur.GoodNumber = reader.GetInt32(2);
                        cur.MiddleNumber = reader.GetInt32(3);
                        cur.BeginNumber = reader.GetInt32(4);
                        cur.CriticalNumber = reader.GetInt32(5);
                        cur.MiddleMark = reader.GetDouble(7);
                        cur.Number = i;
                        res.Add(cur);
                        i++;
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

        public Student GetStudent(string id)
        {
            string sql = $"SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic, " +
                         "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city, " +
                         "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam " +
                         "FROM students s " +
                         $"WHERE s.student_id='{id}';";
            Student cur = null;
            try
            {
                SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cur = new Student();
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
                    }

                    reader.Close();
                }

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }

            return cur;
        }

        public List<Subject> GetSubjectsStatistics(Class c, string type)
        {
            string sql = $"WITH Temp AS( " +
                         $"SELECT m.grade, m.student_id, m.subject " +
                         $"FROM Marks AS m INNER JOIN Classes AS c ON c.class_id= m.class_id " +
                         $"WHERE c.class_id= '{c.ClassId}' AND mark_type = '{type}') " +
                         $"SELECT subject, [5] AS HN, [4] AS GN,[3] AS MN,[2] AS BN,[1] AS CN,[1]+[2]+[3]+[4]+[5] AS Summ " +
                         $"FROM Temp " +
                         $"PIVOT(COUNT(student_id) FOR grade IN([1],[2],[3],[4],[5])) AS NumTable; ";

            List<Subject> res = new List<Subject>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Subject cur = new Subject(reader.GetInt32(6), reader.GetInt32(1), reader.GetInt32(2),
                            reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                        cur.Name = reader.GetString(0);
                        res.Add(cur);
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

        public bool DeleteTeacherClass(Teacher t, Class c)
        {
            string sql =
                $"DELETE FROM head_teachers_classes WHERE class_id = '{c.ClassId}' AND tab_number='{t.TabNumber}'; ";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool AddTeacherClass(Teacher t, Class c)
        {
            string sql = $"INSERT INTO head_teachers_classes (class_id,tab_number) VALUES ('{c.ClassId}', " +
                         $"'{t.TabNumber}'); ";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public Teacher GetTeacher(string id)
        {
            string sql = $"SELECT h_name, patronymic, surname FROM head_teachers WHERE tab_number='{id}'";
            try
            {
                SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
                myConn.Open();
                Teacher t = null;
                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        t = new Teacher(id);
                        t.HName = reader.GetString(0);
                        t.Patronymiс = reader.GetString(1);
                        t.Surname = reader.GetString(2);
                    }

                    reader.Close();
                }

                myConn.Close();
                return t;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }

            return null;
        }

        //------------------Classes----------------

        #region Classes

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

        public bool DeleteClass(Class c)
        {
            string sql = $"DELETE FROM classes WHERE class_id = '{c.ClassId}'; ";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool AddClass(Class c)
        {
            string sql = $"INSERT INTO classes (number, letter, st_year) VALUES ('{c.Number}', " +
                         $"'{c.Letter}', '{c.StYear}'); ";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool UpdateClass(Class c)
        {
            string sql = "UPDATE classes " +
                         $"SET number='{c.Number}', " +
                         $" letter = '{c.Letter}', " +
                         $" st_year = '{c.StYear}' " +
                         $"WHERE class_id ='{c.ClassId}';";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public Class GetCurrentClass(User u)
        {
            string sql1 = "SELECT c.class_id, c.number, c.letter, c.st_year " +
                          "FROM classes c INNER JOIN head_teachers_classes htc on c.class_id = htc.class_id " +
                          "WHERE htc.tab_number IN(SELECT tab_number" +
                          "                         FROM head_teachers" +
                          $"                        WHERE login='{u.Username}') " +
                          "AND c.st_year = (SELECT cur_st_year" +
                          "                 FROM helping_st_year);";

            Class res = new Class();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.ClassId = reader.GetInt64(0).ToString();
                        res.Number = reader.GetString(1);
                        res.Letter = reader.GetString(2);
                        res.StYear = reader.GetString(3);
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

        public List<Student> GetStudents()
        {
            string sql = "SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic," +
                         "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city," +
                         "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam " +
                         "FROM Students s; ";

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

        public List<Student> GetFreeStudents(Class c)
        {
            string sql = "SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic," +
                         "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city," +
                         "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam " +
                         "FROM Students s " +
                         "WHERE s.student_id NOT IN (SELECT st.student_id" +
                         "                          FROM ((students st INNER JOIN classes_students cs ON st.student_id=cs.student_id) " +
                         "                          INNER JOIN classes c ON cs.class_id=c.class_id) " +
                         "                          INNER JOIN helping_st_year h ON c.st_year=h.cur_st_year)" +
                         "      AND s.student_id IN (SELECT student_id" +
                         "                         FROM classes_students" +
                        $"                         WHERE class_id={c.ClassId}); ";

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

        public List<Student> GetFreeStudents()
        {
            string sql = "SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic," +
                         "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city," +
                         "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam " +
                         "FROM Students s " +
                         "WHERE s.student_id NOT IN (SELECT st.student_id" +
                         "                          FROM ((students st INNER JOIN classes_students cs ON st.student_id=cs.student_id) " +
                         "                          INNER JOIN classes c ON cs.class_id=c.class_id) " +
                         "                          INNER JOIN helping_st_year h ON c.st_year=h.cur_st_year); ";

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

        public List<Teacher> GetTeachers(Class c)
        {
            string sql =
                "SELECT ht.tab_number, ht.h_name, ht.patronymic, ht.surname" +
                " FROM (head_teachers ht INNER JOIN head_teachers_classes htc ON ht.tab_number=htc.tab_number) " +
                $"WHERE htc.class_id='{c.ClassId}'";

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

        public List<Class> GetClasses(string year)
        {
            string sql = "SELECT c.class_id, c.number, c.letter, COUNT(cs.student_id) " +
                         $"FROM (classes c LEFT OUTER JOIN classes_students cs ON c.class_id=cs.class_id) " +
                         $"WHERE c.st_year = '{year}' " +
                         $"GROUP BY c.class_id, c.number, c.letter; ";

            List<Class> res = new List<Class>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Class cur = new Class(reader.GetInt64(0).ToString());
                        cur.Number = reader.GetString(1);
                        cur.Letter = reader.GetString(2);
                        cur.StYear = year;
                        cur.NumOfStudents = reader.GetInt32(3).ToString();

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

        #endregion

        //------------------Plans----------------------

        #region Plans

        public bool AddPlan(Plan p)
        {
            string sql = $"INSERT INTO [plan] (st_year, date_term1, date_term2, date_year) VALUES ('{p.StYear}', " +
                         $"@dateterm1, @dateterm2, @dateyear); ";

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
                    command.Parameters.Add("@dateterm1", SqlDbType.DateTime);
                    command.Parameters.Add("@dateterm2", SqlDbType.DateTime);
                    command.Parameters.Add("@dateyear", SqlDbType.DateTime);

                    command.Parameters["@dateterm1"].Value = (object)p.DateTerm1 ?? DBNull.Value;
                    command.Parameters["@dateterm2"].Value = (object)p.DateTerm2 ?? DBNull.Value;
                    command.Parameters["@dateyear"].Value = (object)p.DateYear ?? DBNull.Value;
                    res = command.ExecuteNonQuery();
                }
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

            return true;
        }

        public bool DeletePlan(Plan p)
        {
            string sql = $"DELETE FROM [plan] WHERE st_year = '{p.StYear}'; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            int res = 0;

            try
            {
                myConn.Open();


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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool UpdatePlan(Plan p, Plan oldP)
        {
            string sql1 = "UPDATE [plan] " +
                          $"SET st_year='{p.StYear}', " +
                          $" date_term1=@dateterm1, " +
                          $" date_term2=@dateterm2," +
                          $" date_year=@dateyear " +
                          $"WHERE st_year='{oldP.StYear}';";

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
                    command.Parameters.Add("@dateterm1", SqlDbType.DateTime);
                    command.Parameters.Add("@dateterm2", SqlDbType.DateTime);
                    command.Parameters.Add("@dateyear", SqlDbType.DateTime);

                    command.Parameters["@dateterm1"].Value = (object)p.DateTerm1 ?? DBNull.Value;
                    command.Parameters["@dateterm2"].Value = (object)p.DateTerm2 ?? DBNull.Value;
                    command.Parameters["@dateyear"].Value = (object)p.DateYear ?? DBNull.Value;
                    res = command.ExecuteNonQuery();
                }
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

            return true;
        }

        public bool UpdateCurYear(string year)
        {
            string sql1 = "SELECT COUNT(*) FROM helping_st_year; ";

            string sql2 = "UPDATE helping_st_year " +
                          $"SET cur_st_year='{year}'; ";

            string sql3 = "INSERT INTO helping_st_year (cur_st_year) " +
                          $"VALUES ('{year}'); ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int count = 0;
                int res = 0;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    count = (int)command.ExecuteScalar();
                }

                if (count == 0)
                {
                    using (SqlCommand command = new SqlCommand(sql3, myConn))
                    {
                        res = command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(sql2, myConn))
                    {
                        res = command.ExecuteNonQuery();
                    }
                }

                return res != 0;
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

        public string GetCurYear()
        {
            string sql1 = "SELECT cur_st_year " +
                          "FROM helping_st_year; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                string res = null;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res = reader.GetString(0);
                    }

                    reader.Close();
                }

                return res;
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
        }

        public List<Plan> GetPlans()
        {
            string sql = "SELECT st_year, date_term1, date_term2, date_year" +
                         " FROM [plan] ";

            List<Plan> res = new List<Plan>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Plan cur = new Plan();
                        cur.StYear = reader.GetString(0);
                        cur.DateTerm1 = reader.GetDateTime(1);
                        cur.DateTerm2 = reader.GetDateTime(2);
                        cur.DateYear = reader.GetDateTime(3);
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

        #endregion

        #region Users

        public bool DeleteUser(User u)
        {
            string sql = $"DELETE FROM [user] WHERE login = '{u.Username}'; ";
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool UpdateUser(User u, User oldU)
        {
            string sql = "UPDATE [user] " +
                         $"SET login='{u.Username}', " +
                         $" password = '{u.Password}' " +
                         $"WHERE login='{oldU.Username}'";
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
                // MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                MessageBox.Show("Пользователь с таким логином уже существует", "Warning");
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool AddUser(User t)
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
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public bool DeleteTeacher(Teacher t)
        {
            string sql1 = $"DELETE FROM head_teachers WHERE tab_number = '{t.TabNumber}'; ";


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
                    res = command.ExecuteNonQuery();
                }

                DeleteUser(t.User);
            }
            catch (Exception ex)
            {
                // MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return false;
            }
            finally
            {
                myConn?.Close();
            }

            return true;
        }

        public Teacher AddTeacher(Teacher t)

        {
            string sql1 = $"INSERT INTO [user] (password, login, rights) VALUES ('{t.User.Password}', " +
                          $"'{t.User.Username}', 'Классный руководитель'); ";

            string sql2 = $"INSERT INTO head_teachers " +
                          $"(tab_number, h_name, patronymic, surname, login) VALUES ('{t.TabNumber}', " +
                          $"'{t.HName}', '{t.Patronymiс}', '{t.Surname}', '{t.User.Username}');";

            string sql3 = $"SELECT tab_number FROM head_teachers WHERE tab_number='{t.TabNumber}'";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            int res = 0;
            Teacher temp = null;

            if (TeacherExists(t.TabNumber))
            {
                MessageBox.Show("Классный руководитель с таким номером уже существует!");
                return null;
            }

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

                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    res = command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(sql3, myConn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        temp = new Teacher(reader.GetString(0));
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

            return temp;
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
                return count1 != 0 && count2 == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }

            return false;
        }

        public bool TeacherExists(string tabNum)
        {
            string sql = $"SELECT COUNT(*) FROM head_teachers WHERE tab_number='{tabNum}'";
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
                    count = (int)command.ExecuteScalar();
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

        #endregion

        public List<string> GetYears()
        {
            string sql = "SELECT DISTINCT st_year FROM [plan];";
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
            string sql =
                "SELECT x.tab_number, x.h_name, x.patronymic, x.surname " +
                "FROM head_teachers x " +
                "WHERE x.tab_number NOT IN (SELECT ht.tab_number " +
                "FROM (head_teachers ht INNER JOIN head_teachers_classes htc ON ht.tab_number=htc.tab_number) " +
                "INNER JOIN classes c ON htc.class_id = c.class_id " +
                $"WHERE c.st_year='{year}' ); ";

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

        //----------------Students------------------

        #region Students

        public Student SaveStudent(Student s)
        {
            string sql1 =
                "INSERT INTO students (type_doc, ser_doc, num_doc, st_name, patronymic,surname, sex, birthday, num_alph_book, [index], city, street, house, apart, home_phone, gpd_attendance, exam) " +
                "OUTPUT INSERTED.student_id " +
                $"VALUES ('{s.TypeDoc}', @serdoc, '{s.NumDoc}'," +
                $"'{s.StName}', @patr, '{s.Surname}'," +
                $"'{s.Sex}', @birthday, '{s.NumAlphBook}'," +
                $"@ind, @city, @street, @house, @apart," +
                $"@phone, '{s.GpdAttendance}', '{s.ExamAllowedToPass}')";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                long res = 0;
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.Add("@serdoc", SqlDbType.VarChar);
                    command.Parameters.Add("@patr", SqlDbType.VarChar);
                    command.Parameters.Add("@ind", SqlDbType.Char);
                    command.Parameters.Add("@birthday", SqlDbType.Date);
                    command.Parameters.Add("@city", SqlDbType.VarChar);
                    command.Parameters.Add("@street", SqlDbType.VarChar);
                    command.Parameters.Add("@house", SqlDbType.VarChar);
                    command.Parameters.Add("@apart", SqlDbType.VarChar);
                    command.Parameters.Add("@phone", SqlDbType.VarChar);

                    command.Parameters["@serdoc"].Value = (object)s.SerDoc ?? DBNull.Value;
                    command.Parameters["@patr"].Value = (object)s.Patronymic ?? DBNull.Value;
                    command.Parameters["@ind"].Value = (object)s.Index ?? DBNull.Value;
                    command.Parameters["@birthday"].Value = s.Birthday;
                    command.Parameters["@city"].Value = (object)s.City ?? DBNull.Value;
                    command.Parameters["@street"].Value = (object)s.Street ?? DBNull.Value;
                    command.Parameters["@house"].Value = (object)s.House ?? DBNull.Value;
                    command.Parameters["@apart"].Value = (object)s.Apart ?? DBNull.Value;
                    command.Parameters["@phone"].Value = (object)s.HomePhone ?? DBNull.Value;

                    res = (long)command.ExecuteScalar();
                }

                s.Id = res.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Произошла ошибка добаления, возможно повторяющийся номер домента или алфавитной книги. " +
                //                "Проверьте правильность данных или наличие данного ученика уже в базе данных.", "Ошибка добавления",
                //    MessageBoxButton.OK, MessageBoxImage.Error);
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
                $"birthday = @birthday, " +
                $"num_alph_book = '{s.NumAlphBook}', " +
                $"[index] = @ind, city=@city, street=@street, house=@house, apart=@apart, " +
                $"home_phone = @phone, " +
                $"gpd_attendance = '{s.GpdAttendance}', " +
                $"exam = '{s.ExamAllowedToPass}' " +
                $"WHERE student_id={s.Id}";

            //string sqlCheckParent = "SELECT COUNT(*) " +
            //                        "FROM student_parent " +
            //                        "WHERE parent_id = @p AND student_id = @s; ";

            //string sqlAddParent = "INSERT INTO student_parent (student_id, parent_id, role) " +
            //                      "VALUES(@s_id, @p_id, @role); ";

            //string sqlUpdateParent = "UPDATE student_parent " +
            //                         "SET student_id = @s_id, parent_id=@p_id, role=@role; ";


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
                    command.Parameters.Add("@birthday", SqlDbType.Date);
                    command.Parameters.Add("@city", SqlDbType.VarChar);
                    command.Parameters.Add("@street", SqlDbType.VarChar);
                    command.Parameters.Add("@house", SqlDbType.VarChar);
                    command.Parameters.Add("@apart", SqlDbType.VarChar);
                    command.Parameters.Add("@phone", SqlDbType.VarChar);

                    command.Parameters["@serdoc"].Value = (object)s.SerDoc ?? DBNull.Value;
                    command.Parameters["@patr"].Value = (object)s.Patronymic ?? DBNull.Value;
                    command.Parameters["@ind"].Value = (object)s.Index ?? DBNull.Value;
                    command.Parameters["@birthday"].Value = s.Birthday;
                    command.Parameters["@city"].Value = (object)s.City ?? DBNull.Value;
                    command.Parameters["@street"].Value = (object)s.Street ?? DBNull.Value;
                    command.Parameters["@house"].Value = (object)s.House ?? DBNull.Value;
                    command.Parameters["@apart"].Value = (object)s.Apart ?? DBNull.Value;
                    command.Parameters["@phone"].Value = (object)s.HomePhone ?? DBNull.Value;

                    res = command.ExecuteNonQuery();
                }


                //if (s.Father != null && !String.IsNullOrEmpty(s.Father.Id))
                //{
                //    int count = 0;
                //    using (SqlCommand command = new SqlCommand(sqlCheckParent, myConn))
                //    {
                //        command.Parameters.AddWithValue("@p", s.Father.Id);
                //        command.Parameters.AddWithValue("@s", s.Id);
                //        count = (int) command.ExecuteScalar();
                //    }

                //    string execSql = (count == 0) ? sqlAddParent : sqlUpdateParent;
                //    using (SqlCommand command = new SqlCommand(execSql, myConn))
                //    {
                //        command.Parameters.AddWithValue("@p_id", s.Father.Id);
                //        command.Parameters.AddWithValue("@s_id", s.Id);
                //        command.Parameters.AddWithValue("@role", "father");
                //        command.ExecuteNonQuery();
                //    }
                //}

                //if (s.Mother != null && !String.IsNullOrEmpty(s.Mother.Id))
                //{
                //    int count = 0;
                //    using (SqlCommand command = new SqlCommand(sqlCheckParent, myConn))
                //    {
                //        command.Parameters.AddWithValue("@p", s.Mother.Id);
                //        command.Parameters.AddWithValue("@s", s.Id);
                //        count = (int) command.ExecuteScalar();
                //    }

                //    string execSql = (count == 0) ? sqlAddParent : sqlUpdateParent;
                //    using (SqlCommand command = new SqlCommand(execSql, myConn))
                //    {
                //        command.Parameters.AddWithValue("@p_id", s.Mother.Id);
                //        command.Parameters.AddWithValue("@s_id", s.Id);
                //        command.Parameters.AddWithValue("@role", "mother");
                //        command.ExecuteNonQuery();
                //    }
                //}

                //if (s.Trustees != null && s.Trustees.Count != 0)
                //{
                //    for (int i = 0; i < s.Trustees.Count; i++)
                //    {
                //        Parent cur = s.Trustees[i];
                //        int count = 0;
                //        if (cur != null && !String.IsNullOrEmpty(cur.Id))
                //        {
                //            using (SqlCommand command = new SqlCommand(sqlCheckParent, myConn))
                //            {
                //                command.Parameters.AddWithValue("@p", cur.Id);
                //                command.Parameters.AddWithValue("@s", s.Id);
                //                count = (int) command.ExecuteScalar();
                //            }

                //            string execSql = (count == 0) ? sqlAddParent : sqlUpdateParent;
                //            using (SqlCommand command = new SqlCommand(execSql, myConn))
                //            {
                //                command.Parameters.AddWithValue("@p_id", cur.Id);
                //                command.Parameters.AddWithValue("@s_id", s.Id);
                //                command.Parameters.AddWithValue("@role", "trustee");
                //                command.ExecuteNonQuery();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка добаления, возможно повторяющийся номер домента или алфавитной книги. " +
                                "Проверьте правильность данных или наличие данного ученика уже в базе данных.", "Ошибка добавления",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            finally
            {
                myConn?.Close();
            }

            return s;
        }

        public bool AssignStudentToClass(Student s, Class c)
        {
            string sql1 =
                "INSERT INTO classes_students (student_id, class_id)" +
                $"VALUES ('{s.Id}', '{c.ClassId}')";
            string sqlCheck =
                "SELECT COUNT(*) " +
                "FROM classes_students " +
                $"WHERE student_id={s.Id} AND class_id={c.ClassId};";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand(sqlCheck, myConn))
                {
                    res = (int)command.ExecuteScalar();
                }

                if (res == 0)
                {
                    using (SqlCommand command = new SqlCommand(sql1, myConn))
                    {
                        res = command.ExecuteNonQuery();
                    }
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

        #endregion

        //-------------Parents---------------------

        #region Parents

        public Parent GetFather(Student s)
        {
            string sql1 =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents " +
                "WHERE parent_id IN(SELECT parent_id " +
                "                   FROM student_parent " +
                "                   WHERE student_id = @st_id AND role = 'father'); ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            Parent father = new Parent();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@st_id", s.Id);
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
            string sql1 =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents " +
                "WHERE parent_id IN(SELECT parent_id " +
                "                   FROM student_parent " +
                "                   WHERE student_id = @st_id AND role = 'mother'); ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            Parent mother = new Parent();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@st_id", s.Id);
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

        public List<Parent> GetTrustees(Student s)
        {
            string sql1 =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents " +
                "WHERE parent_id IN(SELECT parent_id " +
                "                   FROM student_parent " +
                "                   WHERE student_id = @st_id AND role = 'trustee'); ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            List<Parent> trustees = new List<Parent>();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@st_id", s.Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Parent cur = new Parent();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.PName = reader.GetString(1);
                        cur.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.Surname = reader.GetString(3);

                        cur.Sex = reader.GetString(4);
                        cur.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        cur.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        cur.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        cur.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        cur.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        cur.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);

                        trustees.Add(cur);
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

            return trustees;
        }

        public List<ParentChild> GetParentChildren(Student s)
        {
            string sql1 =
                "SELECT p.parent_id, p.p_name, p.patronymic, p.surname, p.sex, p.birthday, p.[index], p.city, p.street, p.house, p.apart, p.home_phone, p.work_phone, p.work, p.commentary, sp.role, sp.trustee, sp.relation " +
                "FROM parents p INNER JOIN  student_parent sp ON p.parent_id=sp.parent_id " +
                "WHERE sp.student_id = @st_id; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            List<ParentChild> parents = new List<ParentChild>();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@st_id", s.Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ParentChild pc = new ParentChild();
                        Parent cur = new Parent();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.PName = reader.GetString(1);
                        cur.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.Surname = reader.GetString(3);

                        cur.Sex = reader.GetString(4);
                        cur.Birthday = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                        cur.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        cur.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        cur.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        cur.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        cur.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);

                        pc.Parent = cur;
                        pc.Child = s;
                        pc.Role = reader.GetString(15);
                        pc.Trustee = reader.IsDBNull(16) ? (bool?)null : reader.GetBoolean(16);
                        pc.Relation = reader.IsDBNull(17) ? "" : reader.GetString(17);

                        parents.Add(pc);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return parents;
            }
            finally
            {
                myConn?.Close();
            }

            return parents;
        }

        public List<ParentChild> GetParentChildren(Parent p)
        {
            string sql1 =
                "SELECT s.student_id, s.type_doc, s.ser_doc, s.num_doc, s.st_name, s.patronymic," +
                "s.surname, s.sex, s.birthday, s.num_alph_book, [index], s.city," +
                "s.street, s.house, s.apart, s.home_phone, s.gpd_attendance, s.exam, sp.role, sp.trustee, sp.relation " +
                "FROM students s INNER JOIN student_parent sp ON s.student_id=sp.student_id " +
                "WHERE sp.parent_id=@p_id; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            List<ParentChild> parents = new List<ParentChild>();
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@p_id", p.Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ParentChild pc = new ParentChild();
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

                        pc.Parent = p;
                        pc.Child = cur;
                        pc.Role = reader.GetString(18);
                        pc.Trustee = reader.IsDBNull(19) ? (bool?)null : reader.GetBoolean(19);
                        pc.Relation = reader.IsDBNull(20) ? "" : reader.GetString(20);

                        parents.Add(pc);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return parents;
            }
            finally
            {
                myConn?.Close();
            }

            return parents;
        }

        public List<Parent> GetParentsInClass(Class c)
        {
            string sql1 =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents " +
                "WHERE parent_id IN (SELECT parent_id " +
                "                   FROM student_parent " +
                "                   WHERE student_id IN(SELECT student_id " +
                "                                       FROM classes_students " +
                "                                       WHERE class_id = @c_id));";


            List<Parent> res = new List<Parent>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@c_id", c.ClassId);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Parent cur = new Parent();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.PName = reader.GetString(1);
                        cur.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.Surname = reader.GetString(3);

                        cur.Sex = reader.GetString(4);
                        cur.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        cur.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        cur.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        cur.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        cur.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.Work = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        cur.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);

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

        public List<Parent> GetAllParents()
        {
            string sql1 =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents ";

            List<Parent> res = new List<Parent>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Parent cur = new Parent();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.PName = reader.GetString(1);
                        cur.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.Surname = reader.GetString(3);

                        cur.Sex = reader.GetString(4);
                        cur.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        cur.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        cur.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        cur.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        cur.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        cur.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);

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

        public Parent SaveParent(Parent p)
        {
            string sql1 =
                "INSERT INTO parents (p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary) " +
                "OUTPUT INSERTED.parent_id " +
                "VALUES (@name, @patr, @surname, @sex, @birthday, @ind, @city, @street, @house, @apart, @home_phone, @work_phone, @work, @commentary); ";

            string sql3 =
                "UPDATE parents " +
                "SET p_name=@name, patronymic=@patr, surname=@surname, sex=@sex, birthday=@birthday, [index]=@ind, city=@city, street=@street, house=@house, apart=@apart, home_phone=@home_phone, work_phone=@work_phone, work=@work, commentary=@commentary " +
                "WHERE parent_id=@p_id";


            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                if (String.IsNullOrEmpty(p.Id))
                {
                    using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(sql1, myConn))
                    {
                        command.Parameters.AddWithValue("@name", p.PName);
                        command.Parameters.AddWithValue("@surname", p.Surname);
                        command.Parameters.AddWithValue("@sex", p.Sex);
                        command.Parameters.Add("@patr", SqlDbType.VarChar);
                        command.Parameters.Add("@birthday", SqlDbType.Date);

                        command.Parameters.Add("@ind", SqlDbType.Char);
                        command.Parameters.Add("@city", SqlDbType.VarChar);
                        command.Parameters.Add("@street", SqlDbType.VarChar);
                        command.Parameters.Add("@house", SqlDbType.VarChar);
                        command.Parameters.Add("@apart", SqlDbType.VarChar);
                        command.Parameters.Add("@work_phone", SqlDbType.VarChar);
                        command.Parameters.Add("@home_phone", SqlDbType.VarChar);
                        command.Parameters.Add("@work", SqlDbType.VarChar);
                        command.Parameters.Add("@commentary", SqlDbType.VarChar);

                        command.Parameters["@patr"].Value = (object)p.Patronymic ?? DBNull.Value;
                        command.Parameters["@birthday"].Value = (object)p.Birthday ?? DBNull.Value;
                        command.Parameters["@ind"].Value = (object)p.Index ?? DBNull.Value;
                        command.Parameters["@city"].Value = (object)p.City ?? DBNull.Value;
                        command.Parameters["@street"].Value = (object)p.Street ?? DBNull.Value;
                        command.Parameters["@house"].Value = (object)p.House ?? DBNull.Value;
                        command.Parameters["@apart"].Value = (object)p.Apart ?? DBNull.Value;
                        command.Parameters["@home_phone"].Value = (object)p.HomePhone ?? DBNull.Value;
                        command.Parameters["@work_phone"].Value = (object)p.WorkPhone ?? DBNull.Value;
                        command.Parameters["@work"].Value = (object)p.Work ?? DBNull.Value;
                        command.Parameters["@commentary"].Value = (object)p.Commentary ?? DBNull.Value;
                        var res = (long)command.ExecuteScalar();
                        p.Id = res.ToString();
                    }
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(sql3, myConn))
                    {
                        command.Parameters.AddWithValue("@p_id", p.Id);
                        command.Parameters.AddWithValue("@name", p.PName);
                        command.Parameters.AddWithValue("@surname", p.Surname);
                        command.Parameters.AddWithValue("@sex", p.Sex);
                        command.Parameters.Add("@patr", SqlDbType.VarChar);
                        command.Parameters.Add("@birthday", SqlDbType.Date);

                        command.Parameters.Add("@ind", SqlDbType.Char);
                        command.Parameters.Add("@city", SqlDbType.VarChar);
                        command.Parameters.Add("@street", SqlDbType.VarChar);
                        command.Parameters.Add("@house", SqlDbType.VarChar);
                        command.Parameters.Add("@apart", SqlDbType.VarChar);
                        command.Parameters.Add("@work_phone", SqlDbType.VarChar);
                        command.Parameters.Add("@home_phone", SqlDbType.VarChar);
                        command.Parameters.Add("@work", SqlDbType.VarChar);
                        command.Parameters.Add("@commentary", SqlDbType.VarChar);

                        command.Parameters["@patr"].Value = (object)p.Patronymic ?? DBNull.Value;
                        command.Parameters["@birthday"].Value = (object)p.Birthday ?? DBNull.Value;
                        command.Parameters["@ind"].Value = (object)p.Index ?? DBNull.Value;
                        command.Parameters["@city"].Value = (object)p.City ?? DBNull.Value;
                        command.Parameters["@street"].Value = (object)p.Street ?? DBNull.Value;
                        command.Parameters["@house"].Value = (object)p.House ?? DBNull.Value;
                        command.Parameters["@apart"].Value = (object)p.Apart ?? DBNull.Value;
                        command.Parameters["@home_phone"].Value = (object)p.HomePhone ?? DBNull.Value;
                        command.Parameters["@work_phone"].Value = (object)p.WorkPhone ?? DBNull.Value;
                        command.Parameters["@work"].Value = (object)p.Work ?? DBNull.Value;
                        command.Parameters["@commentary"].Value = (object)p.Commentary ?? DBNull.Value;
                        command.ExecuteNonQuery();
                    }
                }

                return p;
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

            return p;
        }

        public Parent ParentExists(Parent p)
        {
            string sql1 =
                "SELECT COUNT(*)" +
                "FROM parents " +
                "WHERE p_name=@name " +
                "   AND patronymic=@patr" +
                "   AND surname=@surname" +
                "   AND birthday=@birthday; ";

            string sql =
                "SELECT parent_id, p_name, patronymic, surname, sex, birthday, [index], city, street, house, apart, home_phone, work_phone, work, commentary " +
                "FROM parents " +
                "WHERE p_name=@name " +
                "   AND patronymic=@patr" +
                "   AND surname=@surname" +
                "   AND birthday=@birthday; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                int res = 0;
                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    command.Parameters.AddWithValue("@name", p.PName);
                    command.Parameters.AddWithValue("@surname", p.Surname);
                    command.Parameters.Add("@patr", SqlDbType.VarChar);
                    command.Parameters.Add("@birthday", SqlDbType.Date);

                    command.Parameters["@patr"].Value = (object)p.Patronymic ?? DBNull.Value;
                    command.Parameters["@birthday"].Value = (object)p.Birthday ?? DBNull.Value;

                    res = (int)command.ExecuteScalar();
                }

                if (res == 0)
                    return null;
                Parent cur = null;

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    command.Parameters.AddWithValue("@name", p.PName);
                    command.Parameters.AddWithValue("@surname", p.Surname);
                    command.Parameters.Add("@patr", SqlDbType.VarChar);
                    command.Parameters.Add("@birthday", SqlDbType.Date);

                    command.Parameters["@patr"].Value = (object)p.Patronymic ?? DBNull.Value;
                    command.Parameters["@birthday"].Value = (object)p.Birthday ?? DBNull.Value;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cur = new Parent();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.PName = reader.GetString(1);
                        cur.Patronymic = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        cur.Surname = reader.GetString(3);

                        cur.Sex = reader.GetString(4);
                        cur.Birthday = reader.IsDBNull(5) ? new DateTime() : reader.GetDateTime(5);
                        cur.Index = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.City = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.Street = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        cur.House = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        cur.Apart = reader.IsDBNull(10) ? "" : reader.GetString(10);


                        cur.HomePhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        cur.WorkPhone = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        cur.Work = reader.IsDBNull(12) ? "" : reader.GetString(13);
                        cur.Commentary = reader.IsDBNull(14) ? "" : reader.GetString(14);

                        break;
                    }

                    reader.Close();
                }

                return cur;
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
        }

        public bool SaveParentChild(ParentChild pc)
        {
            string sql1 =
                "INSERT INTO student_parent (student_id, parent_id, role, trustee, relation) " +
                "VALUES (@s_id, @p_id, @role, @trustee, @relation); ";

            string sqlCheckExists =
                "SELECT COUNT(*) " +
                "FROM student_parent " +
                "WHERE parent_id=@p_id AND student_id=@s_id";

            string sql3 =
                "UPDATE student_parent " +
                "SET role=@role, trustee=@trustee, relation=@relation " +
                "WHERE parent_id=@p_id AND student_id=@s_id";


            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }

                int count = 0;
                using (SqlCommand command = new SqlCommand(sqlCheckExists, myConn))
                {
                    command.Parameters.AddWithValue("@p_id", pc.Parent.Id);
                    command.Parameters.AddWithValue("@s_id", pc.Child.Id);
                    count = (int)command.ExecuteScalar();
                }

                if (count == 0)
                {
                    using (SqlCommand command = new SqlCommand(sql1, myConn))
                    {
                        command.Parameters.AddWithValue("@s_id", pc.Child.Id);
                        command.Parameters.AddWithValue("@p_id", pc.Parent.Id);
                        command.Parameters.AddWithValue("@role", pc.Role);
                        command.Parameters.Add("@trustee", SqlDbType.Bit);
                        command.Parameters.Add("@relation", SqlDbType.VarChar);

                        command.Parameters["@trustee"].Value = (object)pc.Trustee ?? DBNull.Value;
                        command.Parameters["@relation"].Value = (object)pc.Relation ?? DBNull.Value;
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand command = new SqlCommand(sql3, myConn))
                    {
                        command.Parameters.AddWithValue("@s_id", pc.Child.Id);
                        command.Parameters.AddWithValue("@p_id", pc.Parent.Id);
                        command.Parameters.AddWithValue("@role", pc.Role);
                        command.Parameters.Add("@trustee", SqlDbType.Bit);
                        command.Parameters.Add("@relation", SqlDbType.VarChar);

                        command.Parameters["@trustee"].Value = (object)pc.Trustee ?? DBNull.Value;
                        command.Parameters["@relation"].Value = (object)pc.Relation ?? DBNull.Value;
                        command.ExecuteNonQuery();
                    }
                }

                return true;
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

        public bool RemoveParentChild(ParentChild pc)
        {
            string sql =
                "DELETE student_parent " +
                "WHERE parent_id=@p_id AND student_id=@s_id";


            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    command.Parameters.AddWithValue("@s_id", pc.Child.Id);
                    command.Parameters.AddWithValue("@p_id", pc.Parent.Id);
                    command.ExecuteNonQuery();
                }

                return true;
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

        #endregion


        public Teacher UpdateTeacher(Teacher t, Teacher oldT)
        {
            //to make with parameters


            string sql2 = "UPDATE  head_teachers " +
                          $"SET h_name = '{t.HName}', " +
                          $"surname = '{t.Surname}', " +
                          $"patronymic = '{t.Patronymiс}', " +
                          $"login = '{t.User.Username}' " +
                          $"WHERE tab_number='{t.TabNumber}'";


            string sql3 = $"SELECT tab_number FROM head_teachers WHERE tab_number='{t.TabNumber}'";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            int res = 0;
            Teacher temp = null;

            try
            {
                myConn.Open();


                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }


                //maybe it is possible just to use (!UserExists(...)) if login is AK in head_teachers

                if (!UserExistsUseless(t.User.Username))
                {
                    bool flag = UpdateUser(t.User, oldT.User);
                    if (!flag) return temp;
                }

                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    res = command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(sql3, myConn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        temp = new Teacher(reader.GetString(0));
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

            return temp;
        }







        //----------------------Marks-----------------------------

        #region Marks

        public List<string> GetSubjects(Class c, string type)
        {
            string sql = "SELECT DISTINCT subject" +
                         " FROM Marks " +
                         $"WHERE class_id={c.ClassId} AND mark_type='{type}';";

            List<string> res = new List<string>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0));
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

        public List<string> GetSubjects(Class c)
        {
            string sql = "SELECT subject" +
                         " FROM classes_subjects " +
                         $"WHERE class_id={c.ClassId};";

            List<string> res = new List<string>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0));
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

        public bool AddSubject(Class c, string subject)
        {
            string sql = "INSERT INTO classes_subjects (class_id, subject)" +
                         $"VALUES ('{c.ClassId}', '{subject}'); ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return false;
            }
            finally
            {
                myConn?.Close();
            }
        }

        public bool RemoveSubject(Class c, string subject)
        {
            string sql = "DELETE FROM classes_subjects " +
                         $"WHERE class_id='{c.ClassId}' AND subject='{subject}'; ";

            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return false;
            }
            finally
            {
                myConn?.Close();
            }
        }

        public List<Mark> GetMarks(Class c, string subject, string type)
        {
            string sql =
                "SELECT m.mark_id, m.grade, m.mark_type, m.subject, m.mark_date, s.student_id, s.st_name, s.surname, m.class_id " +
                " FROM Marks m INNER JOIN students s ON m.student_id=s.student_id " +
                $"WHERE class_id={c.ClassId} AND subject='{subject}' AND mark_type='{type}';";

            List<Mark> res = new List<Mark>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Mark cur = new Mark();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.Grade = reader.GetInt32(1).ToString();
                        cur.MarkType = reader.GetString(2);
                        cur.Subject = subject;
                        cur.MarkDate = reader.GetDateTime(4);
                        cur.StudentId = reader.GetInt64(5).ToString();
                        cur.StudentName = reader.GetString(6);
                        cur.StudentSurname = reader.GetString(7);
                        cur.ClassId = reader.GetInt64(8).ToString();
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

        public List<Mark> SaveMarks(List<Mark> l)
        {
            string sqlCheck = "SELECT COUNT(*) " +
            "                   FROM marks " +
            "                   WHERE student_id=@s_id AND subject=@sub AND mark_type=@m_t AND class_id=@c_id ;";
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand("set ANSI_WARNINGS  OFF;", myConn))
                {
                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < l.Count; i++)
                {
                    if (String.IsNullOrEmpty(l[i].Grade))
                        continue;

                    string sql = "UPDATE marks " +
                                 $" SET grade={l[i].Grade}, mark_type='{l[i].MarkType}', subject='{l[i].Subject}', mark_date=@date, student_id={l[i].StudentId} " +
                                 $"WHERE mark_id={l[i].Id};";

                    if (!String.IsNullOrEmpty(l[i].Id))
                    {
                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            command.Parameters.Add("@date", SqlDbType.DateTime);
                            command.Parameters["@date"].Value = l[i].MarkDate;
                            
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        int check = 0;
                        using (SqlCommand command = new SqlCommand(sqlCheck, myConn))
                        {
                            command.Parameters.AddWithValue("@s_id", l[i].StudentId);
                            command.Parameters.AddWithValue("@sub", l[i].Subject);
                            command.Parameters.AddWithValue("@m_t", l[i].MarkType);
                            command.Parameters.AddWithValue("@c_id", l[i].ClassId);
                            check = (int)command.ExecuteScalar();
                        }

                        if (check == 0)
                        {
                            sql = "INSERT INTO marks (grade, mark_type, subject, mark_date, student_id, class_id) " +
                                  " OUTPUT INSERTED.mark_id " +
                                  $" VALUES ({l[i].Grade}, '{l[i].MarkType}', '{l[i].Subject}', @date, {l[i].StudentId}, {l[i].ClassId});";
                        }
                        else
                        {
                            sql = "UPDATE marks " +
                                  $"SET grade={l[i].Grade}, mark_date=@date " +
                                  $"OUTPUT INSERTED.mark_id " +
                                  $"WHERE mark_type='{l[i].MarkType}' AND subject='{l[i].Subject}'AND student_id={l[i].StudentId} " +
                                  $"AND class_id={l[i].ClassId} ;";
                        }

                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                      
                                command.Parameters.Add("@date", SqlDbType.DateTime);
                                command.Parameters["@date"].Value = l[i].MarkDate;
                                var res = command.ExecuteScalar();
                                l[i].Id = res.ToString();
                           
                        }
                    }
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

            return l;
        }

        public bool RemoveMarks(List<Mark> l)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                for (int i = 0; i < l.Count; i++)
                {
                    if (!String.IsNullOrEmpty(l[i].Id))
                    {
                        string sql = "DELETE FROM marks " +
                                     $"WHERE mark_id={l[i].Id};";

                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            var res = command.ExecuteNonQuery();
                        }
                    }
                }

                return true;
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

        #endregion

        //-----------------Comments----------------

        #region Comments

        public List<Comment> GetComments(Student s)
        {
            string sql =
                "SELECT comment_id, descr " +
                " FROM Comments " +
                $"WHERE student_id={s.Id};";

            List<Comment> res = new List<Comment>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Comment cur = new Comment();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.Descr = reader.GetString(1);
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

        public List<string> GetAllComments()
        {
            string sql =
                "SELECT comment " +
                " FROM helping_comments; ";

            List<string> res = new List<string>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand(sql, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0));
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

        public List<Comment> SaveComments(Student s)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                foreach (Comment c in s.Comments)
                {
                    if (String.IsNullOrEmpty(c.Id))
                    {
                        string sql = "INSERT INTO comments (student_id, descr) " +
                                     "OUTPUT INSERTED.comment_id " +
                                     $" VALUES ({s.Id},  '{c}');";
                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            var res = (long)command.ExecuteScalar();
                            c.Id = res.ToString();
                        }
                    }
                }

                return s.Comments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return s.Comments;
            }
            finally
            {
                myConn?.Close();
            }
        }

        public bool RemoveComments(Comment c)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                if (!String.IsNullOrEmpty(c.Id))
                {
                    string sql = "DELETE FROM comments " +
                                 $"WHERE comment_id={c.Id};";
                    using (SqlCommand command = new SqlCommand(sql, myConn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return true;
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

        public List<Comment> SaveComments(List<Comment> l)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                foreach (Comment c in l)
                {
                    if (String.IsNullOrEmpty(c.Id))
                    {
                        string sql = "INSERT INTO comments (student_id, descr) " +
                                     "OUTPUT INSERTED.comment_id " +
                                     $" VALUES ({c.StudentId},  '{c.Descr}');";
                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            var res = (long)command.ExecuteScalar();
                            c.Id = res.ToString();
                        }
                    }
                }

                return l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
                return l;
            }
            finally
            {
                myConn?.Close();
            }
        }

        #endregion

        //----------------Movement-----------------

        #region Movement

        public List<Movement> GetMovements(Class c)
        {
            string sql1 =
                "SELECT motion_id, s.student_id, s.surname, s.st_name, s.patronymic, sch_city, sch_region, sch_country, motion_date " +
                "FROM(motion INNER JOIN students s on motion.income_st_id = s.student_id) " +
                " INNER JOIN classes_students cs ON s.student_id = cs.student_id " +
                $"WHERE cs.class_id='{c.ClassId}'";

            string sql2 =
                "SELECT motion_id, s.student_id, s.surname, s.st_name, s.patronymic, sch_city, sch_region, sch_country, motion_date " +
                "FROM(motion INNER JOIN students s on motion.outcome_st_id = s.student_id) " +
                " INNER JOIN classes_students cs ON s.student_id = cs.student_id " +
                $"WHERE cs.class_id='{c.ClassId}'";

            List<Movement> res = new List<Movement>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Movement cur = new Movement();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.StudentId = reader.GetInt64(1).ToString();
                        string f = reader.GetString(2);
                        string i = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string o = reader.GetString(4);
                        cur.StudentFIO = f + " " + i + " " + o;
                        cur.SchCity = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.SchRegion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.SchCountry = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.MovementDate = reader.GetDateTime(8);
                        cur.Income = true;
                        res.Add(cur);
                    }

                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Movement cur = new Movement();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.StudentId = reader.GetInt64(1).ToString();
                        string f = reader.GetString(2);
                        string i = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string o = reader.GetString(4);
                        cur.StudentFIO = f + " " + i + " " + o;
                        cur.SchCity = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.SchRegion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.SchCountry = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.MovementDate = reader.GetDateTime(8);
                        cur.Income = false;
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

        public List<Movement> GetMovements()
        {
            string sql1 =
                "SELECT motion_id, s.student_id, s.surname, s.st_name, s.patronymic, sch_city, sch_region, sch_country, motion_date " +
                "FROM(motion INNER JOIN students s on motion.income_st_id = s.student_id) " +
                " INNER JOIN classes_students cs ON s.student_id = cs.student_id; ";

            string sql2 =
                "SELECT motion_id, s.student_id, s.surname, s.st_name, s.patronymic, sch_city, sch_region, sch_country, motion_date " +
                "FROM(motion INNER JOIN students s on motion.outcome_st_id = s.student_id) " +
                " INNER JOIN classes_students cs ON s.student_id = cs.student_id; ";

            List<Movement> res = new List<Movement>();
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();

                using (SqlCommand command = new SqlCommand(sql1, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Movement cur = new Movement();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.StudentId = reader.GetInt64(1).ToString();
                        string f = reader.GetString(2);
                        string i = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string o = reader.GetString(4);
                        cur.StudentFIO = f + " " + i + " " + o;
                        cur.SchCity = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.SchRegion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.SchCountry = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.MovementDate = reader.GetDateTime(8);
                        cur.Income = true;
                        res.Add(cur);
                    }

                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(sql2, myConn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Movement cur = new Movement();
                        cur.Id = reader.GetInt64(0).ToString();
                        cur.StudentId = reader.GetInt64(1).ToString();
                        string f = reader.GetString(2);
                        string i = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string o = reader.GetString(4);
                        cur.StudentFIO = f + " " + i + " " + o;
                        cur.SchCity = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        cur.SchRegion = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        cur.SchCountry = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        cur.MovementDate = reader.GetDateTime(8);
                        cur.Income = false;
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


        public List<Movement> SaveMovements(List<Movement> l)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                foreach (Movement mov in l)
                {
                    if (!String.IsNullOrEmpty(mov.Id))
                    {
                        string sql = "UPDATE Motion " +
                                     "SET sch_city=@city, sch_region=@region, sch_country=@country, motion_date=@date " +
                                     $"WHERE motion_id='{mov.Id}'";
                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            if (String.IsNullOrEmpty(mov.SchCity))
                                command.Parameters.AddWithValue("@city", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@city", mov.SchCity);

                            if (String.IsNullOrEmpty(mov.SchRegion))
                                command.Parameters.AddWithValue("@region", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@region", mov.SchRegion);

                            if (String.IsNullOrEmpty(mov.SchCountry))
                                command.Parameters.AddWithValue("@country", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@country", mov.SchCountry);

                            command.Parameters.AddWithValue("@date", mov.MovementDate);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sql =
                            "INSERT INTO Motion (income_st_id, outcome_st_id, sch_city, sch_region, sch_country, motion_date) " +
                            "OUTPUT INSERTED.motion_id " +
                            "VALUES (@in_id, @out_id,  @city, @region, @country, @date);";
                        using (SqlCommand command = new SqlCommand(sql, myConn))
                        {
                            command.Parameters.Add("@in_id", SqlDbType.BigInt);
                            command.Parameters.Add("@out_id", SqlDbType.BigInt);
                            command.Parameters["@in_id"].Value = mov.Income ? (object)mov.StudentId : DBNull.Value;
                            command.Parameters["@out_id"].Value = !mov.Income ? (object)mov.StudentId : DBNull.Value;

                            if (String.IsNullOrEmpty(mov.SchCity))
                                command.Parameters.AddWithValue("@city", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@city", mov.SchCity);

                            if (String.IsNullOrEmpty(mov.SchRegion))
                                command.Parameters.AddWithValue("@region", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@region", mov.SchRegion);

                            if (String.IsNullOrEmpty(mov.SchCountry))
                                command.Parameters.AddWithValue("@country", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@country", mov.SchCountry);

                            command.Parameters.AddWithValue("@date", mov.MovementDate);

                            var t = (long)command.ExecuteScalar();
                            mov.Id = t.ToString();
                        }
                    }
                }


                return l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's problem with you connection!\n" + ex.Message);
            }
            finally
            {
                myConn?.Close();
            }

            return l;
        }

        public bool RemoveMovement(Movement m)
        {
            SqlConnection myConn = new SqlConnection(StationManager.ConnectionString);
            try
            {
                myConn.Open();
                if (!String.IsNullOrEmpty(m.Id))
                {
                    string sql = "DELETE FROM motion " +
                                 $"WHERE motion_id={m.Id};";
                    using (SqlCommand command = new SqlCommand(sql, myConn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return true;
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

        #endregion
    }
}