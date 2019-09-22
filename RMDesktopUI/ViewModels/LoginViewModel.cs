using Caliburn.Micro;
using RM.WPF.Library.Api;
using System;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IApiHelper _apiHelper;
        private string _errorMessage;

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn); //needs to be here because of PasswordBox specificity
            }
        }
        public bool CanLogIn
        {
            get
            {
                var output = false;

                if (UserName?.Length > 0 && Password?.Length > 3)
                    output = true;

                return output;
            }
        }
        public bool IsErrorVisible => ErrorMessage?.Length > 0;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }


        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var authUser = await _apiHelper.Authenticate(UserName, Password);

                //gets user info a saves it as singleton
                await _apiHelper.GetLoggedInUserInfo(authUser.Access_Token);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
