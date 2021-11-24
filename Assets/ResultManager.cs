using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private AudioClip kettei;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 0"))
        {
            audioSource.PlayOneShot(kettei);
            canvas.SetActive(false);
            //これをコピペすればフェードが動きます
            GameObject.Find("AwaParents").GetComponent<TestScript>().Fade();
        }
    }
}
