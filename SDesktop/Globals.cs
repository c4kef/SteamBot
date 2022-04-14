global using System.Windows.Navigation;
global using ModernWpf.Controls;
global using System.Windows;
global using System;
global using System.Threading.Tasks;
global using System.IO;
global using Newtonsoft.Json;
global using System.ComponentModel;
global using System.Windows.Media;
global using Microsoft.WindowsAPICodePack.Dialogs;
global using System.Runtime.InteropServices;
global using System.Text;
global using Newtonsoft.Json.Linq;
global using System.Text.RegularExpressions;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;

namespace SDesktop;

public static class Globals
{
    private const string NameSetupFile = "Setup.json";
    public static Setup Setup { get; private set; } = null!;

    public static async Task Init()
    {
        Setup = (File.Exists(NameSetupFile)
            ? JsonConvert.DeserializeObject<Setup>(await File.ReadAllTextAsync(NameSetupFile))
            : new Setup())!;

        if (!File.Exists(NameSetupFile))
            await SaveSetup();
    }

    public static async Task SaveSetup()
    {
        await File.WriteAllTextAsync(NameSetupFile, JsonConvert.SerializeObject(Setup));
    }
}

#region Images

[Serializable]
public class Setup
{
    public int CountThread = 1;
    public string PathToProxy = string.Empty;
    public string ApiKey = string.Empty;

    public string PathToAccounts = string.Empty;
    public bool GenNameFromFile = false;
    public string PathToGenNameFile = string.Empty;
    public string About = string.Empty;
}

#endregion