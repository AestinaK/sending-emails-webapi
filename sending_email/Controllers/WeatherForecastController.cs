using Microsoft.AspNetCore.Mvc;
using sending_email.Service;

namespace sending_email.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IEmailService _emailService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpPost]
    public async Task GetEmail(MessageOnly message)
    {
      await _emailService.SendMessage(message);
    }

    [HttpPost]
    public async Task SendEmailWithFile()
    {
        var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
        var sendMail = new Message("astina", "astina.prathamit@gmail.com","This is content", "This is subject",files);
        await _emailService.SendMailWithFileAsync(sendMail);
    }
}