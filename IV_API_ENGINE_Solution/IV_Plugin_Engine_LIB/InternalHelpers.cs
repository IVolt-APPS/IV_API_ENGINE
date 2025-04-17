

using System;
using System.Diagnostics;
using System.Reflection;


namespace IVolt.Apps.PluginEngine.Extensions
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	An internal helpers. </summary>
	///
	/// <remarks>	Markalicz, 4/16/2025. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////
	public static class InternalHelpers
	{
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	A string extension method that queries if a given file exists. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <param name="FilePath">	The path to save or append the file. </param>
		///
		/// <returns>	True if it succeeds, false if it fails. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static bool FileExists(this string FilePath)
		{
			if (File.Exists(FilePath)) { return true; }
			else { return false; }
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	A string extension method that null or empty. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <param name="StringToTest">	The StringToTest to act on. </param>
		///
		/// <returns>	True if it succeeds, false if it fails. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static bool NullOrEmpty(this string StringToTest)
		{
			if (String.IsNullOrEmpty(StringToTest)) { return true; }
			else { return false; }
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   Saves the provided data to the specified file path. If the file exists, it appends with a
		///   timestamp header.
		/// </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <exception cref="ArgumentException">	Thrown when one or more arguments have unsupported or
		/// 													illegal values. </exception>
		///
		/// <param name="FilePath">  	The path to save or append the file. </param>
		/// <param name="DataToSave">	The content to write or append. </param>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static void SaveToFile(this string FilePath, string DataToSave)
		{
			if (string.IsNullOrWhiteSpace(FilePath))
				throw new ArgumentException("FilePath cannot be null or empty.", nameof(FilePath));

			string directory = Path.GetDirectoryName(FilePath);
			if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			if (File.Exists(FilePath))
			{
				string timestampHeader = $"\r\n\r\n****** {DateTime.Now} ******\r\n\r\n";
				File.AppendAllText(FilePath, timestampHeader + DataToSave);
			}
			else
			{
				File.WriteAllText(FilePath, DataToSave);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	A string extension method that logs an error. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <param name="ErrorMessage">	The ErrorMessage to act on. </param>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static void LogError(this string ErrorMessage)
		{
			string FileToLogTo = GetFileNameForLog();
			ErrorMessage.SaveToFile(FileToLogTo);
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets file name for log. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <returns>	The file name for log. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		static string GetFileNameForLog()
		{
			string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Logs");
			Directory.CreateDirectory(logsDirectory);

			// Find all *_Log.txt files
			var logFiles = Directory.GetFiles(logsDirectory, "*_Log.txt")
											.Select(f => new FileInfo(f))
											.OrderByDescending(f => f.CreationTime)
											.ToList();

			FileInfo latestLogFile = logFiles.FirstOrDefault();

			if (latestLogFile != null && (DateTime.Now - latestLogFile.CreationTime).TotalDays <= 60)
			{
				// Reuse existing recent file
				return latestLogFile.FullName;
			}

			// Create a new log file
			string newFileName = $"{DateTime.Now:MM-yyyy-mm}_Log.txt";
			string newFilePath = Path.Combine(logsDirectory, newFileName);

			File.Create(newFilePath).Dispose(); // Create and close
			return newFilePath;
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	An application environment. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		/////////////////////////////////////////////////////////////////////////////////////////////

		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Query if this object is console application. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <returns>	True if console application, false if not. </returns>
		//////////////////////////////////////////////////////////////////////////////////////////
		public static bool IsConsoleApp()
		{
			try
			{
				var entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly == null)
					return false;

				var subsystem = GetSubsystem(entryAssembly.Location);
				return subsystem == ImageSubsystem.WindowsCUI; // Console
			}
			catch
			{
				return false;
			}
		}

		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Values that represent image subsystems. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		//////////////////////////////////////////////////////////////////////////////////////////
		private enum ImageSubsystem : ushort
		{
			/// <summary>	An enum constant representing the unknown option. </summary>
			Unknown = 0,
			/// <summary>	An enum constant representing the native option. </summary>
			Native = 1,
			/// <summary>	An enum constant representing the windows Graphical user interface option. </summary>
			WindowsGUI = 2,
			/// <summary>	An enum constant representing the windows cui option. </summary>
			WindowsCUI = 3,
			/// <summary>	An enum constant representing the Operating system 2 cui option. </summary>
			OS2CUI = 5,
			/// <summary>	An enum constant representing the posix cui option. </summary>
			PosixCUI = 7,
			/// <summary>	An enum constant representing the native windows option. </summary>
			NativeWindows = 8,
			/// <summary>	An enum constant representing the windows cegui option. </summary>
			WindowsCEGUI = 9,
			/// <summary>	An enum constant representing the efi application option. </summary>
			EFIApplication = 10,
			/// <summary>	An enum constant representing the ef iboot service driver option. </summary>
			EFIbootServiceDriver = 11,
			/// <summary>	An enum constant representing the efi runtime driver option. </summary>
			EFIRuntimeDriver = 12,
			/// <summary>	An enum constant representing the efirom option. </summary>
			EFIROM = 13,
			/// <summary>	An enum constant representing the xbox option. </summary>
			Xbox = 14,
			/// <summary>	An enum constant representing the windows boot application option. </summary>
			WindowsBootApplication = 16
		}

		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets a subsystem. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <param name="exePath">	Full pathname of the executable file. </param>
		///
		/// <returns>	The subsystem. </returns>
		//////////////////////////////////////////////////////////////////////////////////////////
		private static ImageSubsystem GetSubsystem(string exePath)
		{
			using var fs = new FileStream(exePath, FileMode.Open, FileAccess.Read);
			using var reader = new BinaryReader(fs);

			fs.Seek(0x3C, SeekOrigin.Begin);
			int peHeaderOffset = reader.ReadInt32();
			fs.Seek(peHeaderOffset + 0x5C, SeekOrigin.Begin); // offset to subsystem in PE header

			return (ImageSubsystem)reader.ReadUInt16();
		}


	}
}
