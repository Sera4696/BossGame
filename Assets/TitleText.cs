using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleText : MonoBehaviour
{

    [SerializeField] GameObject targetObj;
    [SerializeField] private CameraShake CameraShake;
    // Start is called before the first frame update
    void Start()
    {
        CameraShake = GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetObj.transform);
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(CameraShake.Shake(0.2f, 1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("あたったで");
            StartCoroutine(CameraShake.Shake(0.2f, 1f));
        }
    }
}
