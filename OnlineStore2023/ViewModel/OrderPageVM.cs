using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.ViewModel
{
    internal class OrderPageVM : BaseAbstractVMPage
    {
        #region Fields
        private List<Order> _orders;
        private object _selectedOrder;

        #endregion

        #region Properties
        public List<Order> Orders
        {
            get { return _orders; }
            set
            {
                if (value == _orders) return;

                _orders = value;
                OnPropertyChanged("Orders");
            }

        }
        public object SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == _selectedOrder) return;

                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand ShowOrderDetail { get; set; }
        #endregion


        protected override void LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
