using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CBundleContext : IBundleContext, IDisposable
	{
		//////////////////////////////////////////////////////////////////////////

		public CBundleContext(CBundle bundle, CSystemBundle systemBundle)
		{
			m_valid = true;
			m_bundle = bundle;
			m_systemBundle = systemBundle;
			m_publishedServices = new List<CServiceRegistration>();
			m_frameworkListeners = new List<IFrameworkListener>();
			m_bundleListeners = new List<IBundleListener>();
			m_serviceListeners = new List<IServiceListener>();
		}

		//////////////////////////////////////////////////////////////////////////

		public void Dispose()
		{
			m_valid = false;

			lock (m_lock)
			{
				while (m_frameworkListeners.Count != 0)
					RemoveFrameworkListener(m_frameworkListeners[m_frameworkListeners.Count - 1]);
				while (m_bundleListeners.Count != 0)
					RemoveBundleListener(m_bundleListeners[m_bundleListeners.Count - 1]);
				while (m_serviceListeners.Count != 0)
					RemoveServiceListener(m_serviceListeners[m_serviceListeners.Count - 1]);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		void validate()
		{
			if (!m_valid)
				throw new BundleException("Bundle context is no longer valid", BundleException.ErrorCode.ILLEGAL_STATE, null);
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle getBundle() 
		{ 
			return m_bundle;
		}

		//////////////////////////////////////////////////////////////////////////

		public string getProperty(string key)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle InstallBundle(string location, System.IO.Stream input)
		{
			validate();
			CBundleRepository repo = m_systemBundle.getBundleRepository();
			return repo.InstallBundle(location, input);
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle InstallBundle(string location) { return InstallBundle(location, null); }

		//////////////////////////////////////////////////////////////////////////

		public IBundle getBundle(long id)
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle[] getBundles()
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddServiceListener(IServiceListener listener, string filter)
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddServiceListener(IServiceListener listener)
		{
			validate();

			IAllServiceListener al = listener as IAllServiceListener;
			if (al != null)
				m_systemBundle.getAllServiceListeners().Add(al);
			else
				m_systemBundle.getServiceListeners().Add(listener);

			lock (m_lock)
			{
				m_serviceListeners.Add(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveServiceListener(IServiceListener listener)
		{
			validate();

			IAllServiceListener al = listener as IAllServiceListener;
			if (al != null)
				m_systemBundle.getAllServiceListeners().Remove(al);
			else
				m_systemBundle.getServiceListeners().Remove(listener);

			lock (m_lock)
			{
				m_serviceListeners.Remove(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddBundleListener(IBundleListener listener)
		{
			validate();
			
			ISynchronousBundleListener sl = listener as ISynchronousBundleListener;
			if (sl != null)
				m_systemBundle.getSyncBundleListeners().Add(sl);
			else
				m_systemBundle.getBundleListeners().Add(listener);

			lock (m_lock)
			{
				m_bundleListeners.Add(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveBundleListener(IBundleListener listener)
		{
			validate();
			
			ISynchronousBundleListener sl = listener as ISynchronousBundleListener;
			if (sl != null)
				m_systemBundle.getSyncBundleListeners().Remove(sl);
			else
				m_systemBundle.getBundleListeners().Remove(listener);

			lock (m_lock)
			{
				m_bundleListeners.Remove(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddFrameworkListener(IFrameworkListener listener)
		{
			validate();
			m_systemBundle.getFrameworkListeners().Add(listener);

			lock (m_lock)
			{
				m_frameworkListeners.Add(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveFrameworkListener(IFrameworkListener listener)
		{
			validate();
			m_systemBundle.getFrameworkListeners().Remove(listener);

			lock (m_lock)
			{
				m_frameworkListeners.Remove(listener);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public IServiceRegistration RegisterService(string[] clazz, object service/*, Dictionary properties*/)
		{
			validate();
			CServiceRegistration reg = m_systemBundle.getServiceRegistery().RegisterService(clazz, service, this);

			lock (m_lock)
			{
				m_publishedServices.Add(reg);
			}

			return reg;
		}

		//////////////////////////////////////////////////////////////////////////

		public IServiceRegistration RegisterService(string clazz, object service/*, Dictionary properties*/)
		{
			string[] clazzs = new string[1];
			clazzs[0] = clazz;
			return RegisterService(clazzs, service);
		}

		//////////////////////////////////////////////////////////////////////////

		internal void UnregisterService(CServiceRegistration service)
		{
			validate();
			m_systemBundle.getServiceRegistery().UnregisterService(service);

			lock (m_lock)
			{
				m_publishedServices.Remove(service);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public object getService(IServiceReference reference)
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public bool ungetService(IServiceReference reference)
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IFilter CreateFilter(string filter)
		{
			validate();
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////
		// Internals
		//////////////////////////////////////////////////////////////////////////

		public IServiceReference[] getRegisteredServices()
		{
			validate();
			lock (m_lock)
			{
				IServiceReference[] ret = new IServiceReference[m_publishedServices.Count];
				for (int i = 0; i != m_publishedServices.Count; ++i)
					m_publishedServices[i].getReference();
				return ret;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public IServiceReference[] getServicesInUse()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////

		CBundle m_bundle;
		CSystemBundle m_systemBundle;

		List<CServiceRegistration> m_publishedServices;
		//TServicesInUseContainer m_servicesInUse;

		List<IFrameworkListener> m_frameworkListeners;
		List<IBundleListener> m_bundleListeners;
		List<IServiceListener> m_serviceListeners;

		volatile bool m_valid;
		object m_lock = new object();
	}
}
