using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float m_levelSpeed;
    [SerializeField] private Vector2 m_finalPos;
    [SerializeField] private Vector2 m_startPos;
    [SerializeField] private CinemachineVirtualCamera m_camera;
    [SerializeField] private Player[] m_player;
    [SerializeField] private float m_playerDeathHeight;
    [SerializeField] private float m_playerWinHeight;
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Animator m_monsterAnimatior;
    private bool m_moving;
    private LevelSate m_state;

    private float frek;
    private float amp;

    private void Awake()
    {
        Vector3 toPos = m_camera.transform.position;
        toPos.x = m_startPos.x;
        toPos.y = m_startPos.y;
        m_camera.transform.position = toPos;

        frek = m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain;
        m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        amp = m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;

        StartCoroutine(CountDown());

    }
    private void LateUpdate()
    {
        if(m_state == LevelSate.moving || m_state == LevelSate.win)
            m_camera.transform.position = Vector3.MoveTowards(m_camera.transform.position, new Vector3(m_finalPos.x, m_finalPos.y, m_camera.transform.position.z), m_levelSpeed * Time.deltaTime);

        if (m_state == LevelSate.moving)
        {
            int players = 0;
            for (int i = 0; i < m_player.Length; i++)
            {

                if (m_player[i].transform.position.y <= m_playerDeathHeight + m_camera.transform.position.y)
                    if (m_state != LevelSate.lose)
                        OnLose();

                if (m_player[i].transform.position.y >= m_playerWinHeight)
                {
                    players++;
                    if(players >= 2)
                        OnWin();
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(0, m_playerWinHeight, 0) + Vector3.left * 20f, new Vector3(0, m_playerWinHeight, 0) + Vector3.right * 20f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, m_playerDeathHeight + m_camera.transform.position.y, 0) + Vector3.left * 20f, new Vector3(0, m_playerDeathHeight + m_camera.transform.position.y, 0) + Vector3.right * 20f);
    }   
#endif


    private void OnLose()
    {
        m_state = LevelSate.lose;
        m_monsterAnimatior.SetTrigger("lose");
        StartCoroutine(LoadLose());
    }

    private IEnumerator LoadLose()
    {
        yield return new WaitForSeconds(2f);
        SceneLoader.LoadSceneTransition(Scenes.MenuScene);
    }

    private IEnumerator LoadWin()
    {
        yield return new WaitForSeconds(3.5f);
        SceneLoader.LoadSceneTransition(Scenes.MenuScene);
    }

    private void OnWin()
    {
        m_monsterAnimatior.SetTrigger("win");
        m_state = LevelSate.win;
        StartCoroutine(LoadWin());
        Debug.Log("finish");
    }

    private void OnStart()
    {
        m_state = LevelSate.moving;
        m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;
        m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frek;
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
        OnStart();
    }


    private enum LevelSate
    {
        countdown,
        moving,
        win,
        lose
    }

}
