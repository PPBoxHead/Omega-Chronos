using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmoTrial : MonoBehaviour
{

    [SerializeField, Range(0.01f, 0.05f)] private float ghostDelay = 0.1f;
    private float ghostDelaySeconds;
    public GameObject body;
    public GameObject ghost;
    public bool makeGhost = false;

    // Start is called before the first frame update
    void Start()
    {
        
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeGhost)
        {
            if(ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;

            }else{
                //Generate the ghost
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = body.GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().flipX = body.GetComponent<SpriteRenderer>().flipX;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1f);
            }
        }
    }
}
