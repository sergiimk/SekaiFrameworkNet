using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using framework.Core;
using NMock2;

namespace framework.Tests
{
	[TestFixture]
	class bundle_load
	{
		static readonly string TEST_BUNDLE_LOCATION = "test_bundle";

		Mockery mockery;
		ISynchronousBundleListener bundle_listener;

		[SetUp]
		public void SetUp()
		{
			mockery = new Mockery();
			bundle_listener = mockery.NewMock<ISynchronousBundleListener>();
		}

		[Test]
		public void LoadBundle()
		{
			IFrameworkFactory factory = new CFrameworkFactory();
			IFramework fwk = factory.NewFramework(null);
			fwk.Init();

			Expect.Once.On(bundle_listener)
				.Method("BundleChanged");

			IBundleContext ctx = fwk.getBundleContext();
			ctx.AddBundleListener(bundle_listener);

			IBundle test_bundle = ctx.InstallBundle(TEST_BUNDLE_LOCATION);
			Assert.IsNotNull(test_bundle);
			Assert.AreEqual(test_bundle.getState(), BundleState.INSTALLED);

			Assert.AreEqual(test_bundle, ctx.getBundle(test_bundle.getBundleId()));
			Assert.AreEqual(test_bundle, ctx.InstallBundle(TEST_BUNDLE_LOCATION));

			mockery.VerifyAllExpectationsHaveBeenMet();
		}
	}
}
