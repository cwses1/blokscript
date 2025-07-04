using System.IO;
using System.Text;

namespace BlokScript.IO
{
	public class StreamCopier
	{
		public static void Copy(Stream SourceStream, Stream TargetStream)
		{
			int BytesRead;
			byte[] Buffer = new byte[1024];

			while ((BytesRead = SourceStream.Read(Buffer, 0, Buffer.Length)) > 0)
				TargetStream.Write(Buffer, 0, BytesRead);
		}

		public static byte[] CopyToNewByteArray(Stream SourceStream)
		{
			using (MemoryStream TargetStream = new MemoryStream())
			{
				Copy(SourceStream, TargetStream);
				return TargetStream.ToArray();
			}
		}

		public static void CopyByteArrayToStream(byte[] SourceBytes, Stream TargetStream)
		{
			using (MemoryStream SourceStream = new MemoryStream(SourceBytes))
			{
				Copy(SourceStream, TargetStream);
			}
		}
		
		public static string CopyStreamToString (Stream SourceStream)
		{
			return Encoding.UTF8.GetString(CopyToNewByteArray(SourceStream));
		}
	}
}
