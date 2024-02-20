using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    //used class'
    private DialogueManager dialogueManager;

    //private fields
    private const float movementSpeed = 3f;
    private const float collisionOffset = 0.05f;
    private Vector2 movementInput = Vector2.zero;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private Animator animator;

    //public variables
    public ContactFilter2D movementFilter;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void FixedUpdate()
    {
        if(movementInput != Vector2.zero)
        {
            //Try to move player in input direction, followed by left and right and up and down input if failed
            bool success = TryMove(movementInput);

            //Try Left/Right
            if(!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                //Try Up/Down if failed
                if(!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            animator.SetBool("isWalking", success);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    //Tries to move the player in a direction by casting in that direction by the amount moved plus an offset.
    //If no collision is found, it moves the player. Returns true/false depending on if a move was executed.
    public bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            //Check for potential collisions
            int count = rb.Cast(
                direction, //x and y values between -1 and 1 that represents the direction from the body to look for collision
                movementFilter, //The settings that determine where a collision occur on such as layers to collide with
                castCollisions, //List of collisions to store the found collisions into after the Cast is finished
                movementSpeed * Time.fixedDeltaTime + collisionOffset); //The amount to cast equal to the movement plus offset

            if(count == 0) //No collision
            {
                rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void ClearDialoguePanel()
    {
        if(dialogueManager.isDisplayingNarratorDialogue)
        {
            dialogueManager.InvokeClearDisplay(1.5f);
        }
    }

    void OnMove(InputValue value)
    {
        ClearDialoguePanel();

        movementInput = value.Get<Vector2>();

        if(movementInput != Vector2.zero)
        {
            animator.SetFloat("XInput", movementInput.x);
            animator.SetFloat("YInput", movementInput.y);    
        }
    }
}