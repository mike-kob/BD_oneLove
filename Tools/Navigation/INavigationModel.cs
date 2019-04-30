namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {
        SignInView,
        TemplateView
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}