
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVolt.API.Engine
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	A default menu item CLI methods. </summary>
	///
	/// <remarks>	Markalicz, 4/15/2025. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////
	internal static  class DefaultMenuItem_CLI_Methods
	{
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes the 'method' operation. </summary>
		///
		/// <remarks>	Markalicz, 4/15/2025. </remarks>
		///
		/// <param name="Name">	The name. </param>
		/////////////////////////////////////////////////////////////////////////////////////////////
		internal static void RunMethod(string Name)
		{
			if (Name.ToLower() == "exit")
			{
				Console.WriteLine("Exiting..."); Console.ReadKey();
				Environment.Exit(0);
			}
			else if (Name.ToLower() == "help")
			{
				Console.WriteLine("Displaying help...");
				Console.ReadKey();// Implement help logic here
			}
			else
			{
				Console.WriteLine($"Method Not Found: {Name}");
				Console.ReadKey();
			}
		}


	}
}
