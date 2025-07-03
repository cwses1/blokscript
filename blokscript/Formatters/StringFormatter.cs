using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlokScript.Formatters
{
	public class StringFormatter
	{
		public static string FormatSlug (string Param)
		{
			return Param.ToLower().Replace(' ', '-');
		}
	}
}
