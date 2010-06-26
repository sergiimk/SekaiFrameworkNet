using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	/// <summary>
	/// A Framework instance. A Framework is also known as a System Bundle.
	/// Framework instances are created using a FrameworkFactory. 
	/// The methods of this interface can be used to manage and control the created framework instance.
	/// </summary>
	public interface IFramework : IBundle
	{
		/// <summary>
		/// Initialize this Framework.
		/// </summary>
		void Init();

		/// <summary>
		/// Wait until this Framework has completely stopped. 
		/// The stop and update methods on a Framework performs an asynchronous stop of the Framework. 
		/// This method can be used to wait until the asynchronous stop of this Framework has completed. 
		/// This method will only wait if called when this Framework is in the Bundle.STARTING, Bundle.ACTIVE, or Bundle.STOPPING states. 
		/// Otherwise it will return immediately.
		/// </summary>
		/// <param name="timeout">Maximum number of milliseconds to wait until 
		/// this Framework has completely stopped. A value of zero will wait indefinitely.</param>
		/// <returns>Framework Event indicating the reason this method returned</returns>
		FrameworkEvent WaitForStop(long timeout);
	}
}
