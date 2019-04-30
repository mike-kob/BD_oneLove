using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Tools.Navigation
{
    internal class ViewNavigationModel : BaseNavigationModel
    {
        public ViewNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                //case ViewType.SignInView:
                //    ViewsDictionary.Add(viewType, new SignInView());
                //    break;
                //case ViewType.MainView:
                //    ViewsDictionary.Add(viewType, new TemplateControlView());
                //    break;
                //case ViewType.AddPersonView:
                //    ViewsDictionary.Add(viewType, new AddPersonView());
                //    break;
                //case ViewType.EditPersonView:
                //    ViewsDictionary.Add(viewType, new EditPersonView());
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
