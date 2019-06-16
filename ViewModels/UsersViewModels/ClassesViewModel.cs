using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class ClassesViewModel : BaseViewModel
    {
        private string _selYear = StationManager.CurrentYear;
        private Class _selClass;

        public ClassesViewModel()
        {
            Classes = StationManager.DataStorage.GetClasses(SelectedYear);
            SelectedYear = StationManager.CurrentYear;
            StationManager.RefreshYearListEvent += () => 
            {
                Years = StationManager.DataStorage.GetYears();
                OnPropertyChanged("Years");
            };
        }

        #region Property

        public List<string> Years { get; set; } = StationManager.DataStorage.GetYears();
        public List<Class> Classes { get; set; } 
        public List<Teacher> Teachers { get; set; }


        public string SelectedYear
        {
            get { return _selYear; }
            set
            {
                _selYear = value;
                Classes = StationManager.DataStorage.GetClasses(SelectedYear);
                OnPropertyChanged("SelectedYear");
                OnPropertyChanged("Classes");
            }
        }

        public Class SelectedClass {
            get { return _selClass; }
            set
            {
                _selClass = value;
                RefreshTeachersList();
            }
        }

        public Teacher SelectedTeacher{get; set;}

        #endregion

                
        #region Commands

        private RelayCommand<object> _addTeacherCommand;
        private RelayCommand<object> _deleteTeacherCommand;
        private RelayCommand<object> _addClassCommand;
        private RelayCommand<object> _deleteClassCommand;
        private RelayCommand<object> _editClassCommand;


        #endregion

        public ICommand AddTeacherCommand
        {
            get
            {
                return _addTeacherCommand ?? (_addTeacherCommand = new RelayCommand<object>(
                         o => AddTeacherImplementation(), o => SelectedClass != null));
            }
        }

        private void AddTeacherImplementation()
        {
            StationManager.CurrentClass = SelectedClass;
            AddTeacherClassView win = new AddTeacherClassView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshTeachersList();
        }

        public ICommand DeleteTeacherCommand
        {
            get
            {
                return _deleteTeacherCommand ?? (_deleteTeacherCommand = new RelayCommand<object>(
                         o => DeleteTeacherImplementation(), o => SelectedClass != null && SelectedTeacher != null));
            }
        }

        private void DeleteTeacherImplementation()
        {
            var res = MessageBox.Show("Вы действитьно хотите удалить учителя из класса?", "Warning", MessageBoxButtons.YesNo,
                  MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                StationManager.DataStorage.DeleteTeacherClass(SelectedTeacher, SelectedClass);
                RefreshTeachersList();
            }
        }

        public ICommand AddClassCommand
        {
            get
            {
                return _addClassCommand ?? (_addClassCommand = new RelayCommand<object>(
                         o => AddClassImplementation()));
            }
        }

        private void AddClassImplementation()
        {
            Class c = new Class();
            c.StYear = SelectedYear;
            StationManager.CurrentClass = c;
            AddClassDialogView win = new AddClassDialogView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public ICommand EditClassCommand
        {
            get
            {
                return _editClassCommand ?? (_editClassCommand = new RelayCommand<object>(
                         o => EditClassImplementation(), o => SelectedClass != null));
            }
        }

        private void EditClassImplementation()
        {
            StationManager.CurrentClass = SelectedClass;
            AddClassDialogView win = new AddClassDialogView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public ICommand DeleteClassCommand
        {
            get
            {
                return _deleteClassCommand ?? (_deleteClassCommand = new RelayCommand<object>(
                         o => DeleteClassImplementation(), o => SelectedClass != null));
            }
        }

        private void DeleteClassImplementation()
        {
            var res = MessageBox.Show("Вы действитьно хотите удалить класс?", "Warning", MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                StationManager.DataStorage.DeleteClass(SelectedClass);
                RefreshList();
            }
        }

        private void RefreshTeachersList()
        {
            if (SelectedClass!=null) Teachers = StationManager.DataStorage.GetTeachers(SelectedClass);
            OnPropertyChanged("Teachers");
        }

        private void RefreshList()
        {
            Classes = StationManager.DataStorage.GetClasses(SelectedYear);
            OnPropertyChanged("Classes");
        }
    }
}
