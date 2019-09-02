using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public float speed = 0.35f;
    private Vector2 dest = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        //保证吃豆人留在原地
        dest = transform.position;
    }

    // 跟物理有关的移动 都写在FixedUpdate
    void FixedUpdate()
    {
        //插值运算得到移动到dest位置的移动坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体来设置物体位置
        GetComponent<Rigidbody2D>().MovePosition(temp);
        //必须达到目标位置才能继续下一步移动
        if ((Vector2)transform.position == dest)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            //获取移动方向
            Vector2 dir = dest - (Vector2)transform.position;
            //把获取到的移动方向设置给动画状态机
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
    }
    private bool Valid(Vector2 dir)
    {
        //自身的位置
        Vector2 pos = transform.position;
        //射线从 自身加向指定方向移动一单位 为起始点 自己身位置为终点发出一条射线，进行射线检测
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        //判断hit是否碰到的是自己 ，如果hit碰到的是墙体， 则说明射线是从墙里发出的 ，所以肯定会碰到墙体
        return (hit.collider == GetComponent<Collider2D>());

    }
}
