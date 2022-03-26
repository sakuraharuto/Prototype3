using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    Rigidbody2D rb2d;
    [SerializeField] float moveSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer(){
        float xValue = Input.GetAxisRaw("Horizontal");
        float yValue = Input.GetAxisRaw("Vertical");

        Vector3 movementVector = new Vector3(xValue, yValue, 0);
        movementVector = movementVector.normalized * moveSpeed * Time.deltaTime;

        rb2d.MovePosition(transform.position + movementVector);

        //transform.Translate(xValue, yValue, 0);
    }
}
