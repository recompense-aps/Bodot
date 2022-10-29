using McMaster.Extensions.CommandLineUtils;
using System.Reflection;

namespace Bodot
{
	public class BodotCommandLine
	{
		[Option(
			Description = "Displays current bodot info",
			ShortName = "i"
		)]
		public bool Info { get; set; } = false;

		[Option(
			Description = "Initialize workspace with a config",
			ShortName = "ii"
		)]
		public bool Init { get; set; } = false;

		[Option(
			Description = "Configure settings",
			ShortName = "cf"
		)]
		public bool Config { get; set; } = false;

		[Option(
			Description = "Build project",
			ShortName = "b"
		)]
		public bool Build { get; set; } = false;

		[Option(
			Description = "Build create zip archive of built project",
			ShortName = "z"
		)]
		public bool Zip { get; set; } = false;

		[Option(
			Description = "Do not increment patch on build and overwrite existing build if it exists",
			ShortName = "ow"
		)]
		public bool Overwrite { get; set; } = false;

		[Argument(0)]
		public string[]? Args { get; set; }

		public void OnExecute()
		{
			try
			{
				LocalBodotConfig.Load();
			}
			catch (Exception e)
			{
				Output.Error($"Could not load config file, please make sure it is not corrupted: {e.Message}");
				return;
			}

			var commands = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => t.IsClass && t.IsSubclassOf(typeof(Command)))
				.Select(t => Activator.CreateInstance(t) as Command)
				.ToArray()
			;

			foreach(var command in commands)
			{
				if (command?.ShouldExecute(this) == true)
				{
					try
					{
						var (data, success) = command.Execute(this);

						if (!success)
						{
							Output.Out($"Failed to execute {command.Name()}", ConsoleColor.Red, ConsoleColor.Black);
							Output.Out(data ?? "", ConsoleColor.Red, ConsoleColor.Black);
						}
						else if (data != null)
						{
							Output.Out(data, ConsoleColor.Green, ConsoleColor.Black);
						}
					}
					catch(Exception e)
					{
						Output.Out($"Failed to execute {command.Name()}. {e.Message}", ConsoleColor.Red, ConsoleColor.Black);
						Output.Log(e);
					}
				}
			}

			Output.CloseStream();
		}
	}
}