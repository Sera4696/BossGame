using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleDash : MonoBehaviour
{
   

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

    [SerializeField] private GameObject titleCameraObj;
    private TitleCamera titleCamera;

    //音関係
    [SerializeField] private AudioClip kettei;
    private AudioSource audioSource;

    //[SerializeField] private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {

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

        pointCount = 5;

        points[0] = Instantiate(insPoint, new Vector3(0,15,35), Quaternion.identity);
        points[1] = Instantiate(insPoint, new Vector3(0, -15, -35), Quaternion.identity);
        points[2] = Instantiate(insPoint, new Vector3(-35, -15, 0), Quaternion.identity);
        points[3] = Instantiate(insPoint, new Vector3(35, 15, 0), Quaternion.identity);
        points[4] = Instantiate(insPoint, new Vector3(0, -15, 35), Quaternion.identity);



        //カメラ
        titleCamera = titleCameraObj.GetComponent<TitleCamera>();

        audioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {

        BoostDash();
    }
    // Update is called once per frame
    void Update()
    {
        //Ins();
        Look();
        Move();
        //Dash();
        Line();
         //スーパーダッシュボタンが押されダッシュ中でないかつスーパーダッシュ中でないかつポイントがnullでないなら
        if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 0")) && !isBoostDash && !isDash && points[0] != null)
        {
            audioSource.PlayOneShot(kettei);
            isBoostDash = true;
            isMove = false;
        }
    }

    #region　移動関係
    public void Move()
    {
        //移動回転の処理
        if (isMove)
        {
            //float hori = Input.GetAxis("Horizontal");

            ////周期の値の増減
            //if (hori < 0)
            //{
            //    aroundTime += 0.01f;
            //}
            //if (hori > 0)
            //{
            //    aroundTime += -0.01f;
            //}

            //if (Input.GetKey(KeyCode.A))
            //{
            //    aroundTime += 0.01f;
            //}

            //else if (Input.GetKey(KeyCode.D))
            //{
            //    aroundTime += -0.01f;
            //}

            //補正の式
            x = radius * Mathf.Sin(aroundTime * speed) * reverse;      //X軸の設定
            z = radius * Mathf.Cos(aroundTime * speed) * reverse;      //Z軸の設定  

            transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。
        }
    }
    #endregion


    //スーパーダッシュの処理
    void BoostDash()
    {
       

        if (isBoostDash == true)
        {
           
            boostdashSpeed += 0.003f;　　　//どのぐらいの速度で加速するのか

            if (boostdashSpeed >= 0.2f)
            {
                boostdashSpeed = 0.2f;
            }

            transform.position = Vector3.Lerp(transform.position, points[pointCount - 1].transform.position, boostdashSpeed);
            //transform.DOMove(new Vector3(points[pointCount - 1].transform.position.x, points[pointCount - 1].transform.position.y, points[pointCount - 1].transform.position.z), 0.2f).SetEase(Ease.OutQuad);

            float distance = Vector3.Distance(transform.position, points[pointCount - 1].transform.position);

            if(distance < 1f)
            {
                transform.position = points[pointCount - 1].transform.position;
            }

            //目標ポイントまで移動出来たかつ配列が終点でないなら
            if (transform.position == points[pointCount - 1].transform.position && pointCount != 0)
            {
                Destroy(points[pointCount - 1]);
                pointCount--;
            }

            //目標ポイントが終点まで行きついたなら
            if (transform.position == points[0].transform.position)
            {
                pointCount = 0;
                dashSpeed = 0.001f;
                boostdashSpeed = 0;
                isBoostDash = false;
                Destroy(points[0]);
                titleCamera.isTitle = true;
                //isMove = true;
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
    #region 線の描画関係
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
    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shot")
        {
         
        }

        if (other.gameObject.tag == "Boss")
        {
           
        }

        
    }
}
