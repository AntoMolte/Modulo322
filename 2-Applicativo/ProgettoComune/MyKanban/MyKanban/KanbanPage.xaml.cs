namespace MyKanban;

public partial class KanbanPage : ContentPage
{
    private static string TaskFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "tasks.txt");

    public KanbanPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CaricaTask();
    }

    private void CaricaTask()
    {
        var daFare = new List<Models.Task>();
        var inCorso = new List<Models.Task>();
        var fatto = new List<Models.Task>();

        if (File.Exists(TaskFilePath))
        {
            var righe = File.ReadAllLines(TaskFilePath);
            foreach (var riga in righe)
            {
                if (string.IsNullOrWhiteSpace(riga)) { continue; }

                var task = Models.Task.FromRiga(riga);

                if (task.statusTask == "Da fare") { daFare.Add(task); }
                else if (task.statusTask == "In corso") { inCorso.Add(task); }
                else if (task.statusTask == "Fatto") { fatto.Add(task); }
            }
        }

        ToDoCollectionView.ItemsSource = daFare;
        OnProgressCollectionView.ItemsSource = inCorso;
        OnFinishCollectionView.ItemsSource = fatto;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateTaskPage());
    }
}