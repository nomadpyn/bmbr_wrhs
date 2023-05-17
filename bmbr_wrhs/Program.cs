using bmbr_wrhs;
using AppContext = bmbr_wrhs.AppContext;

// Загрузка всех данных из базы по методу для заглушки main

using (AppContext db = new())
{
    var data = GetClass.getAllAutopartsFromDB();
    if(data != null)
    {
        Console.WriteLine("Подключение к БД успешно");
    }
}