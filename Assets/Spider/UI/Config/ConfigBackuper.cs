using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spider.Parser;
using Spider.Controller;

namespace Spdier.UI.Config{

	public class ConfigBackuper : MonoBehaviour {

		private IniParser _configBack;
		private ConfigController _configC;

		void Start() { 

			// Get config controller

			_configC = ConfigController.Instance;
			_configBack = _configC.GetConfig();
		}



		public void SaveChanges() {
			_configC.SaveConfig ();
			_configBack = _configC.GetConfig ();
		}

		public void RemoveChanges() {
			_configC.SetConfig (_configBack);
		}


		void OnEnable() {
			if (_configC != null)
				_configBack = _configC.GetConfig ();
		}

	}
}