/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 * @author gerzon
 */
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Net.Http.Json;
using System.Text.Json;
using Client;
using Client.Util;

Param param;
SerialPort sp;
bool test, view, initPackage = true;
var count = 0;
var list = new List<Plot.Point>();
DateTime firstPackage = DateTime.Now;
var dataList = new List<Data>();
var jsonOpt = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

//run.config -v
if (args.Length < 2 || !(args[1] == "-t" || args[1] == "-r" || args[1] == "-v"))
{
    Console.WriteLine("Invalid argument");
}
else
{
    param = Param.GetInstance(args[0]);

    const string df = "yyyy/MM/dd-HH:mm:ss";

    Console.WriteLine($"INIT \"{param.StationCode}\" to \"{param.EndPoint}\" - {DateTime.Now.ToString(df)}\n__________________________________________________________________\n");

    sp = new SerialPort(param.SerialPort);

    sp.BaudRate = param.SerialBaudRate;
    sp.Parity = Parity.None;
    sp.StopBits = StopBits.One;
    sp.DataBits = 8;
    sp.Handshake = Handshake.None;
    sp.RtsEnable = true;

    sp.Open();
    sp.WriteLine($"NAVG={param.Average}\r");
    sp.WriteLine($"OUTPUTFORMAT={param.Format}\r");
    sp.WriteLine($"DECIMALS={param.Decimals}\r");
    sp.WriteLine($"LATITUDE={param.Latitude:###0.00}\r");
    sp.WriteLine("AUTORUN=N\r");
    sp.WriteLine("START\r");

    test = args[1] == "-t";
    view = args[1] == "-v";

    while (DateTime.Now.Second != 59)
    {

    }

    sp.DataReceived += DataReceivedHandler;

    if (test)
    {
        try
        {
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine($"\b\n__________________________________________________________________\nEND - {DateTime.Now.ToString(df)}\n");
    }
    else
    {
        while (true)
        {

        }
    }
}
return;

void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
{
    if (!float.TryParse(((SerialPort)sender).ReadExisting(), out var value))
    {
        return;
    }
    if (test)
    {
        count++;
        list.Add(new Plot.Point(count, (int)(value * 10)));
        if (count >= 20)
        {
            sp.Close();
            Plot.DrawChart(list);
        }
    }
    else
    {
        if (initPackage)
        {
            dataList.Clear();
            initPackage = false;
            firstPackage = DateTime.Now;
        }
        var diff = DateTime.Now - firstPackage;
        if (diff.TotalMinutes <= 5)
        {
            var data = new Data
            {
                Time = DateTime.Now,
                Value = value
            };
            dataList.Add(data);
            if (view)
            {
                Console.WriteLine(data.ToString());
            }
            if (param.LogPath is { Length: > 0 })
            {
                try
                {
                    var dir = Directory.CreateDirectory(param.LogPath);
                    if (dir.Exists)
                    {
                        using var outputFile = new StreamWriter($"{param.LogPath}/{DateTime.Now:yyyyMMdd}.csv", true);
                        _ = outputFile.WriteLineAsync(data.ToString());
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
        else
        {
            _ = Task.Run(() => Send(dataList.AsReadOnly()));
            initPackage = true;
        }
    }
}

async Task Send(ReadOnlyCollection<Data> ds)
{
    try
    {
        var client = new HttpClient();
        await client.PostAsJsonAsync($"{param.EndPoint}/{param.StationCode}", ds, jsonOpt);
        client.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    Console.WriteLine($"Sending package with {ds.Count} samples");
}