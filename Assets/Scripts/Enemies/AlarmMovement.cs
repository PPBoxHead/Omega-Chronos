using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmMovement : MonoBehaviour
{

    private bool checkTrigger;
    public float speed;
    public Transform targetAlarm;

    void Start()
    {
        targetAlarm = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkTrigger)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetAlarm.position.x, transform.position.y), speed * Time.deltaTime);
            //Bo si alguno sabe como hacer esto mas fluido genial, porque la verdad ahora mismo se mueve muy trancado
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            checkTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            checkTrigger = false;
        }
    }
}
