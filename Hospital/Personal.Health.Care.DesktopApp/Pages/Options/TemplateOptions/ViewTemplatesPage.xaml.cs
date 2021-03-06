﻿using Personal.Health.Care.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Personal.Health.Care.DesktopApp.Pages.Options.TemplateOptions
{
    /// <summary>
    /// Interaction logic for ViewTemplatesPage.xaml
    /// </summary>
    public partial class ViewTemplatesPage : UserControl
    {
        public ViewTemplatesPage()
        {
            InitializeComponent();
            this.DataContext = TemplatesViewModel.GetInstance();
        }
    }
}
