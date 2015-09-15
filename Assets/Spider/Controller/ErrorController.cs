using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using Spider;
using Spider.Parser;

namespace Spider.Controller {

	/// <summary>
	/// Package for error info.
	/// </summary>

	public class ErrorPackage {

		public string Message;
		public string Target;
		public string ErrorTime;

		public ErrorPackage (string m, string t) {
			Message = m;
			Target = t;
			ErrorTime = DateTime.Now.ToLongTimeString();
			ErrorTime = ErrorTime.Replace ('/', '-');
		}

	}

	public enum SaveType {XML, Bin, Ini, Log}

	/// <summary>
	/// Main error controller.
	/// </summary>

	public class ErrorController : SpiderController<ErrorController>, IController {
	
		[SerializeField]
		public string ErrorDirectory = "Error";
		[SerializeField]
		public SaveType SType = SaveType.Log;

		private List<ErrorPackage> _errors = new List<ErrorPackage> ();
		EventController _ec;
		string _path;

		void Start() {
			_path = Path.Combine (Application.dataPath, ErrorDirectory);
			if (!Directory.Exists (_path))
				Directory.CreateDirectory (_path);
		}

		public void PushError (string message, object ob) {
			ErrorPackage e = new ErrorPackage (message, ob.ToString ());
			_errors.Add (e);
		}

		public override void OnAppStop() {
			if (_errors.Count == 0)
				return;
			_path = Path.Combine (Application.dataPath, ErrorDirectory);
			if (!Directory.Exists (_path))
				Directory.CreateDirectory (_path);
			string name = "Log_" + Path.GetRandomFileName ();
			string file = Path.Combine (_path, name);
			switch (SType) {
			case SaveType.XML :
				XmlParser xp = new XmlParser(ErrorDirectory);
				xp.Save<List<ErrorPackage>>(name + ".xml", _errors); 
				break;
			case SaveType.Bin:
				BinaryParser bp = new BinaryParser(ErrorDirectory);
				bp.Save<List<ErrorPackage>>(name + ".bin", _errors);
				break;
			case SaveType.Ini:
				file += ".ini";
				if (File.Exists(file))
					File.Delete(file);
				StreamWriter sw = new StreamWriter(file, true);
				IniParser ip = new IniParser(ErrorDirectory + name);
				foreach (ErrorPackage ep in _errors) {
					ip.AddSetting("Object", ep.Target, ep.ErrorTime);
					ip.AddSetting("Message", ep.Message, ep.ErrorTime);
				}
				ip.SaveSettings();
				sw.Close();
				break;
			case SaveType.Log:
				file += ".log";
				StreamWriter swr = File.CreateText(file);
				foreach(ErrorPackage ep in _errors) {
					string line = "[" + ep.ErrorTime + "]" + " / [" + ep.Target + "] -- " + ep.Message + ";";
					swr.WriteLine(line);
				}
				swr.Close();
				break;
			default:
				Debug.LogError("Bad type for save error log.");
				break;
			}

		}
	}
}