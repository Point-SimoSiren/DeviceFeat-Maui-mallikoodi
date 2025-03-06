namespace DeviceFeat_Maui
{
    public partial class MainPage : TabbedPage
    {

        // Muistion tallennuspaikan alustaminen muuttujaksi
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
            .LocalApplicationData), "muistio.txt");

        string text = "";


        // Konstruktorimetodi joka suoritetaan kun sivu latautuu
        public MainPage()
        {
            // Ladataan xaml
            InitializeComponent();

            // Asetusarvot

            // Sliderin arvoasteikko
            vahvistusKytkin.Minimum = 0;
            vahvistusKytkin.Maximum = 100;


            // Onko tiedostoa ennestään olemassa
            bool doesExist = File.Exists(fileName);

            if (doesExist == true)
            {
                text = File.ReadAllText(fileName);
                if (text.Length > 0)
                {
                    outputLabel.Text = text;
                }
                else
                {
                    outputLabel.Text = "Mitään ei ole talletettu muistioon.";
                }

            }
            else
            {
                outputLabel.Text = "Tervetuloa uudelle käyttäjälle!";
            }

        }

        // -----x-----x----- MUISTION TOIMINNOT ----x-------x-------x---------

        // Muistiinpanon tallentaminen
        private void tallennusNappi_Clicked(object sender, EventArgs e)
        {
            text = text + Environment.NewLine + inputKentta.Text;
            File.WriteAllText(fileName, text);
            outputLabel.Text = text;
            inputKentta.Text = "";
        }

        private void poistoNappi_Clicked(object sender, EventArgs e)
        {
            poistoNappi.IsVisible = false;
            vahvistusInfo.IsVisible = true;
            vahvistusKytkin.IsVisible = true;
        }

        private void vahvistusKytkin_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (vahvistusKytkin.Value > 90)
            {
                //Vibration.Vibrate();
                vahvistusKytkin.Value = 0;
                text = "";
                outputLabel.Text = "";
                vahvistusKytkin.IsVisible = false;
                vahvistusInfo.IsVisible = false;
                poistoNappi.IsVisible = true;
            }
        }


        // -----x-----x----- LAMPPU ----x-------x-------x---------


        private async void valo_nappi_on_Clicked(object sender, EventArgs e)
        {
            if (Battery.ChargeLevel < 0.05)
            {
                Vibration.Vibrate();
                await DisplayAlert("Akku vähissä", "En halua käyttää akkuasi loppuun", "ok");
            }
            else
            {
                await Flashlight.Default.TurnOnAsync();
                valo_nappi_off.IsVisible = true;
                valo_nappi_on.IsVisible = false;
            }
        }

        private async void valo_nappi_off_Clicked(object sender, EventArgs e)
        {
            await Flashlight.Default.TurnOffAsync();
            valo_nappi_off.IsVisible = false;
            valo_nappi_on.IsVisible = true;
        }


        // ---x-------x--- BAROMETRI KOKEILU ----x-------x-----------x---

        private void BarometerButton_Clicked(object sender, EventArgs e)
        
        {

            if (Barometer.Default.IsSupported)
            {
                Barometer.Default.ReadingChanged += (s, e) =>
                {
                    // Haetaan ilmanpaine hPa-yksikössä
                    double pressure = e.Reading.PressureInHectopascals;

                    // Näytetään lukema Label-komponentissa
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        BarometerLabel.Text = $"Ilmanpaine: {pressure} hPa";
                    });
                };

                // Käynnistetään barometrin lukeminen
                try
                {
                    Barometer.Default.Start(SensorSpeed.UI);
                }
                catch (Exception ex)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        BarometerLabel.Text = $"Virhe: {ex.Message}";
                    });
                }
            }
            else
            {
                BarometerLabel.Text = "Barometri ei ole tuettu tällä laitteella.";
            }
    }

       

        
    }

}
