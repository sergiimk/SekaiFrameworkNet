using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	public class BundleEvent
	{
		public enum Type
		{
			INSTALLED,			// The bundle has been installed.
			LAZY_ACTIVATION,	// The bundle will be lazily activated.
			RESOLVED,			// The bundle has been resolved.
			STARTED,			// The bundle has been started.
			STARTING,			// The bundle is about to be activated.
			STOPPED,			// The bundle has been stopped.
			STOPPING,			// The bundle is about to deactivated.
			UNINSTALLED,		// The bundle has been uninstalled.
			UNRESOLVED,			// The bundle has been unresolved.
			UPDATED,			// The bundle has been updated.
		}

		/// <summary>
		/// Returns the bundle which had a lifecycle change. This bundle is the source of the event.
		/// </summary>
		public IBundle getBundle()
		{
			return m_bundle;
		}

		/// <summary>
		/// Returns the type of lifecyle event.
		/// </summary>
		public BundleEvent.Type getType()
		{
			return m_type;
		}

		/// <summary>
		/// Creates a bundle event of the specified type.
		/// </summary>
		public BundleEvent(BundleEvent.Type type, IBundle bundle)
		{
			m_type = type;
			m_bundle = bundle;
		}

		Type m_type;
		IBundle m_bundle;
	}

	
	//////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A BundleEvent listener. 
	/// BundleListener is a listener interface that may be implemented by a bundle developer. 
	/// When a BundleEvent is fired, it is asynchronously delivered to a BundleListener. 
	/// The Framework delivers BundleEvent objects to a BundleListener in order and must not concurrently call a BundleListener.
	/// </summary>
	public interface IBundleListener
	{
		/// <summary>
		/// Receives notification that a bundle has had a lifecycle change.
		/// </summary>
		void BundleChanged(BundleEvent evnt);
	}


	//////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// A synchronous BundleEvent listener. 
	/// SynchronousBundleListener is a listener interface that may be implemented 
	/// by a bundle developer. When a BundleEvent is fired, it is synchronously 
	/// delivered to a SynchronousBundleListener. 
	/// The Framework may deliver BundleEvent objects to a SynchronousBundleListener 
	/// out of order and may concurrently call and/or reenter a SynchronousBundleListener.
	/// </summary>
	public interface ISynchronousBundleListener : IBundleListener
	{
		
	}


	//////////////////////////////////////////////////////////////////////////

}
