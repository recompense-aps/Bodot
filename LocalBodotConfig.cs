using Newtonsoft.Json;
namespace Bodot
{
	public class LocalBodotConfig
	{
		[JsonIgnore]
		private const string filename = "./bodot.config";

		[JsonIgnore]
		public static string LastLoadedString { get; private set; } = "";

		[JsonIgnore]
		public static LocalBodotConfig Instance { get; private set;} = new LocalBodotConfig();

		public static void Load()
		{
			if (File.Exists(filename))
			{
				LastLoadedString = File.ReadAllText(filename);
				Instance = JsonConvert.DeserializeObject<LocalBodotConfig>(LastLoadedString) ?? new LocalBodotConfig();
			}	
		}

		public static void Save()
		{
			File.WriteAllText(filename, JsonConvert.SerializeObject(Instance, Formatting.Indented));
		}

		public static void ChangePatch(int amount)
		{
			int num = 0;
			if (int.TryParse(Instance.PatchVersion, out num))
			{
				Instance.PatchVersion = (amount + num).ToString();
			}
		}

		public string ProjectName { get; set; } = "";
		public string MajorVersion { get; set; } = "0";
		public string MinorVersion { get; set; } = "0";
		public string PatchVersion { get; set; } = "0";
		public string? MetaVersion { get; set; }
		public bool AutoIncrementPatch { get; set; } = true;
		public bool UseLog { get; set; } = false;
		public string GodotFilePath { get; set; } = "";
		public string ExportOutputPath { get; set; } = "";
		public string[] FilesToCopyPostBuild { get; set; } = new string[0];
		public string[] DirectoriesToCopyPostBuild { get; set; } = new string[0];
		public string ComputedMetaVersion => MetaVersion != null ? "-" + MetaVersion : "";
		public string SemanticVersion => $"{MajorVersion}.{MinorVersion}.{PatchVersion}{ComputedMetaVersion}";
		public string ComputedExportName => $"{ProjectName}v{SemanticVersion}";
	}
}