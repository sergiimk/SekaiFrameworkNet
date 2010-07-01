using System;
using System.Collections.Generic;
using System.Text;

namespace framework_console
{
	public interface ICommandInterpreter
	{
		string getCommandString();

		int getArgumentCount();

		string getArgument(int i);

		bool getArgument(int i, out int val);

		bool getArgument(int i, out float val);

		void Error(string message);

		void Error(string message, Exception ex);
	}
}
