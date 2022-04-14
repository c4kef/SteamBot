﻿namespace SDesktop.Pages;

public partial class Dashboard : INotifyPropertyChanged
{
    public Dashboard()
    {
        InitializeComponent();
        DataContext = this;
    }

    #region Variables

    private static bool _isBusy;

    #endregion
    
    #region Variables UI
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

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
            _textLogs = value;
            OnPropertyChanged("TextLogs");
        }
    }
    
    #endregion

    private async void StartUpdate(object sender, RoutedEventArgs e)
    {
        ProgressValue = 50;
    }
}