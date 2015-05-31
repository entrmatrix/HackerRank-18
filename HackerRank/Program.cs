using System;
using System.Linq;

namespace PrettySimpleTestTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var klm = Console.ReadLine().Split(' ');
            var k = int.Parse(klm[0]);
            var l = int.Parse(klm[1]);
            var m = int.Parse(klm[2]);
            var text = Console.ReadLine();
            if (text.Length > n) text = text.Substring(0, n);

            var suffixes = BuildSuffixArray(text);
            var max = 0;
            for (var size = k; size < l; size++)
            {
                string substr;
                var maxOccurrence = GetSubstringOfSizeOccurrence(suffixes, size, out substr);
                if (maxOccurrence > max && substr.Length <= m) max = maxOccurrence;
            }
            Console.WriteLine(max);
        }

        private static int GetSubstringOfSizeOccurrence(Tuple<int[], string[]> suffixes, int size, out string substr)
        {
            var suffWithSize = suffixes.Item2.Where(s => s.Length >= size);
            var occurrences = suffWithSize.Select(s => s.Substring(0, 2)).ToArray();

            substr = string.Empty;
            var max = 0;
            for (var i = 0; i < occurrences.Length; i++)
            {
                var count = 1;
                for (int j = 0; j < occurrences.Length; j++)
                    if (i != j && occurrences[i] == occurrences[j]) count++;
                
                if (max < count)
                {
                    substr = occurrences[i];
                    max = count;
                }
            }
            return max;
        }
        
        public static Tuple<int[], string[]> BuildSuffixArray(string text)
        {
            var size = text.Length;
            var indexes = Enumerable.Range(0, size).ToArray();
            var array = new string[size];
            for (var i = 0; i < size; i++)
                array[i] = text.Substring(i); // <<<<< MemoryOverflow with very long string
            Array.Sort(array, indexes);
            return new Tuple<int[], string[]>(indexes, array);
        }
    }
}