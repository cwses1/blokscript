using System.Collections;
using BlokScript.RequestModels;
using BlokScript.Models;

namespace BlokScript.RequestModelFactories
{
	public class CreateSpaceRequestModelFactory
	{
		public static Hashtable CreateRequestModel (string SpaceName)
		{
			Hashtable RequestModel = new Hashtable();
			RequestModel["name"] = SpaceName;
			return RequestModel;
		}

		public static Hashtable CreateRequestModel (UpdateModel[] SpaceUpdateModels)
		{
			Hashtable RequestModel = new Hashtable();
			return RequestModel;
		}
	}
}
