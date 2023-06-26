using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("ParamData")]
    [SerializeField] Animator WerWolf;
    [Range(-0.5f, 0.5f)]
    [SerializeField] float AnimatorMoveX = 0;
    [Range(-0.5f, 0.5f)]
    [SerializeField] float AnimatorMoveY = 0;
    [SerializeField] bool Moving;
    [SerializeField] float LimitMove = 0.5f;
    [Header("GroundCheckStates")]
    [SerializeField] float groundCheckRadius = 0.4f;
    [SerializeField] float groundCheckOffsetY = 0.45f;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayers = 0;

    RaycastHit hit;

    void Update()
    {
        Move();
        SetAnim();
    }
    //Animation Param Set
    void SetAnim()
    {
        WerWolf.SetFloat("MoveX", AnimatorMoveX);
        WerWolf.SetFloat("MoveY", AnimatorMoveY);
        WerWolf.SetBool("Moving", Moving);
        WerWolf.SetBool("IsJump", Jump());
        WerWolf.SetBool("IsGround", CheckGroundStatus());
        WerWolf.SetInteger("Idle", RandomIdle());
        WerWolf.SetInteger("Attack", Attack());
        WerWolf.SetBool("Dodge", Dodge());
        Damage();
    }
    //Move Controll
    void Move()
    {
        if (Input.GetKey(KeyCode.W) & Input.GetKey(KeyCode.S)) AnimatorMoveY = 0f;
        else
        {
            if (Input.GetKey(KeyCode.W)) AnimatorMoveY += 1f * Time.deltaTime;
            else
            {
                if (AnimatorMoveY > 0)
                {
                    AnimatorMoveY = 0f;
                }
            }
            if (Input.GetKey(KeyCode.S)) AnimatorMoveY -= 1f * Time.deltaTime;
            else
            {
                if (AnimatorMoveY < 0)
                {
                    AnimatorMoveY = 0f;
                }
            }
        }
        if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.D)) AnimatorMoveX = 0f;
        else
        {
            if (Input.GetKey(KeyCode.A)) AnimatorMoveX -= 1f * Time.deltaTime;
            else
            {
                if (AnimatorMoveX < 0)
                {
                    AnimatorMoveX = 0f;
                }
            }
            if (Input.GetKey(KeyCode.D)) AnimatorMoveX += 1f * Time.deltaTime;
            else
            {
                if (AnimatorMoveX > 0)
                {
                    AnimatorMoveX = 0f;
                }
            }
        }

        if (AnimatorMoveX > LimitMove) AnimatorMoveX = LimitMove;
        if (AnimatorMoveX < -LimitMove) AnimatorMoveX = -LimitMove;
        if (AnimatorMoveY > LimitMove) AnimatorMoveY = LimitMove;
        if (AnimatorMoveY < -LimitMove) AnimatorMoveY = -LimitMove;

        if(AnimatorMoveX != 0 | AnimatorMoveY != 0)
        {
            Moving = true;
        }
        else
        {
            Moving = false;
        }
    }
    //Jumping
    bool Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) & CheckGroundStatus()) return true;
        else return false;
    }
    //Damage Anim
    void Damage()
    {
        if (Input.GetKeyDown(KeyCode.Q) & CheckGroundStatus()) WerWolf.SetTrigger("IsHit");
    }
    int Attack()
    {
        if (Input.GetKeyDown(KeyCode.E)) return 1;
        else return 0;
    }
    bool Dodge()
    {
        if (Input.GetKeyDown(KeyCode.C)) return true;
        else return false;
    }
    //Ground Check "return bool"
    bool CheckGroundStatus()
    {
        return Physics.SphereCast(transform.position + groundCheckOffsetY * Vector3.up, groundCheckRadius, Vector3.down, out hit, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore);
    }
    //Random Idle "return int"
    int RandomIdle()
    {
        int rand = Random.Range(0, 4);
        return rand;
    }
}
