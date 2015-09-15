using UnityEngine;
using System.Collections;

namespace Spider.UI.Display {
	public class PanelSwitcher : MonoBehaviour {

		public bool SwitchOffAtStart = true;
		public bool ShowMainPanel = false;
		public bool CloseOnDoubleClick = true;
		public GameObject MainPanel;
		public GameObject[] Panels;

		GameObject current = null;

		void Start() {

			if (Panels.Length < 1)
				return;

			if (SwitchOffAtStart) {
				foreach (GameObject go in Panels)
					go.SetActive(false);
			}
			if (ShowMainPanel && MainPanel != null) {
				MainPanel.SetActive (true);
				current = MainPanel;
			}
		}

		public void Switch (GameObject panel) {

			if (current == null) {
				current = panel;
				current.SetActive(true);
			} else if (panel == current && CloseOnDoubleClick) {
				current.SetActive (false);
				current = null;
			} else if (panel != current) {
				current.SetActive(false);
				current = panel;
				current.SetActive(true);
			}
		}

		public void CloseCurrent() {
			if (current != null){
				current.SetActive (false);
				current = null;
			}
		}

	}
}