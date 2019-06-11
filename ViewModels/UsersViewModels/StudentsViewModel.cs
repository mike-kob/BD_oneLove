using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using Microsoft.Office.Interop.Word;


namespace BD_oneLove.ViewModels.UsersViewModels
{
    class StudentsViewModel:BaseViewModel
    {

        public StudentsViewModel()
        {
            ViewSource = new CollectionViewSource();
            ViewSource.Source = ClassStudents;
            Classes = StationManager.DataStorage.GetClasses(CurrentYear);
            Class temp = new Class();
            temp.NumberLetter = "Все";
            Classes.Add(temp);
            ViewSource.Filter += new FilterEventHandler(ShowWithFilter);

        }

        #region Fields

        private Class _selectedClass;
        private Student _selectedStudent;
        public string[] _filters = { "Id", "Тип док.", "Серия", "Номер", "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения", "Алф. книга",
        "Индекс", "Город", "Улица", "Дом", "Квартира", "Дом. телефон"};


        private Visibility _isShowId;
        private Visibility _isShowName;
        private Visibility _isShowSurname;
        private Visibility _isShowPatr;
        private Visibility _isShowAddr;
        private Visibility _isShowAlph;
        private Visibility _isShowSex;
        private Visibility _isShowBirthday;
        private Visibility _isShowDoc;
        private Visibility _isShowExam;
        private Visibility _isShowGPD;
        private Visibility _isShowPhone;
        private Visibility _isShowMobile;

        private ICommand _saveCommand;
        private ICommand _removeCommand;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _addCommand;
        private ICommand _filterCommand;
        private ICommand _createDocumentCommand;

        public event System.Windows.Data.FilterEventHandler Filter;
        #endregion

        #region Props

        public int  SelectedIndex
        {
            get; set;
        }

        public CollectionViewSource ViewSource
        {
            get;
        }
        public List<Class> Classes { get; set; }

        public string FilterString { get; set; }

        public IEnumerable<string> Filters
        {
            get { return _filters; }
        }

        public string CurrentYear { get; set; } = StationManager.DataStorage.GetCurYear();

