using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

public interface IData : ISerializable {
	bool Prepare(out string e);
	void SaveData(string dir);
	bool LoadData(string dir);
	void Clear();
	void GetObjectData (SerializationInfo info, StreamingContext ctxt);
}
