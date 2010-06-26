using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// A reference to a service.
	/// </summary>
	public interface IServiceReference : IComparable<IServiceReference>
	{
		/// <summary>
		/// Returns the property value to which the specified property key is 
		/// mapped in the properties Dictionary object of the service referenced 
		/// by this ServiceReference object.
		/// </summary>
		object getProperty(string key);


		/// <summary>
		/// Returns an array of the keys in the properties Dictionary object of 
		/// the service referenced by this ServiceReference object.
		/// </summary>
		/// <returns></returns>
		string[] getPropertyKeys();


		/// <summary>
		/// Returns the bundle that registered the service referenced by this ServiceReference object.
		/// This method must return null when the service has been unregistered. 
		/// This can be used to determine if the service has been unregistered.
		/// </summary>
		IBundle getBundle();


		/// <summary>
		/// Returns the bundles that are using the service referenced by this 
		/// ServiceReference object. Specifically, this method returns the 
		/// bundles whose usage count for that service is greater than zero.
		/// </summary>
		IBundle[] getUsingBundles();
	}
}
