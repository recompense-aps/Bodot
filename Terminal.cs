using System.Diagnostics;
using System.IO;

namespace Bodot
{
	public class Terminal
	{
		private List<string> args = new List<string>();
		private string fileName = "";

		public static Terminal Create() => new Terminal();

		public void Execute()
		{
			Validate();

			var process = new Process();

			process.StartInfo.FileName = fileName;
			process.StartInfo.Arguments = string.Join(" ", args);
			process.StartInfo.UseShellExecute = string.IsNullOrWhiteSpace(fileName);
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

		public Terminal WithArg(string arg)
		{
			args.Add(arg);
			return this;
		}

		public Terminal AsGodot()
		{
			fileName = LocalBodotConfig.Instance.GodotFilePath;
			return this;
		}

		public Terminal AsBash()
		{
			fileName = "bash";
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