using System;
using McMaster.Extensions.CommandLineUtils;
namespace Bodot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CommandLineApplication.Execute<BodotCommandLine>(args);
		}
	}
}