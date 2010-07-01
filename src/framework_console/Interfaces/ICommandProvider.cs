using System;
using System.Collections.Generic;
using System.Text;

namespace framework_console
{
	/// <summary>
	/// This interface is intended to be implemented by modules who
	/// want to extend the console functionality
	/// </summary>
	public interface ICommandProvider
	{
		ICommand[] getCommands();

		IVariable getVariables();
	}
}
