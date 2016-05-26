using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Islam.Models
{
    public class ViewModel
    {
        public IEnumerable<Islam.Books> Books { get; set; }
        public IEnumerable<Islam.Surahs> Surahs { get; set; }
        public IEnumerable<Islam.Ayats> Ayats { get; set; }

        public IEnumerable<Islam.SurahNumbers> SurahNumbers { get; set; }

        public IEnumerable<Islam.Articles> Articles { get; set; }

        public IEnumerable<Islam.Menu> Menu { get; set; }

        //public IEnumerable<Islam.SearchResult> SearchResult { get; set; }
    }

   
}