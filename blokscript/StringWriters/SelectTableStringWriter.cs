using System;
using System.IO;
using System.Globalization;

using BlokScript.Models;

using CsvHelper;
using CsvHelper.Configuration;

namespace BlokScript.StringWriters
{
	public class SelectTableStringWriter
	{
		public static string Write (SelectTable Table)
		{
			using (StringWriter TargetStringWriter = new StringWriter())
			{
				CsvConfiguration Config = new CsvConfiguration(CultureInfo.CurrentCulture);

				using (CsvWriter TargetCsvWriter = new CsvWriter(TargetStringWriter, Config))
				{
					foreach (SelectColumn Column in Table.Columns)
					{
						TargetCsvWriter.WriteField(Column.DisplayName);
					}

					TargetCsvWriter.NextRecord();

					foreach (SelectField[] Fields in Table.Rows)
					{
						foreach (SelectField Field in Fields)
						{
							TargetCsvWriter.WriteField($"{Field.FieldValue}");
						}

						TargetCsvWriter.NextRecord();
					}
				}

				return TargetStringWriter.ToString();
			}
		}
	}
}
