using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CBundleContext : IBundleContext
	{
		//////////////////////////////////////////////////////////////////////////

		public CBundleContext(CBundle bundle, CSystemBundle systemBundle)
		{
			m_bundle = bundle;
			m_systemBundle = systemBundle;
			m_frameworkListeners = new List<IFrameworkListener>();
			m_bundleListeners = new List<IBundleListener>();
			m_serviceListeners = new List<IServiceListener>();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle getBundle() { return m_bundle; }

		//////////////////////////////////////////////////////////////////////////

		public string getProperty(string key)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle InstallBundle(string location, System.IO.Stream input)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle InstallBundle(string location)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle getBundle(long id)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle[] getBundles()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddServiceListener(IServiceListener listener, string filter)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddServiceListener(IServiceListener listener)
		{
			IAllServiceListener al = listener as IAllServiceListener;
			if (al != null)
				m_systemBundle.getAllServiceListeners().Add(al);
			else
				m_systemBundle.getServiceListeners().Add(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveServiceListener(IServiceListener listener)
		{
			IAllServiceListener al = listener as IAllServiceListener;
			if (al != null)
				m_systemBundle.getAllServiceListeners().Remove(al);
			else
				m_systemBundle.getServiceListeners().Remove(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddBundleListener(IBundleListener listener)
		{
			ISynchronousBundleListener sl = listener as ISynchronousBundleListener;
			if (sl != null)
				m_systemBundle.getSyncBundleListeners().Add(sl);
			else
				m_systemBundle.getBundleListeners().Add(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveBundleListener(IBundleListener listener)
		{
			ISynchronousBundleListener sl = listener as ISynchronousBundleListener;
			if (sl != null)
				m_systemBundle.getSyncBundleListeners().Remove(sl);
			else
				m_systemBundle.getBundleListeners().Remove(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public void AddFrameworkListener(IFrameworkListener listener)
		{
			m_systemBundle.getFrameworkListeners().Add(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveFrameworkListener(IFrameworkListener listener)
		{
			m_systemBundle.getFrameworkListeners().Remove(listener);
		}

		//////////////////////////////////////////////////////////////////////////

		public object getService(IServiceReference reference)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public bool ungetService(IServiceReference reference)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public IFilter CreateFilter(string filter)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////

		CBundle m_bundle;
		CSystemBundle m_systemBundle;
		List<IFrameworkListener> m_frameworkListeners;
		List<IBundleListener> m_bundleListeners;
		List<IServiceListener> m_serviceListeners;

		object m_lock = new object();
	}
}
