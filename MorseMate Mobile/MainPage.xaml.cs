namespace MorseMate_Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {

        }
        private async void OnOpenSettingsBtnClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Settings));
        }

        private async void OnOpenTranslateBtnClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Translate));
        }
    }
}
