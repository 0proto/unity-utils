using UnityEngine;
using System.Collections;

public class TestUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Utils.Instance.SafeInvoke("one",()=>{Debug.Log("Woohooo!");},2,false);
		Utils.Instance.SafeInvoke("two",()=>{Debug.Log("Eeeeeeeey!");},1,true);
		Utils.Instance.CancelSafeInvoke("one");
		Utils.Instance.CancelSafeInvoke("one");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
