using System;
using BD_oneLove.Views;
using TemplateControlView = BD_oneLove.Views.TemplateControlView;

namespace BD_oneLove.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.SignInView:
                    ViewsDictionary.Add(viewType, new SignInView());
                    break;
                case ViewType.MainView:
                    ViewsDictionary.Add(viewType, new TemplateControlView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}