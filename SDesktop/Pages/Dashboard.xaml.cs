using SDesktop.Steam;

namespace SDesktop.Pages;

public partial class Dashboard : INotifyPropertyChanged
{
    public Dashboard()
    {
        InitializeComponent();
        DataContext = _instance = this;
    }

    #region Variables

    private static bool _isBusy;

    private static Dashboard _instance;

    #endregion

    #region Variables UI

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    private int _progressValue;

    public int ProgressValue
    {
        get => _progressValue;
        set
        {
            _progressValue = value;
            OnPropertyChanged("ProgressValue");
        }
    }

    private string _textLogs;

    public string TextLogs
    {
        get => _textLogs;
        set
        {
            Dispatcher.Invoke(() =>
            {
                _textLogs += $"{value}\n";
                OnPropertyChanged("TextLogs");
            });
        }
    }

    #endregion

    public static Dashboard GetInstance() => _instance;
    
    private async void StartUpdate(object sender, RoutedEventArgs e)
    {
        var task = new Task(async () =>
        {
            TextLogs = "Запущен процесс";
            var client = new SClient("_tp_k_", "Zz1236547", "neapolitanovaksenii1986@lenta.ru", "Vg3ShaPlJ");
            await client.Login();
            TextLogs = "Авторизация закончена";
            await client.ChangePassword("Zz1236547");
        });
        task.Start();
        ProgressValue = 50;
    }
}