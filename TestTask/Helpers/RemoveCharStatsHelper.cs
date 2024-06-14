using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask.Helpers
{
    public class RemoveCharStatsHelper
    {
        /// <summary>
        /// Гласные
        /// </summary>
        private static string vowels =
            "ауоыиэяюёе" + // рус
            "aeiouy" // англ
        ;

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        public static IList<LetterStats> RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    return letters.Where(stat => stat.Letter.All(c => vowels.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

                case CharType.Vowel:
                    return letters.Where(stat => stat.Letter.All(c => vowels.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase) < 0)).ToList();

                default:
                    throw new ArgumentException("Не обработан тип символа");
            }
        }
    }
}
