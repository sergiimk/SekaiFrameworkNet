using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// A factory for creating Framework instances.
	/// </summary>
	public interface IFrameworkFactory
	{
		/// <summary>
		/// Create a new Framework instance.
		/// </summary>
		/// <param name="configuration">
		/// The framework properties to configure the new framework instance. 
		/// If framework properties are not provided by the configuration argument, 
		/// the created framework instance must use some reasonable default 
		/// configuration appropriate for the current VM. For example, the system 
		/// packages for the current execution environment should be properly exported. 
		/// The specified configuration argument may be null. 
		/// The created framework instance must copy any information needed from 
		/// the specified configuration argument since the configuration argument 
		/// can be changed after the framework instance has been created.</param>
		/// <returns></returns>
		IFramework NewFramework(object configuration);
	}
}
