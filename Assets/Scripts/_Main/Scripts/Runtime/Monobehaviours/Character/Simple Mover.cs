using InputManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using InputManagement;

public enum ENTITY_TYPE 
{ 
    PLAYER,
    ENEMY

}

public class SimpleMover : MonoBehaviour
{

    public static float angleDifferenceInY;

    public int idx;



    public ENTITY_TYPE entityType;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float dashAmount = 20f;
    public CharacterController characterController;
    public Animator animator;
    public TrailRenderer trailRenderer;
    public float angleDifferenceInYAxis;
    public FixedJoystick joystick;

    


    string[] basicAtk = { "1 [Basic Atk 1]", "1_1 [Basic Atk 2]"};
    int max = 2,baIdx= 0;

    public InputActionAsset inputActionAsset;
    public InputActionMap playerInputMap;

    private void Awake()
    {
        angleDifferenceInY = angleDifferenceInYAxis;
        playerInputMap = inputActionAsset.FindActionMap("Player", true);
    }

    private Vector2 GetPlayerNavigationVector() 
    {
        return playerInputMap.FindAction("Move").ReadValue<Vector2>();
    }

    private Vector2 GetSecondVec() 
    {
        return playerInputMap.FindAction("AnalogueLock").ReadValue<Vector2>();
    }

    private bool GetDashInput() 
    {
        //do it this way.
        return playerInputMap.FindAction("Dash").WasPerformedThisFrame();
    }

    private bool GetUltimateInput() 
    { 
        return playerInputMap.FindAction("Ultimate").ReadValue<float>() == 1;
    }
    
    private bool GetCloseRangedAtk() 
    { 
        return playerInputMap.FindAction("CloseRangedAttack").ReadValue<float>() == 1;
    }
    
    private bool GetLongRangeAtk() 
    { 
        return playerInputMap.FindAction("LongRangedAttack").ReadValue<float>() == 1;
    }

    private bool GetAutoLockInput() 
    {
        return playerInputMap.FindAction("AutoLock").WasPerformedThisFrame();
    }

    public float time;

    public GameObject i_gameObject => gameObject;

    public Transform i_Transform => transform;

    void Update()
    {
        time += Time.time;

        Vector2 moveVector ;

        if (idx  == 0) 
            moveVector = GetPlayerNavigationVector();
        else
            moveVector = GetSecondVec();

        Vector3 moveDirection = new Vector3(moveVector.x, 0,moveVector.y);
        moveDirection = Quaternion.AngleAxis(angleDifferenceInYAxis, Vector3.up) * moveDirection;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

      
        characterController.SimpleMove(moveDirection * moveSpeed);
        animator.SetFloat("MoveX", characterController.velocity.magnitude);
        Attack();
       
    }

    

    public void Attack() 
    {
        string attack = string.Empty;
        if (GetCloseRangedAtk() || GetLongRangeAtk()) 
        {
            attack = basicAtk[baIdx];
            baIdx++;
            baIdx %= max;
            animator.applyRootMotion = true;
            animator.Play(attack);
            StartCoroutine(WaitForCompletion(attack, () => { animator.applyRootMotion = false; }));
            return;
        }

        if (GetDashInput()) 
        {
            Dash();
            return;
        }

        if (GetUltimateInput())
        {
            animator.applyRootMotion = true;
            animator.Play(attack);
            StartCoroutine(WaitForCompletion(attack, () => { animator.applyRootMotion = false; }));
        }
    }

    private IEnumerator WaitForCompletion(string animName,Action onComplete) 
    {
        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash(animName)) 
        {
            yield return null;
        }

        onComplete();
    }

   

    private void Dash()
    {
        ClearTrail();
        trailRenderer.enabled = true;
        animator.SetFloat("MoveX", 2f);
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.forward * dashAmount;
        List<Vector3> vals = new List<Vector3>();

        for (float i = 0; i < .1; i += .2f)
        {
            vals.Add(Vector3.Lerp(startPos, endPos, i));
        }
        trailRenderer.SetPositions(vals.ToArray());
        characterController.Move(endPos);

        Invoke(nameof(ClearTrail), .2f);

    }

    private void ClearTrail()
    {
        animator.SetFloat("MoveX", 0f);
        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }



}

