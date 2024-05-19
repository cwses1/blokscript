namespace BlokScript.Utils
{
	public class StringLiteralTrimmer
	{
		public static string Trim (string Literal)
		{
			return Literal.Trim(new char[]{'\''});
		}


		public static string TrimSpaceId (string Literal)
		{
			return Literal.Trim(new char[]{'\'', '#'});
		}
	}
}
