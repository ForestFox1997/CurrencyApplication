using CurrencyWorker.Infrastructure;
using CurrencyWorker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyWorker.Services
{
    internal class CurrencyUpdater : ICurrencyUpdater
    {
        private readonly HttpClient _httpClient;
        private readonly FinanceDbContext _context;
        private readonly ILogger<CurrencyUpdater> _logger;
        private readonly Encoding _encoding;

        public CurrencyUpdater(HttpClient httpClient, FinanceDbContext context, ILogger<CurrencyUpdater> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _encoding = Encoding.GetEncoding("windows-1251");
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            
            var xml = await _httpClient.GetStringAsync("http://www.cbr.ru/scripts/XML_daily.asp", cancellationToken);

            var document = XDocument.Parse(xml);

            var currencies = document
                .Descendants("Valute")
                .Select(v => new
                {
                    Code = v.Element("CharCode")?.Value,
                    Value = v.Element("VunitRate")?.Value
                })
                .Where(x => x.Code != null)
                .ToList();

            foreach (var item in currencies)
            {
                var rate = GetDecimalValueFromString(item.Value!);

                var existing = await _context.Currencies
                    .FirstOrDefaultAsync(c => c.Name == item.Code, cancellationToken);

                if (existing == null)
                {
                    _context.Currencies.Add(new Currency
                    {
                        Id = Guid.NewGuid(),
                        Name = item.Code!,
                        Rate = rate
                    });
                }
                else
                {
                    existing.Rate = rate;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private decimal GetDecimalValueFromString(string input)
        {
            input = input.Replace(',', '.');
            var value = decimal.Parse(input, NumberStyles.Float, CultureInfo.InvariantCulture);

            return value;
        }
    }
}
