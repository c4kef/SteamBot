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
global using OpenQA.Selenium;
global using OpenQA.Selenium.Chrome;
global using OpenQA.Selenium.Support.UI;
global using OpenQA.Selenium.DevTools;
global using System.Threading;
global using SDesktop.Pages;
using System.Diagnostics.CodeAnalysis;
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
namespace SDesktop;

public static class Globals
{
    private const string NameSetupFile = "Setup.json";
    public const string NameFolderSetupAccounts = "ReadyAccounts";
    public static Setup Setup { get; private set; } = null!;

    public static async Task Init()
    {
        Setup = (File.Exists(NameSetupFile)
            ? JsonConvert.DeserializeObject<Setup>(await File.ReadAllTextAsync(NameSetupFile))
            : new Setup())!;

        if (!File.Exists(NameSetupFile))
            await SaveSetup();

        if (!Directory.Exists(NameFolderSetupAccounts))
            Directory.CreateDirectory(NameFolderSetupAccounts);
    }

    public static async Task SaveSetup()
    {
        await File.WriteAllTextAsync(NameSetupFile, JsonConvert.SerializeObject(Setup));
    }
    
    public static bool IsElementExist(ChromeDriver driver, By by)
    {
        try
        {
            var element = driver.FindElement(by);
            return element.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    
    public static async Task<string> GetMailCode(string userName, string password, DateTimeOffset timeOffsetFrom, int timeoutSeconds = 20)
    {
        var client = new ImapClient();
        
        await client.ConnectAsync("imap.rambler.ru", 993, true);
        await client.AuthenticateAsync(userName, password);

        if (!client.IsConnected)
            throw new Exception("Client is not connected");
        
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
        
        if (client.Inbox.Count <= 0)
            return string.Empty;

        repeat:
        await Task.Delay(500);
        
        var message = await client.Inbox.GetMessageAsync(client.Inbox.Count - 1);

        if ((timeOffsetFrom - message.Date).TotalSeconds > timeoutSeconds)
             goto repeat;
        
        return new Regex("[A-Z0-9]{5}", RegexOptions.Multiline).Match(message.TextBody).Value;
    }
    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public static async Task<(string login, string password, string email, string eUsername, string ePassword, string emailNew, string eUsernameNew, string ePasswordNew)> GetAccountSteam(string[] busyAccounts)
    {
        var accountsFree = (await File.ReadAllLinesAsync(Setup.PathToAccounts)).Select(account => account.Split(',')).Where(dataAccount => !busyAccounts.Contains(dataAccount[0])).ToList();

        if (accountsFree.Count == 0)
            return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty);
        
        var accountFree = accountsFree[new Random().Next(0, accountsFree.Count)];

        return (accountFree[0], accountFree[1], accountFree[2], accountFree[3], accountFree[4], accountFree[5],
            accountFree[6], accountFree[7]);
    }
    
    public static string GetPictureAccount()
    {
        var accountsFree = Directory.GetFiles(Setup.PathToImages);

        return accountsFree.Length == 0 ? string.Empty : accountsFree[new Random().Next(0, accountsFree.Length - 1)];
    }
    
    public static async Task<(string username, string realName)> GetUsername()
    {
        var names = (await File.ReadAllLinesAsync(Setup.PathToGenNameFile)).Select(name => name.Split(',')).ToList();

        var name = names[new Random().Next(0, names.Count)];

        return (name[0], name[1]);
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
    public string PathToImages = string.Empty;
    public bool GenNameFromFile = false;
    public string PathToGenNameFile = string.Empty;
    public string About = string.Empty;
}

#endregion