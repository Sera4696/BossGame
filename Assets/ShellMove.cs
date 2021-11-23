using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShellMove : MonoBehaviour
{

    [SerializeField] private GameObject shellTop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shellTop.transform.DORotate(new Vector3(90, 0, 0), 3);
    }
}
