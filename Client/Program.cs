﻿/*
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
var firstPackage = DateTime.Now;
var dataList = new List<Data>();
var jsonOpt = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
bool isOpen;

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
    sp.Write($"NAVG={param.Average}\r\n");
    sp.Write($"OUTPUTFORMAT={param.Format}\r\n");
    sp.Write($"DECIMALS={param.Decimals}\r\n");
    sp.Write($"LATITUDE={param.Latitude:###0.00}\r\n");
    sp.Write("AUTORUN=N\r\n");
    sp.Write("START\r\n");

    test = args[1] == "-t";
    view = args[1] == "-v";

    while (DateTime.Now.Second != 59)
    {

    }
    isOpen = true;
    await Run();

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
}
return;

async Task Run()
{
    if (sp is { IsOpen: true })
    {
        using var reader = new StreamReader(sp.BaseStream);
        while (isOpen)
        {
            var text = await reader.ReadLineAsync();
            if (text == null)
            {
                continue;
            }
            if (!float.TryParse(text, out var value))
            {
                return;
            }
            if (test)
            {
                count++;
                list.Add(new Plot.Point(count, (int)(value * 10)));
                if (count < 20) continue;
                isOpen = false;
                Plot.DrawChart(list);
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

                    if (param.LogPath is not { Length: > 0 }) continue;
                    try
                    {
                        var dir = Directory.CreateDirectory(param.LogPath);
                        if (dir.Exists)
                        {
                            await using var outputFile = new StreamWriter($"{param.LogPath}/{DateTime.Now:yyyyMMdd}.csv", true);
                            _ = outputFile.WriteLineAsync(data.ToString());
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }
                else
                {
                    _ = Send(dataList.AsReadOnly());
                    initPackage = true;
                }
            }
        }
        sp.Close();
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