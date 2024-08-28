using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{
//    private GameObject playerObject; // Oggetto del giocatore
//    NavMeshAgent agent;
//    [SerializeField] LayerMask groundLayer;

//    Animator animator;
//    public BoxCollider boxColliderTesta;
//    bool isAttacking = false;

//    //patrol
//    Vector3 destPoint;
//    bool walkpointSet;
//    [SerializeField] float walkrange;
//    //attack

//    //state change
//    public float moveSpeed = 3f; // Velocità di movimento del nemico
//    public float detectionRadius = 10f; // Raggio di rilevamento del giocatore
//    public float attackRange = 1f; //Range di attacco del nemico
//    public float currentHealth; // Salute attuale del personaggio
//    public float maxHealth = 100f; // Salute massima del personaggio

//    //private bool playerInRange = false; // Flag per indicare se il giocatore è nel raggio di rilevamento

//    HealthBarScript healthBar;
   
//    private void Awake()
//    {
//        healthBar = GetComponentInChildren<HealthBarScript>();
//    }

//    private void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        currentHealth = maxHealth; // All'inizio, la salute attuale è uguale alla salute massima
//        healthBar.UpdateHealthBar(currentHealth, maxHealth);
//        playerObject = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
//        animator = GetComponent<Animator>();
//    }

//    private void Update()
//    {
//        if (playerObject == null)
//            return;

//        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

//        // Controlla se il giocatore è nel raggio di rilevamento
//        if (distanceToPlayer < detectionRadius)
//        {
//            // Se il giocatore è anche nell'intervallo di attacco, attacca senza inseguirlo
//            if (distanceToPlayer <= attackRange)
//            {
//                if (!isAttacking)
//                {
//                    Attack();
//                }
//            }
//            else
//            {
//                if (!isAttacking)
//                {
//                    Chase();
//                }
//            }
//        }
//        else
//        {
//            Patrol();
//        }
//    }

//    void Patrol()
//    {
//        if (!walkpointSet) SearchForDest();
//        if (walkpointSet) agent.SetDestination(destPoint);
//        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
//    }

//    void SearchForDest()
//    {
//        float z = Random.Range(-walkrange, walkrange);
//        float x = Random.Range(-walkrange, walkrange);

//        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

//        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
//        {
//            walkpointSet = true;
//        }
//    }

//    void Chase()
//    {
//        agent.SetDestination(playerObject.transform.position);
//    }
//    // Disegna un gizmo visivo per mostrare il raggio di rilevamento quando l'oggetto è selezionato
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, detectionRadius);
//    }

//    public void TakeDamage(float damageAmount)
//    {
//        animator.Play("Damage");

//        currentHealth -= damageAmount; // Sottrai i danni dalla salute attuale
//        healthBar.UpdateHealthBar(currentHealth, maxHealth);
//        // Controlla se la salute è inferiore o uguale a zero
//        if (currentHealth <= 0)
//        {
//            Die(); // Se sì, il personaggio muore
//        }
//    }

//    public void DamageAnimFinished()
//    {
//        if (isAttacking) Attack();
//    }

//    private void Die()
//    {
//        // Opzionale: Aggiungi qui effetti visivi o sonori per la morte del personaggio
//        gameObject.GetComponent<BoxCollider>().enabled = false;
//        animator.SetBool("Die", true);
//    }

//    IEnumerator DestroyWolfBodyAfterDelay()
//    {
//        yield return new WaitForSeconds(4f);
//        Destroy(gameObject);
//    }

//    private void DieAnimationFinished()
//    {
//        //Debug.Log("DIE ANIMATION FINISHED !!! PLAYER GET SCORE POINTS");
//        GameController.Instance.AddScore(50);
//        StartCoroutine(DestroyWolfBodyAfterDelay());
//    }

//    private void Attack()
//    {
//        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
//        {
//            animator.SetTrigger("Attack");

//            // Calcola la posizione obiettivo leggermente spostata rispetto al giocatore
//            //Vector3 targetPosition = transform.position + transform.forward * 1.2f;

//            // Imposta la destinazione dell'agente alla posizione obiettivo
//            agent.SetDestination(transform.position);

//            // Imposta il flag di attacco su true
//            isAttacking = true;
//        }
//    }

//    // Metodo chiamato dall'animazione dell'attacco al termine
//    public void AttackAnimationFinished()
//    {
//        isAttacking = false;
//    }
//    void EnableAttack()
//    {
//        boxColliderTesta.enabled = true;
//    }

//    void DisableAttack()
//    {
//        boxColliderTesta.enabled = false;
//    }

//    //private void OnTriggerEnter(Collider other)
//    //{
//    //    // Verifica se l'oggetto in collisione ha un tag "Player"
//    //    if (other.gameObject.CompareTag("Player"))
//    //    {
//    //        Debug.Log("Hit");
//    //    }
//    //}
//}

