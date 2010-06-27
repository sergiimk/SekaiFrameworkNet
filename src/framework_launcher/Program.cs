using System;
using System.Collections.Generic;
using System.Text;
using framework.Core;

namespace framework_launcher
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				IFrameworkFactory factory = new CFrameworkFactory();
				IFramework fwk = factory.NewFramework(null);
				fwk.Init();
				fwk.Start();
				fwk.Stop();
				fwk.WaitForStop(0);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
