using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для AdminCustomersPage.xaml
    /// </summary>
    public partial class AdminCustomersPage : Page
    {
        public AdminCustomersPage()
        {
            InitializeComponent();
            try
            {
                CustomerRepository customerRepository = new();

                //GridCustomers.ItemsSource = customerRepository.GetCustomerModelsFromDB();
                    var x = customerRepository.GetCustomerList();
                GridCustomers.ItemsSource = x;
            }
            catch (Exception ex)
            {
                string mess = "При загрузке данных произошла ошибка!\n";
                StandardMessages.Error(mess + ex.Message);
            }

        }
        private void EditCustomerTable_Click(object sender, RoutedEventArgs e)
        {
            //var x = GridCustomers.SelectedItem;
            //if (GridCustomers.SelectedItem != null)
            //    NavigationService.Navigate(new AdminStaffAddEditPage((EmployeeModel)GridCustomers.SelectedItem));
            //else MessageBox.Show("Выберите элемент");
        }
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new AdminStaffAddEditPage(null));
        }
        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customersForRemoving = GridCustomers.SelectedItems
                .Cast<Customer>()
                .ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {customersForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteCustomer(customersForRemoving);
            }
        }
        private void DisplayOrderList_Click(object sender, RoutedEventArgs e)
        {

            //if (GridEmployees.SelectedItem != null)
            //    NavigationService.Navigate(new SearchOrSortPage("Employee ID", ((Employee)GridEmployees.SelectedItem).Employee_ID));
            //else MessageBox.Show("Выберите элемент");
        }
        private bool DeleteCustomer(List<Customer> customers)
        {
            try
            {
                CustomerRepository customerRepository = new();

                customerRepository.RemoveRange(customers);
                customerRepository.SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                //GridCustomers.ItemsSource = customerRepository.GetCustomerModelsFromDB();
                GridCustomers.ItemsSource = customerRepository.GetCustomerList();
            }
            catch (Exception ex)
            {
                string mess = "При загрузке данных произошла ошибка!\n";
                StandardMessages.Error(mess + ex.Message);
            }
            return true;
        }
    }
}
