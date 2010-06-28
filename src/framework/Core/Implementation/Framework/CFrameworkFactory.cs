using System;
using System.Collections.Generic;
using System.Text;
using framework.Core.Implementation;
using System.Reflection;

namespace framework.Core
{
	public class CFrameworkFactory : IFrameworkFactory
	{
		//////////////////////////////////////////////////////////////////////////

		public IFramework NewFramework(FrameworkConfig configuration)
		{
			if (configuration == null)
				configuration = new FrameworkConfig();

			CManifest manifest = new CManifest();

			manifest.SymbolicName = "Sekai Framework";
			manifest.Version = Assembly.GetExecutingAssembly().GetName().Version;
			manifest.AssemblyPath = Assembly.GetExecutingAssembly().Location;

			CSystemBundle sys_bundle = new CSystemBundle(configuration, manifest);
			return sys_bundle;
		}

		//////////////////////////////////////////////////////////////////////////
	}
}
