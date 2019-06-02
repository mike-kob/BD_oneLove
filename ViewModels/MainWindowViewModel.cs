using System;
using System.IO;
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
                    string props = File.ReadAllText(FileFolderHelper.StorageFilePath);
                    string[] splitProps = props.Split(' ');
                    if (splitProps.Length == 5)
                    {
                        StationManager.ConnectionString = $"Data Source={splitProps[0]}\\{splitProps[1]};" +
                                                          $"Initial Catalog={splitProps[2]};User ID={splitProps[3]};Password={splitProps[4]}";
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