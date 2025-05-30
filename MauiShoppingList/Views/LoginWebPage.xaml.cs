using System;
using System.Web;
using Test_MauiApp1.ViewModels;

namespace Test_MauiApp1.Views;

public partial class LoginWebPage : ContentPage
{
	public LoginWebPage(LoginWebViewModel loginWebViewModel)
	{
		InitializeComponent();

        BindingContext = loginWebViewModel;
        loginWebViewModel.Navigation = Navigation;
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {

        var token = string.Empty;

        if (e.Url.Contains("#access_token="))
        {
            token = e.Url.Split("#access_token=")[1].Split("&")[0];

        }


        if (!string.IsNullOrEmpty(token))
        {
            await ((LoginWebViewModel)BindingContext).ObtainedAccessTokenAsync(token);

        }

        //if (GetParameter(e.Url, "access_token") != null)
        //{


        //    await ((LoginWebViewModel)BindingContext).ObtainedAccessTokenAsync(GetParameter(e.Url, "access_token"));
        //    //Navigation.RemovePage(this);
        //}
    }

    public static string GetParameter(string urlString, string param)
    {
        var uri = "";
        if (urlString.EndsWith("#_=_"))
            uri = urlString.Replace("#_=_", "");
        else
            uri = urlString;

        var index = uri.IndexOf("?");

        if (index == -1) return null;

        if (uri[index + 1] == '#')
            uri = uri.Substring(index + 2);
        else
            uri = uri.Substring(index + 1);

        var arrayOfParams = uri.Split("&");

        var dic = new Dictionary<string, string>();

        foreach (var item in arrayOfParams)
        {
            var p = item.Split("=");

            if (p.Length > 1)
                dic.Add(HttpUtility.UrlDecode(p[0]), HttpUtility.UrlDecode(p[1]));
        }

        if (!dic.ContainsKey(param))
            return null;

        return dic[param];
    }


}

