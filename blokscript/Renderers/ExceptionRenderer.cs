using System.Text;

namespace BlokScript.Renderers
{
	public class ExceptionRenderer
	{
		public static string RenderFullStackTrace(Exception E)
		{
			StringBuilder StackTraceBuilder = new StringBuilder();
			StackTraceBuilder.AppendLine(E.StackTrace);

			if (E.InnerException != null)
				StackTraceBuilder.AppendLine(RenderFullStackTrace(E.InnerException));

			return StackTraceBuilder.ToString();
		}
	}
}
