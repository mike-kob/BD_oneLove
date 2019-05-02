using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    internal class ResponPerson
    {
        #region Props

        public string Id { get; }

        public string PName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }

        public string Sex { get; set; }
        public string Birthday { get; set; }

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apart { get; set; }

        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Commentary { get; set; }

        public bool Trustee { get; set; }
        public string Relation { get; set; }
        #endregion
    }
}
