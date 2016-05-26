using System;
using System.Linq;
using System.Web.Mvc;
using Islam.Models;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Collections;
using System.Web;
using PagedList.Mvc;
using PagedList;

namespace Islam.Controllers
{
    public class HomeController : Controller
    {
        public AlKuranDBEntities db = new AlKuranDBEntities();
        /*
        public ActionResult Articles(string Id = "")
        {
            ViewModel mymodel = new ViewModel();

            var ArticleTitles = from a in db.Articles
                                join m in db.Menu on a.M_M_Id equals m.M_Id
                                where m.MenuEng == Id
                                select a;
            
            mymodel.Articles = ArticleTitles.ToList();

            var mNameRu = (from m in db.Menu where m.MenuEng == Id
                           select new {m.MenuName}).SingleOrDefault();

            ViewBag.Title2 = mNameRu.MenuName;
            return View(mymodel);
        }
        */
        public ActionResult ArticleText(int Id)
        {
            var at = (from a in db.Articles
                     where a.A_Id == Id
                     select a).SingleOrDefault();

            ViewBag.ArticleTitle = at.ArticleTitle;
            ViewBag.Annotation = at.ArticleAnnotation;
            ViewBag.ArticleText = at.ArticleText;

            return View();
        }

        protected int GetRandomSurah_ID()
        {
            int iS_ID = 0; int i = 1;
            Hashtable ht = new Hashtable();
            //получаем список s_id из базы:
            var query = from s in db.Surahs select s;
            foreach (var vs_id in query.ToList())
            {
                ht.Add(i, vs_id.S_Id);
                i++;
            }
            Random rand = new Random();
            int iRandomNumber = rand.Next(1, 570);

            iS_ID = (int)ht[iRandomNumber];

            return iS_ID;
        }

