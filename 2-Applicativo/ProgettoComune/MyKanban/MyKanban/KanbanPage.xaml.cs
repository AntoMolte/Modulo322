using System.Collections.ObjectModel;

namespace MyKanban;

public partial class KanbanPage : ContentPage
{
    public KanbanPage()
    {
        InitializeComponent();
    }
 
    // Viene chiamato ogni volta che la pagina appare
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CaricaTask();
    }
 
    private void CaricaTask()
    {
        // Legge tutti i file .txt salvati
        var cartella = FileSystem.AppDataDirectory;
        var files = Directory.GetFiles(cartella, "*.txt");
 
        var daFare    = new List<Models.Task>();
        var inCorso   = new List<Models.Task>();
        var fatto     = new List<Models.Task>();
 
        foreach (var file in files)
        {
            var righe = File.ReadAllLines(file);
            foreach (var riga in righe)
            {
                if (string.IsNullOrWhiteSpace(riga)) continue;
 
                var task = Models.Task.FromRiga(riga);
 
                // Smista il task nella colonna giusta in base allo stato
                if (task.statusTask == "Da fare")
                    daFare.Add(task);
                else if (task.statusTask == "In corso")
                    inCorso.Add(task);
                else if (task.statusTask == "Fatto")
                    fatto.Add(task);
            }
        }
 
        // Assegna le liste alle 3 CollectionView dello XAML
        ToDoCollectionView.ItemsSource       = daFare;
        OnProgressCollectionView.ItemsSource = inCorso;
        OnFinishCollectionView.ItemsSource   = fatto;
    }
 
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateTaskPage());
    }
}