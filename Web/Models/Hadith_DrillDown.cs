using RationalizingIslam.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace QuranX.Models
{
	public class Hadith_DrillDown
	{
		public HadithCollection Collection { get; private set; }
		public List<KeyValuePair<string, string>> SelectedKeyParts { get; private set; }
		public string NextKeyPartName { get; private set; }
		public IEnumerable<Hadith> HadithsInCurrentSelection { get; private set; }
		public IEnumerable<string> NextKeyPartSelection { get; private set; }

		public Hadith_DrillDown(
			string collectionCode,
			string path)
		{
			path = path ?? "";
			this.Collection = SharedData.Document.HadithDocument[collectionCode];
			this.NextKeyPartSelection = new List<string>();
			this.SelectedKeyParts =
				path.Split('/')
				.Where(x => !string.IsNullOrEmpty(x))
				.Select(x => x.Trim())
				.Where(x => x != "")
				.Select(x =>
					{
						string[] keyAndValue = x.Split('-');
						return new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]);
					}
				)
				.ToList();

			int referencePartIndex = 0;
			HadithsInCurrentSelection = new List<Hadith>(Collection.Hadiths);
			foreach (var keyPartAndValue in SelectedKeyParts)
			{
				if (string.Compare(keyPartAndValue.Key, Collection.ReferencePartNames[referencePartIndex], true) != 0)
					throw new ArgumentException(string.Format("Expected key part {0} but found {1}", Collection.ReferencePartNames[referencePartIndex], keyPartAndValue.Key));

				HadithsInCurrentSelection = HadithsInCurrentSelection
					.Where(x => string.Compare(x.Reference[referencePartIndex], keyPartAndValue.Value, true) == 0)
					.ToList();
				referencePartIndex++;
			}
			if (referencePartIndex < Collection.ReferencePartNames.Length)
			{
				NextKeyPartName = Collection.ReferencePartNames[referencePartIndex];
				NextKeyPartSelection = HadithsInCurrentSelection.Select(x => x.Reference[referencePartIndex]).Distinct();
			}
		}

	}
}