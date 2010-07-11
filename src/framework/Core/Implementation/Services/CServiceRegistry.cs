using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CServiceRegistry
	{
		//////////////////////////////////////////////////////////////////////////

		public CServiceRegistry(CSystemBundle systemBundle)
		{
			m_systemBundle = systemBundle;
			m_services = new Dictionary<string, List<CServiceRegistration>>();
		}

		//////////////////////////////////////////////////////////////////////////

		public CServiceRegistration RegisterService(string[] clazz, object service/*, Dictionary properties*/, CBundleContext bundleCtx)
		{
			CServiceRegistration reg = new CServiceRegistration(clazz, service, bundleCtx);

			lock (m_lock)
			{
				foreach (string clz in clazz)
				{
					List<CServiceRegistration> sbucket;
					if (!m_services.TryGetValue(clz, out sbucket))
					{
						sbucket = new List<CServiceRegistration>();
						m_services.Add(clz, sbucket);
					}
					sbucket.Add(reg);
				}
			}

			return reg;
		}

		//////////////////////////////////////////////////////////////////////////

		public void UnregisterService(CServiceRegistration service)
		{
			lock (m_lock)
			{
				foreach (string clz in service.getClazz())
				{
					List<CServiceRegistration> sbucket = m_services[clz];
					sbucket.Remove(service);
					if (sbucket.Count == 0)
						m_services.Remove(clz);
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		CSystemBundle m_systemBundle;
		Dictionary<string, List<CServiceRegistration>> m_services;

		object m_lock = new object();
	}
}
