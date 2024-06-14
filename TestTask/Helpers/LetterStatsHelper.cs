﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTask.Helpers
{
    public static class LetterStatsHelper
    {
        /// <summary>
        /// Разделители букв (игнорируются в статистике)
        /// первые что пришли в голову (не стал заморачиваться про все возможные варианты, т.к. тестовое задание и на логику сильно не влияет)
        /// </summary>
        private static List<char> separators = new List<char> { ' ', ',', '.', ';', '\n', '\r', '\t' };

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

                if (separators.Contains(c))
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

                if (separators.Contains(c))
                {
                    continue;
                }

                doubleLetter.Append(c.ToString().ToUpper());

                if (doubleLetter.Length == 2)
                {
                    IncStatistic(statsDic, doubleLetter.ToString());
                    doubleLetter.Clear();
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
