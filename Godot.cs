using System.Diagnostics;
using System.IO;

namespace Bodot
{
	public class Godot
	{
		private List<string> args = new List<string>();

		public static Godot Create() => new Godot();

		public void Execute()
		{
			Validate();

			var process = new Process();

			process.StartInfo.FileName = LocalBodotConfig.Instance.GodotFilePath;
			process.StartInfo.Arguments = string.Join(" ", args);
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = false;
			process.StartInfo.RedirectStandardError = false;
			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = false;

			//process.OutputDataReceived += OutputHandler;
			//process.ErrorDataReceived += OutputHandler;

			process.Start();

			if (process.StartInfo.RedirectStandardOutput)
				process.BeginOutputReadLine();

			if (process.StartInfo.RedirectStandardError)
				process.BeginErrorReadLine();
			
			process.WaitForExit();
			process.Close();
		}

		public Godot WithArg(string arg)
		{
			args.Add(arg);
			return this;
		}

		private void Validate()
		{
			if (!File.Exists(LocalBodotConfig.Instance.GodotFilePath))
				throw new Exception($"Could not find godot binary at {LocalBodotConfig.Instance.GodotFilePath}");

			if (!File.Exists("./export_presets.cfg"))
				throw new Exception($"Could not find export_presets.cfg. Please use the godot editor to configure exports.");
		}
	}
}