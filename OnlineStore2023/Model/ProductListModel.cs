using OnlineStore2023.DataContext.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.Model
{
    internal class ProductListModel
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        //public string Image { get; set; }
        public List<SpecifitationModel> Specifitation { get; private set; }

        public void SetSpecification(List<SpecifitationModel> spec)
        {
            if (spec == null) throw new NullReferenceException("Specification is null");
            Specifitation = spec;
        }
        public void SetSpecification(Guid id)
        {
            ProductRepository productRepository = new();
            List<SpecifitationModel> buff = productRepository.GetProductSpecifitation(id);

            SetSpecification(buff);
        }
    }
}
