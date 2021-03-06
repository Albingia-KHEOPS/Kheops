using System;
using System.Linq;


namespace ALBINGIA.Framework.Common.Extensions
{


    /// <summary>
    /// Allow the up to the first eight elements of an array to take part in C# 7's destructuring syntax.
    /// </summary>
    /// <example>
    /// (int first, _, int middle, _, int[] rest) = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    /// var (first, second, rest) = new[] { 1, 2, 3, 4 };
    /// </example>
    public static class ArrayDeconstructionExtensions
    {
        private static T GetOrDefault<T>(this T[] array, int index) {
            return index < array.Length ? array[index] : default(T);
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T[] rest)
        {
            first = array.GetOrDefault(0);
            rest = GetRestOfArray(array, 1);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            rest = GetRestOfArray(array, 2);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            rest = GetRestOfArray(array, 3);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            fourth = array.GetOrDefault(3);
            rest = GetRestOfArray(array, 4);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            fourth = array.GetOrDefault(3);
            fifth = array.GetOrDefault(4);
            rest = GetRestOfArray(array, 5);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            fourth = array.GetOrDefault(3);
            fifth = array.GetOrDefault(4);
            sixth = array.GetOrDefault(5);
            rest = GetRestOfArray(array, 6);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T seventh, out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            fourth = array.GetOrDefault(3);
            fifth = array.GetOrDefault(4);
            sixth = array.GetOrDefault(5);
            seventh = array.GetOrDefault(6);
            rest = GetRestOfArray(array, 7);
        }
        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T seventh, out T eighth,out T[] rest)
        {
            first = array.GetOrDefault(0);
            second = array.GetOrDefault(1);
            third = array.GetOrDefault(2);
            fourth = array.GetOrDefault(3);
            fifth = array.GetOrDefault(4);
            sixth = array.GetOrDefault(5);
            seventh = array.GetOrDefault(6);
            eighth = array.GetOrDefault(7);
            rest = GetRestOfArray(array, 8);
        }
        private static T[] GetRestOfArray<T>(T[] array, int skip)
        {
            return array.Skip(array.Length>skip? skip :array.Length ).ToArray();
        }
    }
}
