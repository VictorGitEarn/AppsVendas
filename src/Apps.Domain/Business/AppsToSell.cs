using Apps.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business
{
    public class AppsToSell : EntityBase
    {
        public AppsToSell() { }

        public AppsToSell(string nome, double valor)
        {
            Name = nome;
            Value = valor;
        }

        public string Name { get; set; }
        public double Value { get; set; }

        public List<AppsToSell> CreateSamples()
        {
            return new List<AppsToSell>()
            {
                new AppsToSell("App de Corrida",    10.0),
                new AppsToSell("App de Cozinha",    20.0),
                new AppsToSell("App da Amazon",     30.0),
                new AppsToSell("App do Windows",    40.0),
            };
        }
    }
}
