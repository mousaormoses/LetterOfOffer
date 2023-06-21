using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

public class AppSettings
{
    public string DbPath { get; set; }

    private static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static string fileName = Path.Combine(appDirectory, "settings.json");

    public static AppSettings Load()
    {
        if (!File.Exists(fileName))
        {
            return new AppSettings
            {
                DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite")
            };
        }

        string json = File.ReadAllText(fileName);
        return JsonConvert.DeserializeObject<AppSettings>(json);
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(this);
        File.WriteAllText(fileName, json);
    }
}

