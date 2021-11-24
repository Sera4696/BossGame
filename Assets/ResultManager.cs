using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 0"))
        {
            canvas.SetActive(false);
            //これをコピペすればフェードが動きます
            GameObject.Find("AwaParents").GetComponent<TestScript>().Fade();
        }
    }
}
