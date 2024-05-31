using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class Statistics
    {
        public double IncomeToday { get; set; }
        public int SoldItemsToday { get; set; }
        public double AveragePriceToday { get; set; }
        public int NewUsersToday { get; set; }
        public double TotalIncome { get; set; }
        public int TotalSoldItems { get; set; }
        public double AveragePrice { get; set; }
        public int TotalUsers { get; set; }
        public List<double> IncomeDaily { get; set; }
        public double MensIncome { get; set; }
        public double WomensIncome { get; set; }
        public double KidsIncome { get; set; }
        public double MensSoldPercentage { get; set; }
        public double WomensSoldPercentage { get; set; }
        public double KidsSoldPercentage { get; set; }
        public List<ProductStatistics> TopSalesProducts { get; set; }

    }
}
