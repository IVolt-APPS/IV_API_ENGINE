using IVolt.Apps.PluginEngine.Interfaces;

namespace IVolt.Apps.PluginEngine
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Interface for execute advanced. </summary>
	///
	/// <remarks>	Markalicz, 4/16/2025. </remarks>
	///
	/// <typeparam name="RT">	Type of the right. </typeparam>
	/// <typeparam name="PT">	Type of the point. </typeparam>
	////////////////////////////////////////////////////////////////////////////////////////////////
	public interface I_Execute_Advanced<RT, PT> : I_IV_Plugin
	{
		
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute();
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes the default plugin behavior. </summary>
		///
		/// <param name="ExecuteOn">	The execute on Date/Time. </param>
		/// <param name="Parameter">	The parameter. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(DateTime ExecuteOn, PT Parameter);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes the default plugin behavior. </summary>
		///
		/// <param name="ExecutionCount">	Number of executions. </param>
		/// <param name="Parameter">			The parameter. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(int ExecutionCount, PT Parameter);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="Parameters">	Options for controlling the operation. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(object[] Parameters);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="RequiredParameter">	Required parameter to pass into the plugin. </param>
		/// <param name="Parameters">				Options for controlling the operation. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(PT RequiredParameter, object[] Parameters);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="RequiredParameter">	Required parameter to pass into the plugin. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(PT RequiredParameter);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="Parameters">	Options for controlling the operation. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		object Execute(Dictionary<string, string> Parameters);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="parameters">	Optional parameters to pass into the plugin. </param>
		/// <param name="selector">  	The selector. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(Dictionary<string, string> parameters, Func<string, RT> selector);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Executes. </summary>
		///
		/// <param name="parameters">	Optional parameters to pass into the plugin. </param>
		/// <param name="selector">  	The selector. </param>
		///
		/// <returns>	A RT. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT Execute(Dictionary<string, string> parameters, Func<Dictionary<string, string>, RT> selector);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   Executes a dynamically provided C# code snippet after applying a string replacement
		///   operation.
		/// </summary>
		///
		/// <param name="code">				 	The C# code as a string. </param>
		/// <param name="replacementFunc">	A function to transform the input code string before
		/// 											execution. </param>
		///
		/// <returns>	The result of executing the modified C# code. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecuteDynamic(string code, Func<string, string> replacementFunc);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   Accepts a C# code snippet, applies replacements, and executes it dynamically.
		/// </summary>
		///
		/// <param name="csharpCode">  	The C# code to execute. </param>
		/// <param name="replacements">	Key-value pairs for placeholder substitution. </param>
		///
		/// <returns>	The result of the executed code. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecuteDynamicCode(string csharpCode, Dictionary<string, string> replacements);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   Executes a dynamically provided C# code snippet after applying a string replacement
		///   operation.
		/// </summary>
		///
		/// <param name="RequiredParameter">	Required parameter to pass into the plugin. </param>
		/// <param name="code">						The C# code as a string. </param>
		/// <param name="replacementFunc">  	A function to transform the input code string before
		/// 												execution. </param>
		///
		/// <returns>	The result of executing the modified C# code. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecuteDynamic(PT RequiredParameter, string code, Func<string, string> replacementFunc);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   Accepts a C# code snippet, applies replacements, and executes it dynamically.
		/// </summary>
		///
		/// <param name="RequiredParameter">	Required parameter to pass into the plugin. </param>
		/// <param name="csharpCode">				The C# code to execute. </param>
		/// <param name="replacements">			Key-value pairs for placeholder substitution. </param>
		///
		/// <returns>	The result of the executed code. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecuteDynamicCode(PT RequiredParameter, string csharpCode, Dictionary<string, string> replacements);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Retrieves and executes a specific plugin version based on identifier. </summary>
		///
		/// <param name="pluginIdentifier">	Fully qualified class name or identifier. </param>
		/// <param name="version">			  	Version number to target (if applicable) </param>
		/// <param name="parameters">		  	(Optional) Optional parameters to pass into the plugin. </param>
		///
		/// <returns>	Result of plugin execution. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecutePluginByVersion(string pluginIdentifier, int version, object[] parameters = null);
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Retrieves and executes a specific plugin version based on identifier. </summary>
		///
		/// <param name="pluginIdentifier"> 	Fully qualified class name or identifier. </param>
		/// <param name="version">					Version number to target (if applicable) </param>
		/// <param name="RequiredParameter">	Required parameter to pass into the plugin. </param>
		/// <param name="parameters">				(Optional) Optional parameters to pass into the plugin. </param>
		///
		/// <returns>	Result of plugin execution. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		RT ExecutePluginByVersion(string pluginIdentifier, int version, PT RequiredParameter, object[] parameters = null);


	}
}
