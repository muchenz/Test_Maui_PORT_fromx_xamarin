using Test_MauiApp1.ViewModels;

namespace Test_MauiApp1.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        BindingContext = loginViewModel;
        loginViewModel.Navigation = Navigation;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Preferences.Default.ContainsKey("UserName"))
            ((LoginViewModel)BindingContext).Model.UserName = Preferences.Default.Get("UserName","");
        if (Preferences.Default.ContainsKey("Password"))
            ((LoginViewModel)BindingContext).Model.Password = Preferences.Default.Get("Password","");
    }
}
