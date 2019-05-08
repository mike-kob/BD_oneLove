using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class ClassesViewModel:BaseViewModel
    {
        public List<string> Years { get; set; } = StationManager.DataStorage.GetYears();
        public List<Class> Classes { get; set; }
        public Class SelectedClass { get; set; }
        public List<Teacher> Teachers { get; set; }

        private string _selYear;

        public ClassesViewModel()
        {
            Classes = StationManager.DataStorage.GetClasses(SelectedYear);
        }

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

    }
}
