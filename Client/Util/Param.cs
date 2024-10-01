/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
namespace Client.Util;

/**
 * @author gerzon
 */
public class Param
{
    public string SerialPort { get; private set; } = "/dev/ttyUSB0";

    public int SerialBaudRate { get; private set; } = 9600;

    public int Average { get; private set; } = 16;

    public int Format { get; private set; } = 3;

    public int Decimals { get; private set; } = 2;

    public double Latitude { get; private set; } = 12;

    public string StationCode { get; private set; } = string.Empty;

    public string EndPoint { get; private set; } = string.Empty;

    public string? LogPath { get; private set; }

    public static Param GetInstance(string file)
    {
        var obj = new Param();
        foreach (var row in File.ReadAllText(file).Split("\n"))
        {
            if (row.StartsWith('#') || row.Trim().Length <= 0)
                continue;
            var pair = row.Split("=");
            if (pair.Length == 2)
            {
                var value = pair[1].Trim();
                switch (pair[0].Trim())
                {
                    case "serial.port":
                        obj.SerialPort = value;
                        break;
                    case "serial.baud.rate":
                        obj.SerialBaudRate = int.Parse(value);
                        break;
                    case "data.average":
                        obj.Average = int.Parse(value);
                        break;
                    case "data.format":
                        obj.Format = int.Parse(value);
                        break;
                    case "data.decimals":
                        obj.Decimals = int.Parse(value);
                        break;
                    case "latitude":
                        obj.Latitude = double.Parse(value);
                        break;
                    case "station.code":
                        obj.StationCode = value;
                        break;
                    case "log.path":
                        obj.LogPath = value;
                        break;
                    case "server.endpoint":
                        obj.EndPoint = value;
                        break;
                }
            }
        }
        return obj;
    }
}