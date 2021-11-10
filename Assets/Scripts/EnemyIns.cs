using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIns : MonoBehaviour
{
    [SerializeField] public GameObject S;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.I))
        {
            for(int i = 0; i < 5; i++)
            {
                // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
                float x = Random.Range(-25, 25);
                // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
                float y = Random.Range(-8, 8);
                // rangeAとrangeBのz座標の範囲内でランダムな数値を作成
                float z = Random.Range(-25, 25);

                Instantiate(S, new Vector3(x, y, z), Quaternion.identity);
            }
        }
    }
}
