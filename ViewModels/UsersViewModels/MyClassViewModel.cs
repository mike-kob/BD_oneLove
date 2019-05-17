using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System.Windows.Forms;
using BD_oneLove.Properties;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class MyClassViewModel : BaseViewModel
    {
        public MyClassViewModel()
        {
            _myClass = StationManager.CurrentClass;
            
            ClassStudents = StationManager.DataStorage.GetStudents(_myClass);
            ViewSource = new CollectionViewSource();
            ViewSource.Source = ClassStudents;
        }

        #region Fields

        private Class _myClass;
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
        private ICommand _importCommand;
        private ICommand _importFileCommand;
        private ICommand _searchCommand;
        #endregion

        #region Props

        public CollectionViewSource ViewSource { get; set; }

        public List<Student> ClassStudents { get; set; }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<object>(o=>OnPropertyChanged("ClassStudents")));
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
                               StationManager.CurrentClass = _myClass;
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
                               StationManager.CurrentClass = _myClass;
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

        public ICommand ImportCommand
        {
            get
            {
                return _importCommand ?? (_importCommand =
                           new RelayCommand<object>(o =>
                           {
                               OpenFileDialog w = new OpenFileDialog();
                               w.Filter = "Excel Worksheets|*.xls;*.xlsx";
                               w.ShowDialog();
                               List<Student> l = ExcelManager.LoadClassStudents(w.FileName, StationManager.CurrentClass);
                               ClassStudents.AddRange(l);
                               ViewSource.View.Refresh();
                               OnPropertyChanged("ClassStudents");
                               MessageBox.Show($"Импортировано учеников: {l.Count}\nПреоверьте правильность данных и нажмите 'Сохранить'",
                                   "Импорт", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                           }));
            }
        }

        public ICommand ImportFileCommand
        {
            get
            {
                return _importFileCommand ?? (_importFileCommand =
                           new RelayCommand<object>(o =>
                           {
                               SaveFileDialog w = new SaveFileDialog();
                               w.Title = "Save file for import";
                               w.Filter = "Excel Worksheets|*.xls";
                               var res = w.ShowDialog();
                               
                               if (res != DialogResult.Cancel && w.FileName != null)
                               {
                                   if(File.Exists(w.FileName))
                                       File.Delete(w.FileName);
                                   
                                   File.WriteAllBytes(w.FileName, Resources.Students);
                               }
                           }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                           new RelayCommand<object>(o =>
                           {
                               foreach (Student s in ClassStudents)
                               {
                                   if (!String.IsNullOrEmpty(s.Id))
                                   {
                                       StationManager.DataStorage.UpdateStudent(s);
                                       StationManager.DataStorage.AssignStudentToClass(s, StationManager.CurrentClass);
                                   }
                                   else
                                   {
                                       StationManager.DataStorage.SaveStudent(s);
                                       StationManager.DataStorage.AssignStudentToClass(s, StationManager.CurrentClass);
                                   }
                               }
                               ViewSource.View.Refresh();
                               OnPropertyChanged("ClassStudents");
                           }));
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand =
                           new RelayCommand<object>(o =>
                           {
                              SearchStudentDialog w = new SearchStudentDialog();
                              var res = w.ShowDialog();
                              if (res == true)
                              {

                              }
                           }));
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
                StationManager.DataStorage.ExpelStudent(_selectedStudent, _myClass);
                ClassStudents = StationManager.DataStorage.GetStudents(_myClass);
                ViewSource.View.Refresh();
                OnPropertyChanged("ClassStudents");
            }
        }

        private bool IsSelected(object obj)
        {
            return _selectedStudent != null;
        }

        private void RefreshList()
        {
            ClassStudents = StationManager.DataStorage.GetStudents(_myClass);
            ViewSource.Source = ClassStudents;
            ViewSource.View.Refresh();
            OnPropertyChanged("ClassStudents");
        }

    }
}
