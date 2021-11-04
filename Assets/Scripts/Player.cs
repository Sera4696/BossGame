using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;                //オブジェクトのスピード
    [SerializeField] private int radius;               //円を描く半径
    private Vector3 defPosition;      //defPositionをVector3で定義する。
    float x;
    float z;
    float aroundTime;

    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 centerUp;
    [SerializeField] private Vector3 centerDown;

    // 回転軸
    [SerializeField] private Vector3 axis;

    // 円運動周期
    [SerializeField] private float _period;

    [SerializeField] private bool isMove;
    [SerializeField] private float moveCount;

   
    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;
        radius = 10;
        aroundTime = 0;

        center = new Vector3(0, 5, 0);
        centerUp = new Vector3(0, 10, 0);
        centerDown = new Vector3(0, -10, 0);
        axis = new Vector3(0, 1, 0);

        isMove = false;
        //defPosition = new Vector3(0,5,0);    //defPositionを自分のいる位置に設定する。
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        MoveUpDown();
        //x = radius * Mathf.Sin(aroundTime * speed);      //X軸の設定
        //z = radius * Mathf.Cos(aroundTime * speed);      //Z軸の設定

        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            aroundTime = 0.01f;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            aroundTime = -0.01f;
        }

        else
        {
            aroundTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMove = true;            
        }

        transform.RotateAround(center, axis, 360 / _period * aroundTime);
    }

    public void MoveUpDown()
    {
        if (center.y == 5)
        {
            if (isMove == true)
            {
                moveCount += 0.001f;
                transform.position = Vector3.Lerp(transform.position, transform.position + centerDown, moveCount);

                if (transform.position.y <= -5)
                {
                    transform.position = new Vector3(transform.position.x, -5, transform.position.z);
                    center = new Vector3(0, -5, 0);

                    isMove = false;
                    moveCount = 0;
                }
            }
        }

        if (center.y == -5)
        {
            if (isMove == true)
            {
                moveCount += 0.001f;
                transform.position = Vector3.Lerp(transform.position, transform.position + centerUp, moveCount);

                if (transform.position.y >= 5)
                {
                    transform.position = new Vector3(transform.position.x, 5, transform.position.z);
                    center = new Vector3(0, 5, 0);
                    isMove = false;
                    moveCount = 0;
                }
            }
        }
    }
}