        /// <summary>
        /// Multiple Model in single view using View Model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string Id = "", string S_Id = "") //необязательный параметр. Id - книги
        {
            ViewModel mymodel = new ViewModel();
            if (Id == "" & S_Id == "")
            {
                ViewBag.SurahOfTheDay = "Сура дня";
                short B_ID = 0;
                int iS_ID = 0;

                mymodel.Books = db.Books.ToList();
                //Генерация случайного числа для суры:=====
                iS_ID = GetRandomSurah_ID();
                //узнаём B_ID=========================================

                var query1 = (from b in db.Books
                              join s in db.Surahs on b.B_Id equals s.B_B_Id
                              join sn in db.SurahNumbers on s.S_Id equals sn.S_S_Id
                              where s.S_Id == iS_ID
                              select new { b.B_Id, b.BookName, s.S_Id, sn.SurahNumber }).SingleOrDefault();

                B_ID = query1.B_Id;

                var query = db.Books.FirstOrDefault(b_id => b_id.B_Id == B_ID);
                ViewBag.Book = query.BookName;
                //Суры================================================
                var query2 = db.Surahs.FirstOrDefault(s_id => s_id.S_Id == iS_ID);
                ViewBag.Surah = query2.SurahName;
                ViewBag.SurahNumber = query1.SurahNumber.ToString();
                mymodel.Surahs = db.Surahs.Where(b_id => b_id.B_B_Id == B_ID).ToList();
                mymodel.Ayats = db.Ayats.Where(s_id => s_id.S_S_Id == iS_ID).ToList();
                //=======================================================
                if(query1.BookName == "Коран. Перевод Г.С. Саблукова")
                    ViewBag.VoImya = "Во имя Бога, милостивого и милосердного.";
                else
                    ViewBag.VoImya = "Во имя Аллаха, Всемилостивого, Милосердного!";
                if (query2.SurahName == "Сура 9. Покаяние")
                    ViewBag.VoImya = "";
                if (query2.SurahName == "Сура 1. Открывающая Книгу")
                    ViewBag.VoImya = "";
                if (query2.SurahName == "Сура 1. Отверзающая дверь к досточтимому писанию")
                    ViewBag.VoImya = "";
                if (query2.SurahName == "1. Открывающая")
                    ViewBag.VoImya = "";
                if (query2.SurahName == "9. Покаяние")
                    ViewBag.VoImya = "";
            }
            else
            { 
                short B_ID = 0;
                if (Id != "")
                {
                    B_ID = short.Parse(Id);
                }
                int _s_id = 0;
                int _SurahNumber = 0;
                bool bFlag = false;
                //распознаём 
                if (S_Id.Contains("_"))
                {
                    if (S_Id.Count() > 1) //если непустое текстовое поле номера суры - проверяем
                    {
                        _SurahNumber = Convert.ToInt32(S_Id.Remove(S_Id.Length - 1, 1)); 

                        if (_SurahNumber <= 114)
                        {
                            var _Ayats = from s in db.Surahs 
                                            join a in db.Ayats on s.S_Id equals a.S_S_Id
                                            join sn in db.SurahNumbers on s.S_Id equals sn.S_S_Id
                                            where (s.B_B_Id == B_ID) & (sn.SurahNumber == _SurahNumber)
                                            select a;

                            mymodel.Ayats = _Ayats.ToList();
                            //название суры: =====================
                            var _sName = (from s in db.Surahs 
                                            join sn in db.SurahNumbers on s.S_Id equals sn.S_S_Id
                                            where (s.B_B_Id == B_ID) & (sn.SurahNumber == _SurahNumber)
                                            select new { s.SurahName }).SingleOrDefault();
                            ViewBag.Surah = _sName.SurahName;
                            //=====================================
                            if (_sName.SurahName != "Сура 9. Покаяние")
                                ViewBag.VoImya = "Во имя Аллаха, Всемилостивого, Милосердного!";
                            if (_sName.SurahName == "Сура 1. Открывающая Книгу")
                                ViewBag.VoImya = "";
                            if (_sName.SurahName == "1. Открывающая")
                                    ViewBag.VoImya = "";
                            if (_sName.SurahName == "9. Покаяние")
                                    ViewBag.VoImya = "";
                            bFlag = true;
                        }
                        else
                            ViewBag.TextError = "Введите число меньшее, равное числу 114";
                    }
                }
                else if (S_Id != "")
                    _s_id = Convert.ToInt32(S_Id);
                //==================================================================
                mymodel.Books = db.Books.ToList();
                if (Id != "")
                {
                    var query = db.Books.FirstOrDefault(b_id => b_id.B_Id == B_ID);
                    ViewBag.Book = query.BookName;
                }

                mymodel.Surahs = db.Surahs.Where(b_id => b_id.B_B_Id == B_ID).ToList();
                if (_s_id != 0)
                {
                    bFlag = false;
                    var query2 = db.Surahs.FirstOrDefault(s_id => s_id.S_Id == _s_id);
                    ViewBag.Surah =  query2.SurahName;
                    //Номер суры для textbox===================================
                    //var query3 = (from s in db.Surahs
                    //                join sn in db.SurahNumbers on s.S_Id equals sn.S_S_Id
                    //                where s.S_Id == _s_id
                    //                select new { sn.SurahNumber }).SingleOrDefault();
                    //_SurahNumber = query3.SurahNumber;
                    //=======================================================
                    if (ViewBag.Book == "Коран. Перевод Г.С. Саблукова")
                        ViewBag.VoImya = "Во имя Бога, милостивого и милосердного.";
                    else
                        ViewBag.VoImya = "Во имя Аллаха, Всемилостивого, Милосердного!";
                    if (query2.SurahName == "Сура 9. Покаяние")
                        ViewBag.VoImya = "";
                    if (query2.SurahName == "Сура 1. Открывающая Книгу")
                        ViewBag.VoImya = "";
                    if (query2.SurahName == "Сура 1. Отверзающая дверь к досточтимому писанию")
                        ViewBag.VoImya = "";
                    if (query2.SurahName == "1. Открывающая")
                            ViewBag.VoImya = "";
                    if (query2.SurahName == "9. Покаяние")
                            ViewBag.VoImya = "";
                }
                if (!bFlag)
                    mymodel.Ayats = db.Ayats.Where(s_id => s_id.S_S_Id == _s_id).ToList();

                //if (_SurahNumber == 0)
                //    ViewBag.SurahNumber = "";
                //else
                //    ViewBag.SurahNumber = _SurahNumber.ToString();
            }
            return View(mymodel);
        }
       
