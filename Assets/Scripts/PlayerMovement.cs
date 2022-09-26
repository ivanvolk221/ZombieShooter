using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    //private static float vertical, horizontal;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called before the first frame update

    private float x;
    private float y;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        RotatePlayer();
        //horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        //vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        //transform.Translate(horizontal, vertical, 0);

        Vector2 moveInput = new Vector2(x, y);
        if (Mathf.Abs(x) + Mathf.Abs(y) > 1)
            moveVelocity = moveInput.normalized * _speed;
        else
            moveVelocity = moveInput * _speed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);    
    }

    private void RotatePlayer()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(x) + Mathf.Abs(y) < 0.3)
            return;

        if (x >= 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270 + (90 * y) / (Mathf.Abs(x) + Mathf.Abs(y))));
        if(x < 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + (90 * -y) / (Mathf.Abs(x) + Mathf.Abs(y))));

    }
}
