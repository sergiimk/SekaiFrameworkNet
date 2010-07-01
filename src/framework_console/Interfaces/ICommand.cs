using System;
using System.Collections.Generic;
using System.Text;

namespace framework_console
{
	//////////////////////////////////////////////////////////////////////////

	public struct CommandInfo
	{
		public string Name;
		public string CallFormat;
		public string ShortDescription;
		public string Help;
	}

	//////////////////////////////////////////////////////////////////////////

	public interface ICommand
	{
		CommandInfo getInfo();

		void Execute(ICommandInterpreter cmd);
	}

	//////////////////////////////////////////////////////////////////////////

	public class DelegateCommand : ICommand
	{
		public delegate void ExecuteDeleg(ICommandInterpreter cmd);
		ExecuteDeleg m_delegate;
		CommandInfo m_info;

		public CommandInfo getInfo()
		{
			return m_info;
		}

		public void Execute(ICommandInterpreter cmd)
		{
			m_delegate(cmd);
		}

		public DelegateCommand(string name, ExecuteDeleg deleg, string fmt, string desc, string help)
		{
			m_info.Name = name;
			m_info.CallFormat = fmt;
			m_info.ShortDescription = desc;
			m_info.Help = help ?? desc;
			m_delegate = deleg;
		}
	}

	//////////////////////////////////////////////////////////////////////////
}
