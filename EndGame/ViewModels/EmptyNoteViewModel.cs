namespace HDT.Plugins.EndGame.ViewModels
{
	public class EmptyNoteViewModel : NoteViewModelBase
	{
		private string _message;

		public string Message
		{
			get { return _message; }
			set { Set(() => Message, ref _message, value); }
		}

		public EmptyNoteViewModel()
		{
			Message = "Not Available";
		}

		public override void Update()
		{
		}
	}
}