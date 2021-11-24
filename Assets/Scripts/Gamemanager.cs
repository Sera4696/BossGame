using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    //GameManagerはメインカメラにアタッチするスクリプトです
    [SerializeField] private CameraShake CameraShake;
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
    }

    void Start()
    {
        CameraShake = GetComponent<CameraShake>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //画面の揺れ_左が時間_右揺れの強さ
           StartCoroutine(CameraShake.Shake(0.2f, 1f));
        }
    }

    public void CameraShakes()
    {
        StartCoroutine(CameraShake.Shake(0.2f, 0.3f));
    }
}
