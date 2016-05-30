using System.Collections.Generic;
using PaperParser.Model;

namespace PaperParser
{
    public class DataParser
    {
        public WarAnalysis Parse(List<string> data)
        {
            var currentKingdom = "(6:18)";

            var warAnalysis = new WarAnalysis();

            foreach (var action in data)
            {
                warAnalysis.AddAction(action);
            }

            return warAnalysis;
        }
    }
}