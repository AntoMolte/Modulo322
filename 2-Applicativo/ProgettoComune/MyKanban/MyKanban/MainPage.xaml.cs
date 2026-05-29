using MyKanban.Models;

namespace MyKanban
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        // Ovveride del metodo OnAppearing per mettere il focus sul campo del nome account quando la pagina viene visualizzata
        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnLoginAccount.Focus();
        }

        // Gestione del click sul pulsante di login
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            //Controllo se i campi nome account e password sono vuoti, se si messaggio di errore
            if (string.IsNullOrEmpty(EntNameAccount.Text) ||
                string.IsNullOrEmpty(EntPasswordAccount.Text))
            {
                await DisplayAlert("Errore", "Nome account o Password non inserito", "Ok");
                return;
            }

            //Costruzione del percorso del file dell'account basato sul nome account inserito
            string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{EntNameAccount.Text}.txt");

            //Controllo se il file dell'account esiste, se no messaggio di errore
            if (!File.Exists(filePath))
            {
                await DisplayAlert("Errore", "Account non esistente", "Ok");
                return;
            }

            //Prova a leggere il file per verificare se nome e password sono giusti o meno
            try
            {
                string[] righe = File.ReadAllLines(filePath);
                bool passwordCorretta = false;
                foreach (var riga in righe)
                {
                    var parti = riga.Split(';');
                    if (parti.Length >= 2)
                    {
                        string username = parti[0];
                        string password = parti[1];
                        if (username == EntNameAccount.Text &&
                            password == EntPasswordAccount.Text)
                        {
                            passwordCorretta = true;
                            break;
                        }
                    }
                }

                //se la password è corretta naviga alla pagina principale dell'app, altrimenti mostra un messaggio di errore
                if (passwordCorretta)
                {
                    await DisplayAlert("Successo", "Login effettuato con successo", "Ok");

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await DisplayAlert("Errore", "Password errata", "Ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Errore", "Errore durante il login", "Ok");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
