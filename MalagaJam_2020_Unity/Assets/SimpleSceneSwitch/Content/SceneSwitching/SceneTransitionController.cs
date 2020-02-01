using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController m_instance;

    [SerializeField] private string m_closeTrigger = "close";
    [SerializeField] private string m_openTrigger = "open";

    private bool m_inTransition;
    private bool m_inClosingAnimation;
    private Animator m_animator;
    private Queue<Scenes> m_sceneLoadQueue = new Queue<Scenes>();


    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this);
            SceneLoader.OnSceneTransitionStart += LoadScene;
            m_animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneTransitionStart -= LoadScene;
    }


    private void LoadScene(Scenes scene)
    {
        if (m_inTransition == false)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Single);
            StartCoroutine(LoadSceneCoroutine(operation));
        }
        else
        {
            Debug.Log(scene.ToString() + " Added Queue");
            m_sceneLoadQueue.Enqueue(scene);
        }
    }

    private IEnumerator LoadSceneCoroutine(AsyncOperation operation)
    {

        m_inTransition = true;
        m_inClosingAnimation = false;

        operation.allowSceneActivation = false;
        
        m_animator.SetTrigger(m_closeTrigger);

        yield return new WaitUntil(() => m_inClosingAnimation == true);
        operation.allowSceneActivation = true;
        yield return new WaitUntil(() => operation.isDone == true);

        m_animator.SetTrigger(m_openTrigger);

    }


    /// Called by the animation.
    public void ClosingDone()
    {
        if(m_inTransition)
            m_inClosingAnimation = true;       
    }


    /// Called by the animation.
    public void TransitionDone()
    {      
        if(m_sceneLoadQueue.Count > 0)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)m_sceneLoadQueue.Dequeue(), LoadSceneMode.Single);
            StartCoroutine(LoadSceneCoroutine(operation));
        }
        else
        {
            m_inTransition = false;
        }
    }

}
