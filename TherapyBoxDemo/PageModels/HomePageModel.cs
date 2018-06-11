using System;
using System.IO;
using System.Threading.Tasks;
using FreshMvvm;
using SQLite;
using TherapyBoxDemo.Services;
using Xamarin.Forms;

namespace TherapyBoxDemo.PageModels
{
	public class HomePageModel : FreshBasePageModel
	{
		public HomePageModel()
		{

		}
        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }
        public async Task LoginCommandHandler()
		{
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<LoginTable>(); //Call Table  
                var data1 = data.Where(x => x.username == Username && x.password == Password).FirstOrDefault(); //Linq Query  
                if (data1 != null)
                {
                    await CoreMethods.DisplayAlert("Login", "Login Successful", "Continue");
                    await CoreMethods.PushPageModel<MainPageModel>();
                }
                else
                {
                    await CoreMethods.DisplayAlert("Login", "Login has failed", "Try again");
                }
            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Error", ex.ToString(), "OK");
            }
            ;
		}
        public async Task RegisterCommandHandler()
        {
            await CoreMethods.PushPageModel<RegistrationPageModel>();
        }
        private FreshAwaitCommand _loginCommand;
		public FreshAwaitCommand LoginCommand
		{
			get
			{
				if (_loginCommand == null)
				{
					_loginCommand = new FreshAwaitCommand(async (tcs) =>
					{
						await LoginCommandHandler();
						tcs.SetResult(true);
					});
				}
				return _loginCommand;
			}

		}
        private FreshAwaitCommand _registerCommand;
        public FreshAwaitCommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await RegisterCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _registerCommand;
            }

        }

    }
}


