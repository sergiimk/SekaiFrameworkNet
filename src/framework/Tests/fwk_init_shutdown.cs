using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using framework.Core;
using framework.Core.Implementation;
using NMock2;

namespace framework.Tests
{
	[TestFixture]
	class TestFrameworkInitShutdown
	{
		Mockery mocks;
		IFrameworkListener mock_fwk_listener;

		[SetUp]
		public void SetUp()
		{
			mocks = new Mockery();
			mock_fwk_listener = mocks.NewMock<IFrameworkListener>();
		}

		[Test]
		public void InitShutdown()
		{
			IFrameworkFactory factory = new CFrameworkFactory();
			
			IFramework fwk = factory.NewFramework(null);
			Assert.IsNotNull(fwk);
			Assert.AreEqual(fwk.getState(), BundleState.INSTALLED);

			fwk.Init();
			Assert.AreEqual(fwk.getState(), BundleState.STARTING);

			IBundleContext ctx = fwk.getBundleContext();
			Assert.IsNotNull(ctx);
			Assert.AreEqual(ctx.getBundle(), fwk);

			Expect.Once.On(mock_fwk_listener)
				.Method("FrameworkEvent")
				.With(new FrameworkEvent(FrameworkEvent.Type.STARTED, fwk, null));			
			ctx.AddFrameworkListener(mock_fwk_listener);

			
			fwk.Start();
			Assert.AreEqual(fwk.getState(), BundleState.ACTIVE);

			fwk.Stop();
			Assert.AreEqual(fwk.getState(), BundleState.STOPPING);

			fwk.WaitForStop(0);
			Assert.AreEqual(fwk.getState(), BundleState.RESOLVED);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void MultipleInits()
		{
			IFrameworkFactory factory = new CFrameworkFactory();
			IFramework fwk = factory.NewFramework(null);
			fwk.Init();
			
			IBundleContext ctx = fwk.getBundleContext();
			
			Expect.Exactly(2).On(mock_fwk_listener)
				.Method("FrameworkEvent")
				.With(new FrameworkEvent(FrameworkEvent.Type.STARTED, fwk, null));
			ctx.AddFrameworkListener(mock_fwk_listener);
			
			fwk.Start();
			fwk.Stop();
			fwk.WaitForStop(0);
			Assert.AreEqual(fwk.getState(), BundleState.RESOLVED);

			fwk.Init();
			fwk.getBundleContext().AddFrameworkListener(mock_fwk_listener);
			fwk.Start();
			Assert.AreEqual(fwk.getState(), BundleState.ACTIVE);
			fwk.Stop();
			fwk.WaitForStop(0);
			Assert.AreEqual(fwk.getState(), BundleState.RESOLVED);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}
	}
}
