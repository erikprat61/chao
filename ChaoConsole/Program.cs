using System.Text.Json;
using Cocona;

var app = CoconaLiteApp.Create(); // is a shorthand for `CoconaApp.CreateBuilder().Build()`

app.AddCommand(
    "save",
    (string notesDirectory) =>
    {
        var settings = new Settings(notesDirectory);
        SaveSettings(settings);
        Console.WriteLine("Settings saved.");
    }
);

app.AddCommand(
    "get",
    () =>
    {
        var settings = GetSettings();
        Console.WriteLine($"Notes Directory: {settings.notesDirectory}");
    }
);

app.Run();

static void SaveSettings(Settings settings)
{
    string filePath = "settings.json";
    string jsonString = JsonSerializer.Serialize(settings);
    File.WriteAllText(filePath, jsonString);
    Console.WriteLine("Settings have been saved.");
}

static Settings GetSettings()
{
    string filePath = "settings.json";
    if (!File.Exists(filePath))
    {
        Console.WriteLine("No settings found. Creating default settings.");
        var defaultSettings = new Settings("DefaultDirectory");
        SaveSettings(defaultSettings); // Reuse the SaveSettings method to write the default settings
        return defaultSettings;
    }

    string jsonString = File.ReadAllText(filePath);
#pragma warning disable CS8603 // Possible null reference return.
    return JsonSerializer.Deserialize<Settings>(jsonString); // this will never be null
#pragma warning restore CS8603 // Possible null reference return.
}

record Settings(string notesDirectory);
