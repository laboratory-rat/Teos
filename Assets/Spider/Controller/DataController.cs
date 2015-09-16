using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spider;
using Spider.Parser;

namespace Spider.Controller {

	public class DataController : SpiderController<DataController>, IController {

		[SerializeField]
		public List<IData> DataBlocks { get; private set; }
		public string Name { get; private set; }

		private string _dataDir = "Data/Save/";

		public override void OnInit ()
		{
			DataBlocks = new List<IData> ();
			Name = "";

		}

		public bool AddData(IData data) {
			foreach (IData id in DataBlocks) {
				if (id.GetType() == data.GetType())
					return false;
			}
			DataBlocks.Add (data);
			return true;
		}

		public override void OnSceneChange ()
		{
			if (DataBlocks.Count == 0)
				return;
			Save ();
		}

		public bool Save(out string e) {
			e = "";
			if (Name == "") {
				e = "No name for save file.";
				return false;
			}
			BinaryParser bp = new BinaryParser (_dataDir);
			bp.Save<List<IData>> (Name, DataBlocks);
			return true;
		}

		public bool Save(string name, out string e) {
			e = "";
			Name = name;
			return Save (out e);
		}

		public void Save() {
			string e;
			Save (out e);
		}

		public bool Load(string name, out string e) {
			e = "";
			Name = name;
			BinaryParser bp = new BinaryParser (_dataDir);
			List<IData> tmp = new List<IData>();
			if (!bp.TryLoad<List<IData>> (Name, ref tmp)) {
				e = "Can`t load data with name: " + Name;
				return false;
			}
			DataBlocks = tmp;
			return true;
		}

		public override void OnUnInit ()
		{
			Name = "";
			DataBlocks.Clear ();
			base.OnUnInit ();
		}
	}
}