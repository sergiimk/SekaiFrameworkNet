using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	//////////////////////////////////////////////////////////////////////////

	public class ServiceEvent
	{
		public enum Type
		{
			MODIFIED,			// The properties of a registered service have been modified
			MODIFIED_ENDMATCH,	// The properties of a registered service have been modified and the new properties no longer match the listener's filter
			REGISTERED,			// This service has been registered
			UNREGISTERING,		// This service is in the process of being unregistered
		}

		/// <summary>
		/// Returns a reference to the service that had a change occur in its lifecycle.
		/// </summary>
		public IServiceReference getServiceReference() 
		{ 
			return m_reference; 
		}

		/// <summary>
		/// Returns the type of event.
		/// </summary>
		public ServiceEvent.Type getType() 
		{ 
			return m_type; 
		}

		/// <summary>
		/// Creates a new service event object.
		/// </summary>
		public ServiceEvent(ServiceEvent.Type type, IServiceReference reference)
		{
			m_type = type;
			m_reference = reference;
		}

		Type m_type;
		IServiceReference m_reference;
	}


	//////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// A ServiceEvent listener.
	/// ServiceListener is a listener interface that may be implemented by a bundle developer.
	/// When a ServiceEvent is fired, it is synchronously delivered to a ServiceListener.
	/// The Framework may deliver ServiceEvent objects to a ServiceListener out of order
	/// and may concurrently call and/or reenter a ServiceListener.
	/// </summary>
	public interface IServiceListener
	{
		/// <summary>
		/// Receives notification that a service has had a lifecycle change.
		/// </summary>
		void ServiceChanged(ServiceEvent evnt);
	}


	//////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A ServiceEvent listener that does not filter based upon package wiring. 
	/// AllServiceListener is a listener interface that may be implemented by a 
	/// bundle developer. When a ServiceEvent is fired, it is synchronously 
	/// delivered to an AllServiceListener. The Framework may deliver ServiceEvent 
	/// objects to an AllServiceListener out of order and may concurrently call 
	/// and/or reenter an AllServiceListener.
	/// 
	/// Unlike normal ServiceListener objects, AllServiceListener objects receive 
	/// all ServiceEvent objects regardless of whether the package source of the
	/// listening bundle is equal to the package source of the bundle that 
	/// registered the service. This means that the listener may not be able to 
	/// cast the service object to any of its corresponding service interfaces 
	/// if the service object is retrieved.
	/// </summary>
	public interface IAllServiceListener : IServiceListener
	{
	}


	//////////////////////////////////////////////////////////////////////////
}
