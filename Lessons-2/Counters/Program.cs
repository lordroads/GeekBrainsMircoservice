using System;
using System.Diagnostics;
using System.Linq;


string category = "Сетевой интерфейс";
string counterSearch = "Intel[R] 82579V Gigabit Network Connection";
string counterName = "Всего байт/с";

var categories = PerformanceCounterCategory.GetCategories().FirstOrDefault(cat => cat.CategoryName == category);
if (categories == null)
{
    Console.WriteLine($"Not category is '{category}'");
    return;
}

var countersInCategory = categories.GetCounters(counterSearch);

DisplayCounter(countersInCategory.First(cnt => cnt.CounterName == counterName));






Console.WriteLine($"Done!");


//var processorCategories = PerformanceCounterCategory.GetCategories().FirstOrDefault(cat => cat.CategoryName == "Processor");
//var countersInCategory = processorCategories.GetCounters("_Total");
//Console.WriteLine(processorCategories.MachineName);

////var countersInCategory = processorCategory.GetCounters("_Total");

//DisplayCounter(countersInCategory.First(cnt => cnt.CounterName == "% загруженности процессора"));


static void DisplayCounter(PerformanceCounter performanceCounter)
{
    while (!Console.KeyAvailable)
    {
        Console.WriteLine("{0}\t{1} = {2}",
            performanceCounter.CategoryName, performanceCounter.CounterName, performanceCounter.NextValue());
        System.Threading.Thread.Sleep(1000);
    }
}