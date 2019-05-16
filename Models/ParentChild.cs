namespace BD_oneLove.Models
{
    internal class ParentChild
    {

        public Student Child { get; set; }
        public Parent Parent { get; set; }

        public string Role
        {
            get
            {
                switch (SelectedRoleIndex)
                {
                    case 0:
                        return "father";
                    case 1:
                        return "mother";
                    case 2:
                        return "trustee";
                    default:
                        return null;
                }
            }
            set
            {
                switch (value)
                {
                    case "father":
                        SelectedRoleIndex = 0;
                        break;
                    case "mother":
                        SelectedRoleIndex = 1;
                        break;
                    case "trustee":
                        SelectedRoleIndex = 2;
                        break;
                    default:
                        SelectedRoleIndex = -1;
                        break;
                }
            }
        }
        public string RoleString
        {
            get { return SelectedRoleIndex != -1 ? Roles[SelectedRoleIndex] : ""; }
        }

        public bool? Trustee { get; set; }
        public string Relation { get; set; }

        public int SelectedRoleIndex { get; set; } = 0;
        public static string[] Roles { get; } = { "Отец", "Мать", "Занимается воспитанием" };

    }
}
