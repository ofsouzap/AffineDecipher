using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffineDecipher
{
    class FrequencyAnalysis
    {

        public static Dictionary<char, int> GetTextCharFrequency(string text,
            char[] countedChars)
        {

            Dictionary<char, int> charFrequency = new Dictionary<char, int>();

            foreach (char c in text)
            {

                if (!countedChars.Contains(c)) continue;

                if (charFrequency.ContainsKey(c))
                {

                    charFrequency[c]++;

                }
                else
                {

                    charFrequency.Add(c, 1);

                }

            }

            return charFrequency;

        }

        public static Dictionary<char, double> CharFrequencyToCharProportion(Dictionary<char, int> charFrequency)
        {

            int totalChars = 0;

            foreach (char c in charFrequency.Keys) totalChars += charFrequency[c];

            Dictionary<char, double> charProportion = new Dictionary<char, double>();

            foreach (char c in charFrequency.Keys)
            {

                charProportion[c] = (double)charFrequency[c] / totalChars;

            }

            return charProportion;

        }

        public static double GetKeyFrequencyDifference<T>(Dictionary<T, double> a, Dictionary<T, double> b)
        {

            double difference = 0;

            foreach (T key in a.Keys)
            {

                double aValue = a.ContainsKey(key) ? a[key] : 0;
                double bValue = b.ContainsKey(key) ? b[key] : 0;

                difference += Math.Abs(a[key] - b[key]);

            }

            return difference;

        }

    }
}
