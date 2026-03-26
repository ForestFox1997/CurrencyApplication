using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWorker.Services
{
    internal interface ICurrencyUpdater
    {
        Task UpdateAsync(CancellationToken cancellationToken);
    }
}
