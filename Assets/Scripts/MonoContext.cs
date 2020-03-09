using System;
using UnityEngine;
using System.Linq;

namespace PiXYZ.Utils  {

    public delegate void VoidHandler();


    [ExecuteInEditMode]
    public sealed class MonoContext : MonoBehaviour {

        public static VoidHandler OnUpdate;

        public static VoidHandler OnApplicationFocused;

        // Name of MonoContext GameObject (Unique)
        const string NAME = "__MonoContext__";

        // Actions remaining to be dispatched in an Unity Thread
        private static ConcurrentQueue<Action> _ActionsToDispatch = new ConcurrentQueue<Action>();

        // Reference to unique MonoContext GameObject
        private static GameObject _MonoContextObject;

        private static MonoContext _MonoContext;
        public static MonoContext Instance {
			get {
                if (_MonoContext == null || _MonoContextObject == null) {
                    // Takes existing context
                    GetMonoContextObject();
                    if (_MonoContextObject == null) {
                        // If no context exists, create one.
                        _MonoContextObject = new GameObject(NAME);
                        _MonoContextObject.hideFlags = HideFlags.HideInHierarchy;
                        _MonoContext = _MonoContextObject.AddComponent<MonoContext>();
                        if (Application.isPlaying) DontDestroyOnLoad(_MonoContextObject);
                    } else {
                        _MonoContext = _MonoContextObject.GetComponent<MonoContext>();
                        if (_MonoContext == null)
                            _MonoContext = _MonoContextObject.AddComponent<MonoContext>();
                    }
                }
                return _MonoContext;
            }
		}

        public static void GetMonoContextObject() {
            foreach (GameObject mcinst in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(x => x.name == NAME)) {
                if (_MonoContextObject == null) {
                    _MonoContextObject = mcinst;
                } else {
                    int ccount = mcinst.GetComponents<MonoContext>().Count();
                    Debug.LogWarning("MonoContext (" + ccount + ") Killed !");
                    DestroyImmediate(mcinst);
                }
            }
        }

        public static void Clear() {
            foreach (GameObject mcinst in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(x => x.name == NAME)) {
                int ccount = mcinst.GetComponents<MonoContext>().Count();
                Debug.LogWarning("MonoContext (" + ccount + ") Killed !");
                DestroyImmediate(mcinst);
            }
            _MonoContextObject = null;
            _MonoContext = null;
        }

        public static bool CheckInstance() {
            return Instance != null;
        }

        public static void DispatchInMainThread(Action action) {
            if (action != null) {
                _ActionsToDispatch.Enqueue(action);
            }
        }

        public void Update() {
            OnUpdate?.Invoke();
            dispatch();
        }

        private void dispatch() {
            while (_ActionsToDispatch.Count != 0) {
                _ActionsToDispatch.Dequeue()?.Invoke();
            }
        }

        private void OnApplicationFocus(bool focus) {
            if (focus) {
                OnApplicationFocused?.Invoke();
            }
        }
    }
}