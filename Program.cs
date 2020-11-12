using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;

/*Heavily based off of VignereDeciphering project*/

namespace AffineDecipher
{
    class Program
    {

        public static readonly int[] multiplierOptions = new int[]
        {
            1,
            3,
            5,
            7,
            9,
            11,
            15,
            17,
            19,
            21,
            23,
            25
        };
        public static readonly char[] validCharacters = new char[]
        {
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z'
        };

        static void Main(string[] args)
        {

            string text;
            int printCount;

            Console.WriteLine("Text:");
            string input = Console.ReadLine();

            if (input[0] == '\\')
            {
                text = GetTextFromFile(input.Substring(1)).ToLower();
            }
            else
            {
                text = input.ToLower();
            }

            Console.Write("Print top how many?> ");
            printCount = int.Parse(Console.ReadLine());

            List<Decoding> decodings = GetAllDecodings(text);

            decodings.Sort((valueA, valueB) => valueA.difference.CompareTo(valueB.difference));

            if (printCount > decodings.Count) printCount = decodings.Count;

            for (int i = 0; i < printCount; i++)
            {

                Decoding decoding = decodings[i];

                Console.WriteLine($"Difference: {decoding.difference}");
                Console.WriteLine($"Decrypted by: {decoding.multiplier}x + {decoding.offset}");

                int inverseMultiplier = InverseMultiplier(decoding.multiplier);
                int decryptOffset = (inverseMultiplier * (-decoding.offset));
                decryptOffset = (decryptOffset + ((Math.Abs(decryptOffset) / validCharacters.Length) * validCharacters.Length) + validCharacters.Length) % validCharacters.Length;

                Console.WriteLine($"Encrypted by: {inverseMultiplier}x + {decryptOffset}");
                Console.WriteLine(decoding.text);
                Console.WriteLine();

            }

            Console.WriteLine("Program Finished");
            Console.ReadKey();

        }

        static string GetTextFromFile(string filename)
        {
            Debug.Assert(File.Exists(filename));
            return File.ReadAllText(filename);
        }

        static List<Decoding> GetAllDecodings(string text)
        {

            List<Decoding> decodingsList = new List<Decoding>();

            foreach (int multiplier in multiplierOptions)
            {

                for (int offset = 0; offset < 26; offset++)
                {

                    string decodeText = EncryptText(text,
                        multiplier,
                        offset);

                    double decodeDifference = FrequencyAnalysis.GetKeyFrequencyDifference(
                        FrequencyAnalysis.CharFrequencyToCharProportion(FrequencyAnalysis.GetTextCharFrequency(decodeText, validCharacters)),
                        EnglishLetterFrequency.GetLetterProportions()
                    );

                    decodingsList.Add(new Decoding(
                        decodeText,
                        multiplier,
                        offset,
                        decodeDifference
                    ));

                }

            }

            return decodingsList;

        }

        static string EncryptText(string text,
            int multiplier,
            int offset)
        {

            string output = "";

            foreach (char c in text)
            {
                output = output + EncryptChar(c, multiplier, offset);
            }

            return output;

        }

        static char EncryptChar(char c,
            int multiplier,
            int offset)
        {

            if (!validCharacters.Contains(c)) { return c; }

            int pos = Array.IndexOf(validCharacters, c);
            pos = (((multiplier * pos) + offset) + validCharacters.Length) % validCharacters.Length;
            return validCharacters[pos];

        }

        public static int InverseMultiplier(int origMultiplier)
        {

            int baseNumber = validCharacters.Length;

            for (int i = 0; i < baseNumber; i++)
            {

                if ((i * origMultiplier) % baseNumber == 1) return i;

            }

            return 0;

        }

        private struct Decoding
        {

            public string text;
            public int multiplier;
            public int offset;
            public double difference;

            public Decoding(string text, int multiplier, int offset, double difference)
            {
                this.text = text;
                this.multiplier = multiplier;
                this.offset = offset;
                this.difference = difference;
            }

        }

    }
}
