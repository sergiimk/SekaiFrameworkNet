using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace framework.Core
{
	/// <summary>
	/// A bundle's execution context within the Framework. 
	/// The context is used to grant access to other methods so that this 
	/// bundle can interact with the Framework.
	/// </summary>
	public interface IBundleContext
	{
		/// <summary>
		/// Returns the value of the specified property. 
		/// If the key is not found in the Framework properties, the system 
		/// properties are then searched. The method returns null if the 
		/// property is not found.
		/// </summary>
		/// <param name="key">The name of the requested property.</param>
		/// <returns>The value of the requested property, or null if the property is undefined.</returns>
		string getProperty(string key);


		/// <summary>
		/// Returns the Bundle object associated with this BundleContext. 
		/// This bundle is called the context bundle.
		/// </summary>
		IBundle getBundle();


		/// <summary>
		/// Installs a bundle from the specified InputStream object.
		/// If the specified Stream is null, the Framework creates the 
		/// Stream from which to read the bundle by interpreting the specified location.
		/// </summary>
		/// <param name="location">The location identifier of the bundle to install (typically URL).</param>
		/// <param name="input">The Stream object from which this bundle will be read or null</param>
		/// <returns>The Bundle object of the installed bundle.</returns>
		IBundle InstallBundle(string location, Stream input);

		/// <summary>
		/// Installs a bundle from the specified location identifier.
		/// Same as InstallBundle(location, null)
		/// </summary>
		IBundle InstallBundle(string location);


		/// <summary>
		/// Returns the bundle with the specified identifier.
		/// </summary>
		IBundle getBundle(long id);


		/// <summary>
		/// Returns a list of all installed bundles.
		/// </summary>
		IBundle[] getBundles();


		/// <summary>
		/// Adds the specified ServiceListener object with the specified filter 
		/// to the context bundle's list of listeners. See IFilter for a 
		/// description of the filter syntax. ServiceListener objects are 
		/// notified when a service has a lifecycle state change.
		/// </summary>
		/// <exception cref="IllegalStateException">If this BundleContext is no longer valid.</exception>
		void AddServiceListener(IServiceListener listener, string filter);

		/// <summary>
		/// Adds the specified ServiceListener object to the context bundle's list of listeners.
		/// </summary>
		void AddServiceListener(IServiceListener listener);


		/// <summary>
		/// Removes the specified ServiceListener object from the context bundle's list of listeners.
		/// If listener is not contained in this context bundle's list of listeners, this method does nothing.
		/// </summary>
		/// <exception cref="IllegalStateException">If this BundleContext is no longer valid.</exception>
		void RemoveServiceListener(IServiceListener listener);


		/// <summary>
		/// Adds the specified BundleListener object to the context bundle's 
		/// list of listeners if not already present. 
		/// BundleListener objects are notified when a bundle has a lifecycle state change.
		/// </summary>
		void AddBundleListener(IBundleListener listener);


		/// <summary>
		/// Removes the specified BundleListener object from the context bundle's list of listeners.
		/// </summary>
		void RemoveBundleListener(IBundleListener listener);


		/// <summary>
		/// Adds the specified FrameworkListener object to the context bundle's 
		/// list of listeners if not already present. 
		/// FrameworkListeners are notified of general Framework events.
		/// </summary>
		void AddFrameworkListener(IFrameworkListener listener);


		/// <summary>
		/// Removes the specified FrameworkListener object from the context bundle's list of listeners.
		/// </summary>
		void RemoveFrameworkListener(IFrameworkListener listener);


		/// <summary>
		/// Registers the specified service object with the specified 
		/// properties under the specified class names into the Framework. 
		/// A ServiceRegistration object is returned. 
		/// The ServiceRegistration object is for the private use of the bundle 
		/// registering the service and should not be shared with other bundles.
		/// </summary>
		//IServiceRegistration registerService(string[] clazzes, object service, Dictionary properties);

		/// <summary>
		/// Registers the specified service object with the specified properties under the specified class name with the Framework.
		/// </summary>
		//IServiceRegistration registerService(string clazz, object service, Dictionary properties);


		/// <summary>
		/// Returns an array of ServiceReference objects. 
		/// The returned array of ServiceReference objects contains services 
		/// that were registered under the specified class and match the specified filter.
		/// </summary>
		//IServiceReference[] getServiceReferences(string clazz, string filter);


		/// <summary>
		/// Returns an array of ServiceReference objects. 
		/// The returned array of ServiceReference objects contains services 
		/// that were registered under the specified class and match the 
		/// specified filter expression.
		/// </summary>
		//IServiceReference[] getAllServiceReferences(string clazz, string filter);


		/// <summary>
		/// Returns a ServiceReference object for a service that implements and was registered under the specified class.
		/// </summary>
		//IServiceReference getServiceReference(string clazz);


		/// <summary>
		/// Returns the service object referenced by the specified ServiceReference object.
		/// </summary>
		object getService(IServiceReference reference);


		/// <summary>
		/// Releases the service object referenced by the specified ServiceReference object. 
		/// If the context bundle's use count for the service is zero, this method returns false. 
		/// Otherwise, the context bundle's use count for the service is decremented by one.
		/// </summary>
		bool ungetService(IServiceReference reference);


		/// <summary>
		/// Creates a File object for a file in the persistent storage area 
		/// provided for the bundle by the Framework. This method will return 
		/// null if the platform does not have file system support.
		/// </summary>
		//java.io.File getDataFile(java.lang.String filename);


		/// <summary>
		/// Creates a Filter object. 
		/// This Filter object may be used to match a ServiceReference 
		/// object or a Dictionary object.
		/// </summary>
		IFilter CreateFilter(string filter);
	}
}
