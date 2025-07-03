namespace BlokScript.Utils
{
	public class UrlUtil
	{
		public static string GetFullSlugEquivalent (string Url)
		{
			return Url.Substring(1);
		}

		public static string GetParentUrl (string Url)
		{
			string[] UrlComponents = Url.Split(new char[]{'/'});

			if (UrlComponents.Length == 2)
				return "/";

			string ParentUrl = "";
			int i;

			for (i = 0; i < UrlComponents.Length - 1; i++)
			{
				if (UrlComponents[i] == "")
					continue;

				ParentUrl += "/" + UrlComponents[i];
			}

			return ParentUrl;
		}

	}
}
