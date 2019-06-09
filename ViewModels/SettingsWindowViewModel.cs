using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Input;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System.Security.Cryptography;
using System.Text;

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
                    
                    byte[] encoded = File.ReadAllBytes(FileFolderHelper.StorageFilePath);
                    if(encoded.Length == 0)
                        return;
                    byte[] decoded = ProtectedData.Unprotect(encoded, StationManager.SecretKey, 
                        DataProtectionScope.CurrentUser);
                    string props = Encoding.Unicode.GetString(decoded);
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(props);
                    CompName = builder.DataSource.Split('\\')[0];
                    ServerName = builder.DataSource.Split('\\')[1];
                    DBName = builder.InitialCatalog;
                    UserName = builder.UserID;
                    StationManager.DbPassword.Password = builder.Password;

                    CompColor = _unmodifiedColor;
                    ServerColor = _unmodifiedColor;
                    DBColor = _unmodifiedColor;
                    UserColor = _unmodifiedColor;

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
                StationManager.ConnectionString = $"Data Source={CompName}\\{ServerName};" +
                                                  $"Initial Catalog={DBName};User ID={UserName};Password={StationManager.DbPassword.Password}";

                byte[] data = Encoding.Unicode.GetBytes(StationManager.ConnectionString);
                byte[] encoded = ProtectedData.Protect(data, StationManager.SecretKey,
                    DataProtectionScope.CurrentUser);
                File.WriteAllBytes(FileFolderHelper.StorageFilePath, encoded);

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
