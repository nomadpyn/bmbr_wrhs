
namespace bmbr_wrhs_bot
{
    public class txtLogger
    {
        private readonly string path;
        private readonly string dateString;

        public txtLogger() 
        {
            this.path = createDirectory();
            this.dateString = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();            
        }

        public async void log(string message)
        {
            string fullPath = path + "\\log" + this.dateString + ".txt";
            using(StreamWriter writer = new(fullPath, true))
            {
                await writer.WriteLineAsync(DateTime.Now.TimeOfDay.ToString() + "   " + message);
            }
        }

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
