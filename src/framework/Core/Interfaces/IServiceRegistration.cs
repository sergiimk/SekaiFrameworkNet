using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// A registered service.
	/// The Framework returns a ServiceRegistration object when a 
	/// BundleContext.RegisterService method invocation is successful. 
	/// The ServiceRegistration object is for the private use of the registering 
	/// bundle and should not be shared with other bundles.
	/// 
	/// The ServiceRegistration object may be used to update the properties 
	/// of the service or to unregister the service.
	/// </summary>
	public interface IServiceRegistration
	{
		/// <summary>
		/// Returns a ServiceReference object for a service being registered.
		/// The ServiceReference object may be shared with other bundles.
		/// </summary>
		IServiceReference getReference();

		/// <summary>
		/// Updates the properties associated with a service.
		/// </summary>
		void setProperties(Dictionary<string, string> properties);

		/// <summary>
		/// Unregisters a service. 
		/// Remove a ServiceRegistration object from the Framework service registry. 
		/// All ServiceReference objects associated with this ServiceRegistration 
		/// object can no longer be used to interact with the service once unregistration is complete.
		/// </summary>
		void Unregister();
	}
}
