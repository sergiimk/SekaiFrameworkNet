using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	//////////////////////////////////////////////////////////////////////////

	interface IEventDispatcher : IDisposable
	{
		void Dispatch();
	}

	//////////////////////////////////////////////////////////////////////////

	class BundleEventDispatcher : IEventDispatcher
	{
		ListenerQueue<IBundleListener>.View m_queueView;
		BundleEvent m_event;

		public BundleEventDispatcher(ListenerQueue<IBundleListener> listeners, BundleEvent evnt)
		{
			m_queueView = listeners.getView();
			m_event = evnt;
		}

		public void Dispose()
		{
			m_queueView.Dispose();
		}

		public void Dispatch()
		{
			List<IBundleListener> listeners = m_queueView.getListeners();

			foreach (IBundleListener l in listeners)
				l.BundleChanged(m_event);
		}
	}

	//////////////////////////////////////////////////////////////////////////

	class SyncBundleEventDispatcher : IEventDispatcher
	{
		ListenerQueue<ISynchronousBundleListener>.View m_queueView;
		BundleEvent m_event;

		public SyncBundleEventDispatcher(ListenerQueue<ISynchronousBundleListener> listeners, BundleEvent evnt)
		{
			m_queueView = listeners.getView();
			m_event = evnt;
		}

		public void Dispose()
		{
			m_queueView.Dispose();
		}

		public void Dispatch()
		{
			List<ISynchronousBundleListener> listeners = m_queueView.getListeners();

			foreach (ISynchronousBundleListener l in listeners)
				l.BundleChanged(m_event);
		}
	}

	//////////////////////////////////////////////////////////////////////////

	class FrameworkEventDispatcher : IEventDispatcher
	{
		ListenerQueue<IFrameworkListener>.View m_queueView;
		FrameworkEvent m_event;

		public FrameworkEventDispatcher(ListenerQueue<IFrameworkListener> listeners, FrameworkEvent evnt)
		{
			m_queueView = listeners.getView();
			m_event = evnt;
		}

		public void Dispose()
		{
			m_queueView.Dispose();
		}

		public void Dispatch()
		{
			List<IFrameworkListener> listeners = m_queueView.getListeners();

			foreach (IFrameworkListener l in listeners)
				l.FrameworkEvent(m_event);
		}
	}

	//////////////////////////////////////////////////////////////////////////

	class ServiceEventDispatcher : IEventDispatcher
	{
		ListenerQueue<IServiceListener>.View m_queueView;
		ServiceEvent m_event;

		public ServiceEventDispatcher(ListenerQueue<IServiceListener> listeners, ServiceEvent evnt)
		{
			m_queueView = listeners.getView();
			m_event = evnt;
			throw new NotImplementedException("add filtering first!!!");
		}

		public void Dispose()
		{
			m_queueView.Dispose();
		}

		public void Dispatch()
		{
			List<IServiceListener> listeners = m_queueView.getListeners();

			foreach (IServiceListener l in listeners)
				l.ServiceChanged(m_event);
		}
	}

	//////////////////////////////////////////////////////////////////////////

	class AllServiceEventDispatcher : IEventDispatcher
	{
		ListenerQueue<IAllServiceListener>.View m_queueView;
		ServiceEvent m_event;

		public AllServiceEventDispatcher(ListenerQueue<IAllServiceListener> listeners, ServiceEvent evnt)
		{
			m_queueView = listeners.getView();
			m_event = evnt;
		}

		public void Dispose()
		{
			m_queueView.Dispose();
		}

		public void Dispatch()
		{
			List<IAllServiceListener> listeners = m_queueView.getListeners();

			foreach (IAllServiceListener l in listeners)
				l.ServiceChanged(m_event);
		}
	}

	//////////////////////////////////////////////////////////////////////////
}
