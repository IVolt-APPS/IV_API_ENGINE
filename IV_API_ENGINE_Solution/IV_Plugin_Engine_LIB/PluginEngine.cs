using IVolt.Apps.PluginEngine.Interfaces;
using System.Reflection;
using LogErr = IVolt.Apps.PluginEngine.ErrorLogger;

namespace IVolt.Apps.PluginEngine
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	A plugin engine. </summary>
	///
	/// <remarks>	Markalicz, 4/16/2025. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////
	public static class PluginEngine
	{
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets or sets the loaded plugins. </summary>
		///
		/// <value>	The loaded plugins. </value>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static List<Plugin_Information> LoadedPlugins { get; private set; } = new();
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Loads all built in pluginsa. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <exception cref="FileNotFoundException">	Thrown when the requested file is not present. </exception>
		///
		/// <param name="GetLatest">	(Optional) True to get latest. </param>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static void LoadAll_BuiltIn_Pluginsa(bool GetLatest = false)
		{
			string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Plugins", "IVolt_Plugins.dll");

			if (!File.Exists(pluginPath))
			{
				var ex = new FileNotFoundException("Missing IVolt_Plugins.dll", pluginPath);
				LogErr.LogError("Plugin file not found.", ex);
				throw ex;
			}

			try
			{

				Assembly asm = Assembly.LoadFrom(pluginPath);
				var types = asm.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();

				var interfaces = types.SelectMany(t => t.GetInterfaces()).Distinct().ToList();

				foreach (var type in types)
				{
					var typeinterfaces = type.GetInterfaces();

					if (typeinterfaces.Length == 0)
					{
						LogErr.LogError($"No interfaces found for type {type.FullName}", null);
						continue;
					}

					if (typeinterfaces.Contains(typeof(I_IV_Plugin)) == false)
					{
						LogErr.LogError($"Type {type.FullName} does not implement I_IV_Plugin", null);
						continue;
					}

					// Safely instantiate the plugin
					if (!(Activator.CreateInstance(type) is I_IV_Plugin pluginInstance))
					{
						LogErr.LogError($"Failed to instantiate {type.FullName} as I_IV_Plugin", null);
						continue;
					}

					var pluginInfo = new Plugin_Information
					{
						FileName = Path.GetFileName(pluginPath),
						FullFilePath = pluginPath,
						Version = pluginInstance.Version,
						Priority = pluginInstance.Priority,
						PluginInterface_Implementations = new Dictionary<Type, List<string>>()
					};

					foreach (var iface in typeinterfaces)
					{
						if (!pluginInfo.PluginInterface_Implementations.ContainsKey(iface))
						{
							pluginInfo.PluginInterface_Implementations[iface] = new List<string>();
						}

						pluginInfo.PluginInterface_Implementations[iface].Add(type.FullName);
					}

					LoadedPlugins.Add(pluginInfo);
				}
			}
			catch (Exception ex)
			{
				LogErr.LogError("Plugin loading failed.", ex);
				throw;
			}

			if (LoadedPlugins.Count == 0) { LogErr.LogError("No Plugins Found", null); }
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Loads all built in plugins. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static void LoadAll_BuiltIn_Plugins()
		{
			string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Plugins");

			if (!Directory.Exists(pluginDirectory))
			{
				LogErr.LogError("Plugin directory does not exist.", null);
				return;
			}

			string[] dllFiles = Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly);
			if (dllFiles.Length == 0)
			{
				LogErr.LogError("No DLL files found in plugin directory.", null);
				return;
			}

			foreach (var pluginPath in dllFiles)
			{
				try
				{
					Assembly asm = Assembly.LoadFrom(pluginPath);
					var types = asm.GetTypes()
						 .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic)
						 .ToList();

					foreach (var type in types)
					{
						var typeInterfaces = type.GetInterfaces();

						if (!typeInterfaces.Contains(typeof(I_IV_Plugin)))
						{
							LogErr.LogError($"Type {type.FullName} does not implement I_IV_Plugin", null);
							continue;
						}

						// Try to instantiate the plugin
						if (!(Activator.CreateInstance(type) is I_IV_Plugin pluginInstance))
						{
							LogErr.LogError($"Failed to instantiate {type.FullName} as I_IV_Plugin", null);
							continue;
						}

						var pluginInfo = new Plugin_Information
						{
							FileName = Path.GetFileName(pluginPath),
							FullFilePath = pluginPath,
							Version = pluginInstance.Version,
							Priority = pluginInstance.Priority,
							PluginInterface_Implementations = new Dictionary<Type, List<string>>()
						};

						foreach (var iface in typeInterfaces)
						{
							if (!pluginInfo.PluginInterface_Implementations.ContainsKey(iface))
								pluginInfo.PluginInterface_Implementations[iface] = new List<string>();

							pluginInfo.PluginInterface_Implementations[iface].Add(type.FullName);
						}

						LoadedPlugins.Add(pluginInfo);
					}
				}
				catch (ReflectionTypeLoadException rtle)
				{
					string errors = string.Join("\n", rtle.LoaderExceptions.Select(e => e.Message));
					LogErr.LogError($"Error loading types from {pluginPath}", new Exception(errors));
				}
				catch (Exception ex)
				{
					LogErr.LogError($"Failed to process plugin: {pluginPath}", ex);
				}
			}

			if (LoadedPlugins.Count == 0)
			{
				LogErr.LogError("No valid plugins found after scanning all DLLs.", null);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets a plugin. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <typeparam name="T">	Generic type parameter. </typeparam>
		/// <param name="SpecificVersion">	(Optional) The specific version. </param>
		///
		/// <returns>	The plugin. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static T GetPlugin<T>(int? SpecificVersion = null) where T : class
		{
			var interfaceType = typeof(T);

			// Filter plugins that implement the desired interface
			var matchingPlugins = LoadedPlugins
				 .Where(p => p.PluginInterface_Implementations.ContainsKey(interfaceType))
				 .ToList();

			if (matchingPlugins.Count == 0)
			{
				LogErr.LogError($"No plugins implement interface: {interfaceType.FullName}", null);
				return null;
			}

			// Filter by version if specified
			if (SpecificVersion.HasValue)
			{
				matchingPlugins = matchingPlugins
					 .Where(p => p.Version == SpecificVersion.Value)
					 .ToList();

				if (matchingPlugins.Count == 0)
				{
					LogErr.LogError($"No plugins implementing {interfaceType.FullName} with version {SpecificVersion.Value}", null);
					return null;
				}
			}

			// Pick highest priority (or first if equal)
			var selectedPlugin = matchingPlugins
				 .OrderByDescending(p => p.Priority)
				 .ThenByDescending(p => p.Version)
				 .FirstOrDefault();

			var typeName = selectedPlugin.PluginInterface_Implementations[interfaceType].FirstOrDefault();
			if (typeName == null)
			{
				LogErr.LogError($"No type found for interface {interfaceType.FullName} in selected plugin.", null);
				return null;
			}

			try
			{
				var type = Type.GetType(typeName) ??
							  AppDomain.CurrentDomain.GetAssemblies()
								  .SelectMany(a => a.GetTypes())
								  .FirstOrDefault(t => t.FullName == typeName);

				if (type == null)
				{
					LogErr.LogError($"Type not found in any loaded assembly: {typeName}", null);
					return null;
				}

				var instance = Activator.CreateInstance(type) as T;
				return instance;
			}
			catch (Exception ex)
			{
				LogErr.LogError($"Failed to create instance of {typeName}", ex);
				return null;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets the plugins. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <typeparam name="T">	Generic type parameter. </typeparam>
		///
		/// <returns>	The plugins. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static List<T> GetPlugins<T>() where T : class
		{
			var interfaceType = typeof(T);

			return LoadedPlugins
				 .Where(p => p.PluginInterface_Implementations.ContainsKey(interfaceType))
				 .SelectMany(p => p.PluginInterface_Implementations[interfaceType])
				 .Select(typeName => CreateInstance<T>(typeName))
				 .Where(instance => instance != null)
				 .ToList();
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets the plugins. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <typeparam name="T"> 	Generic type parameter. </typeparam>
		/// <typeparam name="T2">	Generic type parameter. </typeparam>
		///
		/// <returns>	The plugins. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		public static List<T> GetPlugins<T, T2>() where T : class where T2 : class
		{
			var interfaceType1 = typeof(T);
			var interfaceType2 = typeof(T2);

			return LoadedPlugins
				 .Where(p =>
					  p.PluginInterface_Implementations.ContainsKey(interfaceType1) &&
					  p.PluginInterface_Implementations.ContainsKey(interfaceType2))
				 .SelectMany(p =>
					  p.PluginInterface_Implementations[interfaceType1]
							.Intersect(p.PluginInterface_Implementations[interfaceType2]))
				 .Select(typeName => CreateInstance<T>(typeName))
				 .Where(instance => instance != null)
				 .ToList();
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Creates an instance. </summary>
		///
		/// <remarks>	Markalicz, 4/16/2025. </remarks>
		///
		/// <typeparam name="T">	Generic type parameter. </typeparam>
		/// <param name="typeName">	Name of the type. </param>
		///
		/// <returns>	The new instance. </returns>
		/////////////////////////////////////////////////////////////////////////////////////////////
		private static T CreateInstance<T>(string typeName) where T : class
		{
			try
			{
				var type = Type.GetType(typeName) ??
							  AppDomain.CurrentDomain.GetAssemblies()
									.SelectMany(a => a.GetTypes())
									.FirstOrDefault(t => t.FullName == typeName);

				if (type == null || !typeof(T).IsAssignableFrom(type))
					return null;

				return Activator.CreateInstance(type) as T;
			}
			catch (Exception ex)
			{
				LogErr.LogError($"Failed to create instance of {typeName}", ex);
				return null;
			}
		}

	}
}



