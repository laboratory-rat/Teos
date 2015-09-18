using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Spider.Parser
{
	public class XmlParser
	{

		public string FilePath;
		private string _base;

		const string extension = ".xml";

		public XmlParser(string path, bool useRealPath = true){
			FilePath = path;

			if (useRealPath)
				_base = "";
			else
				_base = Application.persistentDataPath;
		}

		public bool TryLoad<T>(string name, ref T file){
			if (!Directory.Exists (_base +  FilePath) && (_base != "" || FilePath != "")) {
				string newDir = (_base == "") ? FilePath : _base + "/" + FilePath;
				Directory.CreateDirectory (newDir);
				Debug.LogWarning("No directory " + newDir + ". " + this.ToString());
				return false;
			}

			string fullPath = _base + FilePath + name + extension;

			if (!File.Exists (fullPath)) {
				Debug.LogWarning("No XML file '" + name +"' at " + FilePath);
				return false;
			}

			FileStream fs = new FileStream (@fullPath, FileMode.Open);
			XmlSerializer xs = new XmlSerializer(typeof(T));

			try
			{
				file = (T)xs.Deserialize (fs);
			}
			catch(XmlException e)
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
				XmlSerializer xs = new XmlSerializer(typeof(T));
				xs.Serialize(fs, file);	
			}
			catch(XmlException e)
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
