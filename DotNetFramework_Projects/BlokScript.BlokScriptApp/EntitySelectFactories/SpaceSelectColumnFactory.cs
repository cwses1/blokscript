using System;
using System.Collections.Generic;

using BlokScript.Entities;
using BlokScript.Models;
using BlokScript.Common;
using BlokScript.EntityReferences;

namespace BlokScript.EntitySelectFactories
{
	public class SpaceSelectColumnFactory
	{
		public static SelectColumn[] CreateSelectColumns (SelectFieldExpr FieldExpr)
		{
			List<SelectColumn> SelectColumnList = new List<SelectColumn>();

			if (FieldExpr.Name == "*")
				SelectColumnList.AddRange(CreateSelectColumnsForSelectStar());
			else
				SelectColumnList.Add(CreateSelectColumnForFieldExpr(FieldExpr));

			return SelectColumnList.ToArray();
		}

		public static SelectColumn[] CreateSelectColumnsForSelectStar ()
		{
			List<SelectColumn> SelectColumnList = new List<SelectColumn>();

			foreach (string FieldName in SpaceReference.SelectStarFields)
			{
				SelectFieldExpr FabricatedFieldExpr = new SelectFieldExpr();
				FabricatedFieldExpr.Name = FieldName;
				SelectColumnList.Add(CreateSelectColumnForFieldExpr(FabricatedFieldExpr));
			}

			return SelectColumnList.ToArray();
		}

		public static SelectColumn CreateSelectColumnForFieldExpr (SelectFieldExpr FieldExpr)
		{
			SelectColumn Column = new SelectColumn();
			Column.Name = FieldExpr.Name;
			Column.DisplayName = FieldExpr.DisplayName != null ? FieldExpr.DisplayName : FieldExpr.Name;
			Column.Type = SpaceReference.GetTypeForFieldName(FieldExpr.Name);
			return Column;
		}
	}
}
