using Microsoft.AspNetCore.Mvc;
using WeatherControl.Model;

namespace WeatherControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly TemperatureStorage _tempetatureStorage;

        public WeatherController(TemperatureStorage tempetatureStorage)
        {
            _tempetatureStorage = tempetatureStorage;
        }

        [HttpPost(Name = "set")]
        public IActionResult SetTemperatureInDateTime([FromQuery] WeatherData weatherData)
        {
            _tempetatureStorage.Add(weatherData);

            return Ok();
        }

        [HttpPut(Name = "update")]
        public ActionResult<string> UpdateTemperatureInDateTime([FromQuery] WeatherData weatherData)
        {
            return _tempetatureStorage.Update(weatherData) ? Ok("Данные обновлены") : BadRequest("Данные не обновлены");
        }

        [HttpDelete(Name = "delete")]
        public ActionResult<bool> DeleteByDateToDate([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            bool result = false;

            var list_date = _tempetatureStorage.Get().Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToList();

            foreach (var item in list_date)
            {
                result = _tempetatureStorage.Delete(item);
            }

            return result ? Ok("Данные удалены"): BadRequest("Ошибка в удалении данных.");
        }

        [HttpGet(Name = "get")]
        public ActionResult<List<WeatherData>> GetTemperatureByDateToDate([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var list_date = _tempetatureStorage.Get().Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToList();

            return Ok(list_date);
        }
    }
}
