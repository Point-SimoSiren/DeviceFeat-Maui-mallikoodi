namespace DeviceFeat_Maui
{
    public partial class MainPage : TabbedPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

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

    }

}
