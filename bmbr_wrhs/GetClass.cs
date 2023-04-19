using Microsoft.EntityFrameworkCore;

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
                .Where(u => u.Count !=0)
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
                .Include(c =>c.Color)
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
            AutoPart ap = db.Autoparts.FirstOrDefault(c => c.Id == partId);
            if(ap != null)
            {
                ap.Count--;
                db.SaveChanges();
            }
            db.Dispose();
        }
    }
}
