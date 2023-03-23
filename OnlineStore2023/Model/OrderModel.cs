using OnlineStore2023.DataContext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.Model
{
    public class OrderModel : INotifyPropertyChanged
    {
        private Guid _orderId;
        private string _orderDate;
        private List<Tuple<Product, int>> _products;
        private string _address;

        public Guid OrderId
        {
            get { return _orderId; }
            set
            {
                if (value == _orderId) return;

                _orderId = value;
                OnPropertyChanged("OrderId");
            }
        }
        public string OrderDate
        {
            get { return _orderDate; }
            set
            {
                if (value == _orderDate) return;

                _orderDate = value;
                OnPropertyChanged("OrderStatus");
            }
        }
        public List<Tuple<Product, int>> Products
        {
            get { return _products; }
            set
            {
                if (value == _products) return;

                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;

                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public OrderModel()
        {
            Products = new List<Tuple<Product, int>>();
            Address = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
