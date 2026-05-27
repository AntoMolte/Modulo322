using MyKanban.Models;

namespace MyKanban
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnLoginAccount.Focus();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntNameAccount.Text) ||
                string.IsNullOrEmpty(EntPasswordAccount.Text))
            {
                await DisplayAlert("Errore", "Nome account o Password non inserito", "Ok");
                return;
            }

            string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{EntNameAccount.Text}.txt");

            if (!File.Exists(filePath))
            {
                await DisplayAlert("Errore", "Account non esistente", "Ok");
                return;
            }

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
