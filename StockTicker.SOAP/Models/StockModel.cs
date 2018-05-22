using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockTicker.Soap.Models
{
    [Serializable]
    public class StockModel
    {

        public string Symbol { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public StockModel()
        {

        }

        public StockModel(string symbol, string name, double price)
        {
            Symbol = symbol;
            Name = name;
            Price = price;
        }
         
    }
}