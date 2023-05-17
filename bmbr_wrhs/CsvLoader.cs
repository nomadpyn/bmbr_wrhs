using Microsoft.EntityFrameworkCore;

namespace bmbr_wrhs
{
    // Класс для загрузки поступлений деталей в БД 
    public static class CsvLoader
    {
        // Загружает данные при чтении из csv файла и возвращает массив строк с результатами работы

        public static string [] getPartsFromCSV(string csvFileName)
        {
            try
            {
                string Fulltext;
                int[] result = new int[5];
                using (StreamReader sr = new StreamReader(csvFileName))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString();
                        string[] rows = Fulltext.Split('\n');
                        
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(';');
                            {
                                saveToDB(rowValues, ref result);
                            }
                        }
                    }
                }
                return new[]
                    {
                    "ok",
                    "Данные успешно добавлены в БД",
                    $"Существующих записей обновлено: {result[0]}",
                    $"Новых записей добавлено: {result[1]}",
                    $"Новых типов деталей добавлено: {result[2]}",
                    $"Новых автомобилей: {result[3]}",
                    $"Новых цветов: {result[4]}"
                    };
            }
            catch(Exception ex)
            {
                return new[]
                    {
                    "error",
                    ex.Message
                    };
             
            }
        }

        // Сохраняет данные из csv в БД с помощью Entity Framework

        private static void saveToDB(string[] data, ref int[] result)
        {
            AppContext db = new();
            AutoPart AP = new();

            // проверяем на наличие совпадение по имени с уже существующими данными в БД, и заполняем поля AP, методами в зависимости от результат

            var datatype = db.PartType.FirstOrDefault(p => p.TypeName == data[0]);

            AP.PartType = addPartType(datatype, data[0], ref result[2]);

            var cartype = db.Car.FirstOrDefault(c => c.CarName == data[1]);

            AP.Car = addCar(cartype, data[1], ref result[3]);
            
            var colortype = db.CarColor
                              .Where(cl => cl.Color.ColorName == data[2])
                              .Include(cl => cl.Color)
                              .Include(cr => cr.CarBelong)
                              .Where(cr => cr.CarBelong.CarName == AP.Car.CarName)
                              .FirstOrDefault(cr => cr.CarBelong.CarName == data[1]);

            var colorDB = db.Color.FirstOrDefault(cl => cl.ColorName == data[2]);
            AP.Color = addColor(colortype, data[2], colorDB, AP.Car, ref result[4]);
            
            // проставляем количество и себестоимость

            int count = Int32.Parse(data[3]);
            int selfPrice = Int32.Parse(data[4]);

            // проверяем, существует ли у нас уже в базе такая деталь, если да, то меняем у нее количество и цену, если нет, создаем новую (избегаем повторений)

            var ExistPart = db.Autoparts
                .Where(cr => cr.Car == AP.Car)
                .Where(cl => cl.Color == AP.Color)
                .Where(p => p.PartType == AP.PartType)
                .FirstOrDefault();
            if (ExistPart == null)
            {
                AP.Count = count;
                AP.SelfPrice = selfPrice;
                db.Autoparts.Add(AP);
                result[1]++;
            }
            else
            {
                ExistPart.Count += count;
                ExistPart.SelfPrice = selfPrice;
                result[0]++;
            }
            db.SaveChanges();
            db.Dispose();
        }

        // заполняем поле Тип Детали, если уже есть такое в БД, то берем оттуда, если нет, то создаем новое

        private static PartType addPartType(PartType searchFromDB, string typeCVS, ref int newPart)
        {
            PartType pt;
            if (searchFromDB == null)
            {
                pt = new();
                pt.TypeName = typeCVS;
                newPart++;
            }
            else
                pt = searchFromDB;
            return pt;
        }

        // заполняем поле ТС, если такое существует в БД, то берем оттуда, если нет, то создаем новое

        private static Car addCar(Car searchFromDB, string carCVS, ref int newCar)
        {
            Car car;
            if (searchFromDB == null)
            {
                car = new();
                car.CarName = carCVS;
                newCar++;
            }
            else
                car = searchFromDB;
            return car;
        }

        // заполняем поле Машина-Цвет, если такое уже существует в БД, то берем оттуда, если нет, то создаем новое

        private static CarColor addColor(CarColor searchFromDB,string colorCVS, Color searchColor, Car colorCar, ref int newColor)
        {
            CarColor color;
            if (searchFromDB == null)
            {
                color = new();
               
                if (searchColor == null)
                {
                    color.Color = new();
                    color.Color.ColorName = colorCVS;
                    newColor++;
                }
                else
                    color.Color = searchColor;
                color.CarBelong = colorCar;
            }
            else
            {
                color = searchFromDB;
                color.Color = searchFromDB.Color;
                color.CarBelong = color.CarBelong;
            }
            return color;
        }
    }
}