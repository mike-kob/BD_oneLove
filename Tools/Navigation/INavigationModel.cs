namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {
        SignInView, 
	MainView
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}