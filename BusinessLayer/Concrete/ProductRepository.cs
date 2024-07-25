using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;

namespace BusinessLayer.Concrete
{
	public class ProductRepository:GenericRepository<Product>
	{
        DataContext db = new DataContext();
        public List<Product> GetProductList()
        {
            return db.Products.ToList();
        }
       
		public List<Product> GetPopularProduct(int count = 6)
		{
			return db.Products.Where(x => x.Popular == true).Take(count).ToList();
		}
	}
}
