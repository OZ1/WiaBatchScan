using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WiaBatchScan
{
	struct WiaPropertyStorageView : IReadOnlyDictionary<string, object>, IReadOnlyDictionary<uint, object>
	{
		readonly IWiaPropertyStorage m_p;

		public WiaPropertyStorageView(IWiaPropertyStorage p)
		{
			m_p = p;
		}

		public int Count => m_p.GetCount();

		public object this[uint key] => Read(new[] { new PROPSPEC(key) })[0];

		public object this[string key]
		{
			get
			{
				using (var prop = new PROPSPEC(key))
					return Read(new[] { prop })[0];
			}
		}

		IEnumerable<uint> IReadOnlyDictionary<uint, object>.Keys => GetKeys().Select(x => x.propid);

		public IEnumerable<string> Keys => GetKeys().Select(x => x.lpwstrName);

		public STATPROPSTG[] GetKeys()
		{
			var count = m_p.GetCount();
			var props = new STATPROPSTG[count];
			var pEnum = m_p.Enum();
			pEnum.Reset();
			pEnum.Next((uint)count, props, out var fetched);
			return props;
		}

		public object[] GetValues(out STATPROPSTG[] keys)
		{
			keys = GetKeys();
			var props = new PROPSPEC[keys.Length];
			for (int i = 0; i < keys.Length; i++)
				props[i] = new PROPSPEC(keys[i].propid);
			return Read(props);
		}

		object[] Read(PROPSPEC[] keys)
		{
			var vars = new PROPVARIANT[keys.Length];
			m_p.ReadMultiple(keys.Length, keys, vars);
			var values = new object[keys.Length];
			for (int i = 0; i < values.Length; i++)
				values[i] = vars[i].Value;
			for (int i = 0; i < values.Length; i++)
				vars[i].Clear();
			return values;
		}

		public IEnumerable<object> Values => GetValues(out var keys);

		public bool ContainsKey(uint key) => TryGetValue(key, out var value);
		public bool ContainsKey(string key) => TryGetValue(key, out var value);

		IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyDictionary<string, object>)this).GetEnumerator();

		IEnumerator<KeyValuePair<uint, object>> IEnumerable<KeyValuePair<uint, object>>.GetEnumerator()
		{
			var values = GetValues(out var keys);
			for (int i = 0; i < values.Length; i++)
				yield return new KeyValuePair<uint, object>(keys[i].propid, values[i]);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			var values = GetValues(out var keys);
			for (int i = 0; i < values.Length; i++)
				yield return new KeyValuePair<string, object>(keys[i].lpwstrName, values[i]);
		}

		public bool TryGetValue(uint key, out object value)
		{
			try
			{
				value = this[key];
				return true;
			}
			catch (COMException)
			{
				value = null;
				return false;
			}
		}

		public bool TryGetValue(string key, out object value)
		{
			try
			{
				value = this[key];
				return true;
			}
			catch (COMException)
			{
				value = null;
				return false;
			}
		}
	}
}
