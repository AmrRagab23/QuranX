using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace RationalizingIslam.DocumentModel
{
	public struct MultiPartReference : 
		IComparable,
		IComparable<MultiPartReference>,
		IEnumerable<string>
	{
		public readonly ReadOnlyCollection<string> Parts;
		public readonly bool IsValid;

		public MultiPartReference(IEnumerable<string> parts)
			: this()
		{
			Parts = parts.ToList().AsReadOnly();
			IsValid = parts.Any();
		}

		public int Length
		{
			get { return Parts.Count; }
		}

		public string this[int index]
		{
			get { return Parts[index]; }
		}

		public string GetCaption(string[] referencePartNames)
		{
			var captionParts = new string[referencePartNames.Length];
			for (int index = 0; index < referencePartNames.Length; index++)
			{
				captionParts[index] = string.Format(
						"{0} {1}", 
						referencePartNames[index], 
						Parts[index]
					);
			}
			return string.Join(", ", captionParts);
		}

		public static MultiPartReference ParseXml(XElement element)
		{
			return new MultiPartReference(element.Elements("part").Select(x => x.Value));
		}

		public int CompareTo(MultiPartReference other)
		{
			return (this as IComparable).CompareTo(other);
		}

		public override string ToString()
		{
			return string.Join(
					separator: ".",
					values: Parts
				);
		}

		int IComparable.CompareTo(object obj)
		{
			var other = (MultiPartReference)obj;

			int length = this.Length < other.Length
				? this.Length
				: other.Length;

			for (int index = 0; index < length; index++)
			{
				string left = this[index];
				string right = other[index];
				int leftInt;
				int rightInt;
				if (int.TryParse(left, out leftInt) && int.TryParse(right, out rightInt))
				{
					if (leftInt != rightInt)
						return leftInt.CompareTo(rightInt);
				}
				else
				{
					left = left.PadRight(right.Length, '0');
					right = right.PadRight(left.Length, '0');
					if (left != right)
						return left.CompareTo(right);
				}
			}
			if (this.Length < other.Length)
				return -1;
			if (this.Length > other.Length)
				return 1;
			return 0;
		}

		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return Parts.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<string>).GetEnumerator();
		}

		public static bool operator ==(MultiPartReference first, MultiPartReference second)
		{
			return first.CompareTo(second) == 0;
		}

		public static bool operator !=(MultiPartReference first, MultiPartReference second)
		{
			return first.CompareTo(second) != 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MultiPartReference))
				return false;
			return ((MultiPartReference)obj).CompareTo(this) == 0;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}


	}
}
