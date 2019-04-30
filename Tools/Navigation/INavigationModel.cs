namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {
        SignInView, MainView, TeachersView, MarkGrid
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}