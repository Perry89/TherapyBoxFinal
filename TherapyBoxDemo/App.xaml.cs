using System;
using Xamarin.Forms;
using TherapyBoxDemo.Services;
using TherapyBoxDemo.Views;
using Xamarin.Forms.Xaml;
using TherapyBoxDemo.PageModels;
using FreshMvvm;
using System.IO;
using SQLite;
using TherapyBoxDemo.Interface;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TherapyBoxDemo
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App()
        {
            InitializeComponent();

			var page = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
			var navContainer = new FreshNavigationContainer(page);
			MainPage = navContainer;
        }
        public string CreateDB()
        {
            var output = "";
            output += "Creating Databse if it doesnt exists";
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Create New Database  
            var db = new SQLiteConnection(dpPath);
            output += "\n Database Created....";
            return output;
        }

        protected override void OnStart()
        {
            CreateDB();
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
