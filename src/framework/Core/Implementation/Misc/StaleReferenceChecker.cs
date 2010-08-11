using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	struct StaleReferenceChecker
	{
		public delegate void OnStaleRefAccess();

		volatile bool m_disposed;
		volatile bool m_disposing;
		OnStaleRefAccess m_deleg;

		public StaleReferenceChecker(OnStaleRefAccess deleg)
		{
			m_deleg = deleg;
			m_disposed = false;
			m_disposing = false;
		}

		public void BeginDispose()
		{
			m_disposing = true;
		}

		public void EndDispose()
		{
			m_disposing = false;
			m_disposed = true;
		}

		public void Check()
		{
			if (!m_disposing && m_disposed)
				m_deleg();
		}
	}
}
