namespace PaperParser.Model
{
    public class UtopiaDate
    {
        public UtopiaMonthEnum Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }

        public int GetDayNumber()
        {
            return Day + ((int)Month * 24) + (Year * 7 * 24);
        }

        public override string ToString()
        {
            return $"{Month} {Day} of YR{Year}";
        }
    }
}