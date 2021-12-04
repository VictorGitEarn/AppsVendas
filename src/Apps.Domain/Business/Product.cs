using Apps.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business
{
    public class Product : EntityBase
    {
        public Product() { }

        public Product(string nome, double valor)
        {
            Name = nome;
            Value = valor;
        }

        public string Name { get; set; }
        public double Value { get; set; }

        public List<Product> CreateSamples()
        {
            return new List<Product>()
            {
                new Product("App de Corrida",    10.0),
                new Product("App de Cozinha",    20.0),
                new Product("App da Amazon",     30.0),
                new Product("App do Windows",    40.0),
            };
        }
    }
}
