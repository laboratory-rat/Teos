using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Spider {

	public interface IData : ISerializable {
		bool Prepare(out string e);
		bool SaveData(string dir, out string e);
		bool LoadData(string dir, out string e);
		void Clear();
		//void GetObjectData (SerializationInfo info, StreamingContext ctxt);
	}	
}