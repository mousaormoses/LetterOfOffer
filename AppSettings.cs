using Newtonsoft.Json;
using System;
using System.IO;

public class AppSettings
{
    public string DbPath { get; set; }

    private const string fileName = "settings.json";

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
