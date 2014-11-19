using System.Collections.Generic;
using System.Linq;

namespace RationalizingIslam.DocumentModel
{
	public class Hadith
	{
		public readonly MultiPartReference Reference;
		public readonly IEnumerable<KeyValuePair<string, string>> OtherReferences;
		public readonly string[] ArabicText;
		public readonly string[] EnglishText;
		public readonly VerseRangeReference[] VerseReferences;

		public Hadith(
			MultiPartReference reference,
			IEnumerable<KeyValuePair<string, string>> otherReferences,
			IEnumerable<string> arabicText,
			IEnumerable<string> englishText,
			IEnumerable<VerseRangeReference> verseReferences)
		{
			this.Reference = reference;
			this.OtherReferences = otherReferences;
			this.ArabicText = arabicText.ToArray();
			this.EnglishText = englishText.ToArray();
			this.VerseReferences = verseReferences.Distinct().OrderBy(x => x).ToArray();
		}
	}
}
