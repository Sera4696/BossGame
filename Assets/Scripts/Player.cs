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
    [SerializeField] private bool isMoveUpDown;
    [SerializeField] private float moveCount;

    [SerializeField] private int insCount;
    [SerializeField] private int pointCount;

    [SerializeField] private float dashSpeedCount;

    [SerializeField] public GameObject insPoint;

    [SerializeField] private bool isDash;

    [SerializeField] private GameObject[] points;

    [SerializeField] private LineRenderer lineRenderer;

    //[SerializeField] private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.8f;       
        aroundTime = 0;

        center = new Vector3(0, 10, 0);
        centerUp = new Vector3(0, 20, 0);
        centerDown = new Vector3(0, -20, 0);
        axis = new Vector3(0, 1, 0);

        isMoveUpDown = false;

        insCount = 3;
        pointCount = 0;
        isMove = true;
        isDash = false;

        //defPosition = new Vector3(0,5,0);    //defPositionを自分のいる位置に設定する。
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        MoveUpDown();
        Dash();
        Line();
        //x = radius * Mathf.Sin(aroundTime * speed);      //X軸の設定
        //z = radius * Mathf.Cos(aroundTime * speed);      //Z軸の設定

        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。
    }

    public void Move()
    {
        if(isMove)
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
                isMoveUpDown = true;
                Ins();
            }

            transform.RotateAround(center, axis, 360 / _period * aroundTime);
        }
        //transform.RotateAround(center, axis, 360 / _period * aroundTime);
    }

    public void MoveUpDown()
    {
        if (center.y == 10)
        {
            if (isMoveUpDown == true)
            {
                moveCount += 0.001f;
                transform.position = Vector3.Lerp(transform.position, transform.position + centerDown, moveCount);

                if (transform.position.y <= -10)
                {
                    transform.position = new Vector3(transform.position.x, -10, transform.position.z);
                    center = new Vector3(0, -10, 0);

                    isMoveUpDown = false;
                    moveCount = 0;
                }
            }
        }

        if (center.y == -10)
        {
            if (isMoveUpDown == true)
            {
                moveCount += 0.001f;
                transform.position = Vector3.Lerp(transform.position, transform.position + centerUp, moveCount);

                if (transform.position.y >= 10)
                {
                    transform.position = new Vector3(transform.position.x, 10, transform.position.z);
                    center = new Vector3(0, 10, 0);
                    isMoveUpDown = false;
                    moveCount = 0;
                }
            }
        }
    }

    void Ins()
    {
        if(insCount > 0)
        {
            points[pointCount] = Instantiate(insPoint, transform.position, Quaternion.identity);           
            insCount--;
            pointCount++;
            if (pointCount > 3)
                pointCount = 3;
        }
    }

    void Dash()
    {
        if(Input.GetKeyDown(KeyCode.B) && points[0] != null)
        {
            isDash = true;
            isMove = false;
        }

        if(isDash)
        {
            dashSpeedCount += 0.003f;
            
            if(transform.transform.position == points[pointCount - 1].transform.position && pointCount != 0)
            {
                Destroy(points[pointCount - 1]);
                pointCount--;
            }

            transform.position = Vector3.Lerp(transform.position, points[pointCount - 1].transform.position, dashSpeedCount);

            if (transform.position == points[0].transform.position)
            {
                if(transform.position.y == 10)
                {
                    center.y = 10;
                }

                else if(transform.position.y == -10)
                {
                    center.y = -10;
                }
                pointCount = 0;
                dashSpeedCount = 0;
                insCount = 3;
                isDash = false;
                Destroy(points[0]);
                isMove = true;
            }
        }
    }

    void Line()
    {
        if (points[0] == null && points[1] == null && points[2] == null)
        {
            lineRenderer.enabled = false;
        }

        if (points[0] != null && points[1] == null && points[2] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]{
            points[0].transform.position,
            transform.position};

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] == null)
        {
            var positions = new Vector3[]{
            points[0].transform.position,
            points[1].transform.position,
            transform.position};

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null)
        {
            var positions = new Vector3[]{
            points[0].transform.position,
            points[1].transform.position,
            points[2].transform.position,
            transform.position};

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }
    }
}
