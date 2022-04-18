namespace SDesktop.Steam;

public class SClient
{
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait;
    private string _id = null!;

    private string _loginSteam;
    private string _passwordSteam;

    private string _mailLogin;
    private string _mailPassword;

    public SClient(string login, string password, string emailLogin, string emailPassword)
    {
        var options = new ChromeOptions();

        var service = ChromeDriverService.CreateDefaultService();

        _loginSteam = login;
        _passwordSteam = password;

        _mailLogin = emailLogin;
        _mailPassword = emailPassword;
        
        service.HideCommandPromptWindow = true;
        options.AddArgument("no-sandbox");
        options.AddArgument("remote-debugging-port=0");
        options.AddArgument("disable-extensions");
        options.AddArgument(
            "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36");
        options.AddArgument("start-maximized");

        _driver = new ChromeDriver(service, options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
    }

    #region Variables Basic

    public string UserName
    {
        get
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");
        
            return _driver.FindElement(By.Name("personaName")).GetAttribute("value");
        }
        set
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");
        
            var element = _driver.FindElement(By.Name("personaName"));
            element.SendKeys(Keys.Control + "A");
            element.SendKeys(Keys.Delete);
            element.SendKeys(value);
            
            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[7]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }    
    
    public string RealUserName
    {
        get
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");
        
            return _driver.FindElement(By.Name("real_name")).GetAttribute("value");
        }
        set
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");
        
