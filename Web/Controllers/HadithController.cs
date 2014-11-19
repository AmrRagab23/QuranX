using QuranX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranX.Controllers
{
	public class HadithController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.HideChapterVerseQuickJump = true;
			var model = SharedData.Document.HadithDocument.Collections;
			return View(model);
		}

		public ActionResult Collection(string collectionCode, string path)
		{
			var model = new Hadith_DrillDown(
				collectionCode: collectionCode,
				path: path
			);
			return View(model);
		}

		[HttpPost]
		public ActionResult RedirectToChapterVerse(int chapter, int verse)
		{
			return RedirectToAction(
				actionName: "Verse",
				controllerName: "HadithsByVerse",
				routeValues: new
				{
					Chapter = chapter,
					Verse = verse
				}
			);
		}



	}
}
