using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmbr_wrhs
{
    public class AutoPart
    {
        public int Id { get; set; }
        public PartType? PartType { get; set; }
        public Car? Car { get; set; }
        public CarColor? Color { get; set; }
        public int Count { get; set; }
        public int SelfPrice { get; set; }
        public override string ToString()
        {
            return $"{this.PartType} {this.Car} {this.Color}. Количество {this.Count}, цена {this.SelfPrice}";
        }
    }
    public class PartType
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }

        public override string ToString()
        {
            return this.TypeName;
        }
    }
    public class Car
    {
        public int Id { get; set; }
        public string? CarName { get; set; }
        public override string ToString()
        {
            return this.CarName;
        }
    }    
    public class CarColor
    {
        public int Id { get; set;}
        public Color? Color { get; set; }
        public Car ? CarBelong { get; set; }
        public override string ToString()
        {
            return this.Color.ColorName;
        }

    }
    public class Color
    {
        public int Id { get; set; }
        public string? ColorName { get; set; }
        public override string ToString()
        {
            return this.ColorName;
        }
    }
    public class SoldPart
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public PartType? PartType { get; set; }
        public Car? Car { get; set; }
        public CarColor? Color { get; set; }

        public override string ToString()
        {
            return $"Продажа {this.Date.ToString()} {this.PartType} {this.Car} {this.Color}";
        }
    }

}
