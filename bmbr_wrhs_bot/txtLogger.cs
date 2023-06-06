
namespace bmbr_wrhs_bot
{

    // текстовый логер для записи всех сообщений в файл в папке logs в корне программы

    public class txtLogger
    {
        // поля для хранения пути и строки даты, для добавления к имени файла

        private readonly string path;
        private string dateString;


        // конструктор по умолчанию

        public txtLogger() 
        {
            this.path = createDirectory();
        }

        // основной метод логирования сообщения в файл

        public async void log(string message)
        {
            this.dateString = '_' + DateTime.Now.Day.ToString() + '_' + DateTime.Now.Month.ToString() + '_' + DateTime.Now.Year.ToString();
            string fullPath = path + "\\log" + this.dateString + ".txt";
            using(StreamWriter writer = new(fullPath, true))
            {
                await writer.WriteLineAsync(DateTime.Now.TimeOfDay.ToString() + "   " + message);
            }
        }


        // проверка на сущестование директории logs, в противном случае ее создание
        private string createDirectory()
        {
            string subdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
            return subdir;
        }
    }
}