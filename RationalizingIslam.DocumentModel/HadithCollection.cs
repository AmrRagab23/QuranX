using System.Collections.Generic;
using System.Linq;

namespace RationalizingIslam.DocumentModel
{
	public class HadithCollection
	{
		readonly Dictionary<VerseReference, List<Hadith>> HadithsByVerse;
		readonly Dictionary<MultiPartReference, Hadith> _Hadiths;
		public readonly string Code;
		public readonly string Name;
		public readonly string Copyright;
		public readonly string[] ReferencePartNames;

		public HadithCollection(
			string code, 
			string name, 
			string copyRight,
			string[] referencePartNames)
		{
			this.Code = code;
			this.Name = name;
			this.Copyright = copyRight;
			this.ReferencePartNames = referencePartNames;
			this._Hadiths = new Dictionary<MultiPartReference, Hadith>();
			this.HadithsByVerse = new Dictionary<VerseReference, List<Hadith>>();
		}

		public IEnumerable<Hadith> Hadiths
		{
			get
			{
				return _Hadiths
					.Select(x => x.Value)
					.OrderBy(x => x.Reference);
			}
		}

		public void AddHadith(Hadith hadith)
		{
			if (_Hadiths.ContainsKey(hadith.Reference))
			{
				System.Console.WriteLine("Duplicate hadith reference " + Code + " " + hadith.Reference.ToString());
				return;
			}
			_Hadiths.Add(hadith.Reference, hadith);
			AddHadithToIndividualVerses(hadith);
		}

		public IEnumerable<Hadith> GetHadithsForVerse(
				int chapterIndex,
				int verseIndex
			)
		{
			List<Hadith> result;
			var verseReference = new VerseReference(
					chapter: chapterIndex,
					verse: verseIndex
				);
			if (!HadithsByVerse.TryGetValue(verseReference, out result))
				return new List<Hadith>();
			return result.OrderBy(x => x.Reference);
		}

		void AddHadithToIndividualVerses(Hadith hadith)
		{
			foreach (var verseRangeReference in hadith.VerseReferences)
			{
				for (int verseIndex = verseRangeReference.FirstVerse;
						verseIndex <= verseRangeReference.LastVerse;
						verseIndex++
					)
				{
					List<Hadith> hadiths;
					var verseReference = new VerseReference(
							chapter: verseRangeReference.Chapter,
							verse: verseIndex
						);
					if (!HadithsByVerse.TryGetValue(verseReference, out hadiths))
					{
						hadiths = new List<Hadith>();
						HadithsByVerse[verseReference] = hadiths;
					}
					hadiths.Add(hadith);
				}
			}
		}

	}
}
