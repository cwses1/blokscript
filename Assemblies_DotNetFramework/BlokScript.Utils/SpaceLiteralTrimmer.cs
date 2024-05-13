namespace BlokScript.Utils
{
	public class SpaceLiteralTrimmer
	{
		public static string Trim (string Literal)
		{
			return Literal.Trim(new char[]{'#'});
		}
	}
}
