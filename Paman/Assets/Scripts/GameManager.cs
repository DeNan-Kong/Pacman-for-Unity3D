using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //单例
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject Pacman;
    public GameObject Blinky;
    public GameObject Clyde;
    public GameObject Inky;
    public GameObject Pinky;
    public GameObject StartPanel;
    public GameObject GamePanel;
    public GameObject StartCountDownPrefabs;
    public GameObject GameOverPrefabs;
    public GameObject WinPrefabs;
    public AudioClip StartClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;
    
    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    private void Awake()
    {
        _instance = this;
        //固定分辨率
        Screen.SetResolution(1024, 768, false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }
        foreach (Transform t in GameObject.Find("Maze").transform)
        {            
            pacdotGos.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Start()
    {
        SetGameState(false);
        
    }

    private void Update()
    {
        if (nowEat == pacdotNum && Pacman.GetComponent<PacmanMove>().enabled != false) 
        {
            GamePanel.SetActive(false);
            Instantiate(WinPrefabs);
            SetGameState(false);
            StopAllCoroutines();
        }
        if (nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }        
        if (GamePanel.activeInHierarchy)
        {
            remainText.text = "Temain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" +nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(StartClip, new Vector3(24,16,-8));
        StartPanel.SetActive(false);       
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(StartCountDownPrefabs);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);
        //定时调用
        Invoke("CreateSuperPacdot", 10f);
        GamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    private void CreateSuperPacdot()
    {
        if (pacdotGos.Count < 5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);        
    }

    public void OnEatSpuerPacdot()
    {
        score += 200;
        Invoke("CreateSuperPacdot", 10f);
        FreezeEnemy();
        isSuperPacman = true;
        //协程调用
        StartCoroutine(RecoveryEnemy());
    }

    IEnumerator RecoveryEnemy()
    {
        //协程方法
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    private void FreezeEnemy()
    {
        //禁用GhostMove脚本，只有update方法不执行
        Blinky.GetComponent<GhostMove>().enabled = false;
        Clyde.GetComponent<GhostMove>().enabled = false;
        Inky.GetComponent<GhostMove>().enabled = false;
        Pinky.GetComponent<GhostMove>().enabled = false;

        Blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }

    private void DisFreezeEnemy()
    {
        Blinky.GetComponent<GhostMove>().enabled = true;
        Clyde.GetComponent<GhostMove>().enabled = true;
        Inky.GetComponent<GhostMove>().enabled = true;
        Pinky.GetComponent<GhostMove>().enabled = true;

        Blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
        Inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
        Pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
    }

    private void SetGameState(bool state)
    {
        Pacman.GetComponent<PacmanMove>().enabled = state;
        Blinky.GetComponent<GhostMove>().enabled = state;
        Clyde.GetComponent<GhostMove>().enabled = state;
        Inky.GetComponent<GhostMove>().enabled = state;
        Pinky.GetComponent<GhostMove>().enabled = state;
    }
}
