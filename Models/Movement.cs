using System;

namespace BD_oneLove.Models
{
    internal class Movement
    {

        #region Props

        public string Id { get; set; }

        public string StudentId { get; set; }
        public string StudentFIO { get; set; }

        public string SchCity { get; set; }
        public string SchRegion { get; set; }
        public string SchCountry { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.Now;

        public bool Income { get; set; }

        #endregion
    }
}
