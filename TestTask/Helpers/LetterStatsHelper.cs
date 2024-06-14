using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTask.Helpers
{
    public static class LetterStatsHelper
    {
        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Буквой считается любой символ кроме разделителей
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        public static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            var statsDic = new Dictionary<string, int>();

            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();

                if (!char.IsLetter(c))
                {
                    continue;
                }

                IncStatistic(statsDic, c.ToString());
            }

            return statsDic.Select(kvp => new LetterStats
            {
                Letter = kvp.Key,
                Count = kvp.Value
            }).ToList();
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Буквой считается любой символ кроме разделителей
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        public static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            var statsDic = new Dictionary<string, int>();

            // Накапливает пару букв
            var doubleLetter = new StringBuilder();

            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();

                if (!char.IsLetter(c))
                {
                    doubleLetter.Clear();
                    continue;
                }

                doubleLetter.Append(c.ToString().ToUpper());

                if (doubleLetter.Length == 2)
                {
                    if (doubleLetter[0] == doubleLetter[1])
                    {
                        IncStatistic(statsDic, doubleLetter.ToString());
                        doubleLetter.Clear();
                    }
                    else
                    {
                        doubleLetter.Remove(0, 1);
                    }
                }
            }

            return statsDic.Select(kvp => new LetterStats
            {
                Letter = kvp.Key,
                Count = kvp.Value
            }).ToList();
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(Dictionary<string, int> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                dic[key]++;
            }
            else
            {
                dic.Add(key, 1);
            }
        }
    }
}
