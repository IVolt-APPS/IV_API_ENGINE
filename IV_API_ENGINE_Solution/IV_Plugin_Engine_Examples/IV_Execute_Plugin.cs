using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVolt.Apps.PluginEngine.Interfaces;
using IVolt.Apps.PluginEngine.Extensions;

namespace IVolt.Examples.PluginEngine
{
	public class IV_Execute_Plugin : I_Execute, I_IV_Plugin
	{
		public int Priority { get => 1; }
		public int Version { get => 1; }

		public void SimpleExecute()
		{
			if (InternalHelpers.IsConsoleApp())
			{
				Console.WriteLine("IV_Execute_Plugin Execute() called");
			}
			
		}

		public object Execute()
		{
			if (InternalHelpers.IsConsoleApp())
			{
				Console.WriteLine("IV_Execute_Plugin Execute() called");
			}
			return null;
		}

		public object Execute(DateTime ExecuteOn, List<object> PArameters)
		{
			if (InternalHelpers.IsConsoleApp())
			{
				Console.WriteLine($"IV_Execute_Plugin Execute(DateTime ExecuteOn, List<object> PArameters) called with ExecuteOn: {ExecuteOn}");
			}
			return null;
		}

		public object Execute(int ExecutionCount, List<object> PArameters)
		{
			throw new NotImplementedException();
		}

		public object Execute(object Parameter)
		{
			throw new NotImplementedException();
		}

		public object Execute(object[] parameters)
		{
			throw new NotImplementedException();
		}

		public object Execute(object Parameter, object[] parameters)
		{
			throw new NotImplementedException();
		}

		public object ExecuteDynamic(string code, Func<string, string> replacementFunc)
		{
			throw new NotImplementedException();
		}

		public object ExecutePlugin(string pluginId, int version, object Parameter = null, object[] parameters = null)
		{
			throw new NotImplementedException();
		}
	}
}
