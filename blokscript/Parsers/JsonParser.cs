using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Collections;

namespace BlokScript.Parsers
{
	public class JsonParser
	{
		public static dynamic ParseAsDynamic (string JsonString)
		{
			return (dynamic)JsonConvert.DeserializeObject(JsonString);
		}

		public static Hashtable ParseAsHashtable (string JsonString)
		{
			return (Hashtable)JsonConvert.DeserializeObject(JsonString, typeof(Hashtable));
		}

		public static object Parse (string JsonString)
		{
			return JsonConvert.DeserializeObject(JsonString);
		}
	}
}
