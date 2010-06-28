using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

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

			m_firstFreeID = 1;
			m_bundlesByID.Add(0, systemBundle);
			m_bundlesByLocation.Add(systemBundle.getLocation(), systemBundle);
		}

		//////////////////////////////////////////////////////////////////////////

		public CBundle getBundle(long id)
		{
			lock (m_lock)
			{
				CBundle ret;
				m_bundlesByID.TryGetValue(id, out ret);
				return ret;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public CBundle getBundle(string location)
		{
			lock (m_lock)
			{
				CBundle ret;
				m_bundlesByLocation.TryGetValue(location, out ret);
				return ret;
			}
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

		public CBundle InstallBundle(string location, System.IO.Stream input)
		{
			/*
			1.If a bundle containing the same location identifier is already installed, the Bundle object for that bundle is returned.
			2.The bundle's content is read from the input stream. If this fails, a BundleException is thrown.
			3.The bundle's associated resources are allocated. The associated resources minimally consist 
			 * of a unique identifier and a persistent storage area if the platform has file system support. 
			 * If this step fails, a BundleException is thrown.
			4.The bundle's state is set to INSTALLED.
			5.A bundle event of type BundleEvent.INSTALLED is fired.
			6.The Bundle object for the newly or previously installed bundle is returned.
			*/
			lock (m_lock)
			{
				if (input != null)
					throw new NotImplementedException();

				CBundle bndl = getBundle(location);
				if (bndl != null)
					return bndl;

				CManifest manifest = new CManifest();
				manifest.SymbolicName = "<symbolic_name>";
				manifest.Version = new Version(6, 6, 6, 6);
				manifest.AssemblyPath = Path.Combine(m_systemBundle.getConfig().BundleRegistryPath, location);
				manifest.AssemblyPath = Path.Combine(manifest.AssemblyPath, location + ".dll");

				bndl = new CBundle(m_firstFreeID++, location, manifest, DateTime.Now, m_systemBundle);
				m_bundlesByID.Add(bndl.getBundleId(), bndl);
				m_bundlesByLocation.Add(bndl.getLocation(), bndl);

				m_systemBundle.RaiseBundleEvent(new BundleEvent(BundleEvent.Type.INSTALLED, bndl));
				return bndl;
			}
		}

		//////////////////////////////////////////////////////////////////////////

		public void RemoveBundle(long id)
		{
			lock (m_lock)
			{
				IBundle bndl = getBundle(id);

				if (bndl == null)
					return;

				m_bundlesByID.Remove(id);
				m_bundlesByLocation.Remove(bndl.getLocation());
			}
		}

		//////////////////////////////////////////////////////////////////////////

		long m_firstFreeID; // TODO: deal with fragmentation?
		CSystemBundle m_systemBundle;
		Dictionary<long, CBundle> m_bundlesByID;
		Dictionary<string, CBundle> m_bundlesByLocation;

		object m_lock = new object();
	}
}
