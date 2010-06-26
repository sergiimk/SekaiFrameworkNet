using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	//////////////////////////////////////////////////////////////////////////
	
	public class FrameworkEvent
	{
		public enum Type
		{
			ERROR,							// An error has occurred.
			INFO,							// An informational event has occurred.
			PACKAGES_REFRESHED,				// A PackageAdmin.refreshPackage operation has completed.
			STARTED,						// The Framework has started.
			STARTLEVEL_CHANGED,				// A StartLevel.setStartLevel operation has completed.
			STOPPED,						// The Framework has stopped.
			STOPPED_BOOTCLASSPATH_MODIFIED,	// The Framework has stopped and the boot class path has changed.
			STOPPED_UPDATE,					// The Framework has stopped during update.
			WAIT_TIMEDOUT,					// The Framework did not stop before the wait timeout expired.
			WARNING,						// A warning has occurred.
		}

		/// <summary>
		/// Returns the bundle associated with the event.
		/// </summary>
		public FrameworkEvent.Type getType()
		{
			return m_type;
		}

		/// <summary>
		/// Returns the bundle associated with the event.
		/// </summary>
		public IBundle getBundle()
		{
			return m_bundle;
		}

		/// <summary>
		/// Returns the exception related to this event.
		/// </summary>
		public Exception getException()
		{
			return m_exception;
		}

		/// <summary>
		/// Creates a Framework event regarding the specified bundle.
		/// </summary>
		public FrameworkEvent(FrameworkEvent.Type type, IBundle bundle, Exception exception)
		{
			m_type = type;
			m_bundle = bundle;
			m_exception = exception;
		}

		Type m_type;
		IBundle m_bundle;
		Exception m_exception;
	}


	//////////////////////////////////////////////////////////////////////////


	public interface IFrameworkListener
	{
		/// <summary>
		/// Receives notification of a general FrameworkEvent object.
		/// </summary>
		void FrameworkEvent(FrameworkEvent evnt);
	}

	
	//////////////////////////////////////////////////////////////////////////
}
