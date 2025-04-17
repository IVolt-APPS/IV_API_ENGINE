using System;
using System.Runtime.InteropServices;
using System.Text;

namespace IVolt.Core.UI.NativeWindowsHelpers
{
	public static class NativeInputBox
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

		private const uint MB_OKCANCEL = 0x00000001;
		private const uint MB_YESNO = 0x00000004;
		private const uint MB_ICONINFORMATION = 0x00000040;
		private const uint MB_ICONQUESTION = 0x00000020;

		public static bool ShowMessage(string message, string title)
		{
			return MessageBox(IntPtr.Zero, message, title, MB_OKCANCEL | MB_ICONINFORMATION) == 1;
		}

		public static bool ShowYesNoDialog(string message, string title)
		{
			return MessageBox(IntPtr.Zero, message, title, MB_YESNO | MB_ICONQUESTION) == 6; // IDYES = 6
		}

		public static string ShowTextInput(string prompt, string title, string defaultValue = "")
		{
			string input = null;
			var script = $"Set WshShell = WScript.CreateObject(\"WScript.Shell\")\n" +
							 $"result = InputBox(\"{prompt}\", \"{title}\", \"{defaultValue}\")\n" +
							 "Wscript.Echo result";

			var tempFile = System.IO.Path.GetTempFileName() + ".vbs";
			System.IO.File.WriteAllText(tempFile, script, Encoding.Unicode);
			try
			{
				var process = new System.Diagnostics.Process
				{
					StartInfo = new System.Diagnostics.ProcessStartInfo("cscript", $"//Nologo \"{tempFile}\"")
					{
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true
					}
				};
				process.Start();
				input = process.StandardOutput.ReadToEnd().Trim();
				process.WaitForExit();
			}
			finally
			{
				System.IO.File.Delete(tempFile);
			}
			return input;
		}


	}
}