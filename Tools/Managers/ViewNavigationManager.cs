using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD_oneLove.Tools.Navigation;

namespace BD_oneLove.Tools.Managers
{
    internal class ViewNavigationManager
    {
        private static readonly object Locker = new object();
        private static ViewNavigationManager _instance;

        internal static ViewNavigationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ?? (_instance = new ViewNavigationManager());
                }
            }
        }

        private INavigationModel _navigationModel;

        private ViewNavigationManager()
        {

        }

        internal void Initialize(INavigationModel navigationModel)
        {
            _navigationModel = navigationModel;
        }

        internal void Navigate(ViewType viewType)
        {
            _navigationModel.Navigate(viewType);

        }

    }
}
