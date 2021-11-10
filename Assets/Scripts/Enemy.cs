using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int reverse;
    public float x;
    public float y;
    public float z;
    // Start is called before the first frame update
    void Start()
    {
        reverse = 1;

        x = Random.Range(-0.5f, 0.5f);
        y = Random.Range(-0.5f, 0.5f);
        z = Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        

        transform.position += new Vector3(x * reverse, y * reverse, z * reverse);

        if (transform.position.x < -25 || transform.position.x > 25 ||
            transform.position.y <  -8 || transform.position.y >  8 ||
            transform.position.z < -25 || transform.position.z > 25)
        {
            reverse *= -1;
           
        }
    }
}
