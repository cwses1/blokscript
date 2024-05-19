using Newtonsoft.Json;
using System.IO;

namespace BlokScript.Formatters
{
	public class JsonFormatter
	{
		public static string FormatIndented (string JsonString)
		{
			object JsonObject = JsonConvert.DeserializeObject(JsonString);
			return JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
		}

		public static string FormatNone (string JsonString)
		{
			object JsonObject = JsonConvert.DeserializeObject(JsonString);
			return JsonConvert.SerializeObject(JsonObject, Formatting.None);
		}

		public static string FormatNone (object JsonObject)
		{
			return JsonConvert.SerializeObject(JsonObject, Formatting.None);
		}

		public static string FormatIndented (object JsonObject)
		{
			return JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
		}

	}
}
