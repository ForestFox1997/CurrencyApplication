using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWorker.Models
{
    internal class Currency
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Rate { get; set; }
    }
}
