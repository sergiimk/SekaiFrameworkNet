using System;
using System.Collections.Generic;
using System.Text;

namespace framework.Core.Implementation
{
	class FreePool<T> where T: new()
	{
		LinkedList<T> m_items = new LinkedList<T>();

		public T Allocate()
		{
			lock(m_items)
			{
				if(m_items.Count > 0)
				{
					T ret = m_items.Last.Value;
					m_items.RemoveLast();
					return ret;
				}
				return default(T);
			}
		}

		public void Free(T item)
		{
			lock (m_items)
			{
				m_items.AddLast(item);
			}
		}
	}
}
