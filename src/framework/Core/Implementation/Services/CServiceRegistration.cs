using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CServiceRegistration : IServiceRegistration
	{
		//////////////////////////////////////////////////////////////////////////

		public CServiceRegistration(string[] clazz, object service, CBundleContext bundleCtx)
		{
			m_clazz = clazz;
			m_instance = service;
			m_bundleCtx = bundleCtx;
		}

		//////////////////////////////////////////////////////////////////////////

		public string[] getClazz() { return m_clazz; }

		public CBundleContext getBundleContext() { return m_bundleCtx; }

		//////////////////////////////////////////////////////////////////////////

		public IServiceReference getReference()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void setProperties(Dictionary<string, string> properties)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void Unregister()
		{
			m_bundleCtx.UnregisterService(this);
		}

		//////////////////////////////////////////////////////////////////////////

		string[] m_clazz;
		object m_instance;
		CBundleContext m_bundleCtx;

		//////////////////////////////////////////////////////////////////////////
	}
}
