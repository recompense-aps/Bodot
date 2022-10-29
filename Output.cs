namespace Bodot
{
	public static class Output
	{
		private static StreamWriter? pipedStream;

		public static void Out(params object[] outputs)
		{
			foreach(var output in outputs)
				Console.WriteLine(output);
		}

		public static void Out(ConsoleColor foreGround, ConsoleColor background, params object[] outputs)
		{
			foreach(var output in outputs)
				Out(output, foreGround, background);
		}

		public static void Out(object content, ConsoleColor foreGround = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
		{
			Console.ForegroundColor = foreGround;
			Console.BackgroundColor = background;
			Write(content);
			Console.ResetColor();
		}

		public static void Warn(object message)
		{
			Out($"[!] {message}\n", ConsoleColor.Yellow, ConsoleColor.Black);
		}

		public static void Error(object message)
		{
			Out($"[!] {message}\n", ConsoleColor.Red, ConsoleColor.Black);
		}

		public static void Info(object message)
		{
			Out($"[-] {message}\n", ConsoleColor.Cyan, ConsoleColor.Black);
		}

		public static void Success(object message)
		{
			Out($"[+] {message}\n", ConsoleColor.Green, ConsoleColor.Black);
		}

		public static string Ask(object content)
		{
			Out($"{content} > ");
			return Console.ReadLine() ?? "";
		}

		public static string AskUntil(object content, string failMessage, Func<string, bool> condition)
		{
			Out($"{content} > ");
			var input = Console.ReadLine() ?? "";

			var passed = condition(input);

			if (!passed)
				Out(failMessage + "\n", ConsoleColor.Red, ConsoleColor.Black);

			return passed ? input : AskUntil(content, failMessage, condition);
		}

		public static void Log(object content)
		{
			pipedStream?.Write(content);
		}

		public static void UseStream(StreamWriter writer)
		{
			pipedStream = writer;
		}

		public static void CloseStream()
		{
			pipedStream?.Close();
		}

		private static void Write(object content)
		{
			Console.Write(content);

			pipedStream?.Write(content);
		}
	}
}