using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace bmbr_wrhs
{
    public static class CsvLoader
    {
        public static void getPartsFromCSV(string csvFileName)
        {
            string Fulltext;
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
                            saveToDB(rowValues);
                        }
                    }                   
                }
            }
        }
        private static void saveToDB(string[] data)
        {
            AppContext db = new();
            AutoPart AP = new();

            var datatype = db.PartType
                             .FirstOrDefault(p => p.TypeName == data[0]);
            AP.PartType = addPartType(datatype, data[0]);

            var cartype = db.Car
                            .FirstOrDefault(c => c.CarName == data[1]);
            AP.Car = addCar(cartype, data[1]);
            
            var colortype = db.CarColor
                              .Where(cl => cl.Color.ColorName == data[2])
                              .Include(cl => cl.Color)
                              .Include(cr => cr.CarBelong)
                              .Where(cr => cr.CarBelong.CarName == AP.Car.CarName)
                              .FirstOrDefault(cr => cr.CarBelong.CarName == data[1]);
            var colorDB = db.Color
                            .FirstOrDefault(cl => cl.ColorName == data[2]);
            AP.Color = addColor(colortype, data[2], colorDB, AP.Car);
            
            int count = Int32.Parse(data[3]);
            int selfPrice = Int32.Parse(data[4]);

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
            }
            else
            {
                ExistPart.Count += count;
                ExistPart.SelfPrice = selfPrice;
            }
            db.SaveChanges();
            db.Dispose();

        }
        private static PartType addPartType(PartType searchFromDB, string typeCVS)
        {
            PartType pt;
            if (searchFromDB == null)
            {
                pt = new();
                pt.TypeName = typeCVS;
            }
            else
                pt = searchFromDB;
            return pt;
        }
        private static Car addCar(Car searchFromDB, string carCVS)
        {
            Car car;
            if (searchFromDB == null)
            {
                car = new();
                car.CarName = carCVS;
            }
            else
                car = searchFromDB;
            return car;
        }
        private static CarColor addColor(CarColor searchFromDB,string colorCVS, Color searchColor, Car colorCar)
        {
            CarColor color;
            if (searchFromDB == null)
            {
                color = new();
               
                if (searchColor == null)
                {
                    color.Color = new();
                    color.Color.ColorName = colorCVS;
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
