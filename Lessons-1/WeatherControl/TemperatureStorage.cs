using WeatherControl.Model;

namespace WeatherControl;

public class TemperatureStorage
{
    private List<WeatherData> _storage;

    public TemperatureStorage()
    {
        _storage = new List<WeatherData>()
        {
            new WeatherData()
            {
                Date = new DateTime(2022, 01, 01, 12, 00, 00),
                Temperature = 0
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 02, 01, 12, 00, 00),
                Temperature = 2
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 03, 01, 12, 00, 00),
                Temperature = 5
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 04, 01, 12, 00, 00),
                Temperature = 8
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 05, 01, 12, 00, 00),
                Temperature = 15
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 06, 01, 12, 00, 00),
                Temperature = 22
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 07, 01, 12, 00, 00),
                Temperature = 25
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 08, 01, 12, 00, 00),
                Temperature = 30
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 09, 01, 12, 00, 00),
                Temperature = 25
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 10, 01, 12, 00, 00),
                Temperature = 10
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 11, 01, 12, 00, 00),
                Temperature = -5
            },
            new WeatherData()
            {
                Date = new DateTime(2022, 12, 01, 12, 00, 00),
                Temperature = -30
            },
        };
    }

    public void Add(WeatherData weatherData)
    {
        _storage.Add(weatherData);
    }

    public List<WeatherData> Get()
    {
        return _storage.OrderBy(x => x.Date).ToList();
    }

    public bool Update(WeatherData weatherData)
    {
        WeatherData findData = _storage.Find(x => x.Date == weatherData.Date);

        if(findData is null)
        {
            return false;
        }

        _storage.Remove(findData);
        findData.Temperature = weatherData.Temperature;
        _storage.Add(findData);

        return true;
    }

    public bool Delete(WeatherData weatherData)
    {

        if (weatherData is null)
        {
            return false;
        }

        _storage.Remove(weatherData);

        return true;
    }
}
