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
                if (value != string.Empty)
                    _textLogs += $"{value}\n";
                else
                    _textLogs = value;
                
                OnPropertyChanged("TextLogs");
            });
        }
    }

    #endregion

    public static Dashboard GetInstance() => _instance;
    
    private async void StartUpdate(object sender, RoutedEventArgs e)
    {
        if (_isBusy)
            return;
        
        if (!File.Exists(Globals.Setup.PathToAccounts) || !File.Exists(Globals.Setup.PathToImages))
        {
            MessageBox.Show("Вы указали не все важные настройки");
            return;
        }

        _isBusy = true;
        
        TextLogs = string.Empty;

        ProgressValue = 100;
        
        var manager = new SManager();

        await manager.Start();

        ProgressValue = 0;
        _isBusy = false;
    }
}