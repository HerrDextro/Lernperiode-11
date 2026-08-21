namespace MorseMate_Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //registering route of my new page
            Routing.RegisterRoute(nameof(PracticePage), typeof(PracticePage));
        }
    }
}
