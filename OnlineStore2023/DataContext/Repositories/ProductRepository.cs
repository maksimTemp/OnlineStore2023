using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace OnlineStore2023.DataContext.Repositories
{
    internal class ProductRepository : OnlineShop2022Context
    {
        public ProductRepository() : base() { }

        public Tuple<string, decimal> GetProductNameAndPriceById(Guid id)
        {
            return Products.Where(x => x.ProductId == id)
                .Select(x => new Tuple<string, decimal>(x.ProductName, x.ProductPrice))
                .FirstOrDefault();
        }

        public Product GetProductById(Guid guid)
        {
            return Products.FirstOrDefault(x => x.ProductId == guid);
        }

        public List<SpecifitationModel> GetProductSpecifitation(Guid id)
        {
            var prod = GetProductById(id);
            var list = new List<SpecifitationModel>();

            if (prod.ProductWeight != null && prod.ProductWeight > 0)
            {
                list.Add(new SpecifitationModel() { Element = "Вес", Value = prod.ProductWeight.ToString() });
            }
            if (prod.ProductVolume != null && prod.ProductVolume > 0)
            {
                list.Add(new SpecifitationModel() { Element = "Объём", Value = prod.ProductVolume.ToString() });
            }
            if (prod.ProductColor != null)
            {
                list.Add(new SpecifitationModel() { Element = "Цвет", Value = prod.ProductColor });
            }
            if (list.Count == 0)
            {
                list.Add(new SpecifitationModel() { Element = "Характеристики", Value = "Отсутсвуют" });
            }
            return list;
        }
        public List<Product> GetProductList()
        {
            return Products.ToList();
        }

        public List<Product> GetProductListById(Guid guid)
        {
            return Products
                .Where(x => x.ProductId == guid)
                .ToList();
        }

        public List<Product> GetProductListByName(string name)
        {
            return Products
                .Where(x => x.ProductName == name)
                .ToList();
        }

        public List<Product> GetProductListByColor(string name)
        {
            return Products
                .Where(x => x.ProductName == name)
                .ToList();
        }

        public List<Product> GetProductListByPrice(decimal price)
        {
            return Products
                .Where(x => x.ProductPrice == price)
                .ToList();
        }

        public List<Product> GetProductListByWeight(decimal weight)
        {
            return Products
                .Where(x => x.ProductWeight == weight)
                .ToList();
        }

        public List<Product> GetProductListByStatus(bool status)
        {
            return Products
                .Where(x => x.IsArchive == status)
                .ToList();
        }
    }
}

