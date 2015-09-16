using UnityEngine;
using Spider;

namespace Spider.Controller {

	public class SpiderController<T> : MonoBehaviour where T : MonoBehaviour, IController
	{
		private static T _instance;
		private static bool applicationIsQuitting = false;
		private static object _lock = new object();
		
		public static T Instance
		{
			get
			{
				if (applicationIsQuitting) {
					Debug.LogWarning("[SpiderController] Instance '"+ typeof(T) +
					                 "' already destroyed on application quit." +
					                 " Won't create again - returning null.");
					return null;
				}
				
				lock(_lock)
				{
					if (_instance == null)
					{
						_instance = (T) FindObjectOfType(typeof(T));
						
						if ( FindObjectsOfType(typeof(T)).Length > 1 )
						{
							Debug.LogError("[SpiderController] Something went really wrong " +
							               " - there should never be more than 1 <Type>Controller!" +
							               " But programm will fix it.");
							return _instance;
						}
						
						if (_instance == null)
						{
							GameObject SpiderController = GameObject.FindGameObjectWithTag("Controller");
							if (SpiderController == null) {
								SpiderController = new GameObject("Controller");
								SpiderController.tag = "Controller";
							}

							_instance = SpiderController.AddComponent<T>();
							DontDestroyOnLoad(SpiderController);
							
							Debug.Log("[SpiderController] An instance of " + typeof(T) + 
							          " is needed in the scene, so '" + SpiderController +
							          "' was created with DontDestroyOnLoad.");
						}
					}
					
					return _instance;
				}
			}
		}
		
		public virtual void Awake() {
			if (Instance != this)
				Destroy (this);
			else {
				Spider.Init (this as IController);
				DontDestroyOnLoad(this);
			}
		}

		public virtual void OnDestroy () {
			applicationIsQuitting = true;
		}

		public virtual void OnInit() {
	
		}

		public virtual void OnUnInit() {
			Destroy (this);
		}

		public virtual void OnSceneChange() {

		}

		public virtual void OnAppStop() {
			Destroy (this);
		}

	}
}