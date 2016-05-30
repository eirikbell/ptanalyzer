using PaperParser.Model;

namespace PaperParser.Actions
{
    public class UtopiaAction
    {
        public UtopiaDate Date { get; set; }
        public string ActionString { get; set; }

        public override string ToString()
        {
            return $"{Date} {ActionString}";
        }
    }
}