using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DyyshWebApplication
{
    public class UrlConvert
    {
        private const int MAX_DIGITS = 32;
        private const double FOLDER_SIZE = 24;

        /// <summary>
        /// Converts alphanumeric string (of Base 62) to decimal long integer.
        /// </summary>
        /// <param name="text" >String which contains only latin letters and arabic digits</param>
        /// <returns>Int64 value</returns>
        /// <exception cref="ArgumentException">String contains non-valid symbols</exception>
        public static long ToInt64(string text)
        {
            long result = 0;

            for (int index = 0, power = text.Count() - 1;
                index < text.Count();
                index++, power--)
            {
                //find corresponding symbol in alphabet
                //base 62 index = string
                int base62symbol = Array.IndexOf(m_alphabet, text[index]);

                if (base62symbol < 0) { throw new ArgumentException("Only latin letters and arabic digits are allowed", "text"); }

                result += base62symbol * (long)Math.Pow(m_alphabet.Length, power);
            }

            return result;
        }


        /// <summary>
        /// Converts decimal long integer to alphanumeric string (of Base 62)
        /// </summary>
        /// <param name="number">Int64 value</param>
        /// <returns>String which contains only latin letters and arabic digits</returns>
        /// <exception cref="ArgumentOutOfRangeException">Negative and null numbers are not allowed</exception>
        public static string ToString(long number)
        {
            if (number <= 0) { throw new ArgumentOutOfRangeException("number", "Negative and null numbers are not allowed"); }

            int[] base10 = new int[MAX_DIGITS];		// [1..9]
            char[] base62 = new char[MAX_DIGITS];	// [1..z]

            int index = 0;
            while (number != 0)
            {
                long remainder = number % m_alphabet.Length;
                base10[index] = (int)remainder;

                //converts decimal number to base 62 number
                //base 10 number = base 62 index in alphabet
                base62[index] = m_alphabet[base10[index]];

                number /= m_alphabet.Length;
                index++;
            }

            //reverses resulting array for correctness
            Array.Reverse(base62);
            return new string(base62).TrimStart('\0');
        }

        /// <summary>
        /// Converts decimal long integer to alphanumeric string (of Base 62)
        /// Usable by naming folders
        /// </summary>
        /// <param name="number">Int64 value</param>
        /// <returns>String which contains only small latin letters and arabic digits</returns>
        /// <exception cref="ArgumentOutOfRangeException">Negative and null numbers are not allowed</exception>
        public static string ToStringCatalog(long initialNumber)
        {
            double d_catalogNumber = (double)initialNumber / FOLDER_SIZE;
            var number = (long)Math.Ceiling(d_catalogNumber);

            if (number <= 0) { throw new ArgumentOutOfRangeException("number", "Negative and null numbers are not allowed"); }

            int[] base10 = new int[MAX_DIGITS];		// [1..9]
            char[] base62 = new char[MAX_DIGITS];	// [1..z]

            int index = 0;
            while (number != 0)
            {
                long remainder = number % m_alphabet_small.Length;
                base10[index] = (int)remainder;

                //converts decimal number to base 62 number
                //base 10 number = base 62 index in alphabet
                base62[index] = m_alphabet[base10[index]];

                number /= m_alphabet_small.Length;
                index++;
            }

            //reverses resulting array for correctness
            Array.Reverse(base62);
            return new string(base62).TrimStart('\0');
        }

        #region Alphabet of 62 symbols

        private static readonly char[] m_alphabet = { 
			
			//numbers (0..9)
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			
			//uppercase (10..35)
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
			'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',

			//lowercase (36..62)
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
			};

        #endregion

        #region Alphabet of 36 symbols

        private static readonly char[] m_alphabet_small = { 
			
			//numbers (0..9)
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			
			//uppercase (10..35)
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
			'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',

			};

        #endregion

    }
    
}