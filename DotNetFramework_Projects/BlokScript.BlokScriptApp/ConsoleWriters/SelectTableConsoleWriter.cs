using System;

using BlokScript.Models;
using BlokScript.StringWriters;

namespace BlokScript.ConsoleWriters
{
	public class SelectTableConsoleWriter
	{
		public static void Write (SelectTable Table)
		{
			Console.WriteLine(SelectTableStringWriter.Write(Table));
		}
	}
}
