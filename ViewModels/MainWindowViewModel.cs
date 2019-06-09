using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ILoaderOwner
    {
        public MainWindowViewModel()
        {
            LoaderManeger.Instance.Initialize(this);
            try
            {
                var file = new FileInfo(FileFolderHelper.StorageFilePath);
                if (file.CreateFolderAndCheckFileExistance())
                {
                    byte[] encoded = File.ReadAllBytes(FileFolderHelper.StorageFilePath);
                    if (encoded.Length != 0)
                    {
                        byte[] decoded = ProtectedData.Unprotect(encoded, StationManager.SecretKey,
                            DataProtectionScope.CurrentUser);
                        StationManager.ConnectionString = Encoding.Unicode.GetString(decoded);
                    }
                    else
                    {
                        StationManager.ConnectionString = "";
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

        #region Fields

        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;

        #endregion

        #region Properties

        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}