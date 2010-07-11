using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace framework.Core.Implementation
{
	//////////////////////////////////////////////////////////////////////////

	class FrameworkService : Attribute
	{
	}

	//////////////////////////////////////////////////////////////////////////

	class CSystemBundleActivator : IBundleActivator
	{
		//////////////////////////////////////////////////////////////////////////

		public void Start(IBundleContext context)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			Type[] types = asm.GetTypes();
			foreach (Type t in types)
			{
				object[] attrs = t.GetCustomAttributes(typeof(FrameworkService), false);
				foreach (object attr in attrs)
				{
					FrameworkService svc_attr = (FrameworkService)attr;
					context.RegisterService(t.FullName, asm.CreateInstance(t.FullName));
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void Stop(IBundleContext context)
		{
		}

		//////////////////////////////////////////////////////////////////////////
	}
}
