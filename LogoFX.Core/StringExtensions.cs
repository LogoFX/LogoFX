using System.Collections.Generic;
using System.Text;

namespace LogoFX.Core
{
    /// <summary>
    /// Extension methods for <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Beautifies the string.
        /// </summary>
        /// <param name="originalName">original string.</param>
        /// <returns></returns>
        public static string Beautify(this IEnumerable<char> originalName)
        {
            StringBuilder word = new StringBuilder();
            List<string> words = new List<string>();
            foreach (char c in originalName)
            {
                // ignore punctuations and blanks
                if (char.IsLetterOrDigit(c))
                {
                    if (char.IsUpper(c))
                    {
                        if (word.Length > 0 && char.IsLower(word[word.Length - 1]))
                        {
                            words.Add(word.ToString());
                            word = new StringBuilder();
                        }
                    }
                    word.Append(c);
                }
            }
            if (word.Length > 0)
                words.Add(word.ToString());
            string beautifiedName = string.Join(" ", words.ToArray());
            return beautifiedName;
        }        
    }
}
