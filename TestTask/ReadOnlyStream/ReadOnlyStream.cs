using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private Stream _localStream;
        
        private StreamReader _streamReader;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            _localStream = File.OpenRead(fileFullPath);

            _streamReader = new StreamReader(_localStream);
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof => _streamReader.EndOfStream;

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            return (char)_streamReader.Read();
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                return;
            }

            _localStream.Position = 0;
            _streamReader.DiscardBufferedData();
        }

        /// <summary>
        /// Закрытие потока
        /// </summary>
        public void Dispose()
        {
            _streamReader.Close();
            _streamReader.Dispose();
            _localStream.Close();
            _localStream.Dispose();
        }
    }
}
