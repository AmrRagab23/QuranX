using RationalizingIslam.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranX.Models
{
	public class Analysis_Root
	{
		readonly Word Word;
		public string ArabicRoot { get; private set; }
		public string LetterNames { get; private set; }
		public Analysis_WordTypeGroup[] WordTypeGroups { get; private set; }

		public Analysis_Root(string arabicRoot)
		{
			if (string.IsNullOrEmpty(arabicRoot))
				throw new ArgumentNullException();

			this.ArabicRoot = arabicRoot;
			this.Word = SharedData.Document.RootWordsDocument[ArabicRoot];
			this.LetterNames = QuranX.ArabicHelper.ArabicToLetterNames(ArabicRoot);
			this.WordTypeGroups =
				Word.References
				.GroupBy(x => x.WordTypeDescription)
				.OrderBy(x => x.Key)
				.Select(x => new Analysis_WordTypeGroup(x))
				.ToArray();
		}
	}

	public class Analysis_WordTypeGroup
	{
		public string WordTypeDescription { get; private set; }
		public Analysis_VerseExtract[] Extracts { get; private set; }

		public Analysis_WordTypeGroup(IGrouping<string, WordReference> groupData)
		{
			this.WordTypeDescription = groupData.Key;
			this.Extracts =
				groupData
				.Select(x => new Analysis_VerseExtract(x))
				.ToArray();
		}
	}

	public class Analysis_VerseExtract
	{
		const int NumberOfSurroundingWordsToDisplay = 4;
		public CorpusVerseWord[] PrecedingWords { get; private set; }
		public WordReference WordReference { get; private set; }
		public CorpusVerseWord[] FollowingWords { get; private set; }

		public Analysis_VerseExtract(WordReference wordReference)
		{
			this.WordReference = wordReference;

			var verseReference = 
				SharedData.Document.CorpusDocument[WordReference.ChapterIndex, WordReference.VerseIndex];
			int wordIndex = wordReference.WordIndex;
			this.PrecedingWords =
				verseReference
				.Words
				.Where(x =>
					x.Index >= wordIndex - NumberOfSurroundingWordsToDisplay
					&& x.Index < wordIndex)
				.ToArray();
			this.FollowingWords =
				verseReference
				.Words
				.Where(x =>
					x.Index > wordIndex
					&& x.Index <= wordIndex + NumberOfSurroundingWordsToDisplay)
				.ToArray();
		}
	}
}