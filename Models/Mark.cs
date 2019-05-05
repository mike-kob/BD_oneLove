using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    internal class Mark
    {
        public Mark()
        {
            
        }

        #region Props

        public string Id { get; set; }
        public int? Grade { get; set; } = null;
        public string MarkType { get; set; }
        public string Subject { get; set; }
        public DateTime MarkDate { get; set; } = DateTime.Now;

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string ClassId { get; set; }

        #endregion
    }
}
