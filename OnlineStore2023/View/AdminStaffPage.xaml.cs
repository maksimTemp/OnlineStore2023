using OnlineStore2023.DataContext;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using OnlineStore2023.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для AdminStaffPage.xaml
    /// </summary>
    public partial class AdminStaffPage : Page
    {
        public AdminStaffPage(AccountVM accountVM) 
        {
            InitializeComponent();
            this.DataContext = new AdminStaffPageVM(accountVM);
        }
    }
}
