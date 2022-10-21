using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //movespeed

    public Transform orientation;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool jumpReady = true;

    [Header("DragVariables")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask CanJumpOff;
    public LayerMask IsInteractable;
    protected bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    //highlighted object reference
    private GameObject highlight;
    [SerializeField] GameObject heldPosition;
    protected GameObject held;
    private Rigidbody heldRB;

    //movement input
    protected float horizontalInput;
    protected float verticalInput;
    //movement values
    public Vector3 moveDir { get; protected set; }
    public Rigidbody rb { get; private set; }

    public virtual void Start()
    {
        //player
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //held position setup
    }

    public virtual void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.55f, CanJumpOff); //raycast for ground below
        if (grounded) { rb.drag = groundDrag; }
        else { rb.drag = 0; }

        //get input from keyboard
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //JUMPING DOES NOT WORK WITH THE NEW PLAYER CONTROLLER YET. WIP
        //check for jumping
        //Debug.Log("jump key detected | jumpReady: " + jumpReady + " | grounded: " + grounded);
        /*if(Input.GetKey(jumpKey) && jumpReady && grounded) //IF JUMP IS NOT WORKING, REMEMBER TO SET GROUND TO "isGround"
        {
            Debug.Log("Jumping");
            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); //reset jump after jump cooldown
        }*/

        //highlight, and if highlight grabbing stuff
        highlightInteract();
        //reset orientation of grab
        resetOrientation();
        //speed normalization
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //move what you're grabbing
        hold();
    }

    protected virtual void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) { GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f); }
        else if (!grounded) { GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f * airMultiplier); }
    }

    private void SpeedControl()
    {
        //ensures the maximum speed is maintained
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        Debug.Log("jump ready");
        jumpReady = true;
    }

    //highlight the thing you are looking at, and unhighlight when needed, and grab if input
    private void highlightInteract()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position + new Vector3(0, playerHeight * .64f, 0), Camera.main.transform.forward * 3, Color.cyan, .5f, true); ///visualization
        if (Physics.Raycast(transform.position + new Vector3(0, playerHeight * .64f, 0), Camera.main.transform.forward, out hit, 3, IsInteractable))
        {
            //Debug.Log("detected " + hit.collider.gameObject.ToString()); //outputs what we're looking at
            if (highlight != hit.collider.gameObject) //if the thing is not what we're highlighting, change our highlight
            {
                if (highlight) //if there's something currently highlighted remove it
                {
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = new Color(1,1,1,0);
                }
                //add new highlight
                highlight = hit.collider.gameObject.GetComponentInParent<Outline>().gameObject; //some objects have multiple parts, so grab the main gameobject.
                highlight.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 1);
            }
        }
        else if (highlight) //if we're highlighting nothing, remove the outline
        {
            highlight.gameObject.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0);
            highlight = null;
        }

        //grab object if button is pressed, nothing is held, and an object is highlighted
        if (Input.GetKeyDown(KeyCode.E) && !held && highlight && highlight.layer == LayerMask.NameToLayer("isGrabbable"))
        {
            held = hit.collider.gameObject;
            held.transform.parent = heldPosition.transform;
            heldRB = held.GetComponent<Rigidbody>();
            heldRB.MovePosition(heldPosition.transform.position); //clips into stuff
            heldRB.drag = 10;
            heldRB.useGravity = false;
            if(held.layer == LayerMask.NameToLayer("isGrabbable")) //shift layer
            {
                held.layer = LayerMask.NameToLayer("grabbed");
            }
            heldRB.freezeRotation = true;
        }
        //drop object if something is held
        else if(Input.GetKeyDown(KeyCode.E) && held)
        {
            heldRB.useGravity = true;
            held.transform.parent = null;
            heldRB.drag = 1;
            if (held.layer == LayerMask.NameToLayer("grabbed"))
            {
                held.layer = LayerMask.NameToLayer("isGrabbable");
            }
            heldRB.freezeRotation = false;
            held = null;
            heldRB = null;
        }
        else if(Input.GetKeyDown(KeyCode.E) && !held && highlight && highlight.layer == LayerMask.NameToLayer("isInteractable"))
        {
            interact(highlight.GetComponent<Interactable>());
        }
    }
    
    public virtual void interact(Interactable i)
    {
        
        i.interact();
    }

    //Most important part is Rigidbody settings.
    //If performance issues arise, changing Interpolate down or collision detection to Speculative could help
    //In testing, Interpolation "none" and Collision Detection "Continuous Dynamic" reduces wall glitching significantly
    private void hold()
    {
        if (held)
        {
            //Debug.Log(Vector3.Distance(heldPosition.transform.position, held.transform.position));
            if (Vector3.Distance(heldPosition.transform.position, held.transform.position) > .1f)
            {
                heldRB.AddForce((heldPosition.transform.position - held.transform.position) * 100);
                //held.transform = Vector3.Lerp(heldPosition.transform.position - held.transform.position) * Time.deltaTime);
            }
        }
    }

    private void resetOrientation()
    {
        if (Input.GetKeyDown(KeyCode.R) && held)
        {
            held.transform.localRotation = Quaternion.identity;
        }
    }
}