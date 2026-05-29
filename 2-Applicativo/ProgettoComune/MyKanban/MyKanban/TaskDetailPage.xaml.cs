using MyKanban.Models;
using System.Threading.Tasks;

namespace MyKanban;

public partial class TaskDetailPage : ContentPage
{
    private KanbanTask currentTask;
    private KanbanPage _kanbanPage;


    public TaskDetailPage(KanbanPage kanbanPage, KanbanTask task)
    {
        InitializeComponent();

        _kanbanPage = kanbanPage;

        currentTask = task;

        CaricaDatiTask();
    }

    private void CaricaDatiTask()
    {
        EntTitle.Text = currentTask.Title;
        EntDescription.Text = currentTask.description;
        EntUnderTask.Text = currentTask.underTask;
        PickStatus.SelectedItem = currentTask.statusTask;
        PickPriority.SelectedItem = currentTask.priorityTask;
        PickDate.Date = currentTask.DueDate;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        currentTask.Title = EntTitle.Text;
        currentTask.description = EntDescription.Text;
        currentTask.underTask = EntUnderTask.Text;
        currentTask.statusTask = PickStatus.SelectedItem?.ToString();
        currentTask.priorityTask = PickPriority.SelectedItem?.ToString();
        currentTask.deadline = PickDate.Date;

        _kanbanPage?.UpdateTask(currentTask);

        await DisplayAlert("Salvato", "Modifiche salvate correttamente", "OK");

        await Navigation.PushAsync(new KanbanPage());
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool conferma = await DisplayAlert("Elimina", "Vuoi eliminare questa attività?", "Si", "No");

        if (!conferma)
            return;

        _kanbanPage?.DeleteTask(currentTask);

        await DisplayAlert("Completato", "Attività eliminata correttamente", "OK");

        await Navigation.PushAsync(new KanbanPage());
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new KanbanPage());
    }
}
