using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islam
{
    public partial class sp_GetAyatsBySearchPhrase_Result
    {
        public string AyatText { get; set; }

        public int AyatNumber { get; set; }

        public string SurahName { get; set; }
        public int SurahNumber { get; set; }
        public string BookName { get; set; }
    }
}