        [HttpPost]
        public ActionResult SearchResult()
        {
            string sSearch = "";
            if (Request.Form["SearchText"] != "")
            {
                sSearch = Request.Form["SearchText"].ToString();
                ViewBag.SearchString = sSearch;
                DbRawSqlQuery<SearchResult> data = db.Database.SqlQuery<SearchResult>
                    (@"DECLARE @SearchWord varchar(100)" +
                        " SET @SearchWord = '" + sSearch + "'" +
                        " select A.[AyatText] as AyatText, A.[AyatNumber] as AyatNumber, S.SurahName as SurahName, " +
                        " SH.SurahNumber as SurahNumber, B.BookName as BookName, B.B_Id, S.S_Id " +
                        " from [dbo].[Ayats] as A inner join " +
                        " [dbo].[Surahs] as S on A.S_S_Id = S.S_Id inner join " +
                        " [dbo].[SurahNumbers] as SH on S.S_Id = SH.S_S_Id inner join " +
                        " [dbo].[Books] as B on S.B_B_Id = b.B_Id " +
                    " where FREETEXT ([AyatText], @SearchWord) order by A.AyatNumber");
                /*
                string sSql = @"declare @Phrase varchar(100) set @Phrase='" + sSearch + "'" +
                        " declare @AyatText varchar(max), @AyatNumber int, @SurahName varchar(100), @SurahNumber int, @BookName varchar(50)," +
                        " @ReplAyatText varchar(max)" +
                    " create table #AyatsTempTable (AyatText text, AyatNumber int, SurahName varchar(100), SurahNumber int, BookName varchar(50))" +
                    " DECLARE a_cursor CURSOR FOR " +
                        "select A.[AyatText] as AyatText, A.[AyatNumber] as AyatNumber, S.SurahName as SurahName, " +
                            "SH.SurahNumber as SurahNumber, B.BookName as BookName " +
                        "from [dbo].[Ayats] as A inner join " +
                            "[dbo].[Surahs] as S on A.S_S_Id = S.S_Id inner join " +
                            "[dbo].[SurahNumbers] as SH on S.S_Id = SH.S_S_Id inner join " +
                            "[dbo].[Books] as B on S.B_B_Id = b.B_Id " +
                        "where FREETEXT ([AyatText], @Phrase) order by A.AyatNumber " +
                    "open a_cursor " +
                    "FETCH NEXT FROM a_cursor " +
                    "INTO @AyatText, @AyatNumber, @SurahName, @SurahNumber, @BookName " +
                    "set @ReplAyatText = (select REPLACE(@AyatText, @Phrase, '<strong>' + @Phrase + '</strong>')) " +
                    "set @ReplAyatText = (replace( replace( @ReplAyatText, '&lt;', '<' ), '&gt;', '>' )) " +
                    "WHILE @@FETCH_STATUS = 0 " +
                        "BEGIN " +
                            "insert into #AyatsTempTable (AyatText, AyatNumber, SurahName, SurahNumber, BookName) " +
                            "values (@ReplAyatText, @AyatNumber, @SurahName, @SurahNumber, @BookName) " +
                            "FETCH NEXT FROM a_cursor  " +
                            "INTO @AyatText, @AyatNumber, @SurahName, @SurahNumber, @BookName " +
                            "set @ReplAyatText = (select REPLACE(@AyatText, @Phrase, '<strong>' + @Phrase + '</strong>')) " +
                            "set @ReplAyatText = (replace( replace( @ReplAyatText, '&lt;', '<' ), '&gt;', '>' )) " +
                        "END " +
                    "CLOSE a_cursor; " +
                    "DEALLOCATE a_cursor; " +
                    "select * from #AyatsTempTable; ";
                DbRawSqlQuery<SearchResult> data = db.Database.SqlQuery<SearchResult>
                   (sSql);
                 */
                //var sRep = "";
                //var myEncodedString = "";
                //List<SearchResult> NewAyatsData = new List<SearchResult>();
                //foreach (var a in data)
                //{
                //    sRep ="<span class='bold-search'>" + sSearch + "</span>";
                //    myEncodedString = sRep.Replace("&lt;", "<");
                //    myEncodedString = myEncodedString.Replace("&gt;", ">");
                //    SearchResult AyatData = new SearchResult();
                //    AyatData.AyatText = a.AyatText.Replace(sSearch, myEncodedString);
                //    AyatData.AyatNumber = a.AyatNumber;
                //    AyatData.SurahName = a.SurahName;
                //    AyatData.SurahNumber = a.SurahNumber;
                //    AyatData.BookName = a.BookName;
                //    AyatData.B_Id = a.B_Id;
                //    AyatData.S_Id = a.S_Id;
                //    NewAyatsData.Add(AyatData);
                //}

                return View(data.ToList());
            }
            else
            {
                ViewBag.SearchString = "";
                return View();
            }
        }

        public string GetBrowserLanguage()
        {
            string language = "en-us";
            //Detect User's Language.
            if (Request.UserLanguages != null)
            {
                //Set the Language.
                language = Request.UserLanguages[0];
            }
            return language;
        }

        public ActionResult GetHadith(int? page)
        {
            ViewBag.Title = "Хадисы пророка Мухаммада";
            var query = (from a in db.Articles where a.M_M_Id == 1022 select a).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Articles(int? page)
        {
            ViewBag.Title = "Статьи";
            var query = (from a in db.Articles where a.M_M_Id == 1005 select a).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }
    }
}