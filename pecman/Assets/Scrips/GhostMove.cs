using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMove : MonoBehaviour
{
    //储存所有路径点的Transform组件
    public GameObject[] WayPointsGos;
    public float speed = 0.2f;
    private List<Vector3> wayPoints = new List<Vector3>();
    //当前在前往哪个路径点的途中
    private int index = 0;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position + new Vector3(0, 3, 0);
        //随机一条路径进行加载
        LoadAPath(WayPointsGos[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
    }

    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else
        {
            index++;
            if (index >= wayPoints.Count)
            {
                index = 0;
                //重新随机一条路径加载
                LoadAPath(WayPointsGos[Random.Range(0, WayPointsGos.Length)]);
            }

        }

        Vector2 dir = wayPoints[index] - transform.position;

        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }
    private void LoadAPath(GameObject go)
    {
        //清空集合
        wayPoints.Clear();
        wayPoints.Add(startPos);
        //遍历GamObject下的所有子物体的Transform
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Add(startPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.name == "Pacman")
        {
            //判断吃豆人是不是超级吃豆人
            if (GameManager.Instance.isSuperPacman)
            {
                //如果是
                //将鬼的位置移回起点
                transform.position = startPos - new Vector3(0, 3, 0);
                //将路径点设为最开始
                index = 0;
                GameManager.Instance.scores += 500;
            }
            else
            {
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameoverPreFab);
                Invoke("ReStart", 3f);
            }

        }
    }

    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
