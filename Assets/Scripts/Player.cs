using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //プレイヤーの回転移動
    [SerializeField] private int speed;                //オブジェクトのスピード
    [SerializeField] private int radius;               //円を描く半径
    [SerializeField] private Vector3 defPosition;      //defPositionをVector3で定義する。
    [SerializeField] float x;
    [SerializeField] float z;
    [SerializeField] float aroundTime;
    [SerializeField] private bool isMove;

    //プレイヤーのダッシュ
　　[SerializeField] private bool isDash;
    [SerializeField] private Transform dashPosition;
    [SerializeField] private Vector3 dashNowPosition;
    [SerializeField] private float dashSpeedCount;
    [SerializeField] private float dashSpeed;
    
    //その他
    [SerializeField] public GameObject targetObject;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int reverse;

    //[SerializeField] private float speed;                //オブジェクトのスピード
    //[SerializeField] private int radius;               //円を描く半径
    //private Vector3 defPosition;      //defPositionをVector3で定義する。
    //float x;
    //float z;


    //[SerializeField] private Vector3 center;
    //[SerializeField] private Vector3 centerUp;
    //[SerializeField] private Vector3 centerDown;

    //// 回転軸
    //[SerializeField] private Vector3 axis;

    //// 円運動周期
    //[SerializeField] private float _period;


    //[SerializeField] private bool isMoveUpDown;
    //[SerializeField] private float moveCount;

    //[SerializeField] private int insCount;
    //[SerializeField] private int pointCount;



    //[SerializeField] public GameObject insPoint;



    //[SerializeField] private GameObject[] points;

    


    //[SerializeField] private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {
          
        //center = new Vector3(0, 10, 0);
        //centerUp = new Vector3(0, 20, 0);
        //centerDown = new Vector3(0, -20, 0);
        //axis = new Vector3(0, 1, 0);

        //isMoveUpDown = false;

        //insCount = 3;
        //pointCount = 0;
        
        //プレイヤーの回転移動
        speed = 1;
        radius = 35;
        aroundTime = 0;
        defPosition = new Vector3(0, 15, 0);    //defPositionを自分のいる位置に設定する。

        //プレイヤーのダッシュ
        isMove = true;
        isDash = false;
        dashSpeed = 0.02f;

        //その他
        reverse = -1;

    }

    // Update is called once per frame
    void Update()
    {
        //Look();
        transform.LookAt(targetObject.transform);
        Move();
        Dash();
        Line();      
    }

    public void Move()
    {
        
        if (isMove)
        {
            if (Input.GetKey(KeyCode.A))
            {
                aroundTime += 0.01f;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                aroundTime += -0.01f;
            }

            x = radius * Mathf.Sin(aroundTime * speed) * reverse;      //X軸の設定
            z = radius * Mathf.Cos(aroundTime * speed) * reverse;      //Z軸の設定  

            transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。
        }

    }

    

    void Ins()
    {
        //if(insCount > 0)
        //{
        //    points[pointCount] = Instantiate(insPoint, transform.position, Quaternion.identity);           
        //    insCount--;
        //    pointCount++;
        //    if (pointCount > 3)
        //        pointCount = 3;
        //}
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.B) && isDash == false)// && points[0] != null)
        {
            dashNowPosition = dashPosition.position;
            isDash = true;
            isMove = false;
        }

        if (isDash == true)
        {
            dashSpeedCount += dashSpeed / 10;
            transform.position = Vector3.Lerp(transform.position, dashNowPosition, dashSpeedCount);

            if(transform.position == dashNowPosition)
            {
                reverse *= -1;
                defPosition.y *= -1;

                dashSpeed -= 0.002f;
                //transform.localScale *= 2;

                isDash = false;
                isMove = true;
                dashSpeedCount = 0;
            }
        }

        //if(isDash)
        //{
        //    

        //    if(transform.transform.position == points[pointCount - 1].transform.position && pointCount != 0)
        //    {
        //        Destroy(points[pointCount - 1]);
        //        pointCount--;
        //    }

        //    

        //    if (transform.position == points[0].transform.position)
        //    {
        //        if(transform.position.y >= 9.5f)
        //        {                    
        //            center.y = 10;
        //        }

        //        else if(transform.position.y == -10)
        //        {
        //            center.y = -10;
        //        }
        //        pointCount = 0;
        //        dashSpeedCount = 0;
        //        insCount = 3;
        //        isDash = false;
        //        Destroy(points[0]);
        //        //反対にうつる処理
        //        isMove = true;
        //    }
        //}
    }

    void Line()
    {
        if(isMove)
        {
            var positions = new Vector3[] { dashPosition.position, transform.position };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if(isDash)
        {
            var positions = new Vector3[] { dashNowPosition, transform.position };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }
        
        //    if (points[0] == null && points[1] == null && points[2] == null)
        //    {
        //        lineRenderer.enabled = false;
        //    }

        //    if (points[0] != null && points[1] == null && points[2] == null)
        //    {
        //        lineRenderer.enabled = true;
        //        var positions = new Vector3[]{
        //        points[0].transform.position,
        //        transform.position};

        //        lineRenderer.positionCount = positions.Length;

        //        lineRenderer.SetPositions(positions);
        //    }

        //    if (points[0] != null && points[1] != null && points[2] == null)
        //    {
        //        var positions = new Vector3[]{
        //        points[0].transform.position,
        //        points[1].transform.position,
        //        transform.position};

        //        lineRenderer.positionCount = positions.Length;

        //        lineRenderer.SetPositions(positions);
        //    }

        //    if (points[0] != null && points[1] != null && points[2] != null)
        //    {
        //        var positions = new Vector3[]{
        //        points[0].transform.position,
        //        points[1].transform.position,
        //        points[2].transform.position,
        //        transform.position};

        //        lineRenderer.positionCount = positions.Length;

        //        lineRenderer.SetPositions(positions);
        //    }
    }

    void Look()
    {
        var aim = targetObject.transform.position - transform.position;
        var look = Quaternion.LookRotation(aim);
        transform.localRotation = look;
    }

    
}
