using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStuff : MonoBehaviour
{
    public GameObject applePrefab;
    public Transform cameraTransform;
    public float throwForce = 10f;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            ThrowApple();
        }
    }

    private void ThrowApple()
    {
        GameObject apple = Instantiate(applePrefab, cameraTransform.position, Quaternion.identity);
        Rigidbody rb = apple.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);
    }
}
