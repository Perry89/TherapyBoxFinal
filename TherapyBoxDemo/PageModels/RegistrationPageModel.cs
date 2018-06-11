using System;
using System.IO;
using System.Threading.Tasks;
using FreshMvvm;
using SQLite;
using TherapyBoxDemo.Services;
using Xamarin.Forms;

namespace TherapyBoxDemo.PageModels
{
	public class RegistrationPageModel : FreshBasePageModel
    {
        public RegistrationPageModel()
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
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged("ConfirmPassword");
            }
        }



        public async Task RegisterCommandHandler()
        {
            try
            {
                if(Password == ConfirmPassword)
                {
                    string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                    var db = new SQLiteConnection(dpPath);
                    db.CreateTable<LoginTable>();
                    LoginTable tbl = new LoginTable();
                    tbl.username = Username;
                    tbl.password = Password;
                    tbl.email = Email;
                    db.Insert(tbl);
                    await CoreMethods.DisplayAlert("Registration", "Username sucessfully created", "OK");
                    await CoreMethods.PushPageModel<HomePageModel>();
                }
                else
                {
                    await CoreMethods.DisplayAlert("Registration", "Entered Password does not match", "Try again");
                }

            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Registration", ex.ToString(), "OK");
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

