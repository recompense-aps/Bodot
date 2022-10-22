using System.Linq;
using System.IO;
using System.IO.Compression;
using static Bodot.Output;

namespace Bodot
{
	public class InfoCommand : Command
	{
		public override string Name() => nameof(InfoCommand);
		public override bool ShouldExecute(BodotCommandLine commandLine) => commandLine.Info;
		public override (object? data, bool success) Execute(BodotCommandLine commandLine)
		{
			Out(
				$"|-----------|",
				$"|---Bodot---|",
				$"|--v0.0.0---|",
				$"|-----------|"
			);

			return (null, true);
		}
		
	}

	public class InitCommand : Command
	{
		public override string Name() => nameof(InitCommand);
		public override bool ShouldExecute(BodotCommandLine commandLine) => commandLine.Init;
		public override (object? data, bool success) Execute(BodotCommandLine commandLine)
		{
			var config = LocalBodotConfig.Instance;

			config.ProjectName = Ask("What is the name of your project?").Trim();
			config.MetaVersion = Ask("What is the meta version of your project?").Trim();
			config.GodotFilePath = AskUntil(
				"What is the path to your godot binary?", 
				"Could not find a valid godot binary at specified path, please try again",
				input => File.Exists(input.Trim())
			).Trim();
			config.ExportOutputPath = AskUntil(
				"What is the path to export your project to?", 
				"Could not find the specified directory, please try again",
				input => Directory.Exists(input.Trim())
			).Trim();

			LocalBodotConfig.Save();

			return ("Successfully configured project", true);
		}
		
	}

	public class ConfigCommand : Command
	{
		private static readonly Dictionary<string, Action<string>> settings = new Dictionary<string, Action<string>>()
		{
			{ "UseLog", value => LocalBodotConfig.Instance.UseLog = value.ToLower() == "true" },
			{ "GodotFilePath", value => LocalBodotConfig.Instance.GodotFilePath = value },
			{ "MetaVersion", value => LocalBodotConfig.Instance.MetaVersion = value },
			{ "AutoIncrementPath", value => LocalBodotConfig.Instance.AutoIncrementPatch = value.ToLower() == "true" }
		};

		public override string Name() => nameof(ConfigCommand);
		public override bool ShouldExecute(BodotCommandLine commandLine) => commandLine.Config;
		public override (object? data, bool success) Execute(BodotCommandLine commandLine)
		{
			var setting = commandLine.Args?.ElementAtOrDefault(0);
			var value = commandLine.Args?.ElementAtOrDefault(1);

			Assert(IsSet(setting), "missing config setting");
			Assert(IsSet(value), "missing config value");

			setting = Get(setting);
			value = Get(value);

			Assert(settings.ContainsKey(setting), $"'{setting}' is not configurable or it doesn't exist");
			
			settings[setting](value);

			LocalBodotConfig.Save();

			return ($"Configured {setting}={value}", true);
		}
		
	}

	public class BuildCommand : Command
	{
		public override string Name() => nameof(BuildCommand);
		public override bool ShouldExecute(BodotCommandLine commandLine) => commandLine.Build;
		public override (object? data, bool success) Execute(BodotCommandLine commandLine)
		{
			var root = $"./{LocalBodotConfig.Instance.ExportOutputPath}/{LocalBodotConfig.Instance.SemanticVersion}";

			Directory.CreateDirectory(root);

			Godot.Create()
				.WithArg("--export")
				.WithArg("--no-window")
				.WithArg("\"Windows Desktop\"")
				.WithArg($"{root}/{LocalBodotConfig.Instance.ComputedExportName}.exe")
				.Execute();
			
			if (commandLine.Zip)
			{
				Out("Creating zip archive...");
				ZipFile.CreateFromDirectory(root, $"./{LocalBodotConfig.Instance.ExportOutputPath}/{LocalBodotConfig.Instance.SemanticVersion}.zip");
			}

			if (LocalBodotConfig.Instance.AutoIncrementPatch)
			{
				LocalBodotConfig.ChangePatch(1);
				LocalBodotConfig.Save();
			}

			return (null, true);
		}
		
	}
}