            var element = _driver.FindElement(By.Name("real_name"));
            element.SendKeys(Keys.Control + "A");
            element.SendKeys(Keys.Backspace);
            element.SendKeys(value);
            
            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[7]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }

    public string UrlProfile
    {
        get
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            return _driver.FindElement(By.Name("customURL")).GetAttribute("value");
        }
        set
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            var element = _driver.FindElement(By.Name("customURL"));
            element.SendKeys(Keys.Control + "A");
            element.SendKeys(Keys.Delete);
            element.SendKeys(value);
            
            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[7]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }

    public string Country
    {
        get
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            return _driver
                .FindElement(
                    By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[4]/div[2]/div[1]/div[2]/div[1]")).Text;
        }
        set
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[4]/div[2]/div/div[2]"))
                .Click();

            Thread.Sleep(1_500);
            
            foreach (var element in _driver.FindElements(By.ClassName("dropdown_DialogDropDownMenu_Item_2oAiZ")))
                if (element.Text == value)
                {
                    element.Click();
                    break;
                }
            
            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[7]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }
    
    public string About
    {
        get
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            return _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[5]/div[2]/textarea")).GetAttribute("value");
        }
        set
        {
            if (!_driver.Url.Contains("edit/info"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/info");

            var element = _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[5]/div[2]/textarea"));
            element.SendKeys(Keys.Control + "A");
            element.SendKeys(Keys.Delete);
            element.SendKeys(value);
            
            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/form/div[7]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }
    
    public string UserPicture
    {
        get
        {
            if (!_driver.Url.Contains("edit/avatar"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/avatar");

            return _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[1]/div[3]/div[1]/div[1]/div[1]/img")).GetAttribute("src");
        }
        set
        {
            if (!_driver.Url.Contains("edit/avatar"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/avatar");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[1]/div[3]/div[2]/input")).SendKeys(value);
            
            Thread.Sleep(500);

            while(!Globals.IsElementExist(_driver, By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[2]/button[1]")))
                Thread.Sleep(500);

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[2]/button[1]")).Click();
            
            Thread.Sleep(1_000);
        }
    }

    #endregion

    #region Variables Privacy
    
    public string AccessToGameInfo
    {
        get
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            return _driver
                .FindElement(
                    By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[1]/div")).Text;
        }
        set
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[1]/div"))
                .Click();

            Thread.Sleep(1_500);
            
            foreach (var element in _driver.FindElements(By.ClassName("contextmenu_contextMenuItem_1n7Wl")))
                if (element.Text == value)
                {
                    element.Click();
                    break;
                }
            
            Thread.Sleep(1_000);
        }
    }
    
    public string AccessToListOfFriends
    {
        get
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            return _driver
                .FindElement(
                    By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[4]/div")).Text;
        }
        set
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[4]/div"))
                .Click();

            Thread.Sleep(1_500);
            
            foreach (var element in _driver.FindElements(By.ClassName("contextmenu_contextMenuItem_1n7Wl")))
                if (element.Text == value)
                {
                    element.Click();
                    break;
                }
            
            Thread.Sleep(1_000);
        }
    }
    
    public string AccessToInventory
    {
        get
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            return _driver
                .FindElement(
                    By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[7]/div")).Text;
        }
        set
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[7]/div"))
                .Click();

            Thread.Sleep(1_500);
            
            foreach (var element in _driver.FindElements(By.ClassName("contextmenu_contextMenuItem_1n7Wl")))
                if (element.Text == value)
                {
                    element.Click();
                    break;
                }
            
            Thread.Sleep(1_000);
        }
    }
    
    public string AccessToComments
    {
        get
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            return _driver
                .FindElement(
                    By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[10]/div")).Text;
        }
        set
        {
            if (!_driver.Url.Contains("edit/settings"))
                _driver.Navigate().GoToUrl($"https://steamcommunity.com/profiles/{76561198995111037}/edit/settings");

            _driver.FindElement(By.XPath("//*[@id='application_root']/div[2]/div[2]/div/div[6]/div[10]/div"))
                .Click();

            Thread.Sleep(1_500);
            
            foreach (var element in _driver.FindElements(By.ClassName("contextmenu_contextMenuItem_1n7Wl")))
                if (element.Text == value)
                {
                    element.Click();
                    break;
                }
            
            Thread.Sleep(1_000);
        }
    }

    #endregion

    private async Task WaitLoadPage()
    {
        _ = _driver.Manage().Timeouts().ImplicitWait;
        
        while (!_driver.ExecuteScript("return document.readyState").Equals("complete"))
            await Task.Delay(1000);

        await Task.Delay(500);
    }
    
    public async Task EnableSteamGuard()
    {
        try
        {
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Включение Steam Guard";
            _driver.Navigate().GoToUrl("https://store.steampowered.com/twofactor/manage");

            _driver.FindElement(By.XPath("//*[@id='email_authenticator_check']")).Click();
            await WaitLoadPage();
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Steam Guard включен, попытка войти в аккаунт";
            await Login();
        }
        catch (Exception ex)
        {
            await File.AppendAllTextAsync("log_error.txt", $"[{DateTime.Now}] [{_loginSteam}]\n{ex.Message}");
            Dashboard.GetInstance().TextLogs =
                $"[{_loginSteam}] Что-то пошло не так, результат был записан в log_error.txt";
        }
    }
    
    public async Task Login(int maxTryLogin = 3)
    {
        try
        {
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Вход в аккаунт";
            var currentTryLogin = 0;
            again:
            if (++currentTryLogin > maxTryLogin)
            {
                Dashboard.GetInstance().TextLogs =
                    $"[{_loginSteam}] Войти в аккаунт не удалось";
                return;
            }

            _driver.Navigate().GoToUrl("https://steamcommunity.com/login/");

            if (!Globals.IsElementExist(_driver, By.Id("input_username")))
            {
                Dashboard.GetInstance().TextLogs =
                    $"[{_loginSteam}] Вход не требуется";
                return;
            }

            _driver.FindElement(By.Id("input_username")).SendKeys(_loginSteam);
            _driver.FindElement(By.Id("input_password")).SendKeys(_passwordSteam);
            _driver.FindElement(By.XPath("//button[@class='btn_blue_steamui btn_medium login_btn']")).Click();

            var timeFrom = DateTimeOffset.Now;

            await Task.Delay(1_500);

            var tryLogin = 0;

            if (Globals.IsElementExist(_driver, By.Id("authcode")))
            {
                _driver.FindElement(By.Id("authcode"))
                    .SendKeys(await Globals.GetMailCode(_mailLogin, _mailPassword, timeFrom));
                _driver.FindElement(By.XPath("//*[@id='auth_buttonset_entercode']/div[1]")).Click();

                while (!Globals.IsElementExist(_driver, By.Id("success_continue_btn")))
                {
                    if (++tryLogin > 3)
                        goto again;

                    await Task.Delay(2000);
                }

                _driver.FindElement(By.Id("success_continue_btn")).Click();
            }

            _id = _driver.Url.Split('/')[4];

            await WaitLoadPage();
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Вход успешен";

        }
        catch (Exception ex)
        {
            await File.AppendAllTextAsync("log_error.txt", $"[{DateTime.Now}] [{_loginSteam}]\n{ex.Message}");
            Dashboard.GetInstance().TextLogs =
                $"[{_loginSteam}] Что-то пошло не так, результат был записан в log_error.txt";
        }
    }

    public async Task ChangeEmail(string emailLogin, string emailPassword)
    {
        try
        {
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Смена почты";
            
            _driver.Navigate().GoToUrl("https://store.steampowered.com/account");

            _driver.FindElement(By.XPath("//*[@id='main_content']/div[2]/div[4]/div[1]/div[3]/a")).Click();
            await WaitLoadPage();

            var timeFrom = DateTimeOffset.Now;

            _driver.FindElement(By.XPath("//*[@id='wizard_contents']/div/a[2]")).Click();

            await WaitLoadPage();
            
            _driver.FindElement(By.XPath("//*[@id='forgot_login_code']"))
                .SendKeys(await Globals.GetMailCode(_mailLogin, _mailPassword, timeFrom));
            _driver.FindElement(By.XPath("//*[@id='forgot_login_code_form']/div[3]/input")).Click();

            await WaitLoadPage();
            
            _driver.FindElement(By.XPath("//*[@id='email_reset']")).SendKeys(emailLogin);

            timeFrom = DateTimeOffset.Now;
            _driver.FindElement(By.XPath("//*[@id='change_email_area']/input")).Click();

            await WaitLoadPage();
            
            _driver.FindElement(By.XPath("//*[@id='email_change_code']"))
                .SendKeys(await Globals.GetMailCode(emailLogin, emailPassword,
                    timeFrom)); //To-Do заменить получение кода из нового ящика указав его логин и пароль
            _driver.FindElement(By.XPath("//*[@id='confirm_email_form']/div[2]/input")).Click();

            await WaitLoadPage();

            _mailLogin = emailLogin;
            _mailPassword = emailPassword;
            
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Смена почты успешна";

        }
        catch (Exception ex)
        {
            await File.AppendAllTextAsync("log_error.txt", $"[{DateTime.Now}] [{_loginSteam}]\n{ex.Message}");
            Dashboard.GetInstance().TextLogs =
                $"[{_loginSteam}] Попытка сменить пароль не удалась, похоже было много попыток";
        }
    }
    
    public async Task ChangePassword(string newPassword)
    {
        try
        {
            Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Смена пароля";
            _driver.Navigate().GoToUrl("https://store.steampowered.com/account");

            _driver.FindElement(By.XPath("//*[@id='main_content']/div[2]/div[6]/div[1]/div[2]/div[2]/a")).Click();
            await WaitLoadPage();

            var timeFrom = DateTimeOffset.Now;

            _driver.FindElement(By.XPath("//*[@id='wizard_contents']/div/a[2]")).Click();

            await WaitLoadPage();

            _driver.FindElement(By.XPath("//*[@id='forgot_login_code']"))
                .SendKeys(await Globals.GetMailCode(_mailLogin, _mailPassword, timeFrom, 15));
            await Task.Delay(1_000);
            _driver.FindElement(By.XPath("//*[@id='forgot_login_code_form']/div[3]/input")).Click();

            await WaitLoadPage();

            _driver.FindElement(By.XPath("//*[@id='password_reset']")).SendKeys(newPassword);
            _driver.FindElement(By.XPath("//*[@id='password_reset_confirm']")).SendKeys(newPassword);

            _driver.FindElement(By.XPath("//*[@id='change_password_form']/div[3]/input")).Click();

            if (Globals.IsElementExist(_driver, By.Id("changepw_error_msg")) ||
                _driver.FindElement(By.Id("changepw_error_msg")).Text == string.Empty)
            {
                Dashboard.GetInstance().TextLogs =
                    $"[{_loginSteam}] Попытка сменить пароль не удалась, скорее всего пароль плохой :(";
                return;
            }

            await WaitLoadPage();
        }
        catch(Exception ex)
        {
            await File.AppendAllTextAsync("log_error.txt", $"[{DateTime.Now}] [{_loginSteam}]\n{ex.Message}");
            Dashboard.GetInstance().TextLogs =
                $"[{_loginSteam}] Попытка сменить пароль не удалась, похоже было много попыток";
            return;
        }

        _passwordSteam = newPassword;

        Dashboard.GetInstance().TextLogs = $"[{_loginSteam}] Завершено, попытка снова войти в аккаунт";

        await Login();
    }
}