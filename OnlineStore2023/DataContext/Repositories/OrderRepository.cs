using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.DataContext.Repositories
{
    internal class OrderRepository : OnlineShop2022Context
    {
        public OrderRepository() : base() { }

        public List<Order> GetOrderList()
        {
            return Orders.Include(x => x.Customer)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.Employee)
                .ThenInclude(x => x.UsersDatum)
                .ToList();
        }

        public Order GetOrderByCustomerId(Guid id)
        {
            return Orders.Include(x => x.Customer)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.Employee)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => ProductOrders)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.CustomerId == id);
        }

        public Order GetOrderByEmployeeId(Guid id)
        {
            return Orders.Include(x => x.Customer)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.Employee)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.ProductOrders)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.EmployeeId == id);
        }

        public Order GetOrderByOrderId(Guid id)
        {
            return Orders.Include(x => x.Customer)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.Employee)
                .ThenInclude(x => x.UsersDatum)
                .Include(x => x.ProductOrders)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.OrderId == id);
        }

        public List<Order> GetOrderListByCustomerId(Guid id)
        {
            return Orders.Where(x => x.CustomerId == id).ToList();
        }

        public List<Order> GetOrderListById(Guid id)
        {
            return Orders.Where(x => x.OrderId == id).ToList();
        }

        public List<Order> GetOrderListByDate(DateTime date)
        {
            return Orders.Where(x => x.OrderDate == date).ToList();
        }

        public List<Order> GetOrderListByCost(decimal totalPrice)
        {
            return Orders.Where(x => x.TotalPrice == totalPrice).ToList();
        }

        public List<Order> GetOrderListByOrderStatus(string orderStatus)
        {
            return Orders.Where(x => x.OrderStatus == orderStatus).ToList();
        }

        public List<Order> GetOrderListByAddress(string deliveryAddress)
        {
            return Orders.Where(x => x.DeliveryAddress == deliveryAddress).ToList();
        }

        public void DeleteOrderByID(Guid id)
        {
            ProductOrders
                .RemoveRange(ProductOrders.Where(po => po.OrderId == id));

            Orders.Remove(Orders.FirstOrDefault(o => o.OrderId == id));
        }
    }
}
