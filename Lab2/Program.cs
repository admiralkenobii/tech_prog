using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Deal // сделка
{
    public int Sum { get; set; }
    public string Id { get; set; }
    public DateTime Date { get; set; }
}

public record SumByMonth(DateTime Month, int Sum); // сумма за каждый месяц

public class Program
{
    // названия месяцев на русском - для вывода
    static readonly string[] RussianMonths = new string[]
    {
        "Январь", "Февраль", "Март", "Апрель",
        "Май", "Июнь", "Июль", "Август",
        "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };
    // парсинг из json-файла
    public static IList<Deal> ParseDealsFromFile(string filePath)
    {
        // Чтение содержимого файла JSON
        var json = File.ReadAllText(filePath);
        // Десериализация JSON в список объектов сделок
        var deals = JsonSerializer.Deserialize<List<Deal>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return deals;
    }
    // возвращает соответствующие условиям номера сделок
    public static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
    {
        // Фильтрация сделок по сумме и дате, выбор 5 сделок с наибольшей суммой
        var filteredDeals = deals
            .Where(deal => deal.Sum >= 100)
            .OrderBy(deal => deal.Date)
            .Take(5)
            .OrderByDescending(deal => deal.Sum)
            .Select(deal => deal.Id)
            .ToList();

        return filteredDeals;
    }
    // сумма всех сделок за месяц
    public static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
    {
        // Группировка сделок по месяцу и вычисление суммы сделок в каждой группе
        var sumsByMonth = deals
            .GroupBy(deal => new DateTime(deal.Date.Year, deal.Date.Month, 1))
            .Select(group => new SumByMonth(group.Key, group.Sum(deal => deal.Sum)))
            .ToList();

        return sumsByMonth;
    }
    public static void Main(string[] args)
    {
        // Путь к файлу JSON
        string filePath = "json/JSON_sample_1.json";

        // Парсинг сделок из файла
        var deals = ParseDealsFromFile(filePath);

        // Получение номеров сделок
        var dealNumbers = GetNumbersOfDeals(deals);
        Console.WriteLine($"Количество найденных значений: {dealNumbers.Count}");
        Console.WriteLine($"Идентификаторы: {string.Join(", ", dealNumbers)}");

        // Получение сумм по месяцам
        var sumsByMonth = GetSumsByMonth(deals);
        sumsByMonth = sumsByMonth.OrderBy(sumByMonth => sumByMonth.Month).ToList();
        Console.WriteLine("Сумма сделок за каждый месяц:");
        foreach (var sumByMonth in sumsByMonth)
        {
            Console.WriteLine($"{RussianMonths[sumByMonth.Month.Month - 1]}: {sumByMonth.Sum}");
        }
    }
}
