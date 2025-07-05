namespace BlokScript.RequestModels
{
	public class CreateSpaceRequestModel
	{
		public string Name;
		public string Domain;
		public string Story_published_hook;
		public string[] Environments;
		public int SearchblokId;
		public bool HasPendingTasks;
	}
}
