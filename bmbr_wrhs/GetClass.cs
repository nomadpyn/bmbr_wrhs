using Microsoft.EntityFrameworkCore;

namespace bmbr_wrhs
{
    // Статические класс, с помощью которого получаем данные из базы с помощью Entity Framework
    public static class GetClass
    {
        // Возвращает List всех деталей из базы
        public static List<AutoPart> getAllAutopartsFromDB()
        {
            AppContext db = new AppContext();
            var data = db.Autoparts
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .ToList();
            db.Dispose();
            return data;
        }

        // Возвращает List деталей, которые есть в наличии, т.е. их количество больше 0

        public static List<AutoPart> getPartsNotNull()
        {
            AppContext db = new AppContext();
            var data = db.Autoparts
                .Where(u => u.Count != 0)
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .ToList();
            db.Dispose();
            return data;
        }

        // Возвращает List всех автомобилей, которые есть в БД

        public static List<Car> getAllCars()
        {
            AppContext db = new AppContext();
            var data = db.Car.ToList();
            db.Dispose();
            return data;
        }

        // Возвращает List цветов ТС, по id машины из БД, которые к ней "привязаны"

        public static List<CarColor> getCarColor(int id)
        {
            AppContext db = new AppContext();
            var data = db.CarColor
                .Where(c => c.CarBelong.Id == id)
                .Include(c => c.Color)
                .ToList();
            db.Dispose();
            return data;
        }

        // Возвращает List деталей, по машине и цвету

        public static List<AutoPart> getAutoPartsbySearch(int carId, int ColorId)
        {
            AppContext db = new AppContext();
            var data = db.Autoparts
                .Where(c => c.Car.Id == carId)
                .Where(c => c.Color.Id == ColorId)
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .ToList();
            db.Dispose();
            return data;
        }

        // Уменьшает значение детали на одну (списание)
        public static void putAwayPart(int partId)
        {
            AppContext db = new();
            AutoPart ap = db.Autoparts
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .Where(c => c.Count >0)
                .FirstOrDefault(c => c.Id == partId);
            if (ap != null)
            {
                SoldPart sp = new SoldPart { Date = DateTime.Now.Date, Car = ap.Car, PartType = ap.PartType, Color = ap.Color };
                db.SoldParts.Add(sp);
                ap.Count--;
                db.SaveChanges();
            }
            db.Dispose();
        }

        // Возвращает List моделей по имени, если оно начинается также, как в name

        public static List<string> getModelsByName(string name)
        {
            AppContext db = new AppContext();
            var data = db.Car.
                ToList();
            var models = data
                .Where(c => c.CarName.StartsWith(name))
                .Select(c => c.CarName)
                .ToList();
            db.Dispose();
            if (data != null)
                return models;
            else
                return new List<string>();
        }

        // Возвращает List цветов ТС, по имени

        public static List<string> getCarColor(string name)
        {
            AppContext db = new AppContext();
            var data = db.CarColor
                .Where(c => c.CarBelong.CarName == name)
                .Include(c => c.Color)
                .Select(c => c.Color.ColorName)
                .ToList();
            db.Dispose();
            if(data!=null)
                return data;
            else
                return new List<string>();
        }

        // Возвращает List типов деталей

        public static List<string> getPartsTypes()
        {
            AppContext db = new AppContext();
            var data = db.PartType
                .Select(p => p.TypeName)
                .ToList();
            db.Dispose();
            if (data != null)
                return data;
            else
                return new List<string>();
        }

        // Возвращает деталь, по полному соответствию с part, name, color (для Telegram бота) 
        public static AutoPart getPartFromBot(string part, string name, string color)
        {
            AppContext db = new();
            AutoPart data = db.Autoparts
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .FirstOrDefault(n => n.Car.CarName == name && n.PartType.TypeName == part && n.Color.Color.ColorName == color);
            db.Dispose();            
                return data;
            
        }

        // Возвращает List строк, которые могут быть запрошены в боте и не считаться ошибкой (инициализация начальных данных при запуске)
        public static List<string> allDataInString()
        {
            AppContext db = new();
            var parts = db.PartType
                .Select(p => p.TypeName)
                .ToList();
            var cars = db.Car
                .Select(c => c.CarName)
                .ToList();
            var colors = db.Color
                .Select(c => c.ColorName)
                .ToList();
            db.Dispose();
            List<string> result = new List<string>();
            result.AddRange(parts);
            result.AddRange(cars);
            result.AddRange(colors);
        return result;
        } 

        public static List<SoldPart> getAllSoldParts()
        {
            AppContext db = new();
            var soldParts = db.SoldParts
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .ToList();
            db.Dispose();
            return soldParts;
        }

        public static List<SoldPart> getSoldPartsByDate(DateTime start, DateTime end)
        {
            AppContext db = new();
            var soldParts = db.SoldParts
                .Where(d => (d.Date >= start) && (d.Date <= end))
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .ToList();
            db.Dispose();
            return soldParts;
        }
    }
}