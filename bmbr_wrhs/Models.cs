
namespace bmbr_wrhs
{

    // Описание классов - моделей, с которыми работает Entity Framework

    // Основной класс Автозапчасть (AutPart), который содержит всю информацию о детали (тип, цвет, количество, себестоимость)
    public class AutoPart
    {
        public int Id { get; set; }
        public PartType? PartType { get; set; }
        public Car? Car { get; set; }
        public CarColor? Color { get; set; }
        public int Count { get; set; }
        public int SelfPrice { get; set; }
        public override string ToString() => $"{this.PartType} {this.Car} {this.Color}. Количество {this.Count}, цена {this.SelfPrice}";
    }

    // Класс Тип Запчасти (PartType), содержит имя типа детали

    public class PartType
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public override string ToString() => this.TypeName;
    }

    // Класс Машина (Car), содержит имя автомобиля

    public class Car
    {
        public int Id { get; set; }
        public string? CarName { get; set; }
        public override string ToString() => this.CarName;
    }

    // Класс Машина-Цвет (CarColor), содержит в себе два класса, для соотношения автомобиля, и какого цвета она может быть

    public class CarColor
    {
        public int Id { get; set;}
        public Color? Color { get; set; }
        public Car ? CarBelong { get; set; }
        public override string ToString() => this.Color.ColorName;

    }

    // Класс Цвет (Color), содержит в себе названия цвета 

    public class Color
    {
        public int Id { get; set; }
        public string? ColorName { get; set; }
        public override string ToString() => this.ColorName;
    }

    // Класс Проданная Запчасть (SoldPart), отдельный класс, экземпляр которого записывается в отдельную таблицу при списании детали

    public class SoldPart
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public PartType? PartType { get; set; }
        public Car? Car { get; set; }
        public CarColor? Color { get; set; }
        public override string ToString() => $"Продажа {this.Date.ToString()} {this.PartType} {this.Car} {this.Color}";
    }
}