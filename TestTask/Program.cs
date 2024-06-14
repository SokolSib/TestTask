using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestTask.Helpers;

namespace TestTask
{
    public class Program
    {
        /// <summary>
        /// Директория с примерами файлов
        /// </summary>
        private const string ExampleDirName = "ExampleFiles";

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.
        /// Если параметры не заданы читает файлы примеров</param>
        static void Main(string[] args)
        {
            var curDir = Directory.GetCurrentDirectory();
            var fileCaseSensitive = args.Length > 0 ? args[0] : Path.Combine(curDir, ExampleDirName, "ExampleCaseSensitive.txt");
            var fileCaseInsensitive = args.Length > 1 ? args[1] : Path.Combine(curDir, ExampleDirName, "ExampleCaseInsensitive.txt");

            using (var streamCaseSensitive = GetInputStream(fileCaseSensitive))
            {
                var singleLetterStats = LetterStatsHelper.FillSingleLetterStats(streamCaseSensitive);                
                var clearedSingleLetterStats = RemoveCharStatsHelper.RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
                
                Console.WriteLine("Кол-во вхождений каждой буквы (регистрозависимо) в очищенной статистике без гласных:");
                PrintStatistic(clearedSingleLetterStats);
            }

            using (var streamCaseInsensitive = GetInputStream(fileCaseInsensitive))
            {
                var doubleLetterStats = LetterStatsHelper.FillDoubleLetterStats(streamCaseInsensitive);
                var clearedDoubleLetterStats = RemoveCharStatsHelper.RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);
                
                Console.WriteLine("Кол-во вхождений парных букв (не регистрозависимо) в очищенной статистике без согласных:");
                PrintStatistic(clearedDoubleLetterStats);
            }

            Console.WriteLine("Для завершения работы нажмите любую клавишу");
            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }       

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            foreach (var stat in letters.OrderBy(l => l.Letter))
            {
                Console.WriteLine($"{stat.Letter} : {stat.Count}");
            }

            Console.WriteLine($"ИТОГО: {letters.Count()}");
        }
    }
}