        public List<Student> ClassStudents { get; set; } = new List<Student>();

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;


            }
        }

        public Class SelectedClass {
            get
            {
                return _selectedClass;
            }
            set
            {
                _selectedClass = value;

                if (_selectedClass.NumberLetter == "Все")
                {

                    ClassStudents.Clear();
                    foreach(Class c in Classes)
                    {
                        IEnumerable<Student> temp = StationManager.DataStorage.GetStudents(c);
                        ClassStudents.AddRange(temp);
                    }
                }
                else
                {
                     ClassStudents = StationManager.DataStorage.GetStudents(_selectedClass);
                    ViewSource.Source = ClassStudents;
                }
                
                ViewSource.View.Refresh();
                

            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<object>(o => OnPropertyChanged("ClassStudents")));
            }
        }

        public ICommand FilterCommand
        {
            get
            {
                return _filterCommand ?? (_filterCommand =
                           new RelayCommand<object>(FilterImplementation));
            }
        }

        public ICommand GradesCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<object>(GradesImplementation, IsSelected));
            }
        }


        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand =
                           new RelayCommand<object>(RemoveImplementation, IsSelected));
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               StationManager.CurrentStudent = new Student();
                               AddStudentDialogView win = new AddStudentDialogView();
                               win.ShowDialog();
                               RefreshList();
                           }));
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand =
                           new RelayCommand<object>(o =>
                           {
                             
                               StationManager.CurrentStudent = _selectedStudent;

                               StationManager.CurrentStudent.Father =
                                   StationManager.DataStorage.GetFather(_selectedStudent);

                               StationManager.CurrentStudent.Mother =
                                   StationManager.DataStorage.GetMother(_selectedStudent);
                               AddStudentDialogView win = new AddStudentDialogView();
                               win.ShowDialog();
                               RefreshList();
                           }, IsSelected));
            }
        }



        public ICommand CreateDocumentCommand
        {
            get
            {
                return _createDocumentCommand ?? (_createDocumentCommand =
                           new RelayCommand<object>(o =>
                           {
                               Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                               winword.Visible = true;
                               object missing = System.Reflection.Missing.Value;
                               Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                               document.Content.SetRange(0, 0);


                               Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                               para1.Range.Text = "СПРАВКА";
                               para1.Range.Bold = 1;
                               para1.Range.Font.Size = 18;
                               para1.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                               para1.Range.InsertParagraphAfter();

                               Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                               para2.Range.Font.Size = 14;
                               para2.Range.Text = "\nДана " + SelectedStudent.SurnameNamePatr + " в том, что она действительно обучается " +
                               "в Государственном бюджетном образовательном учреждении \"Стахановская специализированная школа I-III ступеней" +
                               " №10\" в "+SelectedClass.NumberLetter +" классе. ";
                               para2.Range.Bold = 0;
                               para2.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                               para2.Range.InsertParagraphAfter();


                               Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
                               para3.Range.Text = "Справка дана для предьявления по месту требования";
                               para3.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                               para3.Range.Font.Size = 14;
                               para3.Range.InsertParagraphAfter();


                           }, IsSelected));
            }
        }


        public Visibility IsShowId => _isShowId;
        public Visibility IsShowName => _isShowName;
        public Visibility IsShowSurname => _isShowSurname;
        public Visibility IsShowPatr => _isShowPatr;
        public Visibility IsShowAddr => _isShowAddr;
        public Visibility IsShowSex => _isShowSex;
        public Visibility IsShowAlph => _isShowAlph;
        public Visibility IsShowBirthday => _isShowBirthday;
        public Visibility IsShowExam => _isShowExam;
        public Visibility IsShowGPD => _isShowGPD;
        public Visibility IsShowPhone => _isShowPhone;
        public Visibility IsShowDoc => _isShowDoc;
        public Visibility IsShowMobile => _isShowMobile;

        public bool IsShowIdBool
        {
            get { return _isShowId == Visibility.Visible; }
            set
            {
                _isShowId = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowId");
            }
        }
        public bool IsShowNameBool
        {
            get { return _isShowName == Visibility.Visible; }
            set
            {
                _isShowName = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowName");
            }
        }
        public bool IsShowSurnameBool
        {
            get { return _isShowSurname == Visibility.Visible; }
            set
            {
                _isShowSurname = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowSurname");
            }
        }
        public bool IsShowPatrBool
        {
            get { return _isShowPatr == Visibility.Visible; }
            set
            {
                _isShowPatr = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowPatr");
            }
        }
        public bool IsShowAddrBool
        {
            get { return _isShowAddr == Visibility.Visible; }
            set
            {
                _isShowAddr = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowAddr");
            }
        }
        public bool IsShowSexBool
        {
            get { return _isShowSex == Visibility.Visible; }
            set
            {
                _isShowSex = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowSex");
            }
        }
        public bool IsShowPhoneBool
        {
            get { return _isShowPhone == Visibility.Visible; }
            set
            {
                _isShowPhone = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowPhone");
            }
        }
        public bool IsShowGPDBool
        {
            get { return _isShowGPD == Visibility.Visible; }
            set
            {
                _isShowGPD = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowGPD");
            }
        }
        public bool IsShowExamBool
        {
            get { return _isShowExam == Visibility.Visible; }
            set
            {
                _isShowExam = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowExam");
            }
        }
        public bool IsShowDocBool
        {
            get { return _isShowDoc == Visibility.Visible; }
            set
            {
                _isShowDoc = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowDoc");
            }
        }
        public bool IsShowBirthdayBool
        {
            get { return _isShowBirthday == Visibility.Visible; }
            set
            {
                _isShowBirthday = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowBirthday");
            }
        }
        public bool IsShowAlphBool
        {
            get { return _isShowAlph == Visibility.Visible; }
            set
            {
                _isShowAlph = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowAlph");
            }
        }

        public bool IsShowMobileBool
        {
            get { return _isShowMobile == Visibility.Visible; }
            set
            {
                _isShowMobile = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowMobile");
            }
        }

        #endregion

        private void FilterImplementation(object obj)
        {
            ViewSource.View.Refresh();
        }


        private void ShowWithFilter(object sender, FilterEventArgs e)
        {
            Student st = e.Item as Student;
            if (st != null && !String.IsNullOrEmpty(FilterString))
            {
                switch (SelectedIndex)
                {
                    case 0:  e.Accepted = (st.Id == FilterString); break;
                    case 1: e.Accepted = (st.TypeDoc == FilterString); break;
                    case 2: e.Accepted = (st.SerDoc == FilterString); break;
                    case 3: e.Accepted = (st.NumDoc == FilterString); break;
                    case 4: e.Accepted = (st.Surname == FilterString); break;
                    case 5: e.Accepted = (st.StName == FilterString); break;
                    case 6: e.Accepted = (st.Patronymic == FilterString); break;
                    case 7: e.Accepted = (st.Sex == FilterString); break;
                    case 8: e.Accepted = (st.BirthdayString == FilterString); break;
                    case 9: e.Accepted = (st.NumAlphBook == FilterString); break;
                    case 10: e.Accepted = (st.Index == FilterString); break;
                    case 11: e.Accepted = (st.City == FilterString); break;
                    case 12: e.Accepted = (st.Street == FilterString); break;
                    case 13: e.Accepted = (st.House == FilterString); break;
                    case 14: e.Accepted = (st.Apart == FilterString); break;
                    case 15: e.Accepted = (st.HomePhone == FilterString); break;
                  
                    
                }
            }
            else
            {
                e.Accepted = true;
            }
        }

        private void GradesImplementation(object obj)
        {
            StationManager.CurrentStudent = SelectedStudent;
            StationManager.CurrentClass = SelectedClass;
            StudentsGradesViewDialog win = new StudentsGradesViewDialog();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
        }

        private void RemoveImplementation(object obj)
        {

            var res = MessageBox.Show("Вы действитьно хотите выписать ученика из класса?", "Warning", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                //StationManager.DataStorage.ExpelStudent(_selectedStudent, _myClass);
                //ClassStudents = StationManager.DataStorage.GetStudents(_myClass);
                OnPropertyChanged("ClassStudents");
            }
        }

        private bool IsSelected(object obj)
        {
            return _selectedStudent != null;
        }

        private void RefreshList()
        {
           // ClassStudents = StationManager.DataStorage.GetStudents(_myClass);
            OnPropertyChanged("ClassStudents");
        }

    }
}

