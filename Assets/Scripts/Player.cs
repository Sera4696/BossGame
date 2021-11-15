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

    //プレーヤーのブーストダッシュ
    [SerializeField] private bool isBoostDash;
    [SerializeField] private float boostdashSpeed;

    //ポイント系
    [SerializeField] private GameObject[] points;
    [SerializeField] private int pointCount;
    [SerializeField] public GameObject insPoint;



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
        
        
        //プレイヤーの回転移動
        speed = 1;
        radius = 35;
        aroundTime = 0;
        defPosition = new Vector3(0, 15, 0);    //defPositionを自分のいる位置に設定する。

        //プレイヤーのダッシュ
        isMove = true;
        isDash = false;
        dashSpeed = 0.02f;

        isBoostDash = false;

        //その他
        reverse = -1;


        pointCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Ins();
        Look();
        Move();
        Dash();
        BoostDash();
        Line();      
    }

    public void Move()
    {
        
        if (isMove)
        {
            float hori = Input.GetAxis("Horizontal");
            if (hori < 0)
            {
                aroundTime += 0.01f;
            }
            if (hori > 0)
            {
                aroundTime += -0.01f;
            }

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
        if (pointCount < 8)
        {
            points[pointCount] = Instantiate(insPoint, transform.position, Quaternion.identity);
            pointCount++;
        }
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
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown("joystick button 1") && !isDash && !isBoostDash)// && points[0] != null)
        {
            Ins();
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
                if(dashSpeed < 0.001f)
                {
                    dashSpeed = 0.001f;
                }
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

        //    
        //}
    }

    void BoostDash()
    {
        if(Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 0")  && !isBoostDash && !isDash && points[0] != null)
        {
            isBoostDash = true;
            isMove = false;
        }

        if(isBoostDash)
        {
            boostdashSpeed += 0.01f;
            transform.position = Vector3.Lerp(transform.position, points[pointCount - 1].transform.position, boostdashSpeed);

            if (transform.transform.position == points[pointCount - 1].transform.position && pointCount != 0)
            {
                Destroy(points[pointCount - 1]);
                pointCount--;
            }

            if (transform.position == points[0].transform.position)
            {
                pointCount = 0;
                dashSpeed = 0.02f;
                boostdashSpeed = 0;
                isBoostDash = false;
                Destroy(points[0]);
                isMove = true;
            }
        }       
    }


    void Look()
    {
        transform.LookAt(targetObject.transform);
        //var aim = targetObject.transform.position - transform.position;
        //var look = Quaternion.LookRotation(aim);
        //transform.localRotation = look;
    }

    void Line()
    {
        //if(isMove)
        //{
        //    var positions = new Vector3[] { dashPosition.position, transform.position };

        //    lineRenderer.positionCount = positions.Length;

        //    lineRenderer.SetPositions(positions);
        //}

        //if(isDash)
        //{
        //    var positions = new Vector3[] { dashNowPosition, transform.position };

        //    lineRenderer.positionCount = positions.Length;

        //    lineRenderer.SetPositions(positions);
        //}

        if (points[0] == null && points[1] == null && points[2] == null &&
            points[3] == null && points[4] == null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = false;
        }

        if (points[0] != null && points[1] == null && points[2] == null &&
            points[3] == null && points[4] == null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] == null &&
            points[3] == null && points[4] == null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] == null && points[4] == null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] == null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] != null && points[5] == null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                points[4].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] != null && points[5] != null &&
            points[6] == null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                points[4].transform.position,
                points[5].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] != null && points[5] != null &&
            points[6] != null && points[7] == null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                points[4].transform.position,
                points[5].transform.position,
                points[6].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] != null && points[5] != null &&
            points[6] != null && points[7] != null && points[8] == null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                points[4].transform.position,
                points[5].transform.position,
                points[6].transform.position,
                points[7].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }

        if (points[0] != null && points[1] != null && points[2] != null &&
            points[3] != null && points[4] != null && points[5] != null &&
            points[6] != null && points[7] != null && points[8] != null)
        {
            lineRenderer.enabled = true;
            var positions = new Vector3[]
            {
                points[0].transform.position,
                points[1].transform.position,
                points[2].transform.position,
                points[3].transform.position,
                points[4].transform.position,
                points[5].transform.position,
                points[6].transform.position,
                points[7].transform.position,
                points[8].transform.position,
                transform.position
            };

            lineRenderer.positionCount = positions.Length;

            lineRenderer.SetPositions(positions);
        }


    }


}
