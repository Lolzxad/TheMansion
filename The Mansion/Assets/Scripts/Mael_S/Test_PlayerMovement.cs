using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerMovement : MonoBehaviour
{
    public float speed;

    private float inputHorizontal;
    private float inputVertical;

    Rigidbody2D rb;
    GameObject player;


    private enum State { climb}
    

    //Ladder Variable
    public bool botLadder = false;
    public bool topLadder = false;
    public bool canClimb = false;
    public Ladder ladder;
    private float defaultGravity;
    [SerializeField] float climbSpeed = 3f;

   /* public float distance;
    public LayerMask whatIsLadder;*/

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    private void Update()
    {
        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            Climb();
            
        }

        if (!canClimb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //canClimb = false;
            GetComponent<BoxCollider2D>().enabled = true;
            rb.gravityScale = defaultGravity;

            Physics2D.IgnoreLayerCollision(9, 10, false);
        }
    }


    private void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);

        

        if (canClimb)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);

            
        }

        // RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);


        /*if(hitInfo.collider != null)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }*/
    }

    void Climb()
    {
        Physics2D.IgnoreLayerCollision(9, 10);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        transform.position = new Vector3(ladder.transform.position.x, rb.position.y);

        //GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;


        inputVertical = Input.GetAxis("Vertical");

        if(inputVertical > .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, inputVertical * climbSpeed);
        }
        else if (inputVertical < -.1f && !botLadder)
        {
            rb.velocity = new Vector2(0f, inputVertical * climbSpeed);
        }
        else
        {

        }

    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hit);
    }*/

}
