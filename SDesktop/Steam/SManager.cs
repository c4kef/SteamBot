namespace SDesktop.Steam;

public class SManager
{
    private readonly List<string> _busyLogins;
    private readonly List<Task> _tasks;

    public SManager()
    {
        _tasks = new List<Task>();
        _busyLogins = new List<string>();
    }

    public async Task Start()
    {
        for (var i = 0; i < Globals.Setup.CountThread; i++)
        {
            var task = Handler();
            
            await Task.Delay(1_000);
            
            _tasks.Add(task);
        }

        Task.WaitAll(_tasks.ToArray(), -1);
    }

    private async Task Handler()
    {
        while (true)
        {
            var freeAccount = await Globals.GetAccountSteam(_busyLogins.ToArray());
            
            if (freeAccount.login == string.Empty)
                break;
            
            Dashboard.GetInstance().TextLogs = $"[{freeAccount.login}] - Вход";

            _busyLogins.Add(freeAccount.login);
            
            var client = new SClient(freeAccount.login, freeAccount.password, freeAccount.email, freeAccount.ePassword);

            if (!await client.Login())
                continue;

            var id = client.Id;
            
            var (username, realName) = (string.Empty, string.Empty);

            if (Globals.Setup.GenNameFromFile)
            {
               var usrProfile = await Globals.GetUsername();

               username = usrProfile.username;
               realName = usrProfile.realName;
            }
            
            Dashboard.GetInstance().TextLogs = $"[{freeAccount.login}] - Установка основных данных об аккаунте";
            
            client.UserName = (Globals.Setup.GenNameFromFile) ? username : Globals.RandomString(10);
            client.RealUserName = (Globals.Setup.GenNameFromFile) ? realName : Globals.RandomString(10);
            
            client.UrlProfile = Globals.RandomString(10);

            client.CountryIndex = new Random().Next(1, 10);

            client.About = Globals.Setup.About;

            client.UserPicture = Globals.GetPictureAccount();

            client.AccessToComments = "Скрытый";

            Dashboard.GetInstance().TextLogs = $"[{freeAccount.login}] - Смена пароля";

            var newPassword = Globals.RandomString(10);

            await client.ChangePassword(newPassword);
            
            Dashboard.GetInstance().TextLogs = $"[{freeAccount.login}] - Смена почты";

            await client.ChangeEmail(freeAccount.eUsernameNew, freeAccount.ePasswordNew);
            
            await File.WriteAllTextAsync($@"{Globals.NameFolderSetupAccounts}\{freeAccount.login}.txt", 
                $"Login: {freeAccount.login}\nPassword: {newPassword}\nID64: {id}\nELogin: {freeAccount.eUsernameNew}\nEPassword: {freeAccount.ePasswordNew}");

            Dashboard.GetInstance().TextLogs = $"[{freeAccount.login}] - Закончено";
        }
    }
}