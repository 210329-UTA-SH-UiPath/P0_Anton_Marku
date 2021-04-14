using System;

namespace PbDb.Domain.Models
{
    public class RevenueRecord
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }

        public RevenueRecord()
        {

        }

        public string MonthPrint()
        {
            return $"Year:{Year}, Month:{Month}, Revenue: {Revenue}";
        }

        public string WeekPrint()
        {
            return $"Year:{Year}, Week:{Week}, Revenue: {Revenue}";
        }
    }
}