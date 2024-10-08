/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using Viewer.Properties;

namespace Viewer;

/**
 * @author gerzon
 */
public sealed partial class FormPlot : Form
{
    private readonly Chart _chartPlot;
    private int _count;
    private bool _isOpen;
    private SerialPort? _sp;

    public FormPlot()
    {
        InitializeComponent();
        Text = Resources.Title;
        using var ms = new MemoryStream(Resources.Icon);
        Icon = new Icon(ms);
        WindowState = FormWindowState.Maximized;
        portBox.Text = Resources.Port;
        portList.DropDownStyle = ComboBoxStyle.DropDownList;
        foreach (var port in SerialPort.GetPortNames())
        {
            portList.Items.Add(port);
        }
        if (portList.Items.Count > 0)
        {
            portList.SelectedIndex = 0;
        }
        portBR.Text = Resources.BaudRate;
        action.Text = Resources.Open;
        command.Text = Resources.Command;
        send.Text = Resources.Send;
        command.Visible = false;
        maxPlot.Text = Resources.MaxPlottedSamples;
        maxValue.Text = Resources.MaxValue;
        output.Multiline = true;
        output.ReadOnly = true;
        output.BackColor = Color.White;
        output.Visible = false;
        _chartPlot = new Chart();
        var area = new ChartArea();
        area.AxisX.MajorGrid.Enabled = false;
        area.AxisX.MinorGrid.Enabled = false;
        area.AxisY.MajorGrid.Enabled = false;
        area.AxisY.MinorGrid.Enabled = false;
        _chartPlot.ChartAreas.Add(area);
        _chartPlot.Location = new Point(0, 135);
        Controls.Add(_chartPlot);
    }

    private void text_KeyPress(object sender, KeyPressEventArgs e)
    {
        e.Handled = !(int.TryParse(e.KeyChar.ToString(), out _) || e.KeyChar == '\b');
    }

    private void connect_Click(object sender, EventArgs e)
    {
        if (_sp is { IsOpen: true })
        {
            //_sp.Write("\u001A");
            _isOpen = false;
            action.Text = Resources.Open;
            portList.Enabled = true;
            portBR.Enabled = true;
            output.Visible = false;
            command.Visible = false;
            maxValue.ReadOnly = false;
        }
        else
        {
            if (portList.SelectedItem != null && portBR.Text is { Length: > 0 } && maxValue.Text is { Length: > 0 })
            {
                _count = 1;
                _chartPlot.Series.Clear();
                _chartPlot.Series.Add(new Series
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Int64,
                    YValueType = ChartValueType.Double
                });
                _sp = new SerialPort((string)portList.SelectedItem);
                _sp.BaudRate = int.Parse(portBR.Text);
                _sp.Parity = Parity.None;
                _sp.StopBits = StopBits.One;
                _sp.DataBits = 8;
                _sp.Handshake = Handshake.None;
                _sp.RtsEnable = true;
                _sp.Open();
                action.Text = Resources.Close;
                portList.Enabled = false;
                portBR.Enabled = false;
                output.Visible = true;
                command.Visible = true;
                maxValue.ReadOnly = true;
                _isOpen = true;
                Task.Run(Run);
            }
            else
            {
                MessageBox.Show(Resources.RequiredField);
            }
        }
    }

    private async Task Run()
    {
        if (_sp is { IsOpen: true })
        {
            using var reader = new StreamReader(_sp.BaseStream);
            while (_isOpen)
            {
                var text = await reader.ReadLineAsync();
                if (text == null)
                {
                    continue;
                }
                if (float.TryParse(text, out var value))
                {
                    Plot(value);
                }
                Print(text);
            }
            _sp.Close();
            _sp = null;
        }
    }

    private void Print(string text)
    {
        if (output.InvokeRequired)
        {
            output.Invoke(new MethodInvoker(delegate { output.Text = text; }));
        }
        else
        {
            output.Text = text;
        }
    }

    private void Plot(float value)
    {
        try
        {
            _chartPlot.Series[0].Points.AddXY(_count, value);
            if (_chartPlot.Series[0].Points.Count > int.Parse(maxValue.Text))
            {
                _chartPlot.Series[0].Points.RemoveAt(0);
            }
            _count++;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void send_Click(object sender, EventArgs e)
    {
        if (commandLine.Text is { Length: > 0 } && _sp is { IsOpen: true })
        {
            _sp.Write($"{commandLine.Text}\r\n");
            commandLine.Text = null;
        }
        else
        {
            MessageBox.Show(Resources.RequiredField);
            commandLine.Focus();
        }
    }

    private void FormPlot_Resize(object sender, EventArgs e)
    {
        _chartPlot.Width = Width;
        _chartPlot.Height = Height - _chartPlot.Location.Y;
    }
}