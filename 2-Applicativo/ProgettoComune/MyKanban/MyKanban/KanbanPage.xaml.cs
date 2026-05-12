namespace MyKanban;

public partial class KanbanPage : ContentPage
{
	public KanbanPage()
	{
		InitializeComponent();
	}
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateTaskPage());
    }
}