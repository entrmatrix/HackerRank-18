using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PrettySimpleTestTool
{
    internal class Program1
    {
        private static bool errors;

        private static void RunCaseTest(Func<bool> testFunc, string message)
        {
            if (testFunc()) return;

            errors = true;
            Console.WriteLine(message);
        }

        private static void RunPerfTest(Action testFunc, double msLimit, string message)
        {
            var sw = new Stopwatch();
            sw.Start();
            testFunc();
            sw.Stop();

            if (msLimit >= sw.ElapsedMilliseconds) return; // it's ok

            errors = true;
            Console.WriteLine(message);
        }

        public static bool Test()
        {
            // example: RunCaseTest(function, "function is failed");
            // example: RunPerfTest(function, 500, "function is failed");

            RunCaseTest(SuffixTreeTest1, "SuffixTreeTest1 is failed");
            RunCaseTest(SuffixTreeTest2, "SuffixTreeTest2 is failed");
            //RunCaseTest(FindAllSubstringsTest, "FindAllSubstringsTest is failed");

            return !errors;
        }

        private static bool SuffixTreeTest1()
        {
            var actual = BuildSuffixArray("ababab");
            string substr;
            var d = GetSubstringOfSizeOccurrence(actual, 2, out substr);


            var expected = new Tuple<int[], string[]>(
                new[] { 9, 4, 0, 5, 7, 1, 8, 3, 6, 2 },
                new[]
                {
                    "a",
                    "aacbca",
                    "abccaacbca",
                    "acbca",
                    "bca",
                    "bccaacbca",
                    "ca",
                    "caacbca",
                    "cbca",
                    "ccaacbca"
                }
                );

            errors = true;
            if (expected.Item1.Length == actual.Item1.Length
                && expected.Item2.Length == actual.Item2.Length)
            {
                for (var i = 0; i < expected.Item1.Length; i++)
                    if (expected.Item1[i] != actual.Item1[i]
                        || expected.Item2[i] != actual.Item2[i])
                        return false;
            }
            errors = false;
            return true;
        }

        private static bool SuffixTreeTest2()
        {
            var actual = BuildSuffixArray("aaaaa");
            var expected = new Tuple<int[], string[]>(
                new[] { 4, 3, 2, 1, 0 },
                new[]
                {
                    "a",
                    "aa",
                    "aaa",
                    "aaaa",
                    "aaaaa"
                }
                );

            errors = true;
            if (expected.Item1.Length == actual.Item1.Length
                && expected.Item2.Length == actual.Item2.Length)
            {
                for (var i = 0; i < expected.Item1.Length; i++)
                    if (expected.Item1[i] != actual.Item1[i]
                        || expected.Item2[i] != actual.Item2[i])
                        return false;
            }
            errors = false;
            return true;
        }

        private static bool FindAllSubstringsTest()
        {
            return false;
        }

        private static void Main1(string[] args)
        {
            //if (Test())
            //{
            //    // code is executed when all is OK
            //    Console.WriteLine("OK");
            //}
            //Console.ReadKey();

            var n = int.Parse(Console.ReadLine());
            var klm = Console.ReadLine().Split(' ');
            var k = int.Parse(klm[0]);
            var l = int.Parse(klm[1]);
            var m = int.Parse(klm[2]);
            var text = Console.ReadLine().Substring(0, n);

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

        /// <summary>
        /// Returns a pair of index array and corresponding sufixes.
        /// </summary>
        /// <remarks>
        /// The index array is buildt from sufix array sorted acsending. Example, {3,2,1} : {"abc","ab","a"}
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Tuple<int[], string[]> BuildSuffixArray(string text)
        {
            var size = text.Length;
            var indexes = Enumerable.Range(0, size).ToArray();
            var array = new string[size];
            for (var i = 0; i < size; i++)
                array[i] = text.Substring(i);
            Array.Sort(array, indexes);
            return new Tuple<int[], string[]>(indexes, array);
        }

        public static Tuple<int[], string[]> BuildSuffixArray(string text1, string text2)
        {
            return null;
        }
    }
}