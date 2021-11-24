using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShellMove : MonoBehaviour
{

    [SerializeField] private GameObject shellTop;
    private bool isOpen = true;
    private float stopTimer = 0,limitter;
    // Start is called before the first frame update
    void Start()
    {
        limitter = Random.Range(5, 7);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen == true)
        {
            shellTop.transform.DOLocalRotate(new Vector3(90, 0, 0), 5);
            stopTimer += Time.deltaTime;
            
            if (stopTimer >= limitter)
            {
                isOpen = false;
                stopTimer = 0;
                limitter = Random.Range(5, 10);
            }
        }

        if (isOpen == false)
        {
            shellTop.transform.DOLocalRotate(new Vector3(0, 0, 0), 5);
            stopTimer += Time.deltaTime;
            //limitter = Random.Range(5, 7);
            if (stopTimer >= limitter)
            {
                isOpen = true;
                stopTimer = 0;
                limitter = Random.Range(5, 10);
            }
        }
    }
}
