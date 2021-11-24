using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] public static int defence;                  //防御力
    [SerializeField] public static int hp;　　　　　　　　　　　 //HP

    [SerializeField] private int bossAttack;                     //
    [SerializeField] private float attackCount;                  //

    [SerializeField] private GameObject[] shotObjects;           //
    [SerializeField] private GameObject insShot;                 //
    [SerializeField] private int shotAttackCount;                //
    [SerializeField] private int shotCount;
    [SerializeField] private int shotTimer;                      //


    [SerializeField] private GameObject insAroundShot;           // 
    private float k;
    public Quaternion rotation = Quaternion.identity;

    [SerializeField] private GameObject[] attackPoints;          //
    [SerializeField] private GameObject[] nowAttack;
    [SerializeField] private GameObject[] nowAttackY;//
    [SerializeField] private int downAttack;                     //
    [SerializeField] private int downAttackCount;                //
    [SerializeField] private GameObject insDown;                 //
    [SerializeField] private GameObject insDownY;

    [SerializeField] private GameObject[] defenceObjects;
    [SerializeField] private GameObject[] nowDefence;
    [SerializeField] private int defenceCount;
    [SerializeField] private GameObject insDefence;

    [SerializeField] private GameObject[] nowRoop;
    [SerializeField] private Vector3 defPosition;
    [SerializeField] private int radius;
    [SerializeField] float aroundTime;
    [SerializeField] float aroundTime2;
    [SerializeField] private float RoopTimer;
    [SerializeField] private float startTimer;

    [SerializeField] public GameObject targetObject;

    [SerializeField] public GameObject HP_Object;
    [SerializeField] public GameObject D_Object;

    [SerializeField] private int reverse_x;
    [SerializeField] private int reverse_y;
    [SerializeField] private int reverse_z;
    public float x;
    public float y;
    public float z;

    [SerializeField] public float minSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float area;

    //体力
    [SerializeField] private Slider BossSlider;
    private int bossMaxHP;


    // Start is called before the first frame update
    void Start()
    {
        hp = 5000;

        BossSlider.value = 1;
        bossMaxHP = hp;

        bossAttack = 0;
        attackCount = 0;
        downAttackCount = 0;

        radius = 35;

        startTimer = 0;

        reverse_x = 1;
        reverse_y = 1;
        reverse_z = 1;

        minSpeed = -0.03f;
        maxSpeed = 0.03f;

        x = Random.Range(minSpeed, maxSpeed);
        y = Random.Range(minSpeed, maxSpeed);
        z = Random.Range(minSpeed, maxSpeed);
        area = 2;

        defence = 1;
    }

    // Update is called once per frame
    void Update()
    {
        startTimer++;
        BossAttack();
        Look();
        Texts();
        BossSlider.value = (float)hp / (float)bossMaxHP;
        if (Input.GetKeyDown(KeyCode.L))
        {
            hp -= 100;
        }
    }

    private void Move()
    {
        transform.position += new Vector3(x * reverse_x, y * reverse_y, z * reverse_z);

        if (transform.position.x < -area || transform.position.x > area)
        {
            reverse_x *= -1;
            x = Random.Range(minSpeed, maxSpeed);
        }

        if (transform.position.y < -area || transform.position.y > area)
        {
            reverse_y *= -1;
            y = Random.Range(minSpeed, maxSpeed);
        }

        if (transform.position.z < -area || transform.position.z > area)
        {
            reverse_z *= -1;
            z = Random.Range(minSpeed, maxSpeed);
        }
    }

    void Look()
    {
        transform.LookAt(targetObject.transform);
    }

    void BossAttack()
    {
        
        if (startTimer >  300)
        {
            startTimer = 350;
            Move();
            attackCount++;
            //攻撃が始まるまでのカウント

            if (bossAttack == 0 && attackCount > 500)
            {
                attackCount = 0;
            }
           

            if (hp >= 4000)
            {
                if (attackCount > 450 && attackCount < 500)
                {
                    transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
                }

                if (attackCount == 500.0f)
                {
                    bossAttack = 1;
                    //度の攻撃かをランダムで選ぶ
                    transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
                    //bossAttack = Random.Range(1, 5);
                }
            }

            if (hp < 4000 && hp >= 3000)
            {
                area = 4;
                minSpeed = -0.06f;
                maxSpeed = 0.06f;
                if (attackCount > 400 && attackCount < 450)
                {
                    transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
                }

                if (attackCount == 450.0f)
                {
                    //bossAttack = 5;
                    //度の攻撃かをランダムで選ぶ
                    transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
                    bossAttack = Random.Range(1, 3);
                }
            }

            if (hp < 3000 && hp >= 2000)
            {
                area = 6;
                minSpeed = -0.09f;
                maxSpeed = 0.09f;
                if (attackCount > 350 && attackCount < 400)
                {
                    transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
                }

                if (attackCount == 400.0f)
                {
                    //bossAttack = 5;
                    //度の攻撃かをランダムで選ぶ
                    transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
                    bossAttack = Random.Range(1, 4);
                }
            }

            if (hp < 2000 && hp >= 1000)
            {
                area = 8;
                minSpeed = -0.12f;
                maxSpeed = 0.12f;
                if (attackCount > 250 && attackCount < 300)
                {
                    transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
                }

                if (attackCount == 300.0f)
                {
                    //bossAttack = 5;
                    //度の攻撃かをランダムで選ぶ
                    transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
                    bossAttack = Random.Range(1, 5);
                }
            }

            if (hp < 1000)
            {
                area = 10;
                minSpeed = -0.15f;
                maxSpeed = 0.15f;
                if (attackCount > 200 && attackCount < 250)
                {
                    transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
                }

                if (attackCount == 250.0f)
                {
                    //bossAttack = 5;
                    //度の攻撃かをランダムで選ぶ
                    transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
                    bossAttack = Random.Range(1, 6);
                }
            }

            if (bossAttack == 1)
            {
                Shot();
            }

            if (bossAttack == 2)
            {
                DefenceIns();
            }

            if (bossAttack == 3)
            {               
                DownAttack();
            }

            if (bossAttack == 4)
            {
                AroundShot();
            }

            if (bossAttack == 5)
            {                
                
                Roop();
            }
        }      
    }

    void Shot()
    {
        //if (shotAttackCount < 1)
        //{
        //    shotObjects[0] = Instantiate(insShot, transform.position, Quaternion.identity);
        //    shotAttackCount++;
        //}

        shotCount++;

        if (shotAttackCount < 5)
        {
            if (shotCount == 1)
            {
                shotObjects[0] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 31)
            {
                shotObjects[1] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 61)
            {
                shotObjects[2] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 91)
            {
                shotObjects[3] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 121)
            {
                shotObjects[4] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }
        }


        if (shotObjects[0] != null)
        {
            shotObjects[0].transform.position += transform.forward * 0.5f;
            shotTimer++;
        }

        if (shotObjects[1] != null)
        {
            shotObjects[1].transform.position += transform.forward * 0.5f;
        }

        if (shotObjects[2] != null)
        {
            shotObjects[2].transform.position += transform.forward * 0.5f;
        }

        if (shotObjects[3] != null)
        {
            shotObjects[3].transform.position += transform.forward * 0.5f;
        }

        if (shotObjects[4] != null)
        {
            shotObjects[4].transform.position += transform.forward * 0.5f;
        }

        if (shotTimer > 250)
        {
            Destroy(shotObjects[0]);
            Destroy(shotObjects[1]);
            Destroy(shotObjects[2]);
            Destroy(shotObjects[3]);
            Destroy(shotObjects[4]);
        }

        if (shotObjects[0] == null)
        {
            shotAttackCount = 0;
            shotTimer = 0;
            bossAttack = 0;
            attackCount = 0;
            shotCount = 0;
        }
    }

    void AroundShot()
    {
        if (shotAttackCount < 5)
        {
            for (int i = 0; i < 5; i++)
            {
                rotation.eulerAngles = new Vector3(0, k, 0);
                shotObjects[shotAttackCount] = Instantiate(insAroundShot, transform.position, rotation);
                k += 70;
                shotAttackCount++;
            }           
            
        }

        shotTimer++;

        if (shotObjects[0] != null)
        {
            shotObjects[0].transform.position += shotObjects[0].transform.forward * 0.4f;           
        }

        if (shotObjects[1] != null)
        {
            shotObjects[1].transform.position += shotObjects[1].transform.forward * 0.4f;
        }

        if (shotObjects[2] != null)
        {
            shotObjects[2].transform.position += shotObjects[2].transform.forward * 0.4f;
        }

        if (shotObjects[3] != null)
        {
            shotObjects[3].transform.position += shotObjects[3].transform.forward * 0.4f;
        }

        if (shotObjects[4] != null)
        {
            shotObjects[4].transform.position += shotObjects[4].transform.forward * 0.4f;
        }

        if (shotTimer > 400)
        {
            Destroy(shotObjects[0]);
            Destroy(shotObjects[1]);
            Destroy(shotObjects[2]);
            Destroy(shotObjects[3]);
            Destroy(shotObjects[4]);
        }

        if (shotObjects[0] == null &&
            shotObjects[1] == null &&
            shotObjects[2] == null &&
            shotObjects[3] == null &&
            shotObjects[4] == null &&
            shotTimer > 300)
        {
            shotAttackCount = 0;
            shotTimer = 0;
            bossAttack = 0;
            attackCount = 0;
        }
    }

    void DownAttack()
    {
        //落石を３つ生成
        if(downAttackCount < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                //９つのポイントのどこかに生成する
                downAttack = Random.Range(1, 8);               
                nowAttack[downAttackCount] = Instantiate(insDown, attackPoints[downAttack].transform.position, Quaternion.identity);
                nowAttackY[downAttackCount] = Instantiate(insDownY, attackPoints[downAttack].transform.position, Quaternion.identity);
                downAttackCount++;               
            }
        }
       
        //生成が終わったら落とす
        if(nowAttack[0] != null && nowAttack[1] != null && nowAttack[2] != null)
        {
            nowAttackY[0].transform.localScale += new Vector3(0, 5.0f, 0);
            nowAttackY[1].transform.localScale += new Vector3(0, 5.0f, 0);
            nowAttackY[2].transform.localScale += new Vector3(0, 5.0f, 0);

            if(nowAttackY[0].transform.localScale.y > 150)
            {
                nowAttack[0].transform.position += new Vector3(0, -1.2f, 0);
                nowAttack[1].transform.position += new Vector3(0, -1.2f, 0);
                nowAttack[2].transform.position += new Vector3(0, -1.2f, 0);
            }           
        }

        //一定の地点まで落ちたら落石を消去、そして値をリセット
        if (nowAttack[0].transform.position.y < -100 && 
            nowAttack[1].transform.position.y < -100 && 
            nowAttack[2].transform.position.y < -100)
        {
            Destroy(nowAttack[0]);
            Destroy(nowAttack[1]);
            Destroy(nowAttack[2]);
            Destroy(nowAttackY[0]);
            Destroy(nowAttackY[1]);
            Destroy(nowAttackY[2]);
            bossAttack = 0;
            attackCount = 0;
            downAttackCount = 0;
        }
    }

    void DefenceIns()
    {
        
        if (defenceCount < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
                float x = Random.Range(-15, 15);
                // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
                float y = Random.Range(-7, 7);
                // rangeAとrangeBのz座標の範囲内でランダムな数値を作成
                float z = Random.Range(-15, 15);

                if (nowDefence[defenceCount] == null)
                {
                    nowDefence[defenceCount] = Instantiate(insDefence, new Vector3(x, y, z), Quaternion.identity);                    
                }
                defenceCount++;
            }
        }

        if (defenceCount == 4)
        {
            defence = 5;
            bossAttack = 0;
            attackCount = 0;
            defenceCount = 0;
        }
    }

    void Roop()
    {
        RoopTimer++;

        float x = radius * Mathf.Sin(aroundTime);      //X軸の設定
        float z = radius * Mathf.Cos(aroundTime);      //Z軸の設定 

        float x2 = radius * Mathf.Sin(aroundTime2);      //X軸の設定
        float z2 = radius * Mathf.Cos(aroundTime2);      //Z軸の設定
        
        Vector3 NR1 = new Vector3(x , 15 , z);
        Vector3 NR2 = new Vector3(x2, -15, z2);

        if (RoopTimer == 5)
        {
            aroundTime = Random.Range(0, 6.28f);
            nowRoop[0] = Instantiate(insShot, NR1, Quaternion.identity);

            aroundTime2 = Random.Range(0, 6.28f);
            nowRoop[1] = Instantiate(insShot, NR2, Quaternion.identity);
        }

        if(nowRoop[0] != null)
        {
            aroundTime += 0.007f;
            nowRoop[0].transform.position = NR1;
        }

        if (nowRoop[1] != null)
        {
            aroundTime2 += 0.007f;
            nowRoop[1].transform.position = NR2;
        }

        if(RoopTimer > 400)
        {
            Destroy(nowRoop[0]);
            Destroy(nowRoop[1]);
        }

        if (RoopTimer > 400 && nowRoop[0] == null && nowRoop[1] == null)
        {
            bossAttack = 0;
            attackCount = 0;
        }
        //transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);
    }

    void Texts()
    {
        Text HP_Text = HP_Object.GetComponent<Text>();
        Text D_Text = D_Object.GetComponent<Text>();

        HP_Text.text = "Boss_HP : " + hp;
        D_Text.text = "Boss_Defence : " + defence;
    }

}

