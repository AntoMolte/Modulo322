using MyKanban.Models;
using System.Collections.ObjectModel;

namespace MyKanban;

public partial class KanbanPage : ContentPage
{
    private static string TaskFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "tasks.txt");

    private ObservableCollection<KanbanTask> daFare = null;
    private ObservableCollection<KanbanTask> inCorso = null;
    private ObservableCollection<KanbanTask> fatto = null;

    public KanbanPage()
    {
        InitializeComponent();
        CaricaTask();
    }


    private void CaricaTask()
    {
        daFare = new ObservableCollection<KanbanTask>();
        inCorso = new ObservableCollection<KanbanTask>();
        fatto = new ObservableCollection<KanbanTask>();

        if (File.Exists(TaskFilePath))
        {
            var righe = File.ReadAllLines(TaskFilePath);
            foreach (var riga in righe)
            {
                if (string.IsNullOrWhiteSpace(riga)) { continue; }

                var task = Models.KanbanTask.FromRiga(riga);

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

    private async void OnTaskTapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame;

        if (frame?.BindingContext is KanbanTask task)
        {
            await Navigation.PushAsync(new TaskDetailPage(task));
        }
    }

    private void SalvaTask()
    {
        List<string> righe = new();

        foreach (var task in daFare)
            righe.Add(task.ToRiga());

        foreach (var task in inCorso)
            righe.Add(task.ToRiga());

        foreach (var task in fatto)
            righe.Add(task.ToRiga());

        File.WriteAllLines(TaskFilePath, righe);
    }

    private KanbanTask taskTrascinata;

    #region Drag Starting

    private void ToDoDragStarting(object sender, DragStartingEventArgs e)
    {
        var frame = sender as DragGestureRecognizer;

        taskTrascinata = frame.BindingContext as KanbanTask;

        if (taskTrascinata != null)
            e.Data.Text = taskTrascinata.ToString();
    }

    private void OnProgressDragStarting(object sender, DragStartingEventArgs e)
    {
        var frame = sender as DragGestureRecognizer;

        taskTrascinata = frame.BindingContext as KanbanTask;

        if (taskTrascinata != null)
            e.Data.Text = taskTrascinata.ToString();
    }

    private void OnFinishDragStarting(object sender, DragStartingEventArgs e)
    {
        var frame = sender as DragGestureRecognizer;

        taskTrascinata = frame.BindingContext as KanbanTask;

        if (taskTrascinata != null)
            e.Data.Text = taskTrascinata.ToString();
    }

    #endregion

    #region Drop

    private void ToDoDrop(object sender, DropEventArgs e)
    {
        if (taskTrascinata == null)
            return;

        daFare.Remove(taskTrascinata);
        inCorso.Remove(taskTrascinata);
        fatto.Remove(taskTrascinata);

        taskTrascinata.statusTask = "Da fare";

        daFare.Add(taskTrascinata);
        SalvaTask();

        taskTrascinata = null;
    }

    private void OnProgressDrop(object sender, DropEventArgs e)
    {
        if (taskTrascinata == null)
            return;

        daFare.Remove(taskTrascinata);
        inCorso.Remove(taskTrascinata);
        fatto.Remove(taskTrascinata);

        taskTrascinata.statusTask = "In corso";

        inCorso.Add(taskTrascinata);
        SalvaTask();

        taskTrascinata = null;
    }

    private void OnFinishDrop(object sender, DropEventArgs e)
    {
        if (taskTrascinata == null)
            return;

        daFare.Remove(taskTrascinata);
        inCorso.Remove(taskTrascinata);
        fatto.Remove(taskTrascinata);

        taskTrascinata.statusTask = "Fatto";

        fatto.Add(taskTrascinata);
        SalvaTask();

        taskTrascinata = null;
    }


    #endregion
}