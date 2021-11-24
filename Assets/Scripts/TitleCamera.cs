using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCamera : MonoBehaviour
{
    // 中心点
    private Vector3 _center = Vector3.zero;

    // 回転軸
    private Vector3 _axis = Vector3.up;

    // 円運動周期
    private float _period = 20f;

    public bool isTitle = false;
    private bool isTextMove = false;
    private bool isPad = false;
    private bool isCoice=true;

    private Vector3 GameStartTexPos = new Vector3(380,-41,0);
    private Vector3 EndTexPos = new Vector3(-14,-161,0);
    [SerializeField] private GameObject PushAText;
    [SerializeField] private GameObject GameStartText;
    [SerializeField] private GameObject EndText;
    [SerializeField] private GameObject TitleTextObj;

    [SerializeField] private GameObject Ring_Up;
    [SerializeField] private GameObject Ring_Down;

    //音関係
    [SerializeField] private AudioClip kettei;
    [SerializeField] private AudioClip sentaku;
    private AudioSource audioSource;
    private bool isSoundSentaku = false,isAfterSentaku=false;

    private void Start()
    {
        TitleTextObj.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        if (isTitle == false)
        {
            // 中心点centerの周りを、軸axisで、period周期で円運動
            transform.RotateAround(
                _center,
                _axis,
                360 / _period * Time.deltaTime
            );
        }

        if (isTitle == true&&isTextMove==false)
        {
            PushAText.SetActive(false);
            
            transform.DOMove(new Vector3(-3, 25, 50), 1).SetEase(Ease.OutBack);
            transform.DORotate(new Vector3(27.5f, 180, 0), 0.5f);

            TitleTextObj.SetActive(true);
            Ring_Up.SetActive(false);
            //Ring_Down.SetActive(false);

            isTextMove = true;
        }
            
            

        if(isTitle == true && isTextMove == true)
        {
            GameStartText.transform.DOLocalMove(GameStartTexPos, 1f).SetEase(Ease.OutQuart);
            
            EndText.transform.DOLocalMove(EndTexPos, 0.5f).SetEase(Ease.OutQuart);
            isPad = true;

        }

        //if (EndText.transform.position == EndTexPos)
        //{
        //    Debug.Log("いきます");
            
            
        //}

        if (isPad == true)
        {
            
            if (isCoice == true)
            {
                //GameStartText.transform.DOLocalMove(GameStartTexPos, 1f).SetEase(Ease.OutQuart);
                //GameStartText.transform.DOLocalMoveX(GameStartTexPos.x - 50, 0.5f).SetEase(Ease.OutQuart);
                GameStartText.transform.DOLocalMoveX(330, 0.5f).SetEase(Ease.OutQuart);
                EndText.transform.DOLocalMoveX(-14, 0.5f).SetEase(Ease.OutQuart);
                //EndText.transform.DOLocalMoveX(EndTexPos.x, 0.5f).SetEase(Ease.OutQuart);
               
            }
            if (isCoice == false)
            {
                //EndText.transform.DOLocalMoveX(EndTexPos.x - 50, 0.5f).SetEase(Ease.OutQuart);
                EndText.transform.DOLocalMoveX(-64, 0.5f).SetEase(Ease.OutQuart);

                GameStartText.transform.DOLocalMoveX(380, 0.5f).SetEase(Ease.OutQuart);
                GameStartText.transform.DOLocalMoveX(GameStartTexPos.x, 0.5f).SetEase(Ease.OutQuart);
               
            }
        }
    }
    private void Update()
    {
        if (isPad == true)
        {
            isAfterSentaku = isSoundSentaku;
            isTextMove = false;
            //float ver = Input.GetAxis("Vertical");
            if (Input.GetAxis("Vertical") < 0)
            {
                isSoundSentaku = false;
                isCoice = false;
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                isSoundSentaku = true;
                isCoice = true;
            }

            //if (Input.GetAxis("Vertical") == 0)
            //{
            //    isSoundSentaku = false;
            //}
            if (isSoundSentaku == true && isAfterSentaku == false || isSoundSentaku == false && isAfterSentaku == true)
            {
                audioSource.PlayOneShot(sentaku);
            }

            if (isCoice == true)
            {
                
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 0"))
                {
                    audioSource.PlayOneShot(kettei);
                    //これをコピペすればフェードが動きます
                    GameObject.Find("AwaParents").GetComponent<TestScript>().Fade();
                }
            }
            if (isCoice == false)
            {
                //audioSource.PlayOneShot(sentaku);
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 0"))
                {
                    audioSource.PlayOneShot(kettei);
                    UnityEngine.Application.Quit();
                    //UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    }


}
