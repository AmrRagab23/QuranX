using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RationalizingIslam.DocumentModel;

namespace RationalizingIslam.DocumentModel.Factories
{
	public class HadithCollectionFactory
	{
		HadithCollection Collection;
		Dictionary<MultiPartReference, HashSet<VerseRangeReference>> VersesByHadith;

		public HadithCollection Create(string hadithFilePath, string additionalHadithXRefsDirectory)
		{
			var doc = XDocument.Load(File.OpenText(hadithFilePath));
			var collectionNode = doc.Document.Root;

			string code = collectionNode.Element("code").Value;
			string name = collectionNode.Element("name").Value;
			string copyright = collectionNode.Element("copyright").Value;
			string[] referencePartNames = ReadRefeferenceDefinition(collectionNode);

			CreateAdditionalHadithXRefs(
					tafsirCode: code, 
					xrefsDirectory: additionalHadithXRefsDirectory
				);

			Collection = new HadithCollection(
					code: code,
					name: name,
					copyRight: copyright,
					referencePartNames: referencePartNames
				);
			ReadHadiths(collectionNode);
			return Collection;
		}

		void CreateAdditionalHadithXRefs(string tafsirCode, string xrefsDirectory)
		{
			VersesByHadith = new Dictionary<MultiPartReference, HashSet<VerseRangeReference>>();
			string xrefsFilePath = Path.Combine(xrefsDirectory, tafsirCode + ".txt");
			if (!File.Exists(xrefsFilePath))
				return;

			string[] lines = File.ReadAllLines(xrefsFilePath);
			foreach (string line in lines)
			{
				string[] lineValues = line.Split('\t');
				if (string.IsNullOrEmpty(lineValues[0]))
					continue;
				var hadithReference = new MultiPartReference(lineValues[0].Split('.'));
				foreach (string verseRangeReferenceText in lineValues.Skip(1))
				{
					if (string.IsNullOrEmpty(verseRangeReferenceText))
						continue;
					AddVerseReference(
							hadithReference: hadithReference,
							verseRangeReferenceText: verseRangeReferenceText
						);
				}
			}
		}

		void AddVerseReference(MultiPartReference hadithReference, string verseRangeReferenceText)
		{
			var verseRangeReference = VerseRangeReference.Parse(verseRangeReferenceText);
			HashSet<VerseRangeReference> verseRangeReferences;
			if (!VersesByHadith.TryGetValue(hadithReference, out verseRangeReferences))
			{
				verseRangeReferences = new HashSet<VerseRangeReference>();
				VersesByHadith[hadithReference] = verseRangeReferences;
			}
			verseRangeReferences.Add(verseRangeReference);
		}

		void ReadHadiths(XElement collectionNode)
		{
			foreach (XElement hadithNode in collectionNode.Elements("hadith"))
			{
				ReadHadith(hadithNode);
			}
		}

		void ReadHadith(XElement hadithNode)
		{
			var reference = MultiPartReference.ParseXml(hadithNode.Element("reference"));
			var secondaryReferences = ReadSecondaryReferences(hadithNode.Element("secondaryReferences"));
			var verseReferences = ReadVerseReferences(hadithNode.Element("verseReferences"));
			var englishTextNode = hadithNode.Element("english");
			var englishText = englishTextNode.Elements("text").Select(x => x.Value);
			var arabicTextNode = hadithNode.Element("arabic");
			var arabicText = arabicTextNode.Elements("text").Select(x => x.Value);

			HashSet<VerseRangeReference> additionalVerseReferences;
			if (VersesByHadith.TryGetValue(reference, out additionalVerseReferences))
			{
				verseReferences = verseReferences.Concat(additionalVerseReferences);
			}

			var hadith = new Hadith(
					reference: reference,
					otherReferences: secondaryReferences,
					arabicText: arabicText,
					englishText: englishText,
					verseReferences: verseReferences
				);
			Collection.AddHadith(hadith);
		}

		IEnumerable<KeyValuePair<string, string>> ReadSecondaryReferences(XElement parentNode)
		{
			return parentNode.Elements("secondaryReference")
				.Select(x => new KeyValuePair<string, string>(
							key: x.Element("type").Value,
							value:  x.Element("value").Value
						)
					);
		}

		IEnumerable<VerseRangeReference> ReadVerseReferences(XElement parentNode)
		{
			return parentNode.Elements("reference")
				.Select(x => VerseRangeReference.ParseXml(x));
		}

		string[] ReadRefeferenceDefinition(XElement rootNode)
		{
			return rootNode
				.Element("referenceDefinition")
				.Elements("definition")
				.Select(x => x.Value)
				.ToArray();
		}

	}
}
