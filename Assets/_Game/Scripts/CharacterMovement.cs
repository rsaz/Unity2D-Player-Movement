using System;
using UnityEngine;
using TMPro;

public class CharacterMovement : MonoBehaviour
{
    #region Properties
    public enum MovementType { TRANSLATE, POSITION, VELOCITY, ADDFORCE, MOVEPOSITION };
    [Header("Movement Type Options")]
    public MovementType movementType;
    public bool force = false;
    public bool impulse = false;
    public float speed;

    private Vector2 direction;
    private Rigidbody2D rb;
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 5f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDirection();
        UpdateAnimationAndSprite();

        // Update the text
        GameObject.Find("TextMoveType").GetComponent<TextMeshProUGUI>()
                    .text = "MOVIMENTO USANDO :: " + Enum.GetName(typeof(MovementType), movementType).ToString();
    }

    private void FixedUpdate()
    {
        switch (Enum.GetName(typeof(MovementType), movementType))
        {
            case "TRANSLATE":
                UsingTranslate();
                break;
            case "POSITION":
                UsingTransformPosition();
                break;
            case "VELOCITY":
                UsingVelocity();
                break;
            case "ADDFORCE":
                UsingAddForce();
                break;
            case "MOVEPOSITION":
                UsingMovePosition();
                break;
        }
    }
    #endregion

    #region Update Direction, Animation and Sprite
    private void UpdateDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        direction = new Vector2(x, y);
    }

    private void UpdateAnimationAndSprite()
    {
        // verify if the player is moving
        if (direction.x != 0)
        {
            GetComponent<Animator>().SetFloat("speed", 0.2f);
        }
        else
            GetComponent<Animator>().SetFloat("speed", 0.0f);


        // Identify which direction player is going to
        if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    #endregion

    #region Movement Without Physics
    private void UsingTransformPosition()
    {
        transform.position += new Vector3(direction.x, 0f) * speed * Time.deltaTime;
    }

    private void UsingTranslate()
    {
        transform.Translate(new Vector2(direction.x, 0f) * speed * Time.deltaTime);
    }

    #endregion

    #region Movement With Physics
    private void UsingVelocity()
    {
        rb.velocity = new Vector2(direction.x * speed, 0f);
    }

    private void UsingAddForce()
    {
        if (force)
        {
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
        else if (impulse)
        {
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }
    }

    private void UsingMovePosition()
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.fixedDeltaTime));
    }

    #endregion
}
