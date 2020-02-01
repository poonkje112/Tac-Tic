using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float m_levelSpeed;
    [SerializeField] private Vector2 m_finalPos;
    [SerializeField] private Vector2 m_startPos;
    [SerializeField] private Camera m_camera;
    private bool m_moving;
    private LevelSate m_state;

    private void Awake()
    {
        Vector3 toPos = m_camera.transform.position;
        toPos.x = m_startPos.x;
        toPos.y = m_startPos.y;
        m_camera.transform.position = toPos;

        StartCoroutine(CountDown());
    }

    private void Update()
    {
        if (m_state == LevelSate.moving)
            m_camera.transform.position = Vector3.MoveTowards(m_camera.transform.position, new Vector3(m_finalPos.x,m_finalPos.y,m_camera.transform.position.z), m_levelSpeed * Time.deltaTime);
    }

    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Go");
        m_state = LevelSate.moving;
    }



    private enum LevelSate
    {
        countdown,
        moving,
        finish,
        dead
    }
}
