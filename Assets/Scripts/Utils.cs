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
				instance = GameObject.FindObjectOfType<Utils>();
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

	public void SafeInvoke(string id, System.Action act, float t, bool timescaleDependent) {
		if (!allInvokes.ContainsKey(id)) {
			allInvokes.Add(id,true);
			StartCoroutine(CoInvoke(id,act,t,timescaleDependent));
			if (debugMessages) {
				var logmsg = string.Format("{0} invoke {1} was scheduled to run after {2} sec.",
				                           timescaleDependent?"Timescaled ":"Not timescaled ",
				                           id,t);
				Debug.Log(logmsg);
			}
		} else {
			Debug.Log("Invoke with same id was already scheduled! Cancelling this one.");
		}
	}

	public void SafeInvoke(System.Action act, float t, bool timeScaleDependant) {
		StartCoroutine(CoInvoke(act,t,timeScaleDependant));
	}

	public void CancelSafeInvoke(string id) {
		if (allInvokes.ContainsKey(id) && allInvokes[id]) {
			allInvokes[id]=false;
			if (debugMessages)
				Debug.Log(string.Format("Invoke {0} was cancelled",id));
		}
	}

	IEnumerator CoInvoke(System.Action act, float t, bool timeScaleDependant) {
		if (!timeScaleDependant) {
			float delay = Time.realtimeSinceStartup + t;
			while (Time.realtimeSinceStartup < delay)
				yield return null;
			act();
		} else {
			yield return new WaitForSeconds(t);
			act();
		}
	}

	IEnumerator CoInvoke(string id, System.Action act, float t, bool timescaleDependent) {
			if (!timescaleDependent) {
				float delay = Time.realtimeSinceStartup + t;
				while (Time.realtimeSinceStartup < delay)
					yield return null;
				if (allInvokes[id])
					act();
				allInvokes.Remove(id);
			} else {
				yield return new WaitForSeconds(t);
				if (allInvokes[id])
					act();
				allInvokes.Remove(id);
			}
	}
}
