using UnityEngine;
using System.Collections;
using Spider;
using Data;
using System.Collections.Generic;
using Spider.Parser;

namespace Spider.Controller {

	public class DataController : SpiderController<DataController>, IController {

		public List<IData> DataBlocks = new List<IData>();
		public string UserName = "Oleg";

		private string _dataDir = "Data/Save/";

		public override void OnInit ()
		{
			GeneralData gd = new GeneralData ();
			gd.SetValue("1111", "156546");
			gd.SetValue("2222", "!!!!");
			gd.SetValue("333", "222");
			DataBlocks.Add (gd);
		}

		public override void OnSceneChange ()
		{
			if (DataBlocks.Count == 0)
				return;
			string path = _dataDir + UserName;
			foreach (IData d in DataBlocks) {
				string e;
				if (d.Prepare(out e))
					d.SaveData(path);
				else {
					ErrorController.Instance.PushError(e, d.GetType());
					return;
				}
			}
		}

		public void Save() {
			string path = _dataDir + UserName;
			BinaryParser bp = new BinaryParser (path);

			foreach (IData d in DataBlocks) {
				string e;
				if (d.Prepare(out e)) {
					bp.Save<IData>(d.GetType().ToString(), d);
				}
			}
		}

		public void Load() {
			string path = _dataDir + UserName;
			BinaryParser bp = new BinaryParser (path);
			List<IData> id = new List<IData> ();
			foreach (IData d in DataBlocks) {
				IData n = new GameData<string>();
				bp.TryLoad(d.GetType().ToString(), ref n);
				id.Add(d);
			}
			DataBlocks = id;
		}

		public void Print() {
			GeneralData gd = (GeneralData)DataBlocks [0];
			foreach (KeyValuePair<string, string> kvp in gd.GetFullData()) {
				print (kvp.Key + " == " + kvp.Value);
			}
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.S))
				Save ();
			else if (Input.GetKeyDown (KeyCode.L))
				Load ();
			else if (Input.GetKeyDown (KeyCode.P))
				Print ();
		}
	}
}