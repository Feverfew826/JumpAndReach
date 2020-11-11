using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour, InputHandler.InputReceiver
{
    [Range(100, 2000)]
    public float speed;
    [Range(100, 1000)]
    public float jumpPower;

    private Rigidbody2D rigidBody;
    private InputHandler inputHandler;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GroundChecker groundChecker;

    public interface DeathHandler
    {
        void OnDeath();
    }
    public DeathHandler deathHandler;

    public void OnHorizontalAxis(float horizontalAxis)
    {
        float currentStatusSpeed = speed; 

        if (!groundChecker.isGrounded)
            currentStatusSpeed *= 0.2f;
        else if (Mathf.Abs(rigidBody.velocity.magnitude) < 2)
            currentStatusSpeed *= 2f;

        if (horizontalAxis > 0)
        {
            if (rigidBody.velocity.x > 8)
                currentStatusSpeed *= 0f;
            spriteRenderer.flipX = false;
            rigidBody.AddForce(Vector2.right * currentStatusSpeed * Time.deltaTime);
        }
        else if (horizontalAxis < 0)
        {
            if (rigidBody.velocity.x < -8)
                currentStatusSpeed *= 0f;
            spriteRenderer.flipX = true;
            rigidBody.AddForce(Vector2.left * currentStatusSpeed * Time.deltaTime);
        }
    }

    public void OnVerticalAxis(float verticalAxis)
    {
    }

    public void OnJumpButtonDown()
    {
        if(groundChecker.isGrounded)
            rigidBody.AddForce(Vector2.up * jumpPower);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<InputHandler>();
        inputHandler.AddInputReceiverRegisterAwaiter(this);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Grounded", groundChecker.isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
    }

    public void Die()
    {
        if (deathHandler != null)
            deathHandler.OnDeath();
    }

    public void Ceremony()
    {
        animator.SetTrigger("Ceremony");
    }

    public void TeleportAt(Vector3 position)
    {
        rigidBody.velocity = Vector2.zero;
        transform.position = position;
    }
}
