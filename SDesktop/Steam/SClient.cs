namespace SDesktop.Steam;

public class SClient
{
    private readonly ChromeDriver _driver;
    private string _id = null!;

    public SClient()
    {
        var options = new ChromeOptions();

        var service = ChromeDriverService.CreateDefaultService();

        service.HideCommandPromptWindow = true;
        options.AddArgument("no-sandbox");
        options.AddArgument("remote-debugging-port=0");
        options.AddArgument("disable-extensions");
        options.AddArgument(
            "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36");
        options.AddArgument("start-maximized");

        _driver = new ChromeDriver(service, options);
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
    }//*[@id="application_root"]/div[2]/div[2]/form/div[5]/div[2]/textarea

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

    #endregion

    public async Task Login(string login, string password)
    {
        _driver.Navigate().GoToUrl("https://steamcommunity.com/login/");

        _driver.FindElement(By.Id("input_username")).SendKeys(login);
        _driver.FindElement(By.Id("input_password")).SendKeys(password);
        _driver.FindElement(By.XPath("//button[@class='btn_blue_steamui btn_medium login_btn']")).Click();

        var timeFrom = DateTimeOffset.Now;

        await Task.Delay(1_500);

        if (Globals.IsElementExist(_driver, By.Id("authcode")))
        {
            _driver.FindElement(By.Id("authcode")).SendKeys(await Globals.GetMailCode(timeFrom));
            _driver.FindElement(By.XPath("//*[@id='auth_buttonset_entercode']/div[1]")).Click();
            await Task.Delay(1_500);
            _driver.FindElement(By.Id("success_continue_btn")).Click();
        }

        _id = _driver.Url.Split('/')[4];
    }
    
    public async Task ChangeUserName(string personaName, string realName, int countryIndex)
    {
        UserName = "Makarinov";
        
        RealUserName = "Artemiy";
        
        UrlProfile = Globals.RandomString(15);
        
        Country = "United States";

        MessageBox.Show(About);
        About = "Hello world!";
        MessageBox.Show(About);
    }
}