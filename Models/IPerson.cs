using System.Collections.Generic;

namespace BD_oneLove.Models
{
    internal interface IPerson
    {
        string SurnameNamePatr { get; }
        List<string> MobileNumbers { get; set; }
    }
}
