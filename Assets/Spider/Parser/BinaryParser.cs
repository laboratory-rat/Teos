using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Spider.Parser
{
	public class BinaryParser
	{
		
		public string FilePath;
		private string _base;
		
		const string extension = ".xml";
		
		public BinaryParser(string path, bool useRealPath = true){
			FilePath = path;
			
			if (useRealPath)
				_base = "";
			else
				_base = Application.persistentDataPath;
		}
		
		public bool TryLoad<T>(string name, ref T file){
			if (!Directory.Exists (_base +  FilePath) && (_base != "" || FilePath != "")) {
				Debug.LogWarning("No directory " + _base +  FilePath + ". " + this.ToString());
				return false;
			}
			
			string fullPath = _base + FilePath + name + extension;
			
			if (!File.Exists (fullPath)) {
				Debug.LogWarning("No binary file '" + name +"' at " + FilePath);
				return false;
			}
			
			FileStream fs = new FileStream (@fullPath, FileMode.Open);
			
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				file = (T)bf.Deserialize(fs);
			}
			catch(SerializationException e)
			{
				Debug.LogError(e.Message);
				return false;
			}
			finally
			{
				fs.Close ();
			}
			return true;
		}
		
		public void Save<T>(string name, T file){
			if (!Directory.Exists (_base + FilePath) && (_base != "" || FilePath !="")) {
				string newDir = (_base == "") ? FilePath : _base + "/" + FilePath;
				Directory.CreateDirectory (newDir);
			}
			
			string fullPath = _base +  FilePath + name + extension;
			
			FileStream fs = new FileStream (@fullPath, FileMode.Create);
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fs, file);
			}
			catch(SerializationException e)
			{
				Debug.LogError(e.Message);
			}
			finally
			{
				fs.Close ();
			}
		}
	}
}


