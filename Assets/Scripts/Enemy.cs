using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int reverse_x;
    [SerializeField] private int reverse_y;
    [SerializeField] private int reverse_z;
    public float x;
    public float y;
    public float z;

    [SerializeField] public float minSpeed;
    [SerializeField] public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        reverse_x = 1;
        reverse_y = 1;
        reverse_z = 1;

        minSpeed = -0.1f;
        maxSpeed = 0.1f;

        x = Random.Range(minSpeed, maxSpeed);
        y = Random.Range(minSpeed, maxSpeed);
        z = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += new Vector3(x * reverse_x, y * reverse_y, z * reverse_z);

        if (transform.position.x < -25 || transform.position.x > 25)
        {
            reverse_x *= -1;           
        }

        if(transform.position.y < -8 || transform.position.y > 8)
        {
            reverse_y *= -1;
        }

        if(transform.position.z < -25 || transform.position.z > 25)
        {
            reverse_z *= -1;
        }
    }
}
