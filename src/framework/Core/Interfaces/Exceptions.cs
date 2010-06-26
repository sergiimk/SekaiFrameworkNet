using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	
	//////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// A Framework exception used to indicate that a bundle lifecycle problem occurred.
	/// A BundleException object is created by the Framework to denote an exception condition 
	/// in the lifecycle of a bundle.
	/// 
	/// BundleExceptions should not be created by bundle developers. 
	/// A type code is used to identify the exception type for future extendability.
	/// </summary>
	public class BundleException : Exception
	{
		public enum ErrorCode
		{
			ACTIVATOR_ERROR,		// The bundle activator was in error
			DUPLICATE_BUNDLE_ERROR,	// The install or update operation failed because another already installed bundle has the same symbolic name and version
			INVALID_OPERATION,		// The operation was invalid
			MANIFEST_ERROR,			// The bundle manifest was in error
			NATIVECODE_ERROR,		// The bundle could not be resolved due to an error with the Bundle-NativeCode header
			RESOLVE_ERROR,			// The bundle was not resolved
			SECURITY_ERROR,			// The operation failed due to insufficient permissions
			START_TRANSIENT_ERROR,	// The start transient operation failed because the start level of the bundle is greater than the current framework start level
			STATECHANGE_ERROR,		// The operation failed to complete the requested lifecycle state change
			UNSPECIFIED,			// No exception type is unspecified
			UNSUPPORTED_OPERATION,	// The operation was unsupported
		}

		/// <summary>
		/// Creates a BundleException with the specified message.
		/// </summary>
		public BundleException(string msg) : base(msg) { }
        
		/// <summary>
		/// Creates a BundleException with the specified message and type.
		/// </summary>
		public BundleException(string msg, ErrorCode type) : base(msg) { m_code = type; }

		/// <summary>
		/// Creates a BundleException with the specified message and exception cause.
		/// </summary>
		public BundleException(string msg, Exception cause) : base(msg, cause) { }

		/// <summary>
		/// Creates a BundleException with the specified message, type and exception cause.
		/// </summary>
		public BundleException(string msg, ErrorCode type, Exception cause) : base(msg, cause) { m_code = type; }

		/// <summary>
		/// Returns exception error code
		/// </summary>
		public ErrorCode getErrorCode()
		{
			return m_code;
		}

		public override string ToString()
		{
			return string.Format("{0}, ErrorCode={1}", base.ToString(), m_code.ToString());
		}

		ErrorCode m_code = ErrorCode.UNSPECIFIED;
	}


	//////////////////////////////////////////////////////////////////////////


	public class ServiceException : Exception
	{
		public enum ErrorCode
		{
			FACTORY_ERROR,		// The service factory produced an invalid service object.
			FACTORY_EXCEPTION,	// The service factory threw an exception.
			REMOTE,				// An error occurred invoking a remote service.
			SUBCLASSED,			// The exception is a subclass of ServiceException.
			UNREGISTERED,		// The service has been unregistered.
			UNSPECIFIED,		// No exception type is unspecified.
		}

		/// <summary>
		/// Creates a BundleException with the specified message.
		/// </summary>
		public ServiceException(string msg) : base(msg) { }
        
		/// <summary>
		/// Creates a BundleException with the specified message and type.
		/// </summary>
		public ServiceException(string msg, ErrorCode type) : base(msg) { m_code = type; }

		/// <summary>
		/// Creates a BundleException with the specified message and exception cause.
		/// </summary>
		public ServiceException(string msg, Exception cause) : base(msg, cause) { }

		/// <summary>
		/// Creates a BundleException with the specified message, type and exception cause.
		/// </summary>
		public ServiceException(string msg, ErrorCode type, Exception cause) : base(msg, cause) { m_code = type; }

		/// <summary>
		/// Returns exception error code
		/// </summary>
		public ErrorCode getErrorCode()
		{
			return m_code;
		}

		public override string ToString()
		{
			return string.Format("{0}, ErrorCode={1}", base.ToString(), m_code.ToString());
		}

		ErrorCode m_code = ErrorCode.UNSPECIFIED;
	}

	//////////////////////////////////////////////////////////////////////////

}
