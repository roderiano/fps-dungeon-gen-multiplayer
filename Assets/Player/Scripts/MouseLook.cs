using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    private float X, Y;
    public float sensitivity;
    public Transform cameraTransform;

    private PhotonView photonView;


    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;
    const float MIN_Y = -90.0f;
    const float MAX_Y = 90.0f;
     
    void Awake()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        X = euler.x;
        Y = euler.y;
    }

    void Start() 
    {
        photonView = GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
    }
     
    void Update()
    {
        if(photonView.IsMine && PhotonNetwork.IsConnected == true) 
        {
            X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);
            if (X < MIN_X) 
                X += MAX_X;
            else if (X > MAX_X) 
                X -= MAX_X;
            Y -= Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);
            if (Y < MIN_Y) 
                Y = MIN_Y;
            else if (Y > MAX_Y) 
                Y = MAX_Y;
            transform.rotation = Quaternion.Euler(0.0f, X, 0.0f);
            cameraTransform.rotation = Quaternion.Euler(Y, X, 0.0f);
        }
        else
        {
            cameraTransform.gameObject.SetActive(false);
        }
        
    }

}
