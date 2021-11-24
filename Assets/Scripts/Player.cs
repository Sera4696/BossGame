using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //プレイヤーのステータス
    [SerializeField] public static int attack;                //攻撃力
    [SerializeField] public static int hp;　　　　　　　　　　//HP

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
    [SerializeField] private TrailRenderer trailRenderer;//ダッシュの軌跡
    private float trailTime=0;
    [SerializeField] private int reverse;              //値の反転
    [SerializeField] private float startTimer;
    //[SerializeField] public GameObject boss;
    //[SerializeField] public Boss bossScr;

    //カメラ演出
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera;
    

    [SerializeField] public GameObject HP_Object;
    [SerializeField] public GameObject A_Object;

    //音関連
    [SerializeField] private AudioClip tackle;
    [SerializeField] private AudioClip BossAttack;
    private AudioSource audioSource;

    //子供関係
    [SerializeField] private GameObject ChildrenOrca;

    //ダメージ関係
    private float bgmTimer = 0;
    private bool isBGM = false;
    private TimeManager timeManager;
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject blackMsk;

    [SerializeField] private GameObject Orca;
    [SerializeField] private GameObject[] Dtarget;

    //体力関係
    //[SerializeField] private Slider BossSlider;
    [SerializeField] private Slider PlayerSlider;
    private int playerMaxHP;
    

    int DCount;
    bool isMMM;

    //シーン関係
    private bool isDead = false,isChange=false;
    

    //[SerializeField] private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        attack = 35;
      
        PlayerSlider.value = 1;
        playerMaxHP = hp;

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
        trailRenderer.enabled = false;//ダッシュの軌跡
        startTimer = 0;
        //bossScr = boss.GetComponent<Boss>();

        pointCount = 0;

        DCount = 0;
        isMMM = false;

        //カメラ
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("Sub Camera");
        subCamera.SetActive(false);
        mainCamera.SetActive(true);

        //音関連
        audioSource = GetComponent<AudioSource>();

        //ダメージ関連
        timeManager = mainCamera.GetComponent<TimeManager>();
        blackMsk.GetComponent<SpriteRenderer>().color = new Color32(245, 59 , 59, 0);
    }

    // Update is called once per frame

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1")) && !isDash && !isBoostDash)// && points[0] != null)
        {
            Ins();
            dashNowPosition = dashPosition.position;   //ボタンを押した瞬間にダッシュ先の位置を代入する
            isDash = true;
            isMove = false;
            isBoostDash = false;
            audioSource.PlayOneShot(tackle);
            audioSource.pitch += 0.5f;

            var sequence = DOTween.Sequence();
            sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(2.5f, 2.5f, 1f), 0.3f)).SetEase(Ease.OutQuart);
            sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(1.5f, 1.5f, 2f), 0.3f)).SetEase(Ease.OutQuart);
            sequence.Play();

        }

        if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 0")) && !isBoostDash && !isDash && points[0] != null)
        {
            isBoostDash = true;
            isMove = false;
            isDash = false;
            trailRenderer.enabled = true;
            audioSource.pitch = 1;
        }
    }


    void FixedUpdate()
    {
        startTimer++;
        //Ins();
        Look();
        if (startTimer > 300)
        {
            Move();
            Dash();
            BoostDash();
            Line();
        }

        if (Player.hp <= 0|| Boss.hp <= 0&&isDead==false)
        {
            isDead = true;
        }
        if (isDead == true&&isChange==false)
        {
            GameObject.Find("AwaParents").GetComponent<TestScript>().Fade();
            isChange = true;
            isDead = false;
        }

        BGMChange();
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
                aroundTime += 0.02f;
                Orca.transform.LookAt(Dtarget[1].transform.position);
            }
            if (hori > 0)
            {
                aroundTime += -0.02f;
                Orca.transform.LookAt(Dtarget[2].transform.position);
            }

            if (Input.GetKey(KeyCode.A))
            {
                aroundTime += 0.02f;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                aroundTime += -0.02f;
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
        

        if (isDash == true)
        {
            isBoostDash = false;
            isMove = false;

            

            //ダッシュスピードカウントを決まった値分増加させる
            dashSpeedCount += dashSpeed / 10;
            transform.position = Vector3.Lerp(transform.position, dashNowPosition, dashSpeedCount);

            float distance = Vector3.Distance(transform.position, dashNowPosition);

            if (distance < 0.5f)
            {
                transform.position = dashNowPosition;
            }

            //位置の補正
            if (transform.position == dashNowPosition)
            {
                reverse *= -1;
                defPosition.y *= -1;

                dashSpeed -= 0.002f;
                if(dashSpeed < 0.001f)
                {
                    dashSpeed = 0.001f;
                }
                //transform.localScale *= 2;

                var sequence = DOTween.Sequence();
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Play();

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

        if((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 0"))  && !isBoostDash && !isDash && points[0] != null)
        {
            isBoostDash = true;
            isMove = false;
            isDash = false;
            trailRenderer.enabled = true;
            audioSource.pitch = 1;
            audioSource.PlayOneShot(tackle);
        }

        

        if(isBoostDash == true)
        {
            isDash = false;
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
            
            boostdashSpeed += 0.001f;　　　//どのぐらいの速度で加速するのか
            transform.position = Vector3.Lerp(transform.position, points[pointCount - 1].transform.position, boostdashSpeed);
            //if (boostdashSpeed >= 0.1f)
            //{
            //    boostdashSpeed = 0.1f;
            //}

            float distance = Vector3.Distance(transform.position, points[pointCount - 1].transform.position);

            if (distance < 1f)
            {
                transform.position = points[pointCount - 1].transform.position;
            }

            //目標ポイントまで移動出来たかつ配列が終点でないなら
            if (transform.transform.position == points[pointCount - 1].transform.position && pointCount != 0)
            {
                audioSource.PlayOneShot(tackle);
                var sequence = DOTween.Sequence();
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Play();

                Destroy(points[pointCount - 1]);
                pointCount--;
                attack += 10;

            }

            //目標ポイントが終点まで行きついたなら
            if (transform.position == points[0].transform.position && points[pointCount] == points[0])
            {
                var sequence = DOTween.Sequence();
                //sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Append(ChildrenOrca.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f)).SetEase(Ease.OutQuart);
                sequence.Play();
                
                pointCount = 0;
                dashSpeed = 0.02f;
                boostdashSpeed = 0;
                attack = 30;
                isBoostDash = false;
                Destroy(points[0]);
                isMove = true;
            }
        }

        if (isBoostDash == false)
        {
            mainCamera.SetActive(true);
            subCamera.SetActive(false);
            
            trailTime += Time.deltaTime;
            if (trailTime >= 2f)
            {
                //後にα値を下げるようにしてください
                trailRenderer.enabled = false;
                trailTime = 0;
            }
        }
    }


    void Look()
    {
        transform.LookAt(targetObject.transform);
        Orca.transform.rotation = transform.rotation;
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

    public void BGMChange()
    {
        GameObject BGMObj = GameObject.Find("BGMObj");
        if (isBGM == true)
        {
            BGMObj.GetComponent<AudioLowPassFilter>().enabled = true;
            bgmTimer += Time.deltaTime;
            if (bgmTimer >= 1.5f)
            {
                isBGM = false;
                bgmTimer = 0;
            }

        }
        if (isBGM == false)
        {
            BGMObj.GetComponent<AudioLowPassFilter>().enabled = false;
        }
    }

    void DamageEfect()
    {
                        
        var sequence = DOTween.Sequence();
        sequence.Append(blackMsk.GetComponent<SpriteRenderer>().DOFade(1, 0.05f).OnComplete(() =>
        {

            blackMsk.GetComponent<SpriteRenderer>().DOFade(0, 0.3f);
        }));

        sequence.Play();
        ChildrenOrca.transform.DOShakeScale(2f, 0.5f);
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        
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
            if (!isBoostDash && !isBGM)
            {
                hp -= 10;
                PlayerSlider.value = (float)hp / (float)playerMaxHP;
                DamageEfect();
                //mainCamera.GetComponent<Gamemanager>().CameraShakes();
                isBGM = true;
                timeManager.SlowDown();
                //Destroy(other.gameObject);
            }            
        }

        if (other.gameObject.tag == "Boss")
        {
            Boss.hp -= attack / Boss.defence;
            
            audioSource.PlayOneShot(BossAttack);
        }

        if (other.gameObject.tag == "Defence")
        {
            Boss.defence -= 1;
            Destroy(other.gameObject);
        }
    }
}
