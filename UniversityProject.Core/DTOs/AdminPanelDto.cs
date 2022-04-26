namespace UniversityProject.Core.DTOs
{
    public class AdminPanelDto
    {
        public int UserCount { get; set; }
        public int CategoryCount { get; set; }
        public int CommentCount { get; set; }
        public int BookCount { get; set; }
        public int BannerCount { get; set; }
        public int SliderCount { get; set; }
        public int MessageCount { get; set; }
        public int UnreadMessageCount { get; set; }
        public int UserBookCount { get; set; }
        public double Penalty { get; set; }
        public int UserBookNotReturn { get; set; }
    }
}
