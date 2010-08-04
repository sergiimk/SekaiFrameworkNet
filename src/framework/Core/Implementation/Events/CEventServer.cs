using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NMock2;
using System.Diagnostics;

namespace framework.Core.Implementation
{
	//////////////////////////////////////////////////////////////////////////
	// CEventServer
	//////////////////////////////////////////////////////////////////////////

	class CEventServer
	{

		public CEventServer()
		{
			m_processingScheduled = false;
			m_pendingQueue = new List<IEventDispatcher>();
			m_deliveringQueue = new List<IEventDispatcher>();
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
			lock (m_lock)
			{
				m_pendingQueue.Add(dispatcher);

				if (!m_processingScheduled)
				{
					m_processingScheduled = true;
					ThreadPool.QueueUserWorkItem(DeliverEvents);
				}
			}
		}

		//////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Waits until all posted events were delivered
		/// </summary>
		public bool WaitAll(int timeout)
		{
			if (timeout == 0)
				timeout = Timeout.Infinite;

			lock (m_lock)
			{
				return m_processingScheduled ? Monitor.Wait(m_lock, timeout) : true;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		void DeliverEvents(object dummy)
		{
			while (true)
			{
				lock (m_lock)
				{
					if (m_pendingQueue.Count == 0)
						break;

					SwapQueues();					
				}

				foreach (IEventDispatcher disp in m_deliveringQueue)
					SendEvent(disp);

				m_deliveringQueue.Clear();
			}

			lock (m_lock)
			{
				m_processingScheduled = false;
				Monitor.PulseAll(m_lock);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		void SwapQueues()
		{
			Debug.Assert(m_deliveringQueue.Count == 0);
			List<IEventDispatcher> t = m_pendingQueue;
			m_pendingQueue = m_deliveringQueue;
			m_deliveringQueue = t;
		}

		//////////////////////////////////////////////////////////////////////////

		object m_lock = new object();
		bool m_processingScheduled;
		List<IEventDispatcher> m_pendingQueue;
		List<IEventDispatcher> m_deliveringQueue;
	}

	//////////////////////////////////////////////////////////////////////////
	// Test
	//////////////////////////////////////////////////////////////////////////

	[TestFixture]
	class TestEventServer
	{
		Mockery mocks;
		CEventServer server;
		IEventDispatcher mock_disp;

		[SetUp]
		public void SetUp()
		{
			mocks = new Mockery();
			server = new CEventServer();
			mock_disp = mocks.NewMock<IEventDispatcher>();
		}

		[Test]
		public void TestSendEvent()
		{
			Expect.Once.On(mock_disp)
				.Method("Dispatch");

			Expect.Once.On(mock_disp)
				.Method("Dispose");

			server.SendEvent(mock_disp);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void TestPostEvent()
		{
			Expect.Once.On(mock_disp)
			    .Method("Dispatch");

			Expect.Once.On(mock_disp)
				.Method("Dispose");

			server.PostEvent(mock_disp);
			bool w = server.WaitAll(0);

			Assert.IsTrue(w);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}
	}

	//////////////////////////////////////////////////////////////////////////
}
