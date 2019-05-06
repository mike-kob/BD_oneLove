namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {

        SignInView, MainView,  MarkGrid, ClassesView, 

        //директор
        TeachersView,
        UsersView,

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