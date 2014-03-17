using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// You need to attach this script to any of GameObjects on Scene.
/// </summary>
public class Utils : MonoBehaviour {

	static GameObject uObject;
	static Utils instance;

	bool debugMessages = true;

	Dictionary<string,bool> allInvokes = new Dictionary<string,bool>();


	public static Utils Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType(typeof(Utils)) as Utils;
				if (instance!=null)
					uObject = instance.gameObject;
			}
			if (instance == null) {
				uObject = new GameObject("Utils");
				instance = uObject.AddComponent<Utils>();
			}
			return instance;
		}
	}

	public void SafeInvoke(string id, System.Action act, float t, bool timeScaleDependant) {
		allInvokes.Add(id,true);
		StartCoroutine(coInvoke(id,act,t,timeScaleDependant));
		if (debugMessages)
			Debug.Log(timeScaleDependant?"Timescaled":"Not timescaled" + " invoke "+id+" was scheduled to run after "+t+" sec.");
	}

	public void SafeInvoke(System.Action act, float t, bool timeScaleDependant) {
		StartCoroutine(coInvoke(act,t,timeScaleDependant));
	}

	public void CancelSafeInvoke(string id) {
		allInvokes[id]=false;
		if (debugMessages)
			Debug.Log("Invoke "+id+" was cancelled");
	}

	IEnumerator coInvoke(System.Action act, float t, bool timeScaleDependant) {
		if (!timeScaleDependant) {
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + t)
				yield return null;
			act();
		} else {
			yield return new WaitForSeconds(t);
			act();
		}
	}

	IEnumerator coInvoke(string id, System.Action act, float t, bool timeScaleDependant) {
			if (!timeScaleDependant) {
				float start = Time.realtimeSinceStartup;
				while (Time.realtimeSinceStartup < start + t)
					yield return null;
				if (allInvokes[id])
					act();
				else
					allInvokes.Remove(id);
			} else {
				yield return new WaitForSeconds(t);
				if (allInvokes[id])
					act();
				else
					allInvokes.Remove(id);
			}
	}
}
