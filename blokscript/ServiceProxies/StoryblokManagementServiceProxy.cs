using System.Collections;

using BlokScript.Entities;
using BlokScript.WebClients;
using BlokScript.PathFactories;
using BlokScript.Serializers;
using BlokScript.ResponseReaders;

namespace BlokScript.ServiceProxies
{
	public class StoryblokManagementServiceProxy
	{
		public SpaceEntity CreateSpace(Hashtable RequestModel)
		{
			_WebClient.PostJson(ManagementPathFactory.SpacesPath(), JsonSerializer.Serialize(RequestModel));

			return new SpaceEntity();
			//return CreateSpaceResponseReader.ReadResponseString(_WebClient.PostJson(ManagementPathFactory.SpacesPath(), JsonSerializer.Serialize(RequestModel)));
		}

		public StoryblokManagementWebClient WebClient
		{
			set
			{
				_WebClient = value;
			}
		}

		private StoryblokManagementWebClient _WebClient;
	}
}
