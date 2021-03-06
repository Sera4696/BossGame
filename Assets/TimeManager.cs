using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{

    //　Time.timeScaleに設定する値
    
    private float timeScale = 0.1f;
    //　時間を遅くしている時間
    
    private float slowTime = 1f;
    //　経過時間
    private float elapsedTime = 0f;
    //　時間を遅くしているかどうか
    private bool isSlowDown = false;

    


    private void Start()
    {
        
        
    }

    private void FixedUpdate()
    {
        if (isSlowDown)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= slowTime)
            {
                SetNormalTime();
            }
        }
    }


    void Update()
    {
        //　スローダウンフラグがtrueの時は時間計測
        //if (isSlowDown)
        //{
        //    elapsedTime += Time.unscaledDeltaTime;
        //    if (elapsedTime >= slowTime)
        //    {
        //        SetNormalTime();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SlowDown();
        //}
    }
    //　時間を遅らせる処理
    public void SlowDown()
    {
        elapsedTime = 0f;
        Time.timeScale = timeScale;
        
        isSlowDown = true;
    }
    //　時間を元に戻す処理
    public void SetNormalTime()
    {
        Time.timeScale = 1f;
        elapsedTime = 0f;
        
        isSlowDown = false;
    }
}