using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace framework.Core.Implementation
{
	class CEventServer
	{
		//////////////////////////////////////////////////////////////////////////

		public CEventServer()
		{
			m_processingPending = false;
			m_pendingEvents = new List<IEventDispatcher>();
		}

		//////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Sends event synchronously
		/// </summary>
		public void SendEvent(IEventDispatcher dispatcher)
		{
			using (dispatcher)
			{
				dispatcher.Dispatch();
			}
		}

		//////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Delivers events in another threads but avoids concurrent calls 
		/// and maintains event order
		/// </summary>
		/// <param name="dispatcher"></param>
		public void PostEvent(IEventDispatcher dispatcher)
		{
			lock (m_pendingEvents)
			{
				m_pendingEvents.Add(dispatcher);

				if (!m_processingPending)
				{
					ThreadPool.QueueUserWorkItem(DeliverEvents);
					m_processingPending = true;
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		void DeliverEvents(object dummy)
		{
			lock (m_pendingEvents)
			{
				for (int i = 0; i != m_pendingEvents.Count; ++i)
					SendEvent(m_pendingEvents[i]);

				m_pendingEvents.Clear();
				m_processingPending = false;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		bool m_processingPending;
		List<IEventDispatcher> m_pendingEvents;

		//////////////////////////////////////////////////////////////////////////
	}
}
