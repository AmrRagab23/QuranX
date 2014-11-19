using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace QuranX.Controllers
{
    public class SiteMapController : Controller
    {
		const string Domain = "http://QuranX.com";
		const int PageSize = 10 * 1000;

		public SiteMapResult Quran()
		{
			var urls = new List<string>();
			foreach(var chapter in SharedData.Document.QuranDocument.Chapters)
				foreach (var verse in chapter.Verses)
					urls.Add(string.Format("{0}/{1}.{2}", Domain, chapter.Index, verse.Index));
			return new SiteMapResult(urls);
		}

		public SiteMapResult Tafsir(string tafsirCode, int pageIndex)
		{
			pageIndex--;
			var urls = new List<string>();
			var tafsir = SharedData.Document.TafsirDocument[tafsirCode];
			var comments = tafsir.Comments.OrderBy(x => x.VerseReference).Skip(pageIndex * PageSize).Take(PageSize);
			foreach (var comment in comments)
				urls.Add(string.Format("{0}/tafsir/{1}/{2}.{3}", Domain, tafsirCode, comment.VerseReference.Chapter, comment.VerseReference.FirstVerse));
			return new SiteMapResult(urls);
		}

		public SiteMapResult Hadith(string collectionCode, int pageIndex)
		{
			pageIndex--;
			var urls = new List<string>();
			var hadithCollection = SharedData.Document.HadithDocument[collectionCode];
			var hadiths = hadithCollection.Hadiths.OrderBy(x => x.Reference).Skip(pageIndex * PageSize).Take(PageSize);
			foreach (var hadith in hadiths)
			{
				string url = Domain + "/hadith/" + collectionCode + "/";
				for (int index = 0; index < hadithCollection.ReferencePartNames.Length; index++)
					url += hadithCollection.ReferencePartNames[index] + "-" + hadith.Reference[index] + "/";
				urls.Add(url);
			}
			return new SiteMapResult(urls);
		}
	}
}
