/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
namespace Viewer;

/**
 * @author gerzon
 */
sealed partial class FormPlot
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        portBox = new GroupBox();
        action = new Button();
        portBR = new TextBox();
        portList = new ComboBox();
        output = new TextBox();
        command = new GroupBox();
        send = new Button();
        commandLine = new TextBox();
        maxPlot = new GroupBox();
        maxValue = new TextBox();
        portBox.SuspendLayout();
        command.SuspendLayout();
        maxPlot.SuspendLayout();
        SuspendLayout();
        // 
        // portBox
        // 
        portBox.Controls.Add(action);
        portBox.Controls.Add(portBR);
        portBox.Controls.Add(portList);
        portBox.Location = new Point(12, 12);
        portBox.Name = "portBox";
        portBox.Size = new Size(369, 54);
        portBox.TabIndex = 0;
        portBox.TabStop = false;
        // 
        // action
        // 
        action.Location = new Point(284, 22);
        action.Name = "action";
        action.Size = new Size(75, 23);
        action.TabIndex = 2;
        action.UseVisualStyleBackColor = true;
        action.Click += connect_Click;
        // 
        // portBR
        // 
        portBR.Location = new Point(178, 22);
        portBR.Name = "portBR";
        portBR.Size = new Size(100, 23);
        portBR.TabIndex = 1;
        portBR.KeyPress += text_KeyPress;
        // 
        // portList
        // 
        portList.FormattingEnabled = true;
        portList.Location = new Point(6, 22);
        portList.Name = "portList";
        portList.Size = new Size(166, 23);
        portList.TabIndex = 0;
        // 
        // output
        // 
        output.Location = new Point(190, 72);
        output.Multiline = true;
        output.Name = "output";
        output.Size = new Size(582, 57);
        output.TabIndex = 3;
        // 
        // command
        // 
        command.Controls.Add(send);
        command.Controls.Add(commandLine);
        command.Location = new Point(387, 12);
        command.Name = "command";
        command.Size = new Size(385, 54);
        command.TabIndex = 1;
        command.TabStop = false;
        // 
        // send
        // 
        send.Location = new Point(304, 22);
        send.Name = "send";
        send.Size = new Size(75, 23);
        send.TabIndex = 1;
        send.UseVisualStyleBackColor = true;
        send.Click += send_Click;
        // 
        // commandLine
        // 
        commandLine.Location = new Point(6, 22);
        commandLine.Name = "commandLine";
        commandLine.Size = new Size(292, 23);
        commandLine.TabIndex = 0;
        // 
        // maxPlot
        // 
        maxPlot.Controls.Add(maxValue);
        maxPlot.Location = new Point(12, 72);
        maxPlot.Name = "maxPlot";
        maxPlot.Size = new Size(172, 57);
        maxPlot.TabIndex = 2;
        maxPlot.TabStop = false;
        // 
        // maxValue
        // 
        maxValue.Location = new Point(6, 22);
        maxValue.Name = "maxValue";
        maxValue.Size = new Size(160, 23);
        maxValue.TabIndex = 0;
        maxValue.KeyPress += text_KeyPress;
        // 
        // FormPlot
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 361);
        Controls.Add(maxPlot);
        Controls.Add(command);
        Controls.Add(output);
        Controls.Add(portBox);
        Name = "FormPlot";
        Text = "Form1";
        Resize += FormPlot_Resize;
        portBox.ResumeLayout(false);
        portBox.PerformLayout();
        command.ResumeLayout(false);
        command.PerformLayout();
        maxPlot.ResumeLayout(false);
        maxPlot.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox portBox;
    private ComboBox portList;
    private TextBox portBR;
    private Button action;
    private TextBox output;
    private GroupBox command;
    private Button send;
    private TextBox commandLine;
    private GroupBox maxPlot;
    private TextBox maxValue;
}
