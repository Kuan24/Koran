using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islam.Models
{
    public class SearchResult
    {
        public string AyatText { get; set; }

        public int AyatNumber { get; set; }

        public string SurahName { get; set; }
        public int SurahNumber { get; set; }
        public string BookName { get; set; }

        public short B_Id { get; set; }
        public int S_Id { get; set; }
    }
}
