/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 * @author gerzon
 */
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTO;

var jsonOpt = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var endpoint = builder.Configuration.GetValue("Endpoint", "http://www.ioc-sealevelmonitoring.org/bgan/post.php");

builder.Services.AddDbContext<CustomDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapPost("/tide/{stationCode}", async (HttpRequest request, string stationCode, CustomDbContext dbContext) => {
    var station = dbContext.Stations
        .Include(s => s.Organization)
        .Include(s => s.Organization.Country).FirstOrDefault(s => s.Code == stationCode);
    if (station == null)
    {
        return Results.NotFound();
    }
    var list = await request.ReadFromJsonAsync<List<Data>>(jsonOpt);
    if (list == null || list.Count == 0)
    {
        return Results.NoContent();
    }
    list.ForEach(l => l.Station = station);
    try
    {
        dbContext.AddRange(list);
        await dbContext.SaveChangesAsync();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    _ = Send(list);
    return Results.Ok();
});

app.Run();
return;

async Task Send(List<Data> list)
{
    try
    {
        var first = list.First();
        var data = "ID:" + first.Station.Organization.Country.Code + "-" + first.Station.Code + "-00 DT:" + first.Time.ToString("yyyy MM dd HH mm") + "\r\n";
        var row = ":PRS /1";
        foreach (var str in list.Select(obj => "00000" + (int)(obj.Value * 1000)))
        {
            var tmp = str;
            if (tmp.Length > 5)
            {
                tmp = str.Substring(str.Length - 5);
            }
            row += " " + tmp;
        }
        data += row;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("filename", Uri.EscapeDataString(first.Station.Organization.Country.Code + "-" + first.Station.Code + "-" + DateTime.Now.ToString("yyyyMMddHHmmss")));
        client.DefaultRequestHeaders.Add("datapack", Uri.EscapeDataString(data));
        await client.PostAsync(endpoint, null);
        client.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}