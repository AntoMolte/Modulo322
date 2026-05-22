using MyKanban.Models;

namespace MyKanban;

public partial class TaskDetailPage : ContentPage
{
    private KanbanTask currentTask;

    public TaskDetailPage(KanbanTask task)
    {
        InitializeComponent();

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

        await DisplayAlert("Salvato", "Modifiche salvate correttamente", "OK");

        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool conferma = await DisplayAlert(
            "Elimina",
            "Vuoi eliminare questa attività?",
            "Si",
            "No");

        if (!conferma)
            return;

        // opzionale:
        // qui potrai eliminare la task dal file

        await Navigation.PopAsync();
    }
}