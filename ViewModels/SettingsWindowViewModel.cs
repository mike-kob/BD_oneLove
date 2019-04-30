using System;
using System.IO;
using System.Windows.Input;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels
{
    class SettingsWindowViewModel : BaseViewModel
    {
        public SettingsWindowViewModel()
        {
            try
            {
                var file = new FileInfo(FileFolderHelper.StorageFilePath);
                if (file.CreateFolderAndCheckFileExistance())
                {
                    string props = File.ReadAllText(FileFolderHelper.StorageFilePath);
                    string[] splitProps = props.Split(' ');
                    if (splitProps.Length == 5)
                    {
                        CompName = splitProps[0];
                        ServerName = splitProps[1];
                        DBName = splitProps[2];
                        UserName = splitProps[3];
                        StationManager.DbPassword.Password = splitProps[4];
                        CompColor = _unmodifiedColor;
                        ServerColor = _unmodifiedColor;
                        DBColor = _unmodifiedColor;
                        UserColor = _unmodifiedColor;
   
                    }
                }
                else
                {
                    file.Create();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to access file {FileFolderHelper.StorageFilePath}", ex);
            }
        }

        #region Fileds

        private string _compName;
        private string _serverName;
        private string _dbName;
        private string _userName;

        private ICommand _cancelCommand;
        private ICommand _saveCommand;

        private string _unmodifiedColor = "#e8ffe8";
        private string _modifiedColor = "#ffffff";
        #endregion

        #region Props

        public string CompName
        {
            get { return _compName;}
            set
            {
                _compName = value;
                CompColor = _modifiedColor;
                OnPropertyChanged("CompName");
                OnPropertyChanged("CompColor");
            }
        }
        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                ServerColor = _modifiedColor;
                OnPropertyChanged("ServerColor");
                OnPropertyChanged("ServerName");
            }
        }
        public string DBName
        {
            get { return _dbName; }
            set
            {
                _dbName = value;
                DBColor = _modifiedColor;
                OnPropertyChanged("DBColor");
                OnPropertyChanged("DBName");
            }
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                UserColor = _modifiedColor;
                OnPropertyChanged("UserColor");
                OnPropertyChanged("UserName");
            }
        }

        public string CompColor { get; set; }
        public string ServerColor { get; set; }
        public string DBColor { get; set; }
        public string UserColor { get; set; }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<object>(o => StationManager.MySettings.Close()));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                           new RelayCommand<object>(SaveImplementation));
            }
        }
        #endregion

        private void SaveImplementation(object obj)
        {
            try
            {
                var file = new FileInfo(FileFolderHelper.StorageFilePath);
                if (!file.CreateFolderAndCheckFileExistance())
                {
                    file.Create();
                }
                using (var stream = new StreamWriter(FileFolderHelper.StorageFilePath))
                {
                    stream.Write(CompName);
                    stream.Write(' ');
                    stream.Write(ServerName);
                    stream.Write(' ');
                    stream.Write(DBName);
                    stream.Write(' ');
                    stream.Write(UserName);
                    stream.Write(' ');
                    stream.Write(StationManager.DbPassword.Password);
                }
                StationManager.ConnectionString = $"Data Source={CompName}\\{ServerName};" +
                                                  $"Initial Catalog={DBName};User ID={UserName};Password={StationManager.DbPassword.Password}";
                CompColor = _unmodifiedColor;
                ServerColor = _unmodifiedColor;
                DBColor = _unmodifiedColor;
                UserColor = _unmodifiedColor;
                OnPropertyChanged("CompColor");
                OnPropertyChanged("ServerColor");
                OnPropertyChanged("DBColor");
                OnPropertyChanged("UserColor");


            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to access file {FileFolderHelper.StorageFilePath}", ex);
            }
        }
    }
}
