
namespace IVolt.API.Engine.CLI
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	A CLI menu system. </summary>
	///
	/// <remarks>	Markalicz, 4/15/2025. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////
	public static class CLI_MenuSystem
	{
		/// <summary>	The active menu. </summary>
		static IV_CLI_Menu_Engine _ActiveMenu;
		/// <summary>	The default menu JSON. </summary>
		static string _defaultMenuJSON = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Data\JSON_Templates\IV_CLI_Menu_System\ivolt_api_menu.json";
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets or sets the default menu JSON. </summary>
		///
		/// <value>	The default menu JSON. </value>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string DefaultMenuJSON
		{
			get { return _defaultMenuJSON; }
			set
			{
				if (File.Exists(value) == false)
				{
					_defaultMenuJSON = "";
					Console.WriteLine("The file does not exist. [" + value + "]");
					Console.ReadKey();
					Environment.Exit(-1);
				}
				try
				{
					_ActiveMenu = IV_CLI_Menu_Engine.FromJson(File.ReadAllText(value));
				}
				catch (Exception ex)
				{
					_defaultMenuJSON = "";
					Console.WriteLine(ex.Message);
					Console.ReadKey();
					Environment.Exit(-1);
				}
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	A program. </summary>
		///
		/// <remarks>	Markalicz, 4/15/2025. </remarks>
		/////////////////////////////////////////////////////////////////////////////////////////////
		internal class Program
		{
			/// <summary>	The current level. </summary>
			static int _CurrentLevel = 1;
			//////////////////////////////////////////////////////////////////////////////////////////
			/// <summary>	Main entry-point for this application. </summary>
			///
			/// <remarks>	Markalicz, 4/15/2025. </remarks>
			///
			/// <param name="args">	An array of command-line argument strings. </param>
			//////////////////////////////////////////////////////////////////////////////////////////
			static void Main(string[] args)
			{
				if (args.Length == 2)
				{
					if (args[1].ToLower().EndsWith(".json"))
					{
						DefaultMenuJSON = args[1];
					}
				}

				_ActiveMenu = IV_CLI_Menu_Engine.FromJson(File.ReadAllText(_defaultMenuJSON));

				while (_ActiveMenu.Continue)
				{
					_ActiveMenu.Draw(_ActiveMenu, _CurrentLevel);
					var selected = _ActiveMenu.GetByLevelAndText(_CurrentLevel, Console.ReadLine());

					if (selected != null)
					{
						Console.WriteLine($"Selected: {selected.Longtext}");

						if (selected.FunctionName != null && selected.FunctionName != "")
						{
							try { DefaultMenuItem_CLI_Methods.RunMethod(selected.FunctionName); }
							catch (Exception ex)
							{
								Console.WriteLine(ex.Message);
								Console.ReadKey();
								Environment.Exit(-1);
							}
						}

						if (int.TryParse(selected.Jumptomenulevel, out int newLevel))
						{
							_CurrentLevel = newLevel;
							continue;
						}
					}

				}
			}
		}
	}
}