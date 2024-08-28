using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class PlayerController : MonoBehaviour
{
    public float Stamina = 100.0f;
    public float maxStamina = 100.0f;
    public Image StaminaBar;

    public float runCost = 10.0f;

    private float WalkingChargeRate = 1;
    private float idleChargeRate = 2;

    public float maxHealth = 100f; // Max Health of Player
    public float currentHealth;    // Current Health of Player

    private float moveSpeed = 3f;      // Walking Speed
    private float runningSpeed = 2.1f; // Running Speed
    private float bushesSpeed = 1.9f;  // Speed in the Bushes

    private float gravity = -9.81f;

    public AudioSource FootStepSound;
    public AudioSource FootBushesSound;

    public bool isRunning = false;
    public bool isWalking = false;
    public bool isDying = false;
    public bool isInBushes = false;
    //private bool isPlayerAttacking = false;
    //public bool CanAttack = true;

    public Vector2 moveVector;
    private Vector2 lookVector;
    private Vector3 rotation;
    private Vector3 _direction;
    private float smoothTime = 0.05f;
    private float _currentVelocity;
    public float verticalVelocity;

    public Transform groundCheck;
    public float groundRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    private CharacterController characterController;
    private Animator animator;

    public bool isDead;
    public GameObject Spada;
    public GameObject Cestino;

    public Quaternion playerQuaternion;

    public PlayerInput InputPlayerController;

    public bool getIsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isDead = false;
        Spada.GetComponent<BoxCollider>().enabled = false; // No weapon active
    }

    void Start()
    {
        currentHealth = maxHealth; // All'inizio, la salute attuale è uguale alla salute massima
        Stamina = maxStamina;
        playerQuaternion = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (!isDead && !isDying)
        {
            //isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, whatIsGround);
            //Debug.Log(isGrounded);
            //if (!isPlayerAttacking) ApplyRotation();
            //if (!isPlayerAttacking) Move();
            ApplyGravity();
            ApplyRotation();
            Move();
            ReChargeStamina();
        }
    }

    //*******************************************************************************************************//
    //************************* RE-CHARGE STAMINA !! ********************************************************//
    //*******************************************************************************************************//
    public void ReChargeStamina()
    {
        if (!isRunning && isWalking)
        {
            Stamina += WalkingChargeRate / 10f;
            if (Stamina > maxStamina) Stamina = maxStamina;
            StaminaBar.fillAmount = Stamina / maxStamina;
        }
        else if (!isRunning && !isWalking)
        {
            Stamina += idleChargeRate / 10f;
            if (Stamina > maxStamina) Stamina = maxStamina;
            StaminaBar.fillAmount = Stamina / maxStamina;
        }
    }

    //*******************************************************************************************************//
    //************************* PLAYER TAKE DAMAGE **********************************************************//
    //*******************************************************************************************************//
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("COLLISIONE CON NEMICO");
        if (!isDying && !isDead)
        {
            //isPlayerAttacking = false;
            //animator.SetBool("isAttacking", false);
            if (collision.collider.CompareTag("Enemy"))
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                isDying = true;
                isInBushes = false;
                if (!isDead) GameController.Instance.LiveLost();

                if (GameController.Instance.Lives >= 0)
                {
                    // RESPAWN !!!!
                    isDead = true;
                    isInBushes = false;
                    //if (isPlayerAttacking) { isPlayerAttacking = false; CanAttack = true; }
                    //if (animator.GetBool("isAttacking")) animator.SetBool("isAttacking", false);
                    Die();
                }
            }
        }
    }

    //****************************************************************************************//
    //********************** TAKE DAMAGE FROM ENEMY : NOT USED IN THIS VERSION ***************//
    //****************************************************************************************//
    public void TakeDamage(float damageAmount)
    {
        Debug.Log("DAMAGE !!!!!!!!!!!!!!!!!!!");
        //currentHealth -= damageAmount; // Sottrai i danni dalla salute attuale    
        //Debug.Log("CurrentHealth => " + currentHealth.ToString());
        //GameController.Instance.LiveLost();
        ////animator.SetTrigger("React");
        // Controlla se la salute è inferiore o uguale a zero
        //if (currentHealth <= 0)
        //{
        //    isDead = true;
        //    Die(); // Se sì, il personaggio muore
        //}
        //if (GameController.Instance.Lives > 0)
        //{
        //    // RESPAWN !!!!
        //}
        //else if (GameController.Instance.Lives == 0)
        //{
        //    isDead = true;
        //    Die();
        //}
    }

    //*******************************************************************************************************//
    //************************* PLAYER DIE ******************************************************************//
    //*******************************************************************************************************//
    private void Die()
    {
        isDying = true;
        animator.SetBool("Die", true);// Opzionale: Aggiungi qui effetti visivi o sonori per la morte del personaggio
    }

    private void PlayerDie(string input)
    {
        isDying = false;
        animator.SetBool("Die", false);

        if (GameController.Instance.Lives > 0)
        {
            // RESPAWN !!!!
            //Debug.Log("DOPO MORTE RESPWAN !!!!!!!!!!!!!!!!!!!!!!");
            GameController.Instance.RespawnPlayer();
        }
        else if (GameController.Instance.Lives == 0)
        {
            GameController.Instance.PlayerCallGameOver();
        }
    }

    //*******************************************************************************************************//
    //*************************** RESPAWN PLAYER ..... ******************************************************//
    //*******************************************************************************************************//
    public void RespawnPlayer()
    {
        //animator.SetBool("Die", false);
        //animator.SetTrigger("Respawn");        
        gameObject.GetComponent<Animator>().Play("Idle");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        transform.rotation = playerQuaternion;
        isDead = false;
        isDying = false;
        isInBushes = false;
        //CanAttack = true;
        Stamina = maxStamina;
        StaminaBar.fillAmount = Stamina / maxStamina;
    }

    //*******************************************************************************************************//
    //************************* PLAYER OnMove ***************************************************************//
    //*******************************************************************************************************//
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        _direction = new Vector3(moveVector.y, 0.0f, moveVector.x);

        if (moveVector.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            isWalking = true;
        }
        else
        {
            animator.SetBool("isWalking", false);
            isWalking = false;
        }

    }

    //*******************************************************************************************************//
    //************************* PLAYER ROTATION *************************************************************//
    //*******************************************************************************************************//
    private void ApplyRotation()
    {
        if (moveVector.sqrMagnitude == 0) return;
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    //*******************************************************************************************************//
    //************************* APPLY GRAVITY TO PLAYER ****************************************************//
    //*******************************************************************************************************//
    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -1.0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        _direction.y = verticalVelocity;
    }

    //*******************************************************************************************************//
    //************************* PLAYER MOVEMENT *************************************************************//
    //*******************************************************************************************************//
    private void Move()
    {
        if (isWalking && !isInBushes) characterController.Move(_direction * moveSpeed * Time.deltaTime);
        if (isWalking && isInBushes) characterController.Move(_direction * bushesSpeed * Time.deltaTime);

        if (Stamina > 0)
        {
            if (isRunning && !isInBushes)
            {
                characterController.Move(_direction * runningSpeed * Time.deltaTime);

                if (moveVector.sqrMagnitude > 0)
                {
                    Stamina -= runCost * Time.deltaTime;
                    if (Stamina < 0) Stamina = 0;
                    StaminaBar.fillAmount = Stamina / maxStamina;
                }
            }
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                animator.SetBool("isRunning", false);
                isWalking = true;
                animator.SetBool("isWalking", true);
            }
        }
    }

    //******************************************************************************************************//
    //******************* NEW INPUT SYSTEM MANAGER *********************************************************//
    //******************************************************************************************************//
    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    //******************************************************************************************************//
    //****************************** PLAYER PRESS SSHIFT BUTTON TO RUN !!! *********************************//
    //******************************************************************************************************//
    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (Stamina > 0)
                {
                    if (isWalking & !isInBushes)
                    {
                        animator.SetBool("isRunning", true);
                        isRunning = true;
                    }
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    isRunning = false;
                }
                break;
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Canceled:
                animator.SetBool("isRunning", false);
                isRunning = false;
                break;
        }
    }

    //******************************************************************************************************//
    //***************************** FOOT STEP EMITTER .... *************************************************//
    //******************************************************************************************************//
    public void OnFootStep(string input)
    {
        if (!isInBushes) { AudioManager.Instance.sfxSource.Stop(); AudioManager.Instance.PlaySoundPlayerFXClip("PlayerStepSound", transform); }//AudioManager.Instance.PlaySFX("PlayerStepSound"); }//if (!isInBushes) FootStepSound.Play();
        if (isInBushes) { AudioManager.Instance.sfxSource.Stop(); AudioManager.Instance.PlaySoundPlayerFXClip("PlayerBrushesSound", transform); }//AudioManager.Instance.PlaySFX("PlayerBrushesSound"); }//if (isInBushes) FootBushesSound.Play();
    }

    //******************************************************************************************************//
    //******************* GET MUSHROOMS TO INCREASE SCORE POINTS *******************************************//
    //******************************************************************************************************//

    public void GetCoins(int value)
    {
        AudioManager.Instance.PlayItemsSFX("Mushrooms");
        GameController.Instance.AddCoins(value);
    }
    //******************************************************************************************************//
    //**************************** GET MORE LIVES FOR RESPAWN !!! ******************************************//
    //******************************************************************************************************//
    public void GetAmpolla()
    {
        AudioManager.Instance.PlayItemsSFX("PowerUp");
        GameController.Instance.LiveUp();
    }

    //******************************************************************************************************//
    //******************************** GET POTIONS FOR ARRIVE TO FINAL LEVEL *******************************//
    //******************************************************************************************************//
    public void GetFialettaBlu()
    {
        AudioManager.Instance.PlayItemsSFX("PowerUp");
    }

    public void GetFialettaGialla()
    {
        AudioManager.Instance.PlayItemsSFX("PowerUp");
    }

    public void GetFialettaRosa()
    {
        AudioManager.Instance.PlayItemsSFX("PowerUp");
    }

    //******************************************************************************************************//
    //******************************* GET BOOKS WITH LETTERS FOR STORY *************************************//
    //******************************************************************************************************//
    public void GetBooks(int value) // NOT USED IN 8 MAY PRESENTATION
    {
        AudioManager.Instance.PlayItemsSFX("PowerUp");
        GameController.Instance.AddBooks(value);
    }

    //*******************************************************************************************************//
    //************************* MANAGE PLAYER ATTACK : NOT USED IN THIS VERSION *****************************//
    //*******************************************************************************************************//
    public void OnHoldAttack(InputAction.CallbackContext context)
    {
        //if (CanAttack && !isDead)
        //{
        //    switch (context.phase)
        //    {
        //        case InputActionPhase.Performed:
        //            break;
        //        case InputActionPhase.Started:
        //            isPlayerAttacking = true;
        //            Cestino.GetComponent<BoxCollider>().enabled = true;
        //            animator.SetBool("isWalking", false);
        //            animator.SetBool("isRunning", false);
        //            animator.SetBool("isAttacking", true);
        //            CanAttack = false;
        //            break;
        //        case InputActionPhase.Canceled:
        //            isPlayerAttacking = false;
        //            break;
        //    }
        //}
    }
    //IEnumerator SetCanAttack()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    CanAttack = true;
    //}
    public void isAttackingFinished()
    {
        //isPlayerAttacking = false;
        //Cestino.GetComponent<BoxCollider>().enabled = true;
        ////Spada.GetComponent<BoxCollider>().enabled = false;

        //if (moveVector.magnitude > 0)
        //{
        //    if (isWalking) animator.SetBool("isWalking", true);
        //    if (isWalking && !isRunning) animator.SetBool("isWalking", true);
        //    if (isWalking && isRunning) animator.SetBool("isRunning", true);
        //    if (!isWalking && isRunning) animator.SetBool("isRunning", true);
        //    if (isRunning) animator.SetBool("isRunning", true);
        //}
        //else
        //{
        //    animator.SetBool("isWalking", false);
        //    animator.SetBool("isRunning", false);
        //}
        //StartCoroutine(SetCanAttack());
    }

    public void AttackFinished(string input)
    {
        animator.SetBool("isAttacking", false);
        Invoke("isAttackingFinished", 0.2f);
    }

    //public void OnAttack(InputAction.CallbackContext context)
    //{
    //    if (CanAttack)
    //    {
    //        isPlayerAttacking = true;
    //        //Spada.GetComponent<BoxCollider>().enabled = true;
    //        Cestino.GetComponent<BoxCollider>().enabled = true;
    //        //if (animator.GetBool("isWalking"))
    //        //{
    //        //    animator.SetBool("isWalking", false);
    //        //    animator.SetBool("isAttacking", true);
    //        //}
    //        //else if (animator.GetBool("isRunning"))
    //        //{
    //        //    animator.SetBool("isRunning", false);
    //        //    animator.SetBool("isAttacking", true);
    //        //}
    //        //else
    //        //{
    //        //    animator.SetBool("isAttacking", true);
    //        //}
    //        animator.SetBool("isWalking", false);
    //        animator.SetBool("isRunning", false);
    //        animator.SetBool("isAttacking", true);
    //        CanAttack = false;
    //    }
    //}
}
