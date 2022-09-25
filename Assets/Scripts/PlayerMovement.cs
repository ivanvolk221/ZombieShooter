using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float Speed;
    //[SerializeField] float MovementSpeed;
    private static float vertical, horizontal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0);
    }
}
