using System;
using System.Collections.Generic;

using BlokScript.Entities;
using BlokScript.Models;
using BlokScript.Common;
using BlokScript.Extractors;

namespace BlokScript.EntitySelectFactories
{
	public class SpaceSelectFieldFactory
	{
		public static SelectField CreateSelectField (SpaceEntity Space, SelectColumn Column)
		{
			SelectField CreatedField = new SelectField();
			CreatedField.Column = Column;
			CreatedField.FieldValue = JsonExtractor.ExtractNativeValue(Space.Data, Column.Name);
			return CreatedField;
		}
	}
}
