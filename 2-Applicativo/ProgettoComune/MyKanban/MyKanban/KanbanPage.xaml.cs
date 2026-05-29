using MyKanban.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyKanban;

public partial class KanbanPage : ContentPage
{
    private static string TaskFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "tasks.txt");

    //ObservableCollection per ogni colonna del Kanban, permette di aggiornare automaticamente la UI quando viene modificata la collezione
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

        //Controlla se esiste il file delle attività e carica le attività nelle rispettive collezioni in base allo stato
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

    //Gestisce il tap su una task, naviga alla pagina di dettaglio della task selezionata
    private async void OnTaskTapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame;

        if (frame?.BindingContext is KanbanTask task)
        {
            await Navigation.PushAsync(new TaskDetailPage(this, task));
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

    public void UpdateTask(KanbanTask currentTask)
    {
        ObservableCollection<KanbanTask>? lista = null;

        if (daFare.Contains(currentTask))
            lista = daFare;
        else if (inCorso.Contains(currentTask))
            lista = inCorso;
        else if (fatto.Contains(currentTask))
            lista = fatto;

        if (lista == null)
            return;

        int index = lista.IndexOf(currentTask);

        lista.RemoveAt(index);
        lista.Insert(index, currentTask);

        SalvaTask();
    }

    public void DeleteTask(KanbanTask currentTask)
    {
        if (currentTask == null)
        {
            return;
        }
        daFare.Remove(currentTask);
        inCorso.Remove(currentTask);
        fatto.Remove(currentTask);

        SalvaTask();
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