using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core
{
	public interface IFilter
	{
		/// <summary>
		/// Filter using a service's properties.
		/// </summary>
		bool Match(IServiceReference reference);
		
		/// <summary>
		/// Filter using a Dictionary. 
		/// This Filter is executed using the specified Dictionary's keys and values. 
		/// The keys are case insensitively matched with this Filter.
		/// </summary>
		bool Match(Dictionary<string, string> dictionary);

		/// <summary>
		/// Filter with case sensitivity using a Dictionary. This Filter is executed using the specified Dictionary's keys and values. The keys are case sensitively matched with this Filter.
		/// </summary>
		bool MatchCase(Dictionary<string, string> dictionary);

		string ToString();

		bool Equals();

		int GetHashCode();
	}
}
