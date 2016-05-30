using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PaperParser
{
    public class DataLoader
    {
        public List<string> GetData(string file)
        {
            return ReadFrom(file).ToList();
        }

        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
