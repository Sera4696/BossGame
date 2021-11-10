using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOfTheStick : MonoBehaviour
{
    private bool isGo = false,isStick=true;

    [SerializeField] private SphereCollider sCol;
    // Start is called before the first frame update
    void Start()
    {
        sCol = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
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
            }
        }
        if (isGo == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        sCol.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        sCol.enabled = true;
    }
}
