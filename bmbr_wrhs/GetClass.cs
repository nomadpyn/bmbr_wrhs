using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace bmbr_wrhs
{
    public static class GetClass
    {
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
        public static List<Car> getAllCars()
        {
            AppContext db = new AppContext();
            var data = db.Car.ToList();
            db.Dispose();
            return data;
        }
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
        public static void putAwayPart(int partId)
        {
            AppContext db = new();
            AutoPart ap = db.Autoparts
                .Include(u => u.PartType)
                .Include(u => u.Car)
                .Include(u => u.Color)
                .ThenInclude(c => c!.Color)
                .FirstOrDefault(c => c.Id == partId);
            if (ap != null)
            {
                SoldPart sp = new SoldPart { Date = DateTime.Now, Car = ap.Car, PartType = ap.PartType, Color = ap.Color };
                db.SoldParts.Add(sp);
                ap.Count--;
                db.SaveChanges();
            }
            db.Dispose();
        }
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
    }
}
