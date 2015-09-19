using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spider.Parser;
using Spider.Controller;

namespace Spdier.UI.Config{

	public class ConfigBackuper : MonoBehaviour {

		private Configuration _config;
		private Configuration _backup;

		void Start() { 

			// Get config and backup
			_config = ConfigController.Instance.Config;
			_backup = new Configuration (_config);

		}

		public void SaveChanges() {
			ConfigController.Instance.SaveConfig ();
			_backup = new Configuration (_config);
		}

		public void RemoveChanges() {
			ConfigController.Instance.ReplaceConfig (_backup);
			_config = ConfigController.Instance.Config;
		}


		void OnEnable() {
			Start ();
		}

	}
}