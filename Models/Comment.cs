namespace BD_oneLove.Models
{
    internal class Comment
    {

        #region Props
        public string Id { get; set; }
        public string Descr { get; set; }
        public string StudentId { get; set; }

        #endregion

        public override string ToString()
        {
            return Descr;
        }
    }
}
