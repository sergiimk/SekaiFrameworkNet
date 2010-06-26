using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation.Framework
{
	class CFrameworkFactory : IFrameworkFactory
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
