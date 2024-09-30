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
bool test, view, initSample = true, initPackage = true;
int count = 0, countSample = 0;
var list = new List<Plot.Point>();
DateTime firstSample = DateTime.Now, firstPackage = DateTime.Now;
var sum = 0f;
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
    Console.WriteLine("INIT \"" + param.StationCode + "\" to \"" + param.EndPoint + "\" - " + DateTime.Now.ToString(df));
    Console.WriteLine("__________________________________________________________________");
    Console.WriteLine();

    sp = new SerialPort(param.SerialPort);

    sp.BaudRate = param.SerialBaudRate;
    sp.Parity = Parity.None;
    sp.StopBits = StopBits.One;
    sp.DataBits = 8;
    sp.Handshake = Handshake.None;
    sp.RtsEnable = true;

    sp.Open();
    sp.WriteLine("NAVG=" + param.Average + "\r");
    sp.WriteLine("OUTPUTFORMAT=" + param.Format + "\r");
    sp.WriteLine("DECIMALS=" + param.Decimals + "\r");
    sp.WriteLine("LATITUDE=" + param.Latitude + "\r");
    sp.WriteLine("AUTORUN=N\r");
    sp.WriteLine("START\r");

    test = args[1] == "-t";
    view = args[1] == "-v";
    
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
        Console.WriteLine("\b");
        Console.WriteLine("__________________________________________________________________");
        Console.WriteLine("END - " + DateTime.Now.ToString(df));
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
        if (count >= 100)
        {
            sp.Close();
            Plot.DrawChart(list);
        }
    }
    else
    {
        if (initSample)
        {
            countSample = 0;
            sum = 0;
            initSample = false;
            firstSample = DateTime.Now;
        }
        var diff = DateTime.Now - firstSample;
        sum += value;
        countSample++;
        if (diff.TotalSeconds >= 1)
        {
            var sample = sum / countSample;
            if (initPackage)
            {
                dataList.Clear();
                initPackage = false;
                firstPackage = DateTime.Now;
            }
            diff = DateTime.Now - firstPackage;
            if (diff.TotalSeconds <= 5)
            {
                var data = new Data
                {
                    Time = DateTime.Now,
                    Value = sample
                };
                dataList.Add(data);
                if (view)
                {
                    Console.WriteLine(data.ToString());
                }
                if (param.LogFile is { Length: > 0 })
                {
                    try
                    {
                        var dir = Directory.CreateDirectory(param.LogFile);
                        if (dir.Exists)
                        {
                            using var outputFile = new StreamWriter(param.LogFile + "/" + DateTime.Now.ToString("yyyyMMdd") + ".csv", true);
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
            initSample = true;
        }
    }
}

async Task Send(ReadOnlyCollection<Data> ds)
{
    try
    {
        var client = new HttpClient();
        await client.PostAsJsonAsync(param.EndPoint + "/" + param.StationCode, ds, jsonOpt);
        client.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    //Console.WriteLine(ds.Count);
}