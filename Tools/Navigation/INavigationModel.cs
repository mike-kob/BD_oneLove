namespace BD_oneLove.Tools.Navigation
{
    internal enum ViewType
    {
        //основные
        SignInView,
        MainView,

        //директор
        TeachersView,
        UsersView,
        PlanView,
        ClassesView,

        //классный рук-ль
        MyClassView,
        PutMarksView,
        SocialPassportView,
        ParentsView,
        MovementView,

        //заместитель
        ProgressView,
        StudentsView
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
        void DeleteView(ViewType viewType);
    }
}