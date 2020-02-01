using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float m_levelSpeed;
    [SerializeField] private Vector2 m_finalPos;
    [SerializeField] private Vector2 m_startPos;
    [SerializeField] private Camera m_camera;
    [SerializeField] private Player m_player;
    [SerializeField] private float m_playerDeathHeight;
    [SerializeField] private float m_playerWinHeight;
    [SerializeField] private TextMeshProUGUI m_text;
    private bool m_moving;
    private LevelSate m_state;

    private void Awake()
    {
        Vector3 toPos = m_camera.transform.position;
        toPos.x = m_startPos.x;
        toPos.y = m_startPos.y;
        m_camera.transform.position = toPos;

        StartCoroutine(CountDown());

        onWin += WinLevel;
    }

    private void WinLevel()
    {
        m_state = LevelSate.finish;
    }

    private void Update()
    {
        if (m_state == LevelSate.moving)
            m_camera.transform.position = Vector3.MoveTowards(m_camera.transform.position, new Vector3(m_finalPos.x,m_finalPos.y,m_camera.transform.position.z), m_levelSpeed * Time.deltaTime);


        Debug.DrawLine(new Vector3(0, m_playerDeathHeight + m_camera.transform.position.y, 0) + Vector3.left * 10f, new Vector3(0, m_playerDeathHeight + m_camera.transform.position.y, 0) + Vector3.right * 10f, Color.red);
        if(m_player.transform.position.y <= m_playerDeathHeight + m_camera.transform.position.y)
        {
            m_state = LevelSate.dead;
            Debug.Log("Death");
        }

        Debug.DrawLine(new Vector3(0, m_playerWinHeight, 0) + Vector3.left * 10f, new Vector3(0, m_playerWinHeight, 0) + Vector3.right * 10f, Color.green);
        if (m_player.transform.position.y >= m_playerWinHeight)
        {
            m_state = LevelSate.finish;
            Debug.Log("finish");
        }
    }

    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            m_text.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        m_text.enabled = false;
        m_state = LevelSate.moving;
    }



    private enum LevelSate
    {
        countdown,
        moving,
        finish,
        dead
    }


    public static Action onWin;
    public static void FireOnWin()
    {
        onWin?.Invoke();
    }

}
