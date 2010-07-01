using System;
using System.Collections.Generic;
using System.Text;

namespace framework_console
{
	/// <summary>
	/// Consoles are intended to be registered as a service.
	/// Console command and variable set can be extended by implementing
	/// and exposing ICommandProvider interface.
	/// Single console can have multiple sessions (execution contexts) that
	/// have separate input and output streams and script contexts.
	/// </summary>
	public interface IConsole
	{
		IConsoleSession CreateSession();
	}
}
