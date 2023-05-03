using bmbr_wrhs;
using AppContext = bmbr_wrhs.AppContext;

using (AppContext db = new())
{
    var data = GetClass.getAllAutopartsFromDB();
    if(data != null)
    {
        Console.WriteLine("Подключение к БД успешно");
    }
}