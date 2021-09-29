using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform bodyTransform;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        rb.MovePosition(transform.position + ((bodyTransform.forward * Y * moveSpeed) + (bodyTransform.right * X * moveSpeed)) * Time.deltaTime);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(transform.up * jumpForce);
    }
}
