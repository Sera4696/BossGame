using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    //[SerializeField] private GameObject Awawa;
    //[SerializeField] private GameObject white;
    //[SerializeField] Animator awawa;

    [SerializeField] private  ParticleSystem awa1;
    [SerializeField] private  ParticleSystem awa2;
    [SerializeField] private  ParticleSystem awa3;
    [SerializeField] private  GameObject White;

    [SerializeField] private GameObject dontDesCamera;

    private float alpha = 0;

    private float changeTime = 0;

    private Animation anim;

    private static bool isLoad = false;// 自身がすでにロードされているかを判定するフラグ

    private void Awake()
    {

        if (isLoad == true)
        { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            Destroy(this.dontDesCamera);
            return;
        }
        isLoad = true; // ロードされていなかったら、フラグをロード済みに設定する

        DontDestroyOnLoad(dontDesCamera);
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        White.GetComponent<SpriteRenderer>().color=new Color(1,1,1,0);
        awa1.Stop();
        awa2.Stop();
        awa3.Stop();

        //GameObject dontDesCamera = GameObject.Find("dontDesCamera");
        transform.position = new Vector3(dontDesCamera.transform.position.x, dontDesCamera.transform.position.y + 1, dontDesCamera.transform.position.z);

        transform.rotation = dontDesCamera.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        #region デバッグ用
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Instantiate(Awawa, transform.position,Quaternion.identity);
            //anim.Play();
            awa1.Play();
            awa2.Play();
            awa3.Play();

            //SearchCamera();

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.Append(White.GetComponent<SpriteRenderer>().DOFade(1, 2).OnComplete(() =>
            {

                ChangeScene();

                //SearchCamera();
            }));

            sequence.AppendInterval(1f);
            sequence.Append(White.GetComponent<SpriteRenderer>().DOFade(0, 2));


            sequence.Play();
        }
        #endregion
    }

    public void Fade()
    {
        Debug.Log("きたで");
        awa1.Play();
        awa2.Play();
        awa3.Play();

        //SearchCamera();

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(White.GetComponent<SpriteRenderer>().DOFade(1, 2).OnComplete(() =>
        {

            ChangeScene();

            //SearchCamera();
        }));

        sequence.AppendInterval(1f);
        sequence.Append(White.GetComponent<SpriteRenderer>().DOFade(0, 2));


        sequence.Play();
    }

    public void SearchCamera()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }

    public static void ChangeScene()
    {
        //ここで飛ぶ先のシーンを弄れるよ
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            SceneManager.LoadScene("GameScene");
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
