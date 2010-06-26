using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace framework.Core
{
	public enum BundleState
	{
		ACTIVE,         // The bundle is now running
		INSTALLED,      // The bundle is installed but not yet resolved
		RESOLVED,       // The bundle is resolved and is able to be started
		STARTING,       // The bundle is in the process of starting
		STOPPING,       // The bundle is in the process of stopping
		UNINSTALLED,    // The bundle is uninstalled and may not be used
	}

	//////////////////////////////////////////////////////////////////////////

	[Flags]
	public enum BundleStartOption
	{
		NONE,
		ACTIVATION_POLICY,	// The bundle start operation must activate the bundle according to the bundle's declared activation policy
		TRANSIENT,			// The bundle start operation is transient and the persistent autostart setting of the bundle is not modified
	}

	//////////////////////////////////////////////////////////////////////////

	[Flags]
	public enum BundleStopOption
	{
		NONE,
		TRANSIENT,			// The bundle stop is transient and the persistent autostart setting of the bundle is not modified
	}

	//////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// An installed bundle in the Framework.
	/// A Bundle object is the access point to define the lifecycle of an 
	/// installed bundle. Each bundle installed in the OSGi environment must 
	/// have an associated Bundle object.
	/// </summary>
	public interface IBundle
	{
		/// <summary>
		/// Returns this bundle's current state.
		/// A bundle can be in only one state at any time.
		/// </summary>
		/// <returns>UNINSTALLED, INSTALLED, RESOLVED, STARTING, STOPPING, ACTIVE</returns>
		BundleState getState();


		/// <summary>
		/// Starts this bundle.
		/// </summary>
		/// <param name="options">The options for starting this bundle</param>
		void Start(BundleStartOption options);

		/// <summary>
		/// Starts this bundle with no options.
		/// </summary>
		void Start();


		/// <summary>
		/// Stops this bundle.
		/// </summary>
		/// <param name="options">The options for stoping this bundle</param>
		void Stop(BundleStopOption options);

		/// <summary>
		/// Stops this bundle with no options.
		/// </summary>
		void Stop();


		/// <summary>
		/// Updates this bundle from a Stream.
		/// </summary>
		/// <param name="input">
		/// The Stream from which to read the new bundle or 
		/// null to indicate the Framework must create the stream from this 
		/// bundle's UpdateLocation Manifest header
		/// </param>
		void Update(Stream input);

		/// <summary>
		/// Updates this bundle, same as Update(null).
		/// </summary>
		void Update();


		/// <summary>
		/// Uninstalls this bundle.
		/// </summary>
		void Uninstall();


		/// <summary>
		/// Returns this bundle's Manifest headers and values.
		/// This method returns all the Manifest headers and values from the 
		/// main section of this bundle's Manifest file; 
		/// that is, all lines prior to the first blank line.
		/// </summary>
		/// <returns></returns>
		Dictionary<string, string> getHeaders();


		/// <summary>
		/// Returns this bundle's Manifest headers and values localized to the specified locale.
		/// </summary>
		Dictionary<string, string> getHeaders(string locale);


		/// <summary>
		/// Returns this bundle's unique identifier. 
		/// This bundle is assigned a unique identifier by the Framework 
		/// when it was installed in the OSGi environment.
		/// </summary>
		long getBundleId();


		/// <summary>
		/// Returns this bundle's location identifier.
		/// </summary>
		string getLocation();


		/// <summary>
		/// Returns the symbolic name of this bundle as specified by its Bundle-SymbolicName manifest header.
		/// The bundle symbolic name together with a version must identify a unique bundle.
		/// </summary>
		string getSymbolicName();


		/// <summary>
		/// Returns the version of this bundle as specified by its Bundle-Version manifest header. 
		/// If this bundle does not have a specified version then Version.emptyVersion is returned.
		/// </summary>
		Version getVersion();


		/// <summary>
		/// Returns the time when this bundle was last modified. 
		/// A bundle is considered to be modified when it is installed, updated or uninstalled.
		/// </summary>
		DateTime getLastModified();


		/// <summary>
		/// Returns this bundle's BundleContext. 
		/// The returned BundleContext can be used by the caller to act on behalf of this bundle.
		/// </summary>
		IBundleContext getBundleContext();


		/// <summary>
		/// Returns this bundle's ServiceReference list for all services it has registered.
		/// </summary>
		/// <returns></returns>
		IServiceReference[] getRegisteredServices();


		/// <summary>
		/// Returns this bundle's ServiceReference list for all services it is using. 
		/// A bundle is considered to be using a service if its use count for that service is greater than zero.
		/// </summary>
		IServiceReference[] getServicesInUse();


		/// <summary>
		/// Determines if this bundle has the specified permissions.
		/// If the runtime does not support permissions, this method always returns true.
		/// </summary>
		bool HasPermission(object permission);


		/// <summary>
		/// Find the specified resource from this bundle's class loader. 
		/// This bundle's class loader is called to search for the specified resource. 
		/// If this bundle's state is INSTALLED, this method must attempt to resolve 
		/// this bundle before attempting to get the specified resource. 
		/// If this bundle cannot be resolved, then only this bundle must be searched 
		/// for the specified resource. Imported packages cannot be searched when this 
		/// bundle has not been resolved. If this bundle is a fragment bundle then null is returned.
		/// </summary>
		//java.net.URL getResource(java.lang.String name)


		/// <summary>
		/// Loads the specified class using this bundle's class loader.
		/// </summary>
		//java.lang.Class loadClass(java.lang.String name);


		/// <summary>
		/// Find the specified resources from this bundle's class loader.
		/// This bundle's class loader is called to search for the specified resources.
		/// </summary>
		//java.util.Enumeration getResources(java.lang.String name);


		/// <summary>
		/// Returns an Enumeration of all the paths (String objects) to entries 
		/// within this bundle whose longest sub-path matches the specified path. 
		/// This bundle's class loader is not used to search for entries. 
		/// Only the contents of this bundle are searched.
		/// </summary>
		//java.util.Enumeration getEntryPaths(java.lang.String path);


		/// <summary>
		/// Returns a URL to the entry at the specified path in this bundle. 
		/// This bundle's class loader is not used to search for the entry. 
		/// Only the contents of this bundle are searched for the entry.
		/// </summary>
		//java.net.URL getEntry(java.lang.String path);


		/// <summary>
		/// Returns entries in this bundle and its attached fragments. 
		/// This bundle's class loader is not used to search for entries.
		/// </summary>
		//java.util.Enumeration findEntries(java.lang.String path, java.lang.String filePattern, boolean recurse);


		/// <summary>
		/// Return the certificates for the signers of this bundle and the certificate chains for those signers.
		/// </summary>
		//java.util.Map getSignerCertificates(int signersType);
	}
}
