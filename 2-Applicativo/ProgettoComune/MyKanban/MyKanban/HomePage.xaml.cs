using System.Linq;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;
using Path = System.IO.Path;

namespace MyKanban;

public partial class HomePage : ContentPage
{
    int mese = DateTime.Today.Month;
    int anno = DateTime.Today.Year;
    int? selezionato;// int? significa che la variabile può contenere un numero intero oppure null (nessun valore)

    private static string TaskFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "tasks.txt"); // FileSystem.AppDataDirectory restituisce una cartella privata

    public HomePage()
    {
        InitializeComponent();

        MonthPicker.ItemsSource = new List<string>
        {
            "Jan","Feb","Mar","Apr","May","Jun",
            "Jul","Aug","Sep","Oct","Nov","Dec"
        };

        YearPicker.ItemsSource = // riempe la parte degli anni creando i numeri da 2020 a 2030 11 valori e trasformando ogni numero in stringa e salvandolo in una lista  
            Enumerable.Range(2020, 11) // Enumerable.Range genera una sequenza di numeri. 
            .Select(x => x.ToString())
            .ToList();

        AggiornaPicker();
        DisegnaCalendario();

        // Mostra subito le task di oggi
        selezionato = DateTime.Today.Day;
        MostraTaskDelGiorno();
    }

    void AggiornaPicker() // Aggiorna i picker impostando mese e anno
                          // in base alle variabili attuali del calendario
    {
        MonthPicker.SelectedIndex = mese - 1;
        YearPicker.SelectedItem = anno.ToString();
    }

    void Picker_Changed(object sender, EventArgs e) // questo metodo viene avviato quando si cambia mese o anno e serve per controllare che mese e hanno sia validi aggiona le variabili e ridisegna il calendario
    {
        if (MonthPicker.SelectedIndex == -1 || // SelectedIndex rappresenta la posizione selezionata nel Picker. si sottrae uno per portare gli indici coerenti a quelli di maui  
            YearPicker.SelectedItem == null)
            return;

        mese = MonthPicker.SelectedIndex + 1;
        anno = int.Parse(YearPicker.SelectedItem.ToString());

        DisegnaCalendario();
    }

    void BtnPrev_Clicked(object sender, EventArgs e) // questo metodo serve a far scalare il mese se si schiaccia - se si schiaccia su gennaio torna a dicembre 2025
    {
        mese--;

        if (mese < 1)
        {
            mese = 12;
            anno--;
        }

        AggiornaPicker();
        DisegnaCalendario();
    }

    void BtnNext_Clicked(object sender, EventArgs e) // stessa cosa solo con il + e se si schiaccuia a dicembre va a gennaio 2027
    {
        mese++;

        if (mese > 12)
        {
            mese = 1;
            anno++;
        }

        AggiornaPicker();
        DisegnaCalendario();
    }

    void DisegnaCalendario()
    {
        DaysGrid.Children.Clear(); // Svuota tutte le celle del calendario precedente

        DateTime primo = new DateTime(anno, mese, 1);  // Crea la data del primo giorno del mese selezionato

        int start = (int)primo.DayOfWeek;  // Calcola in che giorno della settimana cade il primo giorno del mese
        int giorni = DateTime.DaysInMonth(anno, mese); // calcola quanti giorni ci nìsono nel mese

        for (int g = 1; g <= giorni; g++) // ciclo for che scorre tutti i giorni e calcola la posizione del primo giorno
        {                                 // Calcola la riga del calendario (ogni 7 giorni si passa alla riga sotto)
            int index = start + g - 1;    // Calcola la colonna del calendario (0 a 6 ? Domenica a Sabato)

            int row = index / 7;
            int col = index % 7;

            bool oggi =   // Variabile che controlla se il giorno corrente è oggi
                g == DateTime.Today.Day &&
                mese == DateTime.Today.Month &&
                anno == DateTime.Today.Year;

            // aggiunge alla griglia tutte le variabili create al posto giusto
            DaysGrid.Add(CreaCella(g, row, col, oggi));
        }
    }

    View CreaCella(int giorno, int row, int col, bool oggi)
    {
        bool selected = selezionato == giorno; // controlla giorno seleyionato = al giorno

        var border = new Border // contenitore grafico della cella del calendario
        {
            WidthRequest = 34,
            HeightRequest = 34,

            StrokeShape = new RoundRectangle // forma cella con angoli arrotondati
            {
                CornerRadius = 20
            },

            BackgroundColor =
                selected ? Colors.Black : //Colore di sfondo: - nero se selezionato - grigio se è oggi - trasparente altrimenti
                oggi ? Colors.DarkGray :
                Colors.Transparent,

            Stroke = Colors.Transparent,

            Content = new Label
            {
                Text = giorno.ToString(),

                TextColor =
                    selected || oggi // Colore del testo: - bianco se selezionato o oggi - nero altrimenti
                    ? Colors.White
                    : Colors.Black,

                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            }
        };

        border.GestureRecognizers.Add(  // Aggiunge un evento di click (tap sulla cella) per rilevare gesti/interazioni dell’utente su un elemento grafico.
            new TapGestureRecognizer // rendere un elemento cliccabile
            {
                Command = new Command(() =>
                {
                    // Quando clicchi un giorno:
                    // lo salva come selezionato
                    selezionato = giorno;
                    DisegnaCalendario();
                    // ? Mostra le task del giorno cliccato
                    MostraTaskDelGiorno();
                })
            });
        // Posiziona la cella nella griglia del calendario
        Grid.SetRow(border, row);
        Grid.SetColumn(border, col);
        // Restituisce la cella creata
        return border;
    }

    // Carica e filtra le task per il giorno selezionato nel calendario
    // Carica e filtra le task per il giorno selezionato nel calendario
    // Carica e filtra le task per il giorno selezionato nel calendario
    // Carica e filtra le task per il giorno selezionato nel calendario
    void MostraTaskDelGiorno()
    {
        if (selezionato == null) return;

        DateTime dataSelezionata = new DateTime(anno, mese, selezionato.Value);

        LblDataSelezionata.Text = $"Attività del {dataSelezionata:dd MMMM yyyy}";

        var daFare = new List<Models.KanbanTask>();
        var inCorso = new List<Models.KanbanTask>();
        var fatte = new List<Models.KanbanTask>();

        if (File.Exists(TaskFilePath))
        {
            var righe = File.ReadAllLines(TaskFilePath);

            foreach (var riga in righe)
            {
                if (string.IsNullOrWhiteSpace(riga)) continue;

                var task = Models.KanbanTask.FromRiga(riga);

                // Controlla lo stato della task ignorando maiuscole e spazi
                string stato = task.statusTask.Trim().ToLower();

                // Calcola la differenza tra scadenza e giorno selezionato
                int giorniDiff = (task.deadline.Date - dataSelezionata.Date).Days;

                // Se il giorno selezionato supera la scadenza
                // la task non viene mostrata in nessuna categoria
                if (giorniDiff < 0)
                    continue;

                // Da fare
                if (stato == "da fare")
                {
                    daFare.Add(task);
                }

                // In corso
                else if (stato == "in corso")
                {
                    inCorso.Add(task);
                }

                // Fatto
                else if (stato == "fatto")
                {
                    fatte.Add(task);
                }
            }
        }

        CvDaFare.ItemsSource = daFare;
        CvInCorso.ItemsSource = inCorso;
        CvFatte.ItemsSource = fatte;

        LblVuotoDaFare.IsVisible = daFare.Count == 0;
        LblVuotoInCorso.IsVisible = inCorso.Count == 0;
        LblVuotoFatte.IsVisible = fatte.Count == 0;
    }

    // Ricarica le task ogni volta che si torna sulla pagina Home
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MostraTaskDelGiorno();
    }
}