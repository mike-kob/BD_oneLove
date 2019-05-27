using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Forms;
using BD_oneLove.Models;
using Excel = Microsoft.Office.Interop.Excel;


namespace BD_oneLove.Tools.Managers
{
    internal class ExcelManager
    {
        public static List<Student> LoadClassStudents(string file, Class cl)
        {
           
            List<Student> res = new List<Student>();
            try
            {
                Excel.Application app = new Excel.Application();
                var book = app.Workbooks.Open(file);
                Excel.Worksheet worksheet = (Excel.Worksheet) book.Worksheets.Item[1];
                var range = worksheet.Range["B4", "S50"];

                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;


                for (int i = 1; i <= rowCount; i++)
                {
                    Student cur = new Student();
                    var val = range.Range["A" + i]?.Value2;
                    if (val != null)
                    {
                        cur.NumAlphBook = val.ToString();
                    }
                    else
                    {
                        break;
                    }

                    val = range.Range["B" + i]?.Value2;
                    if (val != null)
                        cur.Surname = val.ToString();
                    val = range.Range["C" + i]?.Value2;
                    if (val != null)
                        cur.StName = val.ToString();
                    val = range.Range["D" + i]?.Value2;
                    if (val != null)
                        cur.Patronymic = val.ToString();

                    val = range.Range["E" + i]?.Value2;
                    if (val != null)
                        cur.Sex = val.ToString();
                    val = range.Range["F" + i]?.Value;
                    if (val != null)
                        cur.Birthday = DateTime.Parse(val.ToString());

                    val = range.Range["G" + i]?.Value2;
                    if (val != null)
                        cur.TypeDoc = val.ToString();
                    val = range.Range["H" + i]?.Value2;
                    if (val != null)
                        cur.SerDoc = val.ToString();
                    val = range.Range["I" + i]?.Value2;
                    if (val != null)
                        cur.NumDoc = val.ToString();

                    val = range.Range["J" + i]?.Value2;
                    if (val != null)
                        cur.Index = val.ToString();
                    val = range.Range["K" + i]?.Value2;
                    if (val != null)
                        cur.City = val.ToString();
                    val = range.Range["L" + i]?.Value2;
                    if (val != null)
                        cur.Street = val.ToString();
                    val = range.Range["M" + i]?.Value2;
                    if (val != null)
                        cur.House = val.ToString();
                    val = range.Range["N" + i]?.Value2;
                    if (val != null)
                        cur.Apart = val.ToString();

                    val = range.Range["P" + i]?.Value2;
                    if (val != null)
                        cur.HomePhone = val.ToString();

                    val = range.Range["Q" + i]?.Value2;
                    cur.GpdAttendance = val != null;
                    val = range.Range["R" + i]?.Value2;
                    cur.ExamAllowedToPass = val != null;

                    res.Add(cur);
                }

                book.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка импорта. " + e.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return res;
        }

        public static void FillSocialPassport(string file, List<Student> students)
        {
            try
            {
                Excel.Application app = new Excel.Application();
                var book = app.Workbooks.Open(file);
                Excel.Worksheet worksheet = (Excel.Worksheet)book.Worksheets.Item[1];
                var range = worksheet.UsedRange;

                //writing students
                for (int i = 0; i < students.Count; i++)
                {
                    range.Cells[i + 4, 1] = students[i].Id;
                    range.Cells[i + 4, 2] = students[i].SurnameNamePatr;
                }

                //writing categories
                List<string> comments = StationManager.DataStorage.GetAllComments();
                for (int i = 0; i < comments.Count; i++)
                {
                    range.Cells[2, i + 4] = comments[i];
                }
                book.Save();
                book.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка импорта. " + e.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static List<Comment> LoadComments(string file)
        {

            List<Comment> res = new List<Comment>();
            Excel.Workbook book = null;
            try
            {
                Excel.Application app = new Excel.Application();
                book = app.Workbooks.Open(file);
                Excel.Worksheet worksheet = (Excel.Worksheet) book.Worksheets.Item[1];
                var range = worksheet.UsedRange;

                for (int i = 4;; i++)
                {
                    var id = range.Range["A" + i]?.Value2;
                    if (id == null)
                        break;


                    int j = 1;
                    var val = range.Range["D2"]?.Value2;
                    var ok = range.Range["D" + i]?.Value2;

                    while (val != null)
                    {
                        if (ok != null)
                        {
                            Comment c = new Comment();
                            c.StudentId = id.ToString();
                            c.Descr = val.ToString();
                            res.Add(c);
                        }

                        char letter = (char) ('D' + j);
                        val = range.Range[letter + "2"]?.Value2;
                        ok = range.Range[letter + "" + i]?.Value2;
                        j++;
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка импорта. " + e.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                book?.Close();
            }
            return res;
        }

    }
}