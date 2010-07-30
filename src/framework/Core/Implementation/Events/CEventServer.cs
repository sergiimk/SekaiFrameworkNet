using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NMock2;

namespace framework.Core.Implementation
{
	//////////////////////////////////////////////////////////////////////////
	// CEventServer
	//////////////////////////////////////////////////////////////////////////

	class CEventServer
	{

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
			Thread.Sleep(100);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}
	}

	//////////////////////////////////////////////////////////////////////////
}
