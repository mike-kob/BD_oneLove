using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

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
           
        }

        #region Fields

        private Class _selectedClass;
        private Student _selectedStudent;


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

        private ICommand _saveCommand;
        private ICommand _removeCommand;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _addCommand;
        #endregion

        #region Props

        public CollectionViewSource ViewSource
        {
            get;
        }
        public List<Class> Classes { get; set; }

        public string CurrentYear { get; set; } = StationManager.DataStorage.GetCurYear();

        public List<Student> ClassStudents { get; set; } = new List<Student>();

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

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; }
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
        #endregion


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

