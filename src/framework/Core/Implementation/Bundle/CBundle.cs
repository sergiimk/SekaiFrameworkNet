using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace framework.Core.Implementation
{
	class CBundle : IBundle
	{
		//////////////////////////////////////////////////////////////////////////

		public CBundle(	long id, string location, CManifest manifest, DateTime lastModified, CSystemBundle systemBundle)
		{
			m_id = id;
			m_location = location;
			m_manifest = manifest;
			m_lastModified = lastModified;
			m_systemBundle = systemBundle;
			m_state = BundleState.INSTALLED;
			m_publishedServices = new List<IServiceRegistration>();
		}

		//////////////////////////////////////////////////////////////////////////

		public BundleState		getState()				{ return m_state; }
		public long				getBundleId()			{ return m_id; }
		public string			getLocation()			{ return m_location; }
		public string			getSymbolicName()		{ return m_manifest.SymbolicName; }
		public Version			getVersion()			{ return m_manifest.Version; }
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

		public virtual void Start(BundleStartOption options)
		{
			// TODO: replace with timeout-based lock
			// TODO: add start-level support

			lock(m_lock) 
			{
				if(m_state == BundleState.UNINSTALLED)
					throw new BundleException("Bundle is uninstalled", BundleException.ErrorCode.ILLEGAL_STATE);

				if(m_state == BundleState.ACTIVE)
					return;

				if(m_state == BundleState.INSTALLED)
				{
					Resolve();
					Debug.Assert(m_state == BundleState.RESOLVED);
				}

				PreStart();
				Debug.Assert(m_state == BundleState.STARTING);
				
				if(m_activator != null)
				{
					try
					{
						m_activator.Start(m_context);
					}
					catch(Exception ex)
					{
						PreStop();
						PostStop();
						throw new BundleException("Bundle activation failed", BundleException.ErrorCode.ACTIVATOR_ERROR, ex);
					}

					if(m_state == BundleState.UNINSTALLED)
						throw new BundleException("Bundle was unregistered in time of activation");
				}

				PostStart();
			}
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
		// Internal
		//////////////////////////////////////////////////////////////////////////

		public CManifest getManifest() { return m_manifest; }

		//////////////////////////////////////////////////////////////////////////

		protected virtual void Resolve()
		{
			m_state = BundleState.RESOLVED;
			m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.RESOLVED, this));
		}

		//////////////////////////////////////////////////////////////////////////

		protected virtual void PreStart()
		{
			m_state = BundleState.STARTING;
			m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.STARTING, this));

			m_activator = null;

			try
			{
				m_assembly = m_systemBundle.getBundleRepository().LoadBundleAssembly(this);

				Type[] exports = m_assembly.GetExportedTypes();
				foreach (Type t in exports)
				{
					TypeAttributes attrs = t.Attributes;
					if ((attrs & TypeAttributes.Interface) == TypeAttributes.Interface ||
						(attrs & TypeAttributes.Abstract) == TypeAttributes.Abstract)
						continue;

					if (t.GetInterface(typeof(IBundleActivator).FullName) != null)
					{
						m_activator = m_assembly.CreateInstance(t.FullName) as IBundleActivator;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				m_state = BundleState.RESOLVED;
				throw new BundleException("Failed to load bundle assembly", BundleException.ErrorCode.STATECHANGE_ERROR, ex);
			}

			m_context = new CBundleContext(this, m_systemBundle);
		}

		//////////////////////////////////////////////////////////////////////////

		protected virtual void PostStart()
		{
			m_state = BundleState.ACTIVE;
			m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.STARTED, this));
		}

		//////////////////////////////////////////////////////////////////////////

		protected virtual void PreStop()
		{
			/*
			This bundle's state is set to STOPPING.
			A bundle event of type BundleEvent.STOPPING is fired.
			Any services registered by this bundle must be unregistered.
			Any services used by this bundle must be released.
			Any listeners registered by this bundle must be removed.
			*/
			m_state = BundleState.STOPPING;
			m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.STOPPING, this));
			// TODO: unregister all links
		}

		//////////////////////////////////////////////////////////////////////////

		protected virtual void PostStop()
		{
			m_state = BundleState.RESOLVED;
			m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.STOPPED, this));
		}

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////

		long m_id;
		string m_location;
		DateTime m_lastModified;
		protected BundleState m_state;
		protected CManifest m_manifest;
		protected CBundleContext m_context;
		protected IBundleActivator m_activator;
		protected CSystemBundle m_systemBundle;
		
		Assembly					m_assembly;
		List<IServiceRegistration>	m_publishedServices;
		//TServicesInUseContainer	m_servicesInUse;

		protected object m_lock = new object();
	}
}
