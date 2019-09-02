using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDownPrefab;
    public GameObject gameoverPreFab;
    public GameObject winPreFab;
    public AudioClip startClip;

    public Text remainText;
    public Text nowText;
    public Text score;

    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };

    
    private List<GameObject> pacdots = new List<GameObject>();
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int scores = 0;

    private void Awake()
    {
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            //移除序号为多少的值
            rawIndex.RemoveAt(tempIndex);
        }
        //拿到所有豆子
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdots.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;

    }
    private void Start()
    {
        SetGameState(false);
        
    }
    private void Update()
    {
        if(nowEat == pacdotNum && pacman.GetComponent<Pacman>().enabled != false)
        {
            gamePanel.SetActive(false);
            Instantiate(winPreFab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if(nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" +  nowEat;
            score.text = "Score:\n\n" + scores;
        }
    }

    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(23, 16, -5));
        startPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(3.8f);
        Destroy(go);
        //创建超级豆子
        Invoke("CreatSuperPacdot", 10f);
        SetGameState(true);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// 吃掉豆子时，将豆子从pacdots中移除
    /// </summary>
    /// <param name="go"> 当前被吃掉的豆子</param>
    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        scores += 100;
        pacdots.Remove(go);
    }
    /// <summary>
    /// 吃掉超级豆子
    /// </summary>
    public void OnEatSuperPacdot()
    {
        scores += 200;
        //过10s重新生成超级豆子
        Invoke("CreatSuperPacdot", 10f);
        //将吃豆人变成超级吃豆人
        isSuperPacman = true;
        //冻结敌人
        FreezeEnemy();
        //开启协程，恢复敌人
        StartCoroutine(RecoveryEnemy());
    }

    /// <summary>
    /// 等待3秒解冻敌人的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoveryEnemy()
    {
        yield return new WaitForSeconds(3f);
        //解冻敌人
        DisFreezeEnemy();
        //并将超级吃豆人变成普通吃豆人
        isSuperPacman = false;
    }

    /// <summary>
    /// 冻结敌人的方法
    /// </summary>
    private void FreezeEnemy()
    {
        blinky.GetComponent<GhostMove>().enabled = false;
        clyde.GetComponent<GhostMove>().enabled = false;
        inky.GetComponent<GhostMove>().enabled = false;
        pinky.GetComponent<GhostMove>().enabled = false;

        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f,0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }
    /// <summary>
    /// 解冻敌人的方法
    /// </summary>
    private void DisFreezeEnemy()
    {
        blinky.GetComponent<GhostMove>().enabled = true;
        clyde.GetComponent<GhostMove>().enabled = true;
        inky.GetComponent<GhostMove>().enabled = true;
        pinky.GetComponent<GhostMove>().enabled = true;

        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }


    /// <summary>
    /// 创造超级豆子
    /// </summary>
    private void CreatSuperPacdot()
    {
        //如果豆子数小于5则不再生成超级豆子，会出bug
        if(pacdots.Count < 5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdots.Count);
        pacdots[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdots[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    private void SetGameState(bool State)
    {
        pacman.GetComponent<Pacman>().enabled = State;
        blinky.GetComponent<GhostMove>().enabled = State;
        clyde.GetComponent<GhostMove>().enabled = State;
        inky.GetComponent<GhostMove>().enabled = State;
        pinky.GetComponent<GhostMove>().enabled = State;
    }
}
