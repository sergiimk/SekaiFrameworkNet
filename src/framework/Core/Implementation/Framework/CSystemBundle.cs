using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace framework.Core.Implementation
{
	class CSystemBundle : CBundle, IFramework
	{
		//////////////////////////////////////////////////////////////////////////

		public CSystemBundle()
			: base(0
			, "System Bundle"
			, "System Bundle"
			, Assembly.GetExecutingAssembly().GetName().Version
			, DateTime.MinValue
			, null)
		{
			m_systemBundle = this;
			m_bundleRepository = new CBundleRepository(this);
			m_serviceRegistry = new CServiceRegistry();
			m_eventServer = new CEventServer();

			m_bundleListeners = new ListenerQueue<IBundleListener>();
			m_syncBundleListeners = new ListenerQueue<ISynchronousBundleListener>();
			m_frameworkListeners = new ListenerQueue<IFrameworkListener>();
			m_serviceListeners = new ListenerQueue<IServiceListener>();
			m_allServiceListeners = new ListenerQueue<IAllServiceListener>();
		}

		//////////////////////////////////////////////////////////////////////////
		
		public void Init()
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public FrameworkEvent WaitForStop(long timeout)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		protected override void Resolve()
		{
			m_state = BundleState.RESOLVED;
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Start(BundleStartOption options)
		{
			throw new NotImplementedException();
		}

		//////////////////////////////////////////////////////////////////////////

		public override void Stop(BundleStopOption options)
		{
			throw new NotImplementedException();
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

		CBundleRepository m_bundleRepository;
		CServiceRegistry m_serviceRegistry;
		CEventServer m_eventServer;

		ListenerQueue<IBundleListener>				m_bundleListeners;
		ListenerQueue<ISynchronousBundleListener>	m_syncBundleListeners;
		ListenerQueue<IFrameworkListener>			m_frameworkListeners;
		ListenerQueue<IServiceListener>				m_serviceListeners;
		ListenerQueue<IAllServiceListener>			m_allServiceListeners;
	}
}
