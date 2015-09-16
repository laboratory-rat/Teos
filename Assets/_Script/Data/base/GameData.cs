using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Spider;
using Spider.Controller;
using Spider.Parser;

namespace Data {

	[System.Serializable]
	public class GameData<T> : MonoBehaviour, IData {

		protected bool _prepared = false;
		protected Dictionary<string, T> _data = new Dictionary<string, T>();

		public virtual bool TryGetValue(string s, ref T value) {
			if (_data.ContainsKey (s)) {
				value = _data [s];
				return true;
			}
			return false;
		}

		public virtual Dictionary<string, T> GetFullData() {
			return _data;
		}

		public virtual void SetValue(string s, T value) {
			if (_data.ContainsKey (s)) {
				_data [s] = value;
			} else {
				_data.Add (s, value);
			}
			_prepared = false;
		}

		public virtual void SetFullData(Dictionary<string, T> data) {
			_data = data;
			_prepared = false;
		}

		public virtual bool SaveData(string dir, out string e) {
			e = "";
			BinaryParser bp = new BinaryParser (dir);
			bp.Save<Dictionary<string, T>> (this.name, _data);
			return true;
		}

		public virtual bool LoadData(string dir, out string e) {
			e = "";
			BinaryParser bp = new BinaryParser (dir);
			_prepared = false;
			if (!bp.TryLoad<Dictionary<string, T>> (this.name, ref _data)) {
				e = "Can`t load data package whis name:" + name + " and type: " + this.name;
				return false;
			}
			return true;
		}

		public virtual void Clear() {
			_data = new Dictionary<string, T> ();
			_prepared = false;
		}

		public virtual bool Prepare (out string e) {
			e = "";
			if (!_prepared)
				_prepared = true;
			return _prepared;
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			//You can use any custom name for your name-value pair. But make sure you
			// read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
			// then you should read the same with "EmployeeId"
			info.AddValue("data", _data);
		}
	}
}