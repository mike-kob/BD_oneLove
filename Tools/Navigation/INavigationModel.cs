namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {

        SignInView, MainView, TeachersView, MarkGrid, ClassesView, UsersView,


        //классный рук-ль
        MyClassView,
        PutMarksView,
        SocialPassportView,
        ParentsView,
        MovementView

    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}