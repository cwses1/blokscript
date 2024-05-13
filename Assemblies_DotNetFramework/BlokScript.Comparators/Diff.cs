using Newtonsoft.Json.Linq;

namespace BlokScript.Comparators
{
	public class Diff
	{
		public string PropertyName;

		public JTokenType Type1;
		public object Value1;

		public JTokenType Type2;
		public object Value2;

		public bool AreEqual;
		public DiffCategory Category;

		//JProperty
			
	}
}
