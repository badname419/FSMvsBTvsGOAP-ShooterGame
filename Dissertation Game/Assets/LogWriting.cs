
using System;
using System.IO;

public class LogWriting
{
    private string filePath;

    public LogWriting(string fileName)
    {
        //this.filePath = "C:/Users/Lukasz/Documents/" + fileName;
        //File.WriteAllText(filePath, String.Empty);
    }

    public void AddLine(string line)
    {
        //File.AppendAllText(filePath, line + Environment.NewLine);
    }

}
