using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOfTheStick : MonoBehaviour
{
    [SerializeField] private SphereCollider sCol;
    private float speed=60;
    private bool isGo = false,isStick=true;

    [SerializeField] private LineRenderer lineRenderer;
    private List<Vector3> LinePoints = new List<Vector3>();

    [SerializeField]private GameObject[] Points;
    [SerializeField] private GameObject insPoint;
    private int pointCount = 0;
    private Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {
        sCol = GetComponent<SphereCollider>();
        lineRenderer.enabled = true;
        
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        PlayerMove();
    }
    void Update()
    {
        
    }

    public void PlayerMove()
    {
        if (isStick == true)
        {
            // 入力を取得
            var v1 = Input.GetAxis("Vertical");
            var h1 = Input.GetAxis("Horizontal");

            var v2 = Input.GetAxis("Vertical2");
            var h2 = Input.GetAxis("Horizontal2");

            // スティックが倒れていれば、移動
            //if (h1 != 0 || v1 != 0)
            //{
            //    var direction = new Vector3(h1, 0, v1);
            //    agent.Move(direction * Time.deltaTime);
            //}
            // スティックが倒れていれば、倒れている方向を向く
            if (h2 != 0 || v2 != 0)
            {
                var direction = new Vector3(h2, 0, v2);
                transform.localRotation = Quaternion.LookRotation(direction);
               
                isGo = true;
                isStick = false;

                //ポイントを増やす場所
                Points[pointCount] = Instantiate(insPoint, transform.position, Quaternion.identity);
                pointCount++;

            }
        }

        if (isGo == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        //ポイントを登録する場所
        //for (int i = 0; i <= Points.Length; i++)
        //{
        //    var positions = new Vector3[]
        //    {
        //         Points[i].transform.position,
        //         transform.position,
        //    };

        //    Debug.Log("登録されとるで");
        //   }
        
        //lineRenderer.positionCount = positions.Length;
        //lineRenderer.SetPositions(positions);
        //Debug.Log(positions.Length);

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //    if (other.gameObject.tag == "Ring")
    //    {
    //        isGo = false;
    //        isStick = true;
    //        Debug.Log("あたった");
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ring")
        {
            

            isGo = false;
            isStick = true;
        }
    }

}
