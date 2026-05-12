namespace MyKanban;
using MyKanban.Models;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntNameNewAccount.Text) ||
            string.IsNullOrEmpty(EntPasswordNewAccount.Text))
        {
            await DisplayAlert("Errore", "Nome account o Password non inserito", "Ok");
            return;
        }

        string filePath = $"{Path.Combine(FileSystem.AppDataDirectory, EntNameNewAccount.Text)}.txt";
        if (File.Exists(filePath))
        {
            await DisplayAlert("Errore", "Account già esistente", "Ok");
            return;
        }

        try
        {
            Account account = new Account
            {
                Username = EntNameNewAccount.Text,
                Password = EntPasswordNewAccount.Text,
            };

            
            File.AppendAllText(filePath, $"{account.ToRiga()}{Environment.NewLine}");
            await DisplayAlert("Successo", "Account effettuato con successo", "Ok");
            await Navigation.PushAsync(new KanbanPage());
        }
        catch (Exception)
        {
            await DisplayAlert("Errore", "Compilari tutti i campi.", "Ok");
        }
    }
}