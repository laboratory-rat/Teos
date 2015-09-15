using UnityEngine;
using System.Collections;

namespace Spider {

	public interface IController{

		void OnInit();
		void OnUnInit();
		void OnSceneChange();
		void OnAppStop();
	}
}