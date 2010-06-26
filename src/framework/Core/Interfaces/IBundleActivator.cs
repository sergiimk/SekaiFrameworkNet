using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// Customizes the starting and stopping of a bundle.
	/// BundleActivator is an interface that may be implemented when a bundle 
	/// is started or stopped. The Framework can create instances of a bundle's 
	/// BundleActivator as required. If an instance's BundleActivator.Start method 
	/// executes successfully, it is guaranteed that the same instance's 
	/// BundleActivator.Stop method will be called when the bundle is to be stopped. 
	/// The Framework must not concurrently call a BundleActivator object.
	/// </summary>
	public interface IBundleActivator
	{
		/// <summary>
		/// Called when this bundle is started so the Framework can perform 
		/// the bundle-specific activities necessary to start this bundle. 
		/// This method can be used to register services or to allocate any 
		/// resources that this bundle needs.
		/// </summary>
		void Start(IBundleContext context);

		/// <summary>
		/// Called when this bundle is stopped so the Framework can perform the 
		/// bundle-specific activities necessary to stop the bundle. In general, 
		/// this method should undo the work that the BundleActivator.Start method started. 
		/// There should be no active threads that were started by this bundle when this bundle returns. 
		/// A stopped bundle must not call any Framework objects.
		/// </summary>
		void Stop(IBundleContext context);
	}
}
