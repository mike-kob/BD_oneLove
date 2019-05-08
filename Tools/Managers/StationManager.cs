using System.Windows;
using System.Windows.Controls;
using BD_oneLove.Models;
using BD_oneLove.Tools.DataStorage;
using BD_oneLove.ViewModels.UsersViewModels;

namespace BD_oneLove.Tools.Managers
{
    internal static class StationManager
    {
        private static IDataStorage _dataStorage = new DataStorage.DataStorage();

        public static User CurrentUser { get; set; }
        public static Parent CurrentParent { get; set; }
        public static Class CurrentClass { get; set; }
        public static Student CurrentStudent { get; set; }
        public static Teacher CurrentTeacher { get; set; }
        public static Plan CurrentPlan { get; set; }
        public static UsersViewModel usersView { get; set; }
        public static TeachersViewModel TeachersView { get; set; }

        public static Window MyMain { get; set; }
        public static Window MySettings { get; set; }

        public static PasswordBox MainPassword { get; set; }
        public static PasswordBox DbPassword { get; set; }

        public static string ConnectionString { get; set; }

        public static IDataStorage DataStorage
        {
            get { return _dataStorage; }
        }
    }
}
