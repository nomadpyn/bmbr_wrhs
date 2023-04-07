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
    }
}
