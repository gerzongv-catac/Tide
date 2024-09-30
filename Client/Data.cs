/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
namespace Client;

/**
 * @author gerzon
 */
public class Data
{
    public DateTime Time { get; init; } = DateTime.Now;

    public float Value { get; set; } = -1;

    public override string ToString() => Time.ToString("yy/MM/dd-HH:mm:ss.ff") + ", " + Value.ToString("###0.00");
}