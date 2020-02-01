using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.SceneManagement
{
    public static class SceneLoader
    {
        public static void LoadScene(Scenes scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public static AsyncOperation LoadSceneAsync(Scenes scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync((int)scene, loadSceneMode);
        }

        public static void LoadSceneTransition(Scenes scene)
        {
            if(OnSceneTransitionStart != null)
            {
                FireSceneTransitionStart(scene);
            }
            else
            {
                Debug.LogError("No scene transition controller found");
                LoadScene(scene);
            }
        }

        public delegate void SceneTransitionStartEvent(Scenes scene);
        public static event SceneTransitionStartEvent OnSceneTransitionStart;
        public static void FireSceneTransitionStart(Scenes scene)
        {
            OnSceneTransitionStart?.Invoke(scene);
        }

        //public delegate void SceneTransitionDoneEvent();
        //public static event SceneTransitionDoneEvent OnSceneTransitionDone;
        //public static void FireSceneTransitionDone()
        //{
        //    OnSceneTransitionDone?.Invoke();
        //}
    }

}
