using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //プレイヤーのステータス
    [SerializeField] static int attack;                //攻撃力
    [SerializeField] static int hp;　　　　　　　　　　//HP

    //プレイヤーの回転移動
    [SerializeField] private int speed;                //オブジェクトのスピード(1)
    [SerializeField] private int radius;               //円を描く半径
    [SerializeField] private Vector3 defPosition;      //中心の座標
    [SerializeField] float x;                          //xの回転補正値
    [SerializeField] float z;　　　　　　　　　　　　　//zの回転補正値
    [SerializeField] float aroundTime;　　　　　　　　 //周期
    [SerializeField] private bool isMove;              //移動できるか、しているか

    //プレイヤーのダッシュ
    [SerializeField] private bool isDash;　　　　　　　//ダッシュできるか、しているか
    [SerializeField] private Transform dashPosition;   //ダッシュ先の位置
    [SerializeField] private Vector3 dashNowPosition;　//ダッシュする瞬間のダッシュ先の位置
    [SerializeField] private float dashSpeedCount;　　 //ダッシュの加速値
    [SerializeField] private float dashSpeed;　　　　　//何ずつダッシュスピードに代入するか

    //プレーヤーのブーストダッシュ
    [SerializeField] private bool isBoostDash;         //スーパーダッシュしてるか
    [SerializeField] private float boostdashSpeed;　　 //スーパーダッシュの加速値

    //ポイント系
    [SerializeField] private GameObject[] points;　　　//ダッシュポイントの配列
    [SerializeField] private int pointCount;　　　　　 //ダッシュポイントの数
    [SerializeField] public GameObject insPoint;　　　 //ダッシュポイントのプレハブ

    //その他
    [SerializeField] public GameObject targetObject;　 //ダッシュ先のポイント
    [SerializeField] private LineRenderer lineRenderer;//ライン表示
    [SerializeField] private int reverse;              //値の反転
    //[SerializeField] public GameObject boss;
    //[SerializeField] public Boss bossScr;

    //カメラ演出
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera;

    [SerializeField] public GameObject HP_Object;
    [SerializeField] public GameObject A_Object;

    //[SerializeField] private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {
        hp = 50;
        attack = 30;

                          
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
        //bossScr = boss.GetComponent<Boss>();

        pointCount = 0;

        //カメラ
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("Sub Camera");
        subCamera.SetActive(false);
        mainCamera.SetActive(true);
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
        Texts();
    }

    public void Move()
    {
        //移動回転の処理
        if (isMove)
        {
            float hori = Input.GetAxis("Horizontal");

            //周期の値の増減
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

            //補正の式
            x = radius * Mathf.Sin(aroundTime * speed) * reverse;      //X軸の設定
            z = radius * Mathf.Cos(aroundTime * speed) * reverse;      //Z軸の設定  

            transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。
        }
    }


    //ダッシュポイントの生成
    void Ins()
    {
        if (pointCount < 8)　//ポイントの上限の数
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

    //ダッシュの処理
    void Dash()
    {
        //ダッシュボタンが押されダッシュ中でないかつスーパーダッシュ中なら
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown("joystick button 1") && !isDash && !isBoostDash)// && points[0] != null)
        {
            Ins();
            dashNowPosition = dashPosition.position;   //ボタンを押した瞬間にダッシュ先の位置を代入する
            isDash = true;　　　　　　　　　　　　　　 
            isMove = false;
        }

        if (isDash == true)
        {
            //ダッシュスピードカウントを決まった値分増加させる
            dashSpeedCount += dashSpeed / 10;
            transform.position = Vector3.Lerp(transform.position, dashNowPosition, dashSpeedCount);

            //位置の補正
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
    }

    //スーパーダッシュの処理
    void BoostDash()
    {
        //スーパーダッシュボタンが押されダッシュ中でないかつスーパーダッシュ中でないかつポイントがnullでないなら
        if(Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 0")  && !isBoostDash && !isDash && points[0] != null)
        {
            isBoostDash = true;
            isMove = false;
        }

        if(isBoostDash==true)
        {
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
            boostdashSpeed += 0.01f;　　　//どのぐらいの速度で加速するのか
            if (boostdashSpeed >=0.1f)
            {
                boostdashSpeed = 0.1f;
            }
            transform.position = Vector3.Lerp(transform.position, points[pointCount - 1].transform.position, boostdashSpeed);



            //目標ポイントまで移動出来たかつ配列が終点でないなら
            if (transform.transform.position == points[pointCount - 1].transform.position && pointCount != 0)
            {
                Destroy(points[pointCount - 1]);
                pointCount--;
            }

            //目標ポイントが終点まで行きついたなら
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

        if (isBoostDash == false)
        {
            mainCamera.SetActive(true);
            subCamera.SetActive(false);
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

    void Texts()
    {
        Text HP_Text = HP_Object.GetComponent<Text>();
        Text A_Text = A_Object.GetComponent<Text>();

        HP_Text.text = "Player_HP : " + hp;
        A_Text.text = "Player_Attack : " + attack;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shot")
        {
            hp -= 10; 
        }

        if (other.gameObject.tag == "Boss")
        {
            Boss.hp -= attack - Boss.defence;
        }

        if (other.gameObject.tag == "Defence")
        {
            Boss.defence -= 5;
            Destroy(other.gameObject);
        }
    }
}
