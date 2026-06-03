namespace MyKanban;
using MyKanban.Models;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        btnCreateAccount.Focus();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    //Gestione del click sul pulsante di registrazione per creare un nuovo account
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        //Controllo se i campi nome account e password sono vuoti, se si messaggio di errore
        if (string.IsNullOrEmpty(EntNameNewAccount.Text) ||
            string.IsNullOrEmpty(EntPasswordNewAccount.Text))
        {
            await DisplayAlert("Errore", "Nome account o Password non inserito", "Ok");
            return;
        }

        //Costruzione del percorso del file dell'account basato sul nome account inserito
        string filePath = $"{Path.Combine(FileSystem.AppDataDirectory, EntNameNewAccount.Text)}.txt";
        //Controllo se il file dell'account esiste già, se si messaggio di errore
        if (File.Exists(filePath))
        {
            await DisplayAlert("Errore", "Account già esistente", "Ok");
            return;
        }

        //Prova a creare un nuovo account, scrive il nuovo account su un nuovo file e naviga alla pagina principale dell'app, altrimenti mostra un messaggio di errore
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