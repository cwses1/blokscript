using System.IO;

namespace BlokScript.IO
{
	public class FileCopier
	{
		public static byte[] CopyToNewByteArray (string FilePath)
		{
			using (FileStream SourceStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
			{
				return StreamCopier.CopyToNewByteArray(SourceStream);
			}
		}
	}
}
