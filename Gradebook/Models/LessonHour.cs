namespace Gradebook.Models
{
    public class LessonHour
    {
        public int StartH { get; set; }
        public int StartM { get; set; }
        public int DurationM { get; set; }

        public LessonHour(int startH, int startM, int durationM)
        {
            StartH = startH;
            StartM = startM;
            DurationM = durationM;
        }
    }
}