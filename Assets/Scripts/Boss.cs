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
    [SerializeField] private GameObject[] nowAttack;             //
    [SerializeField] private int downAttack;                     //
    [SerializeField] private int downAttackCount;                //
    [SerializeField] private GameObject insDown;                 //

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

    [SerializeField] public GameObject targetObject;

    [SerializeField] public GameObject HP_Object;
    [SerializeField] public GameObject D_Object;

    // Start is called before the first frame update
    void Start()
    {
        hp = 500;

        bossAttack = 0;
        attackCount = 0;
        downAttackCount = 0;

        radius = 35;
    }

    // Update is called once per frame
    void Update()
    {
        BossAttack();
        Look();
        Texts();
    }

    void Look()
    {
        transform.LookAt(targetObject.transform);
    }

    void BossAttack()
    {
        
        attackCount++;
        //攻撃が始まるまでのカウント
        if (attackCount > 400 && attackCount < 500)
        {
            transform.localScale = new Vector3(10, 4, 10);
        }

        if (attackCount == 500.0f)
        {
            //bossAttack = 5;
            //度の攻撃かをランダムで選ぶ
            transform.localScale = new Vector3(14, 6, 14);
            bossAttack = Random.Range(1, 5);
        }

        if (bossAttack == 1)
        {
            Shot();
        }

        if (bossAttack == 2)
        {
            DownAttack();
        }

        if (bossAttack == 3)
        {
            DefenceIns();
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

            if (shotCount == 21)
            {
                shotObjects[1] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 41)
            {
                shotObjects[2] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 61)
            {
                shotObjects[3] = Instantiate(insShot, transform.position, Quaternion.identity);
                shotAttackCount++;
            }

            if (shotCount == 81)
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

        if (shotTimer > 500)
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
            shotObjects[0].transform.position += shotObjects[0].transform.forward * 0.5f;           
        }

        if (shotObjects[1] != null)
        {
            shotObjects[1].transform.position += shotObjects[1].transform.forward * 0.5f;
        }

        if (shotObjects[2] != null)
        {
            shotObjects[2].transform.position += shotObjects[2].transform.forward * 0.5f;
        }

        if (shotObjects[3] != null)
        {
            shotObjects[3].transform.position += shotObjects[3].transform.forward * 0.5f;
        }

        if (shotObjects[4] != null)
        {
            shotObjects[4].transform.position += shotObjects[4].transform.forward * 0.5f;
        }

        if (shotTimer > 500)
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
            shotTimer > 500)
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
                downAttack = Random.Range(0, 8);               
                nowAttack[downAttackCount] = Instantiate(insDown, attackPoints[downAttack].transform.position, Quaternion.identity);
                downAttackCount++;               
            }
        }
       
        //生成が終わったら落とす
        if(nowAttack[0] != null && nowAttack[1] != null && nowAttack[2] != null)
        {
            nowAttack[0].transform.position += new Vector3(0, -0.8f, 0);
            nowAttack[1].transform.position += new Vector3(0, -0.8f, 0);
            nowAttack[2].transform.position += new Vector3(0, -0.8f, 0);
        }


        //一定の地点まで落ちたら落石を消去、そして値をリセット
        if (nowAttack[0].transform.position.y < -100 && 
            nowAttack[1].transform.position.y < -100 && 
            nowAttack[2].transform.position.y < -100)
        {
            Destroy(nowAttack[0]);
            Destroy(nowAttack[1]);
            Destroy(nowAttack[2]);
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
                float x = Random.Range(-25, 25);
                // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
                float y = Random.Range(-8, 8);
                // rangeAとrangeBのz座標の範囲内でランダムな数値を作成
                float z = Random.Range(-25, 25);

                if (nowDefence[defenceCount] == null)
                {
                    nowDefence[defenceCount] = Instantiate(insDefence, new Vector3(x, y, z), Quaternion.identity);                    
                }
                defenceCount++;
            }
        }

        if (defenceCount == 4)
        {
            defence = 20;
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
            aroundTime += 0.002f;
            nowRoop[0].transform.position = NR1;
        }

        if (nowRoop[1] != null)
        {
            aroundTime2 += 0.002f;
            nowRoop[1].transform.position = NR2;
        }

        if(RoopTimer > 500)
        {
            Destroy(nowRoop[0]);
            Destroy(nowRoop[1]);
        }

        if (RoopTimer > 500 && nowRoop[0] == null && nowRoop[1] == null)
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

