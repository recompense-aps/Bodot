namespace Bodot
{
	public class CommandException : Exception 
	{ 
		public CommandException(string message) : base(message) { }
	}
	public abstract class Command
	{
		public abstract string Name();
		public abstract (object? data, bool success) Execute(BodotCommandLine commandLine);
		public abstract bool ShouldExecute(BodotCommandLine commandLine);
		protected bool IsSet(string? thing) => !string.IsNullOrWhiteSpace(thing);
		protected string Get(string? thing) => thing ?? "";
		protected void Assert(bool cond, string errorMessage)
		{
			if (!cond)
				throw new CommandException(errorMessage);
		}
	}
}