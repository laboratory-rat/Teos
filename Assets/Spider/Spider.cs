using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Spider {
	
	public static class @Spider : object {

		private static List<IController> _controllers = new List<IController>();

		public static void Init(IController target) {
			_controllers.Add (target);
			target.OnInit ();
		}

		public static void UnInit(IController target) {
			if (_controllers.Contains (target)) {
				_controllers.Remove(target);
				target.OnUnInit();
			}
		}

		public static void StopApp() {
			foreach (IController target in _controllers) {
				target.OnAppStop();
			}
			_controllers.Clear ();
			Application.Quit ();
		}

		public static void ChangeScene(string s) {
			if (s != Application.loadedLevelName) {
				foreach (IController ic in _controllers)
					ic.OnSceneChange();
				try {
					Application.LoadLevel(s);
				}
				catch(UnityException ue) {
					Debug.LogError(ue.Message);
				}
			}
		}

	}
}