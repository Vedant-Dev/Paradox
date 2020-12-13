using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float SprintSpeed;
    public float JumpForce;
    public float G;
    public Transform GroundCheckPoint;
    public float GroundCheckRadius;
    public LayerMask GroundCheckMask;
    public GameObject Sword;
    public GameObject Shield;
    public GameObject SwordHolder;
    public GameObject ShieldHolder;


    private CharacterController characterController;
    private Vector3 movement;
    private bool IsFacingRight;
    private float InGameSpeed;
    private bool IsPlayerGrounded;
    private Vector3 playerVelocity;
    private bool IsCarringWeapon;
    private bool IsAttacking;
    private bool IsBlocking;
    private void Start()
    {
        IsAttacking = false;
        IsBlocking = false;
        IsCarringWeapon = true;
        IsFacingRight = true;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        IsPlayerGrounded = Physics.CheckSphere(GroundCheckPoint.position, GroundCheckRadius, GroundCheckMask);
        if (IsPlayerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float moveX = Input.GetAxis("Horizontal");
        movement = Vector3.right * Time.fixedDeltaTime * moveX * InGameSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            InGameSpeed = SprintSpeed;
        }
        else
        {
            InGameSpeed = Speed;
        }

        if ((moveX > 0.1f || moveX < -0.1f) && IsPlayerGrounded && !IsAttacking && !IsBlocking)
        {
            if (moveX > 0.5f || moveX < -0.5f)
            {
                if (InGameSpeed > Speed)
                {
                    if (!IsCarringWeapon)
                    {
                        SetAnimation("Sprint");
                    }
                    else
                    {
                        SetAnimation("Run");
                    }
                }
                else
                {
                    if (!IsCarringWeapon)
                    {
                        SetAnimation("Run");
                    }
                    else
                    {
                        SetAnimation("WeaponWalk");
                    }
                }
            }
            else
            {
                if (!IsCarringWeapon)
                {
                    SetAnimation("Walk");
                }
                else
                {
                    SetAnimation("WeaponWalk");
                }
            }
        }
        else
        {
            if (IsPlayerGrounded && !IsAttacking && !IsBlocking)
            {
                if (!IsCarringWeapon)
                {
                    SetAnimation("Idle");
                }
                else
                {
                    SetAnimation("WeaponIdle");
                }
            }
        }

        if (moveX > 0f && !IsFacingRight)
        {
            Flip();
        }

        if (moveX < 0f && IsFacingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerGrounded)
        {
            JumpPlayer();
            SetAnimation("Jump");
        }

        if (IsPlayerGrounded)
        {
            characterController.center = new Vector3(characterController.center.x, 1.07f, characterController.center.z);
        }
        else
        {
            characterController.center = new Vector3(characterController.center.x, 2.07f, characterController.center.z);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon();
        }

        if (Input.GetMouseButtonDown(0) && !IsAttacking)
        {
            AttackSword();
        }


        if (Input.GetKeyDown(KeyCode.Q) && !IsAttacking)
        {
            AttackShield();
        }

        if (Input.GetMouseButtonDown(1) && IsPlayerGrounded && !IsAttacking && !IsBlocking)
        {
            Block();
        }
        playerVelocity.y += G * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        MovePlayer(movement);
    }

    private void MovePlayer(Vector3 move)
    {
        if (!IsAttacking)
        {
            characterController.Move(move);
        }
    }

    private void JumpPlayer()
    {
        playerVelocity.y += Mathf.Sqrt(JumpForce * -3.0f * G);
    }
    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    private void AttackSword()
    {
        StartCoroutine("SetAttackTime");
        SetAnimation("AttackSword");
    }

    private void AttackShield()
    {
        StartCoroutine("SetAttackTime");
        SetAnimation("AttackShield");
    }

    private void Block()
    {
        StartCoroutine("SetBlockTime");
        SetAnimation("BlockOnce");
    }

    private void ChangeWeapon()
    {
        IsCarringWeapon = !IsCarringWeapon;
        Sword.SetActive(IsCarringWeapon);
        Shield.SetActive(IsCarringWeapon);
        SwordHolder.SetActive(!IsCarringWeapon);
        ShieldHolder.SetActive(!IsCarringWeapon);
    }

    IEnumerator SetAttackTime()
    {
        IsAttacking = true;
        yield return new WaitForSeconds(1f);
        IsAttacking = false;
    }

    IEnumerator SetBlockTime()
    {
        IsBlocking = true;
        yield return new WaitForSeconds(1f);
        IsBlocking = false;
    }

    private void SetAnimation(string name)
    {
        if (name == "Idle")
            Action.instance.SetAnimation(0);
        if (name == "Walk")
            Action.instance.SetAnimation(1);
        if (name == "Run")
            Action.instance.SetAnimation(2);
        if (name == "Sprint")
            Action.instance.SetAnimation(3);
        if (name == "Jump")
            Action.instance.SetAnimation(4);
        if (name == "WeaponIdle")
            Action.instance.SetAnimation(5);
        if (name == "WeaponWalk")
            Action.instance.SetAnimation(6);
        if (name == "AttackSword")
            Action.instance.SetAnimation(7);
        if (name == "AttackShield")
            Action.instance.SetAnimation(8);
        if (name == "BlockOnce")
            Action.instance.SetAnimation(9);
        if (name == "KeepBlocked")
            Action.instance.SetAnimation(10);
        if (name == "Damage1")
            Action.instance.SetAnimation(11);
        if (name == "Damage2")
            Action.instance.SetAnimation(12);
        if (name == "Death")
            Action.instance.SetAnimation(13);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheckPoint.position, GroundCheckRadius);
    }
}   