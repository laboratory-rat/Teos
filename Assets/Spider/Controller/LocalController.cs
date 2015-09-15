using UnityEngine;
using System.Collections;
using Spider.Parser;
using System.IO;
using System.Text.RegularExpressions;
using Spider;

namespace Spider.Controller {

	public class LocalController : SpiderController<LocalController>, IController {

		string _language = "RUSSIAN";
		string _localPath = "Local/";
		string _ex = ".ini";
		IniParser _local = null;
		string _path;
		string _file;

		public override void OnInit() {

			_path = Path.Combine (Application.dataPath, _localPath);

			if (!Directory.Exists (_path)) {
				Directory.CreateDirectory(_path);
				Debug.LogWarning("No localization directory.");
				return;
			}

			_language = ConfigController.Instance.GetValue ("LANGUAGE", "GAME");
			_file = Path.Combine (_path, _language + _ex);


			if (!File.Exists (_file)) {
				Debug.LogWarning("No language " + _language);
				return;
			}

			_local = new IniParser (_localPath + _language + _ex);
		}

		public string Local(string key, string section) {
			if (_local == null)
				return key;
			string str = _local.GetSetting(key, section);
			if (str.Length == 0)
				return key;
			return str;
		}

		public bool ChangeLocal(string name) {
			name = name.ToUpper ();
			_local = null;
			if (!File.Exists (_localPath + _language + _ex)) {
				Debug.LogWarning("No language " + _language);
				return false;
			}
			
			_local = new IniParser (_localPath + _language + _ex);
			_language = name;
			return true;
		}

		public string GetLanguage() {
			return _language;
		}

		public override void OnAppStop ()
		{
			_local = null;
			base.OnAppStop ();
		}
	}
}