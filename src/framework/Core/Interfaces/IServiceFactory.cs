using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// Allows services to provide customized service objects in the OSGi environment.
	/// When registering a service, a ServiceFactory object can be used 
	/// instead of a service object,  so that the bundle developer can gain 
	/// control of the specific service object granted to a bundle that is using the service.
	/// </summary>
	public interface IServiceFactory
	{
		/// <summary>
		/// Creates a new service object.
		/// The Framework invokes this method the first time the specified bundle 
		/// requests a service object using the BundleContext.getService(ServiceReference) method. 
		/// The service factory can then return a specific service object for each bundle.
		/// </summary>
		object getService(IBundle bundle, IServiceRegistration registration);


		/// <summary>
		/// Releases a service object.
		/// The Framework invokes this method when a service has been 
		/// released by a bundle. The service object may then be destroyed.
		/// </summary>
		void ungetService(IBundle bundle, IServiceRegistration registration, object service);
	}
}
