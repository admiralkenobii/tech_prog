using System;
// класс установки
public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryId { get; set; }
}
// класс завода
public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
// класс резервуара
public class Tank
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }
}

class Program
{
    // получение резервуаров
    public static Tank[] GetTanks()
    {
        return new Tank[]
        {
            new Tank { Id = 1, Name = "Резервуар 1", Description = "Надземный - вертикальный", Volume = 1500, MaxVolume = 2000, UnitId = 1 },
            new Tank { Id = 2, Name = "Резервуар 2", Description = "Надземный - горизонтальный", Volume = 2500, MaxVolume = 3000, UnitId = 1 },
            new Tank { Id = 3, Name = "Дополнительный резервуар 24", Description = "Надземный - горизонтальный", Volume = 3000, MaxVolume = 3000, UnitId = 2 },
            new Tank { Id = 4, Name = "Резервуар 35", Description = "Надземный - вертикальный", Volume = 3000, MaxVolume = 3000, UnitId = 2 },
            new Tank { Id = 5, Name = "Резервуар 47", Description = "Подземный - двустенный", Volume = 4000, MaxVolume = 5000, UnitId = 2 },
            new Tank { Id = 6, Name = "Резервуар 256", Description = "Подводный", Volume = 500, MaxVolume = 500, UnitId = 3 }
        };
    }
    // получение установок
    public static Unit[] GetUnits()
    {
        return new Unit[]
        {
            new Unit { Id = 1, Name = "ГФУ-2", Description = "Газофракционирующая установка", FactoryId = 1 },
            new Unit { Id = 2, Name = "АВТ-6", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 1 },
            new Unit { Id = 3, Name = "АВТ-10", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 2 }
        };
    }
    // получение заводов
    public static Factory[] GetFactories()
    {
        return new Factory[]
        {
            new Factory { Id = 1, Name = "НПЗ№1", Description = "Первый нефтеперерабатывающий завод" },
            new Factory { Id = 2, Name = "НПЗ№2", Description = "Второй нефтеперерабатывающий завод" }
        };
    }
    // поиск установки по имени резервуара
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        Tank foundTank = null; // резервуар

        // Найдем резервуар по имени
        foreach (var tank in tanks)
            if (tank.Name == tankName) {
                foundTank = tank;
                break;
            }

        // Если резервуар не найден
        if (foundTank == null)
            return null;

        // Найдем установку по Id резервуара
        foreach (var unit in units)
            if (unit.Id == foundTank.UnitId)
                return unit;

        return null; // если установка не найдена
    }
    // поиск завода по установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null; // если установка не найдена

        foreach (var factory in factories)
            if (factory.Id == unit.FactoryId)
                return factory;
        return null; // если не найден
    }
    // подсчёт числа резервуаров
    public static int GetTotalVolume(Tank[] tanks)
    {
        int totalVolume = 0;
        foreach (var tank in tanks)
            totalVolume += tank.Volume;
        return totalVolume;
    }

    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();

        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
    }
}