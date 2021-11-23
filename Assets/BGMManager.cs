using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{

    private static bool isLoad = false;// 自身がすでにロードされているかを判定するフラグ

    // Start is called before the first frame update
    void Start()
    {
        if (isLoad == true)
        { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            return;
        }
        isLoad = true; // ロードされていなかったら、フラグをロード済みに設定する


        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
