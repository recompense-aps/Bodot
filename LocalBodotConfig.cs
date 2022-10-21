
namespace Bodot
{
	public class LocalBodotConfig
	{
		public string MajorVersion { get; set; } = "0";
		public string MinorVersion { get; set; } = "0";
		public string PatchVersion { get; set; } = "0";
		public string? MetaVersion { get; set; }
		public bool AutoIncrementPath { get; set; } = true;
		public string ComputedMetaVersion => MetaVersion != null ? "-" + MetaVersion : "";
		public string SemanticVersion => $"{MajorVersion}.{MinorVersion}.{PatchVersion}{ComputedMetaVersion}";
	}
}