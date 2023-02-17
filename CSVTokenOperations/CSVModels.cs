using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVTokenOperations
{
    public class CSVModels
    {
        [Delimiter(",")]        
        public class MasterCSV_Model
        {
            [Ignore]
            public int TokenNumber { get; set; }
            [Ignore]
            public string InstrumentType { get; set; } = string.Empty;
            [Ignore]
            public string Symbol { get; set; } = string.Empty;
            [Ignore]
            public string ExpiryDate { get; set; } = string.Empty;
            [Ignore]
            public double StrikePrice { get; set; }
            [Ignore]
            public string OptionType { get; set; } = string.Empty;
            [Name("DeleteFlag", "deleteflag", "DELETEFLAG", "Delete_Flag", "deleteF_flag")]
            public string DeleteFlag { get; set; } = string.Empty;
            [Ignore]
            public decimal LowPriceRange { get; set; }
            [Ignore]
            public decimal HighPriceRange { get; set;}
        }

    }

    public enum DeleteFlagEnum
    {
        N = 0,
        Y=1,
        BLANK = 2,
        OTHER = 3
    }
}
