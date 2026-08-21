namespace MorseMate_Mobile;

public partial class Translate : ContentPage
{
	public Translate()
	{
		InitializeComponent();

        BindingContext = new MorseTextTranslator();

    }
}