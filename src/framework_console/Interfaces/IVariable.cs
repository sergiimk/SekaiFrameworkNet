using System;
using System.Collections.Generic;
using System.Text;

namespace framework_console
{
	//////////////////////////////////////////////////////////////////////////

	public class VariableInfo
	{
		public string Name;
		public string ShortDescription;
		public string Help;
	}

	//////////////////////////////////////////////////////////////////////////

	public interface IVariable
	{
		VariableInfo getInfo();

		object getValue();

		void setValue(ICommandInterpreter cmd);
	}

	//////////////////////////////////////////////////////////////////////////
	// Convenience
	//////////////////////////////////////////////////////////////////////////

	public abstract class GenericVariable<T> : IVariable
	{
		public delegate bool ChangeDeleg(T curr_val, T new_val);

		T m_value;
		VariableInfo m_info;
		ChangeDeleg m_delegate;

		public VariableInfo getInfo()
		{
			return m_info;
		}

		public object getValue()
		{
			return m_value;
		}

		public void setValue(ICommandInterpreter cmd)
		{
			T new_val;
			string svalue = cmd.getArgument(0);

			if (!parseValue(svalue, out new_val))
			{
				cmd.Error("Invalid variable value");
				return;
			}
			if (!m_value.Equals(new_val) && (m_delegate == null || m_delegate(m_value, new_val)))
				m_value = new_val;
		}

		public GenericVariable(string name, T default_val, ChangeDeleg deleg, string desc, string help)
		{
			m_info.Name = name;
			m_info.ShortDescription = desc;
			m_info.Help = help ?? desc;
			m_delegate = deleg;
			m_value = default_val;
		}

		protected abstract bool parseValue(string value, out T new_val);
	}

	//////////////////////////////////////////////////////////////////////////

	public class IntVariable : GenericVariable<int>
	{
		protected override bool parseValue(string value, out int new_val)
		{
			return int.TryParse(value, out new_val);
		}

		public IntVariable(string name, int default_val, ChangeDeleg deleg, string desc, string help)
			: base(name, default_val, deleg, desc, help)
		{
		}
	}

	//////////////////////////////////////////////////////////////////////////

	public class FloatVariable : GenericVariable<float>
	{
		protected override bool parseValue(string value, out float new_val)
		{
			return float.TryParse(value, out new_val);
		}

		public FloatVariable(string name, float default_val, ChangeDeleg deleg, string desc, string help)
			: base(name, default_val, deleg, desc, help)
		{
		}
	}

	//////////////////////////////////////////////////////////////////////////

	public class StringVariable : GenericVariable<string>
	{
		protected override bool parseValue(string value, out string new_val)
		{
			new_val = value;
			return true;
		}

		public StringVariable(string name, string default_val, ChangeDeleg deleg, string desc, string help)
			: base(name, default_val, deleg, desc, help)
		{
		}
	}

	//////////////////////////////////////////////////////////////////////////
}
