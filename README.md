unity-utils
===========

Utils.cs singletone class for some Unity missing-out-of-the-box features.

Current features:
-----------------
- SafeInvoke based on Unity Coroutines. You can use it to schelude functions to run like this:
```
Utils.Instance.SafeInvoke(string id, System.Action act, float t, bool timescaleDependent);
```
**id** - string identificator.  
**act** - can be anonymous function or delegate.  
**t** - time in seconds.  
**timescaleDependent** - should it rely on Unity built-in Time.timescale parameter or not.  

**id** can be used later to cancel this SafeInvoke via:
```
Utils.CancelSafeInvoke(string id);
```
