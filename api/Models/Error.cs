namespace backend.Models
{
	public readonly struct Error
	{
		public readonly string Description;
		public readonly string Name;
		public readonly int Id;

		public Error(string Name, string Description, int Id)
		{
			this.Name = Name;
			this.Description = Description;
			this.Id = Id;
		}
	}

	public class Errors
	{
		Error DBCreationError = new Error("DATABASE_ERROR", "there was an error while creating the database", 1);
		public Error DbCreationError { get { return DBCreationError; } }
	}

}

