using BD_oneLove.Models;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class ParentCardViewModel
    {
        public ParentCardViewModel()
        {
            CurParent = StationManager.CurrentParent;
        }

        #region Props

        public string ParentType
        {
            get { return CurParent?.Sex == "м" ? "Отец" : "Мать"; }
        }

        public Parent CurParent { get; set; }

        #endregion
    }


}
