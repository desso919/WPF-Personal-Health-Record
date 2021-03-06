﻿using Hospital.Models;
using NInjectConfigProject;
using Personal.Health.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ninject;
using Personal.Health.Care.DesktopApp.Utills;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Security;
using Personal.Health.Models;

namespace Personal.Health.Care.DesktopApp.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RegistrationFormModel registrationFormModel;
        private ICommand addPatientCommand;
        private IPatientService service;
        private SecureString password;
        private Patient patient;
        
        public RegistrationViewModel()
        {
            patient = new Patient();          
            service = NinjectConfig.Container.Get<IPatientService>();
            addPatientCommand = new RelayCommand(RegisterPatient);
            registrationFormModel = RegistrationFormModel.GetInstance();
        }

        #region Properties

        public Patient Patient
        {
            get { return patient; }
            set { patient = value; NotifyPropertyChanged(); }
        }

        public ICommand AddPatientCommand
        {
            get { return addPatientCommand; }
            set { addPatientCommand = value; }
        }


        public RegistrationFormModel RegistrationFormModel
        {
            get { return registrationFormModel; }
            set { registrationFormModel = value; NotifyPropertyChanged(); }
        }

        public SecureString Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }
        }

        #endregion

        #region Register Patient Code

        public void RegisterPatient(object obj)
        {
            Boolean isAdded = false;

            if (isValidPatient())
            {
                Patient.Password = SecurityUtil.HashPassword(Password);
                Patient.Age = Utills.Utill.GetAge(Patient.BirhtDate);
                isAdded = service.RegisterUser(Patient);

                if (isAdded)
                {
                    if (Patient != null)
                    {
                        var loginWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                        if (loginWindow != null)
                        {
                            LoggedInPatient.Init(Patient);
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            loginWindow.Close();
                        }
                        else
                        {
                            Messenger.ShowMessage(" Something went wrong! ");
                        }
                    }
                }
                else
                {
                    Messenger.ShowMessage(" Error while trying to register you. ");
                }
            }                  
        }

        private bool isValidPatient()
        {
            RegistrationFormModel.clearFormMessages();
            if (!SecurityUtil.isValidString(Patient.Username))
            {
                RegistrationFormModel.UsernameMessage = SecurityUtil.getProperMessage(Patient.Username, "username");
                return false;
            }
            if (!SecurityUtil.isValidString(Patient.FirstName))
            {
                RegistrationFormModel.FirstNameMessage = SecurityUtil.getProperMessage(Patient.FirstName, "first name"); 
                return false;

            }
            else if (!SecurityUtil.isValidString(Patient.LastName))
            {

                RegistrationFormModel.LastNameMessage = SecurityUtil.getProperMessage(Patient.LastName, "last name"); 
                return false;
            }

            if (Patient.SecondName != null)
            {
                if (!SecurityUtil.isValidString(Patient.SecondName))
                {
                    RegistrationFormModel.SecondNameMessage = SecurityUtil.getProperMessage(Patient.SecondName, "second name"); 
                    return false;
                }
            }
            if(!SecurityUtil.isValidDate(Patient.BirhtDate)) 
            {
                RegistrationFormModel.BirthDateMessage = SecurityUtil.getProperMessage(Patient.BirhtDate, "birth date"); ;
              return false;
            }
            if(!SecurityUtil.isEGN(Patient.EGN)) 
            {
                RegistrationFormModel.EGNMessage = SecurityUtil.getProperMessage(Patient.EGN, "egn. It Must contain only digits!");
                return false;
            }
            return true;
        }

        #endregion
    }
}
