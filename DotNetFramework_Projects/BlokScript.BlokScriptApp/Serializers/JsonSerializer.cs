using Newtonsoft.Json;

namespace BlokScript.Serializers
{
	public class JsonSerializer
	{
		public static string Serialize (object SourceObject)
		{
			return JsonConvert.SerializeObject(SourceObject);
		}

		public static object SerializeToNative (object SourceObject)
		{
			return JsonConvert.DeserializeObject(Serialize(SourceObject));
		}
	}
}
