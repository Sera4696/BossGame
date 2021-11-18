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
        if (attackCount == 500.0f)
        {
            //bossAttack = 3;
            //度の攻撃かをランダムで選ぶ
            bossAttack = Random.Range(1, 4);
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
    }

    void Shot()
    {
        if(shotAttackCount < 1)
        {
            shotObjects[0] = Instantiate(insShot, transform.position, Quaternion.identity);
            shotAttackCount++;           
        }
        
        if(shotObjects[0] != null)
        {
            shotObjects[0].transform.position += transform.forward * 0.5f;
            shotTimer++;
        }

        if (shotTimer > 500)
        {
            Destroy(shotObjects[0]);
        }

        if (shotObjects[0] == null)
        {
            shotAttackCount = 0;
            shotTimer = 0;
            bossAttack = 0;
            attackCount = 0;
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
            shotObjects[4] == null)
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
                if(nowDefence[defenceCount] == null)
                {
                    nowDefence[defenceCount] = Instantiate(insDefence, transform.position, Quaternion.identity);                    
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

    void Texts()
    {
        Text HP_Text = HP_Object.GetComponent<Text>();
        Text D_Text = D_Object.GetComponent<Text>();

        HP_Text.text = "Boss_HP : " + hp;
        D_Text.text = "Boss_Defence : " + defence;
    }

}

