﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for SearchParentDialog.xaml
    /// </summary>
    public partial class SearchParentDialog : Window
    {
        public SearchParentDialog()
        {
            InitializeComponent();
            DataContext = new SearchParentViewModel();
        }
    }
}
