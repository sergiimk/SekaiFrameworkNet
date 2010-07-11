using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.IO;

namespace framework.Core.Implementation
{
	class CSystemBundle : CBundle, IFramework
	{
		//////////////////////////////////////////////////////////////////////////

		// TODO: unify config and system bundle's manifest
		public CSystemBundle(FrameworkConfig config, CManifest manifest)
			: base(0
			, "System Bundle"
			, manifest
			, DateTime.MinValue
			, null)
		{
			m_systemBundle = this;
			m_config = config;

			m_config.FrameworkWorkingDirectory = Path.GetDirectoryName(manifest.AssemblyPath);

			if (string.IsNullOrEmpty(m_config.BundleRegistryPath))
				m_config.BundleRegistryPath = Path.Combine(m_config.FrameworkWorkingDirectory, "repository");
		}

		//////////////////////////////////////////////////////////////////////////
		
		public void Init()
		{
			/*
			After calling this method, this Framework must:
			Be in the Bundle.STARTING state.
			Have a valid Bundle Context.
			Be at start level 0.
			Have event handling enabled.
			Have reified Bundle objects for all installed bundles.
			Have registered any framework services. For example, PackageAdmin, ConditionalPermissionAdmin, StartLevel.
			This Framework will not actually be started until start is called.

			This method does nothing if called when this Framework is in the Bundle.STARTING, Bundle.ACTIVE or Bundle.STOPPING states.
			*/

			lock(m_lock)
			{
				if(m_state == BundleState.STARTING ||
					m_state == BundleState.ACTIVE ||
					m_state == BundleState.STOPPING)
					return;

				m_bundleRepository = new CBundleRepository(this);
				m_serviceRegistry = new CServiceRegistry(this);
				m_eventServer = new CEventServer();

				m_bundleListeners = new ListenerQueue<IBundleListener>();
				m_syncBundleListeners = new ListenerQueue<ISynchronousBundleListener>();
				m_frameworkListeners = new ListenerQueue<IFrameworkListener>();
				m_serviceListeners = new ListenerQueue<IServiceListener>();
				m_allServiceListeners = new ListenerQueue<IAllServiceListener>();

				m_state = BundleState.STARTING;
				m_context = new CBundleContext(this, this);
				m_activator = new CSystemBundleActivator();
				m_activator.Start(m_context);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Start(BundleStartOption options)
		{
			/*
			The following steps are taken to start this Framework:
			1.If this Framework is not in the Bundle.STARTING state, initialize this Framework.
			2.All installed bundles must be started in accordance with each bundle's persistent autostart setting. 
			 * This means some bundles will not be started, some will be started with eager activation 
			 * and some will be started with their declared activation policy. 
			 * If this Framework implements the optional Start Level Service Specification, then the start 
			 * level of this Framework is moved to the start level specified by the beginning start level 
			 * framework property, as described in the Start Level Service Specification. 
			 * If this framework property is not specified, then the start level of this Framework 
			 * is moved to start level one (1). Any exceptions that occur during bundle starting must be 
			 * wrapped in a BundleException and then published as a framework event of type FrameworkEvent.ERROR
			3.This Framework's state is set to Bundle.ACTIVE.
			4.A framework event of type FrameworkEvent.STARTED is fired
			*/

			lock (m_lock)
			{
				if (m_state == BundleState.ACTIVE)
					return;

				if (m_state != BundleState.STARTING)
					Init();

				m_state = BundleState.ACTIVE;
				RaiseFrameworkEvent(new FrameworkEvent(FrameworkEvent.Type.STARTED, this, null));
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Stop(BundleStopOption options)
		{
			/*
			The method returns immediately to the caller after initiating the following steps to be taken on another thread.
			*/
			lock (m_lock)
			{
				if (m_state == BundleState.STOPPING ||
					m_state == BundleState.RESOLVED)
					return;

				m_state = BundleState.STOPPING;

				ThreadPool.QueueUserWorkItem(doStop);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		void doStop(object dummy)
		{
			/*
			The method returns immediately to the caller after initiating the following steps to be taken on another thread.

			1.This Framework's state is set to Bundle.STOPPING.
			2.All installed bundles must be stopped without changing each bundle's persistent autostart setting. 
			 * If this Framework implements the optional Start Level Service Specification, then the start 
			 * level of this Framework is moved to start level zero (0), as described in the Start Level 
			 * Service Specification. Any exceptions that occur during bundle stopping must be wrapped in a 
			 * BundleException and then published as a framework event of type FrameworkEvent.ERROR
			3.Unregister all services registered by this Framework.
			4.Event handling is disabled.
			5.This Framework's state is set to Bundle.RESOLVED.
			6.All resources held by this Framework are released. This includes threads, bundle class loaders, open files, etc.
			7.Notify all threads that are waiting at waitForStop that the stop operation has completed.
			8.After being stopped, this Framework may be discarded, initialized or started.
			*/

			lock (m_lock)
			{
				m_activator.Stop(m_context);

				m_state = BundleState.RESOLVED;

				m_shutdownResult = new FrameworkEvent(FrameworkEvent.Type.STOPPED, this, null);
				Monitor.PulseAll(m_lock);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public FrameworkEvent WaitForStop(int timeout)
		{
			// Wait and Pulse technique explained
			// http://www.albahari.com/threading/part4.aspx#_Wait_and_Pulse

			if (timeout == 0)
				timeout = Timeout.Infinite;

			lock (m_lock)
			{
				while (m_state == BundleState.STARTING ||
					m_state == BundleState.ACTIVE ||
					m_state == BundleState.STOPPING)
				{
					Monitor.Wait(m_lock, timeout);
				}

				return m_shutdownResult ?? new FrameworkEvent(FrameworkEvent.Type.STOPPED, this, null);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Update(System.IO.Stream input)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Uninstall()
		{
			throw new BundleException("System bundle cannot be uninstalled", BundleException.ErrorCode.INVALID_OPERATION);
		}

		//////////////////////////////////////////////////////////////////////////
		// Internal
		//////////////////////////////////////////////////////////////////////////

		public FrameworkConfig getConfig() { return m_config; }

		public CBundleRepository getBundleRepository() { return m_bundleRepository; }

		public CServiceRegistry getServiceRegistery() { return m_serviceRegistry; }

		public ListenerQueue<IFrameworkListener> getFrameworkListeners() { return m_frameworkListeners; }

		public ListenerQueue<IBundleListener> getBundleListeners() { return m_bundleListeners; }

		public ListenerQueue<ISynchronousBundleListener> getSyncBundleListeners() { return m_syncBundleListeners; }

		public ListenerQueue<IServiceListener> getServiceListeners() { return m_serviceListeners; }

		public ListenerQueue<IAllServiceListener> getAllServiceListeners() { return m_allServiceListeners; }

		//////////////////////////////////////////////////////////////////////////

		public void RaiseBundleEvent(BundleEvent evnt)
		{
			m_eventServer.PostEvent(new BundleEventDispatcher(m_bundleListeners, evnt));
			m_eventServer.SendEvent(new SyncBundleEventDispatcher(m_syncBundleListeners, evnt));
		}

		public void RaiseFrameworkEvent(FrameworkEvent evnt)
		{
			m_eventServer.PostEvent(new FrameworkEventDispatcher(m_frameworkListeners, evnt));
		}

		public void RaiseServiceEvent(ServiceEvent evnt)
		{
			m_eventServer.SendEvent(new ServiceEventDispatcher(m_serviceListeners, evnt));
			m_eventServer.SendEvent(new AllServiceEventDispatcher(m_allServiceListeners, evnt));
		}
		
		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////

		FrameworkConfig m_config;
		CBundleRepository m_bundleRepository;
		CServiceRegistry m_serviceRegistry;
		CEventServer m_eventServer;

		ListenerQueue<IBundleListener>				m_bundleListeners;
		ListenerQueue<ISynchronousBundleListener>	m_syncBundleListeners;
		ListenerQueue<IFrameworkListener>			m_frameworkListeners;
		ListenerQueue<IServiceListener>				m_serviceListeners;
		ListenerQueue<IAllServiceListener>			m_allServiceListeners;

		FrameworkEvent m_shutdownResult;
	}
}
