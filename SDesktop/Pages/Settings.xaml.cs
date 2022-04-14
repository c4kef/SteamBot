using System.Windows.Controls;

namespace SDesktop.Pages;

public partial class Settings : INotifyPropertyChanged
{
    public Settings()
    {
        InitializeComponent();
        DataContext = this;
    }

    #region Variables
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    public int CountThread
    {
        get => Globals.Setup.CountThread;
        set => Globals.Setup.CountThread = value;
    }
    
    public string ApiKey
    {
        get => Globals.Setup.ApiKey;
        set => Globals.Setup.ApiKey = value;
    }
    
    public bool GenNameFromFile
    {
        get => Globals.Setup.GenNameFromFile;
        set => Globals.Setup.GenNameFromFile = value;
    }
    
    public string About
    {
        get => Globals.Setup.About;
        set => Globals.Setup.About = value;
    }
    
    public Brush ColorPathToProxy =>
        (string.IsNullOrEmpty(Globals.Setup.PathToProxy) ||
         !File.Exists(Globals.Setup.PathToProxy))
            ? Brushes.Red
            : Brushes.GreenYellow;
    
    public Brush ColorPathToAccounts =>
        (string.IsNullOrEmpty(Globals.Setup.PathToAccounts) ||
         !File.Exists(Globals.Setup.PathToAccounts))
            ? Brushes.Red
            : Brushes.GreenYellow;
    
    public Brush ColorPathToGenNameFile =>
        (string.IsNullOrEmpty(Globals.Setup.PathToGenNameFile) ||
         !File.Exists(Globals.Setup.PathToGenNameFile))
            ? Brushes.Red
            : Brushes.GreenYellow;

    #endregion

    private async void SelectProxy(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.Filters.Add(new CommonFileDialogFilter("Файл с прокси", ".txt"));
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            if ((await File.ReadAllLinesAsync(dialog.FileName)).Any(line => line.Split(',').Length != 3))
            {
                MessageBox.Show("Выбранный вами файл не валиден");
                return;
            }
            
            Globals.Setup.PathToProxy = dialog.FileName;
            await Globals.SaveSetup();
        }

        OnPropertyChanged("ColorPathToProxy");
    }
    
    private async void SelectNames(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.Filters.Add(new CommonFileDialogFilter("Файл с именами", ".txt"));
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            if ((await File.ReadAllLinesAsync(dialog.FileName)).Any(line => line.Split(',').Length != 2))
            {
                MessageBox.Show("Выбранный вами файл не валиден");
                return;
            }
            
            Globals.Setup.PathToGenNameFile = dialog.FileName;
            await Globals.SaveSetup();
        }

        OnPropertyChanged("ColorPathToGenNameFile");
    }
    
    private async void SelectAccounts(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.Filters.Add(new CommonFileDialogFilter("Файл с аккаунтами", ".txt"));
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            if ((await File.ReadAllLinesAsync(dialog.FileName)).Any(line => line.Split(',').Length != 7))
            {
                MessageBox.Show("Выбранный вами файл не валиден");
                return;
            }
            
            Globals.Setup.PathToAccounts = dialog.FileName;
            await Globals.SaveSetup();
        }

        OnPropertyChanged("ColorPathToAccounts");
    }
    
    private async void GenNameFromFileClicked(object sender, RoutedEventArgs e)
    {
        GenNameFromFile = ((sender as CheckBox)!).IsChecked!.Value;
        OnPropertyChanged("GenNameFromFile");

        await Globals.SaveSetup();
    }
    
    private async void ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args) => await Globals.SaveSetup();
    
    private async void TextChanged(object sender, TextChangedEventArgs e) => await Globals.SaveSetup();
}