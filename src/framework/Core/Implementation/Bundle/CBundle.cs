using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CBundle : IBundle
	{
		//////////////////////////////////////////////////////////////////////////

		public CBundle(	long id, string location, string symbolicName, 
			Version version, DateTime lastModified, CSystemBundle systemBundle)
		{
			m_id = id;
			m_location = location;
			m_symbolicName = symbolicName;
			m_version = version;
			m_lastModified = lastModified;
			m_systemBundle = systemBundle;
			m_state = BundleState.INSTALLED;
			m_publishedServices = new List<IServiceRegistration>();
		}

		//////////////////////////////////////////////////////////////////////////

		public BundleState		getState()				{ return m_state; }
		public long				getBundleId()			{ return m_id; }
		public string			getLocation()			{ return m_location; }
		public string			getSymbolicName()		{ return m_symbolicName; }
		public Version			getVersion()			{ return m_version; }
		public DateTime			getLastModified()		{ return m_lastModified; }
		public IBundleContext	getBundleContext()		{ return m_context; }

		//////////////////////////////////////////////////////////////////////////

		public IServiceReference[] getRegisteredServices()
		{
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

		protected virtual void Resolve()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public virtual void Start(BundleStartOption options)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void Start() { Start(BundleStartOption.NONE); }

		//////////////////////////////////////////////////////////////////////////

		public virtual void Stop(BundleStopOption options)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void Stop() { Stop(BundleStopOption.NONE); }

		//////////////////////////////////////////////////////////////////////////

		public virtual void Update(System.IO.Stream input)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public void Update() { Update(null); }

		//////////////////////////////////////////////////////////////////////////

		public virtual void Uninstall()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public Dictionary<string, string> getHeaders()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public Dictionary<string, string> getHeaders(string locale)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public bool HasPermission(object permission)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////

		long m_id;
		protected BundleState m_state;
		string m_location;
		string m_symbolicName;
		Version m_version;
		DateTime m_lastModified;
		CBundleContext m_context;
		IBundleActivator m_activator;
		protected CSystemBundle m_systemBundle;

		//ModuleHandle				m_module;
		List<IServiceRegistration>	m_publishedServices;
		//TServicesInUseContainer	m_servicesInUse;

		object m_lock = new object();
	}
}
