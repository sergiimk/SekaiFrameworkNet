using System;
using System.Collections.Generic;
using System.Text;
using framework.Core.Implementation;

namespace framework.Core
{
	public class CFrameworkFactory : IFrameworkFactory
	{
		//////////////////////////////////////////////////////////////////////////

		public IFramework NewFramework(object configuration)
		{
			CSystemBundle sys_bundle = new CSystemBundle();
			return sys_bundle;
		}

		//////////////////////////////////////////////////////////////////////////
	}
}
