using BlokScript.Common;

namespace BlokScript.Models
{
	public class SelectFnExpr
	{
		public object Evaluate ()
		{
			return null;
		}

		public string Name;
		public object[] Args;
		public SelectFnExpr OnlyChildSelectFnExpr;
		public SelectFnExpr LeftSelectFnExpr;
		public SelectFnExpr RightSelectFnExpr;
	}
}
