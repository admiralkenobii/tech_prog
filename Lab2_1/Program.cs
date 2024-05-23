using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

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

// Класс для загрузки и сохранения данных
public static class DataLoader
{
    // загрузчик из json-файла
    public static T[] LoadFromJson<T>(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T[]>(jsonString);
    }
    // сохранение в json-файл
    public static void SaveToJson<T>(string filePath, T[] data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, jsonString);
    }
}

class Program
{
    // Получение резервуаров из JSON файла
    public static Tank[] GetTanks() => DataLoader.LoadFromJson<Tank>("json/tanks.json");

    // Получение установок из JSON файла
    public static Unit[] GetUnits() => DataLoader.LoadFromJson<Unit>("json/units.json");

    // Получение заводов из JSON файла
    public static Factory[] GetFactories() => DataLoader.LoadFromJson<Factory>("json/factories.json");

    // Поиск установки по имени резервуара
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        /* 
        // Синтаксис методов
        Tank foundTank = null; // резервуар

        // Найдем резервуар по имени
        foreach (var tank in tanks)
            if (tank.Name == tankName)
            {
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
        */
        // Синтаксис запросов
        var foundTank = (from t in tanks
                         where t.Name == tankName
                         select t).FirstOrDefault();

        if (foundTank == null)
            return null;

        return (from u in units
                where u.Id == foundTank.UnitId
                select u).FirstOrDefault();
    }

    // Поиск завода по установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null; // если установка не найдена
        /*
        // Синтаксис методов
        foreach (var factory in factories)
            if (factory.Id == unit.FactoryId)
                return factory;
        return null; // если не найден
        */
        // Синтаксис запросов
        return (from f in factories
                where f.Id == unit.FactoryId
                select f).FirstOrDefault();
    }

    // Подсчёт общего объёма резервуаров
    public static int GetTotalVolume(Tank[] tanks)
    {
        /*
        // Синтаксис методов
        int totalVolume = 0;
        foreach (var tank in tanks)
            totalVolume += tank.Volume;
        return totalVolume;
        */
        // Синтаксис запросов
        return (from t in tanks
                select t.Volume).Sum();
    }

    static void Main(string[] args)
    {
        // Чтение данных из JSON файлов
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();
        // Отображение количества объектов
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");
        // Поиск установки по имени резервуара
        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);
        // резервуар 2 - поиск
        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");
        // Подсчёт общего объёма резервуаров
        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");

        // Выгрузка данных в JSON файлы
        DataLoader.SaveToJson("json/tanks.json", tanks);
        DataLoader.SaveToJson("json/units.json", units);
        DataLoader.SaveToJson("json/factories.json", factories);
    }
}
