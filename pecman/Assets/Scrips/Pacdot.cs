using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour
{
    public bool isSuperPacdot = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Pacman")
        {
            //判断吃的豆子是不是超级豆子
            if (isSuperPacdot == true)
            {
                //吃的豆子为超级豆子时
                //吃掉豆子，移除在GameManager中pacdots中的豆子游戏物体 参数为当前被吃掉的豆子
                GameManager.Instance.OnEatPacdot(gameObject);
                //调用吃掉超级豆子后的方法
                GameManager.Instance.OnEatSuperPacdot();
                Destroy(gameObject);
            }
            else
            {
                //吃的豆子为普通豆子时
                //吃掉豆子，移除在GameManager中pacdots中的豆子游戏物体 参数为当前被吃掉的豆子
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
            
        }
    }
}
