﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;

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
        
        public void LogIn()
        {
            Console.WriteLine();
        }
    }
}