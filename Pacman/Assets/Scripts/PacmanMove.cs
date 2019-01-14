using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    public float speed = 0.35f;
    //吃豆人下一次移动将要去的目的地
    private Vector2 dest = Vector2.zero;

    private void Start()
    {
        //保证吃豆人在游戏开始的时候不动
        dest = this.transform.position;
    }
    //按键检测
    private void FixedUpdate()
    {
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体来设置物体的位置
        GetComponent<Rigidbody2D>().MovePosition(temp);
        //必须到达下一个dest位置才能发新的目的地位置指令
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
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
            //获取移动方向
            Vector2 dir = dest - (Vector2)transform.position;
            //把取到的移动方向设置给动画状态机
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
    }

    //下一个位置是否有效
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        //从目标位置到当前位置的射线碰撞点
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
       // return true;
    }
}
