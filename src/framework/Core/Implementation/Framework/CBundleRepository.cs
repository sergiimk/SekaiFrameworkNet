using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class CBundleRepository
	{
		//////////////////////////////////////////////////////////////////////////

		public CBundleRepository(CSystemBundle systemBundle)
		{
			m_systemBundle = systemBundle;
			m_bundlesByID = new Dictionary<long, CBundle>();
			m_bundlesByLocation = new Dictionary<string, CBundle>();

			lock (m_lock)
			{
				m_bundlesByID.Add(0, systemBundle);
				m_bundlesByLocation.Add(systemBundle.getLocation(), systemBundle);
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public CBundle getBundle(long id)
		{
			CBundle ret;
			m_bundlesByID.TryGetValue(id, out ret);
			return ret;
		}

		//////////////////////////////////////////////////////////////////////////

		public CBundle getBundle(string location)
		{
			CBundle ret;
			m_bundlesByLocation.TryGetValue(location, out ret);
			return ret;
		}

		//////////////////////////////////////////////////////////////////////////

		public IBundle[] getBundles()
		{
			lock (m_lock)
			{
				int i = 0;
				IBundle[] ret = new IBundle[m_bundlesByID.Count];
				
				foreach (CBundle bndl in m_bundlesByID.Values)
					ret[i++] = bndl;

				return ret;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveBundle(long id)
		{
			IBundle bndl = getBundle(id);
			
			if (bndl == null)
				return;

			m_bundlesByID.Remove(id);
			m_bundlesByLocation.Remove(bndl.getLocation());
		}

		//////////////////////////////////////////////////////////////////////////

		CSystemBundle m_systemBundle;
		Dictionary<long, CBundle> m_bundlesByID;
		Dictionary<string, CBundle> m_bundlesByLocation;

		object m_lock = new object();
	}
}
