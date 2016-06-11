namespace HDT.Plugins.EndGame.Services
{
	internal static class RepositoryFactory
	{
		private static IArchetypeDecksRepository _repository;

		internal static T Create<T>()
		{
			if (_repository == null)
				_repository = new ArchetypeDecksFileRepository();
			return (T)_repository;
		}
	}
}