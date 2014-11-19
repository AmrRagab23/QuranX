using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RationalizingIslam.DocumentModel
{
	public class CollectionAndHadith : IComparable
	{
		public HadithCollection Collection { get; private set; }
		public Hadith Hadith { get; private set; }

		public CollectionAndHadith(HadithCollection collection, Hadith hadith)
		{
			this.Collection = collection;
			this.Hadith = hadith;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is CollectionAndHadith))
				return false;

			var other = (CollectionAndHadith)obj;
			return
				this.Collection.Code == other.Collection.Code
				&& this.Hadith.Reference == other.Hadith.Reference;
		}

		public static bool operator ==(CollectionAndHadith left, CollectionAndHadith right)
		{
			return (left.Equals(right));
		}

		public static bool operator !=(CollectionAndHadith left, CollectionAndHadith right)
		{
			return (!left.Equals(right));
		}

		public int CompareTo(CollectionAndHadith other)
		{
			int stringCompare = string.Compare(this.Collection.Code, other.Collection.Code);
			if (stringCompare != 0)
				return stringCompare;
			return Hadith.Reference.CompareTo(other.Hadith.Reference);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return Collection.Code.GetHashCode() + Hadith.Reference.GetHashCode();
			}
		}

		int IComparable.CompareTo(object obj)
		{
			if (!(obj is CollectionAndHadith))
				throw new ArgumentException();
			return CompareTo((CollectionAndHadith)obj);
		}
	}
}
