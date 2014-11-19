using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RationalizingIslam.DocumentModel;

namespace RationalizingIslam.DocumentModel.Factories
{
	public class DocumentFactory
	{
		QuranDocument Quran;
		HadithDocument Hadith;
		TafsirDocument Tafsir;
		WordsDocument RootWords;
		CorpusDocument Corpus;
		string GeneratedTranslationsDirectory;
		string GeneratedHadithsDirectory;
		string AdditionalHadithXRefsDirectory;
		string GeneratedTafsirsDirectory;
		string GeneratedCorpusXmlFilePath;
		string GeneratedLaneLexiconXmlFilePath;

		public Document Create(
			string generatedTranslationsDirectory,
			string generatedHadithsDirectory,
			string additionalHadithXRefsDirectory,
			string generatedTafsirsDirectory,
			string generatedCorpusXmlFilePath,
			string generatedLaneLexiconXmlFilePath
			)
		{
			this.GeneratedTranslationsDirectory = generatedTranslationsDirectory;
			this.GeneratedHadithsDirectory = generatedHadithsDirectory;
			this.AdditionalHadithXRefsDirectory = additionalHadithXRefsDirectory;
			this.GeneratedTafsirsDirectory = generatedTafsirsDirectory;
			this.GeneratedCorpusXmlFilePath = generatedCorpusXmlFilePath;
			this.GeneratedLaneLexiconXmlFilePath = generatedLaneLexiconXmlFilePath;

			CreateQuran();
			CreateHadith();
			CreateTafsir();
			CreateRootWords();
			CreateCorpus();
			return new Document(
					quranDocument: Quran,
					hadithDocument: Hadith,
					tafsirDocument: Tafsir,
					rootWordsDocument: RootWords,
					corpusDocument: Corpus
				);
		}

		void CreateQuran()
		{
			Console.WriteLine("Loading Quran");
			var factory = new QuranDocumentFactory();
			Quran = factory.Create(GeneratedTranslationsDirectory);
		}

		void CreateHadith()
		{
			Console.WriteLine("Loading Hadiths");
			var factory = new HadithDocumentFactory();
			Hadith = factory.Create(
					generatedHadithsDirectory: GeneratedHadithsDirectory,
					additionalHadithXRefsDirectory: AdditionalHadithXRefsDirectory
				);
		}

		void CreateTafsir()
		{
			Console.WriteLine("Loading Tafsirs");
			var factory = new TafsirDocumentFactory();
			Tafsir = factory.Create(GeneratedTafsirsDirectory);
		}

		void CreateRootWords()
		{
			Console.WriteLine("Loading root words");
			var factory = new RootWordsDocumentFactory();
			RootWords = factory.Create(GeneratedCorpusXmlFilePath);
		}

		void CreateCorpus()
		{
			Console.WriteLine("Loading words by verse");
			var factory = new CorpusDocumentFactory();
			Corpus = factory.Create(GeneratedCorpusXmlFilePath);
		}

	}
}
