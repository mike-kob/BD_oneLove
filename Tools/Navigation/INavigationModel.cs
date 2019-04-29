namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {
        SignInView
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}