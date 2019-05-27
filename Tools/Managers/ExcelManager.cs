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
                var range = worksheet.UsedRange;

                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;


                for (int i = 4; i <= rowCount; i++)
                {

                    Student cur = LoadStudent(range, i);
                    if(cur == null)
                        break;

                    res.Add(cur);

                    Student answ = StationManager.DataStorage.SaveStudent(cur);
                    if (answ == null)
                    {
                        MessageBox.Show(
                            $"Ученик {cur.SurnameNamePatr} не добавлен. Возможно он уже занесён в базу. Проверьте его наличие в базе.",
                            "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        
                        //TODO edit window
                    }
                    

                    Parent mother = LoadParent(range, i, 1);
                    if (mother != null)
                    {
                        Parent parentAnsw = StationManager.DataStorage.ParentExists(mother);
                        if (parentAnsw != null)
                        {
                            var dialogRes = MessageBox.Show(
                                $"Родитель {mother.SurnameNamePatr}, {mother.Birthday?.ToString("dd/MMM/yyyy")} уже существует." +
                                $"Добавить как родителя ребёнка {cur.SurnameNamePatr}?" ,
                                "Ошибка добавления", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                            //TODO edit window

                            if (dialogRes == DialogResult.Yes)
                            {
                                if (!String.IsNullOrEmpty(parentAnsw.Id) && !String.IsNullOrEmpty(cur.Id))
                                {
                                    ParentChild pc = new ParentChild();
                                    pc.Parent = parentAnsw;
                                    pc.Child = cur;
                                    pc.Role = "mother";

                                    StationManager.DataStorage.SaveParentChild(pc);
                                }
                            }
                        }
                        else
                        {
                            StationManager.DataStorage.SaveParent(mother);
                            if (!String.IsNullOrEmpty(mother.Id) && !String.IsNullOrEmpty(cur.Id))
                            {
                                ParentChild pc = new ParentChild();
                                pc.Parent = mother;
                                pc.Child = cur;
                                pc.Role = "mother";

                                StationManager.DataStorage.SaveParentChild(pc);
                            }
                        }
                    }

                    Parent father = LoadParent(range, i, 2);
                    if (father != null)
                    {
                        Parent parentAnsw = StationManager.DataStorage.ParentExists(father);
                        if (parentAnsw != null)
                        {
                            var dialogRes = MessageBox.Show(
                                $"Родитель {father.SurnameNamePatr}, {father.Birthday?.ToString("dd/MMM/yyyy")} уже существует." +
                                $"Добавить как родителя ребёнка {cur.SurnameNamePatr}?",
                                "Ошибка добавления", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                            //TODO edit window

                            if (dialogRes == DialogResult.Yes)
                            {
                                if (!String.IsNullOrEmpty(parentAnsw.Id) && !String.IsNullOrEmpty(cur.Id))
                                {
                                    ParentChild pc = new ParentChild();
                                    pc.Parent = parentAnsw;
                                    pc.Child = cur;
                                    pc.Role = "father";

                                    StationManager.DataStorage.SaveParentChild(pc);
                                }
                            }
                        }
                        else
                        {
                            StationManager.DataStorage.SaveParent(father);
                            if (!String.IsNullOrEmpty(father.Id) && !String.IsNullOrEmpty(cur.Id))
                            {
                                ParentChild pc = new ParentChild();
                                pc.Parent = father;
                                pc.Child = cur;
                                pc.Role = "father";

                                StationManager.DataStorage.SaveParentChild(pc);
                            }
                        }
                    }

                    Parent trustee1 = LoadParent(range, i, 3);
                    if (trustee1 != null)
                    {
                        Parent parentAnsw = StationManager.DataStorage.ParentExists(trustee1);
                        if (parentAnsw != null)
                        {
                            var dialogRes = MessageBox.Show(
                                $"Родитель {trustee1.SurnameNamePatr}, {trustee1.Birthday?.ToString("dd/MMM/yyyy")} уже существует." +
                                $"Добавить как родителя ребёнка {cur.SurnameNamePatr}?",
                                "Ошибка добавления", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                            //TODO edit window

                            if (dialogRes == DialogResult.Yes)
                            {
                                if (!String.IsNullOrEmpty(parentAnsw.Id) && !String.IsNullOrEmpty(cur.Id))
                                {
                                    ParentChild pc = new ParentChild();
                                    pc.Parent = parentAnsw;
                                    pc.Child = cur;
                                    pc.Role = "trustee";
                                    var val = range.Range["BP" + i]?.Value2;
                                    pc.Trustee = val != null;
                                    val = range.Range["BQ" + i]?.Value2;
                                    if (val != null)
                                    {
                                        pc.Relation = val.ToString();
                                    }

                                    StationManager.DataStorage.SaveParentChild(pc);
                                }
                            }
                        }
                        else
                        {
                            StationManager.DataStorage.SaveParent(trustee1);
                            if (!String.IsNullOrEmpty(trustee1.Id) && !String.IsNullOrEmpty(cur.Id))
                            {
                                ParentChild pc = new ParentChild();
                                pc.Parent = trustee1;
                                pc.Child = cur;
                                pc.Role = "trustee";
                                var val = range.Range["BP" + i]?.Value2;
                                pc.Trustee = val != null;
                                val = range.Range["BQ" + i]?.Value2;
                                if (val != null)
                                {
                                    pc.Relation = val.ToString();
                                }
                                StationManager.DataStorage.SaveParentChild(pc);
                            }
                        }
                    }

                    Parent trustee2 = LoadParent(range, i, 4);
                    if (trustee2 != null)
                    {
                        Parent parentAnsw = StationManager.DataStorage.ParentExists(trustee2);
                        if (parentAnsw != null)
                        {
                            var dialogRes = MessageBox.Show(
                                $"Родитель {trustee2.SurnameNamePatr}, {trustee2.Birthday?.ToString("dd/MMM/yyyy")} уже существует." +
                                $"Добавить как родителя ребёнка {cur.SurnameNamePatr}?",
                                "Ошибка добавления", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                            //TODO edit window

                            if (dialogRes == DialogResult.Yes)
                            {
                                if (!String.IsNullOrEmpty(parentAnsw.Id) && !String.IsNullOrEmpty(cur.Id))
                                {
                                    ParentChild pc = new ParentChild();
                                    pc.Parent = parentAnsw;
                                    pc.Child = cur;
                                    pc.Role = "trustee";
                                    var val = range.Range["CH" + i]?.Value2;
                                    pc.Trustee = val != null;
                                    val = range.Range["CI" + i]?.Value2;
                                    if (val != null)
                                    {
                                        pc.Relation = val.ToString();
                                    }
                                    StationManager.DataStorage.SaveParentChild(pc);
                                }
                            }
                        }
                        else
                        {
                            StationManager.DataStorage.SaveParent(trustee1);
                            if (!String.IsNullOrEmpty(trustee2.Id) && !String.IsNullOrEmpty(cur.Id))
                            {
                                ParentChild pc = new ParentChild();
                                pc.Parent = trustee2;
                                pc.Child = cur;
                                pc.Role = "trustee";
                                var val = range.Range["CH" + i]?.Value2;
                                pc.Trustee = val != null;
                                val = range.Range["CI" + i]?.Value2;
                                if (val != null)
                                {
                                    pc.Relation = val.ToString();
                                }
                                StationManager.DataStorage.SaveParentChild(pc);
                            }
                        }
                    }

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

        private static Student LoadStudent(Excel.Range range, int row)
        {
            Student cur = new Student();
            var val = range.Range["B" + row]?.Value2;
            if (val != null)
            {
                cur.NumAlphBook = val.ToString();
            }
            else
            {
                return null;
            }

            val = range.Range["C" + row]?.Value2;
            if (val != null)
                cur.Surname = val.ToString();
            val = range.Range["D" + row]?.Value2;
            if (val != null)
                cur.StName = val.ToString();
            val = range.Range["E" + row]?.Value2;
            if (val != null)
                cur.Patronymic = val.ToString();

            val = range.Range["F" + row]?.Value2;
            if (val != null)
                cur.Sex = val.ToString();
            val = range.Range["G" + row]?.Value;
            if (val != null)
                cur.Birthday = DateTime.Parse(val.ToString());

            val = range.Range["H" + row]?.Value2;
            if (val != null)
                cur.TypeDoc = val.ToString();
            val = range.Range["I" + row]?.Value2;
            if (val != null)
                cur.SerDoc = val.ToString();
            val = range.Range["J" + row]?.Value2;
            if (val != null)
                cur.NumDoc = val.ToString();

            val = range.Range["K" + row]?.Value2;
            if (val != null)
                cur.Index = val.ToString();
            val = range.Range["L" + row]?.Value2;
            if (val != null)
                cur.City = val.ToString();
            val = range.Range["M" + row]?.Value2;
            if (val != null)
                cur.Street = val.ToString();
            val = range.Range["N" + row]?.Value2;
            if (val != null)
                cur.House = val.ToString();
            val = range.Range["O" + row]?.Value2;
            if (val != null)
                cur.Apart = val.ToString();

            //TODO read mobile number

            val = range.Range["Q" + row]?.Value2;
            if (val != null)
                cur.HomePhone = val.ToString();

            val = range.Range["R" + row]?.Value2;
            cur.GpdAttendance = val != null;
            val = range.Range["S" + row]?.Value2;
            cur.ExamAllowedToPass = val != null;

            return cur;
        }

        private static Parent LoadParent(Excel.Range usedRange, int row, int parentIndex)
        {
            Excel.Range range = null;
            switch (parentIndex)
            {
                case 1:
                    range = usedRange.Range["U1", "AI50"];
                    break;
                case 2:
                    range = usedRange.Range["AK1", "AY50"];
                    break;
                case 3:
                    range = usedRange.Range["BA1", "BQ50"];
                    break;
                case 4:
                    range = usedRange.Range["BS1", "CI50"];
                    break;
                default:
                    range = usedRange.Range["U1", "AI50"];
                    break;
            }
            Parent cur = new Parent();
            var val = range.Range["A" + row]?.Value2;
            if (val != null)
            {
                cur.Surname = val.ToString();
            }
            else
            {
                return null;
            }

            val = range.Range["B" + row]?.Value2;
            if (val != null)
                cur.PName = val.ToString();
            val = range.Range["C" + row]?.Value2;
            if (val != null)
                cur.Patronymic = val.ToString();

            val = range.Range["D" + row]?.Value2;
            if (val != null)
                cur.Sex = val.ToString();
            val = range.Range["E" + row]?.Value;
            if (val != null)
                cur.Birthday = DateTime.Parse(val.ToString());

            val = range.Range["F" + row]?.Value2;
            if (val != null)
                cur.HomePhone = val.ToString();
            
            //TODO read mobile number

            val = range.Range["H" + row]?.Value2;
            if (val != null)
                cur.Index = val.ToString();
            val = range.Range["I" + row]?.Value2;
            if (val != null)
                cur.City = val.ToString();
            val = range.Range["J" + row]?.Value2;
            if (val != null)
                cur.Street = val.ToString();
            val = range.Range["K" + row]?.Value2;
            if (val != null)
                cur.House = val.ToString();
            val = range.Range["L" + row]?.Value2;
            if (val != null)
                cur.Apart = val.ToString();

            val = range.Range["M" + row]?.Value2;
            if (val != null)
                cur.WorkPhone = val.ToString();

            val = range.Range["N" + row]?.Value2;
            if (val != null)
                cur.Work = val.ToString();
            val = range.Range["O" + row]?.Value2;
            if (val != null)
                cur.Commentary = val.ToString();

            return cur;
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