namespace MyKanban;

public partial class CreateTaskPage : ContentPage
{
	public CreateTaskPage()
	{
		InitializeComponent();
	}

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntTitle.Text))
        {
            await DisplayAlert("Errore", "Inserisci il titolo", "OK");
            return;
        }

        if (PickStatus.SelectedIndex == -1)
        {
            await DisplayAlert("Errore", "Seleziona uno stato", "OK");
            return;
        }

        if (PickPriority.SelectedIndex == -1)
        {
            await DisplayAlert("Errore", "Seleziona una priorità", "OK");
            return;
        }
        if (PickDate.Date < DateTime.Today)
        {
            await DisplayAlert("Errore", "La data di scadenza non può essere precedente a oggi", "OK");
            return;
        }

        string path = Path.Combine(FileSystem.AppDataDirectory, "tasks.txt");

        Models.Task newTask = new Models.Task()
        {
            Title = EntTitle.Text,
            statusTask = PickStatus.SelectedItem.ToString(),
            priorityTask = PickPriority.SelectedItem.ToString(),
            description = EntDescription.Text,
            underTask = EntUnderTask.Text,
            deadline = PickDate.Date
        };

        await File.AppendAllTextAsync(path, Environment.NewLine);
        await DisplayAlert("Successo", "Task salvata", "OK");

        await Navigation.PushAsync(new KanbanPage());

        EntTitle.Text = "";
        EntDescription.Text = "";
        EntUnderTask.Text = "";
        PickStatus.SelectedIndex = -1;
        PickPriority.SelectedIndex = -1;
        PickDate.Date = DateTime.Today;
    }

    private async void OnAnnulaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new KanbanPage());
    }
}
