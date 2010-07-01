using System;
using System.Collections.Generic;
using System.Text;
using framework.Core;
using System.IO;

namespace framework_console
{
	/// <summary>
	/// Console executions context.
	/// Different console session share all the variables and commands,
	/// but are able to send own input and receive own output
	/// </summary>
	public interface IConsoleSession : IDisposable
	{
		IConsole getConsole();

		Stream getOutputStream();

		Stream getInputStream();
	}
}
