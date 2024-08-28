using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public GameObject playerObject; // Oggetto del giocatore
    public BoxCollider CorpoLupo;
    private NavMeshAgent agent;
    private Animator animator;
    public BoxCollider boxColliderTesta;
    private bool isAttacking = false;

    private EnemyFieldOfView fieldOfView; // Riferimento allo script EnemyFieldOfView

    //patrol
    private Vector3 destPoint;
    private bool walkpointSet = false;
    [SerializeField] private float walkRange;
    public float runSpeed = 8f; // Nuova velocità desiderata per l'agente
    private float walkSpeed; //Velocità iniziale

    //state change
    private float attackRange = 1f; //Range di attacco del nemico
    //private bool playerInAttackRange = false;

    // Dichiarazione del delegato per la notifica del giocatore visto
    public delegate void PlayerSeenDelegate();
    public PlayerSeenDelegate OnPlayerSeen;

    // Variabile per la durata casuale della pausa
    private bool hasIdled = false;
    //private bool isIdle = false;
    // Dichiarazione della variabile booleana per tenere traccia dello stato di "sit"
    private bool isSitting = false;
    public GameObject puntoEsclamativo;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool resetting = false;
    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        agent = GetComponent<NavMeshAgent>();
        playerObject = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
        animator = GetComponent<Animator>();
        walkSpeed = agent.speed;
        // Ottenere il riferimento allo script EnemyFieldOfView
        fieldOfView = GetComponent<EnemyFieldOfView>();
        // Registra il metodo Chase() per essere chiamato quando il giocatore è visto
        fieldOfView.OnPlayerSeen += Chase;
        // Avvia la coroutine EnemyRouteCoroutine()
        //StartCoroutine(EnemyRouteCoroutine());
        //GameController.OnPlayerCollision += OnPlayerCollision;
    }

    private void Update()
    {
        EnemyRoutine();
    }

    //private IEnumerator EnemyRouteCoroutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.2f); // Esegui la coroutine ogni 0.2 secondi (5 volte al secondo)

    //        EnemyRoutine();
    //    }
    //}

    private void EnemyRoutine()
    {
        if (playerObject == null)
            return;

        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerObject.transform.position);

        // Controlla se il giocatore è visibile
        if (fieldOfView.playerSeen)
        {
            // Se il giocatore è anche nell'intervallo di attacco, attacca senza inseguirlo
            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                Attack();
            }
            if (distanceToPlayer >= attackRange && !isAttacking)
            {
                Chase();
            }
            else agent.SetDestination(agent.transform.position);
        }
        else
        {
            // Altrimenti, esegui la logica per il pattugliamento
            Patrol();
        }
    }

    private void Patrol()
    {
        // Controlla se l'agente è nell'animazione "sit" e se sì, fermalo
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
        {
            agent.velocity = Vector3.zero;
            isSitting = true; // Imposta la variabile booleana su true se è nell'animazione "sit"
        }
        else
        {
            // Se non è nell'animazione "sit", assicurati che la variabile booleana sia impostata su false
            isSitting = false;
        }

        // Se non è nell'animazione "sit", imposta la velocità dell'agente a walkSpeed
        if (!isSitting)
        {
            agent.speed = walkSpeed;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            animator.SetTrigger("Walk");
        }

        // Se non abbiamo un punto di destinazione attuale, cerchiamo uno
        if (!walkpointSet)
        {
            SearchForDest();
        }

        // Imposta la destinazione dell'agente alla posizione obiettivo se il punto di destinazione è stato trovato
        if (walkpointSet)
        {
            // Se l'agente non è in movimento, imposta la destinazione
            if (!agent.pathPending && !agent.hasPath)
            {
                agent.SetDestination(destPoint);
            }

            // Controlla se l'agente ha raggiunto il punto di destinazione
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                if (!hasIdled)
                {
                    // Attiva l'animazione di idle
                    Idle();
                    hasIdled = true;
                    walkpointSet = false;
                }
                //if (resetting)
                //{
                //    transform.rotation = initialRotation;
                //    resetting = false;
                //}
            }
            if (agent.remainingDistance > 0.1f) hasIdled = false;
        }

    }

    private void Idle()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
        {
            AudioManager.Instance.PlaySoundWolfFXClip("WolfTired", transform); //AudioManager.Instance.PlayWolfSFX("WolfTired");
            animator.SetTrigger("Idle");
        }
    }
    // Funzione per riprendere il movimento dopo la pausa
    private void ResumeWalking()
    {
        // Resetta il flag per cercare una nuova destinazione
        walkpointSet = false;
        hasIdled = false; // Resetta anche la variabile hasIdled
        animator.SetTrigger("Walk");
    }
    private void SearchForDest()
    {

        // Calcolare la posizione del prossimo punto di destinazione basato sulla direzione del movimento
        Vector3 backwardPoint = transform.position - transform.forward * walkRange;
        destPoint = backwardPoint;
        walkpointSet = true; // Assumiamo che il punto sia sempre raggiungibile su NavMesh con questa logica
    }
    public void ResetPosition()
    {
        Debug.Log("funziona!");
        if (fieldOfView.playerSeen)
        {
            Debug.Log("funziona comunque!");
            animator.SetTrigger("Walk");
            resetting = true;
            destPoint = initialPosition;
            //transform.rotation = initialRotation;
            agent.SetDestination(destPoint);
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            walkpointSet = true;

            fieldOfView.playerSeen = false;
        }
    }
    private void Chase()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            animator.SetTrigger("Run");
        }
        // Imposta la destinazione dell'agente alla posizione del giocatore
        agent.SetDestination(playerObject.transform.position);
        agent.speed = runSpeed;
    }

    private void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            //Debug.Log("WOLF ATTACK");
            animator.SetTrigger("Attack");
            AudioManager.Instance.PlaySoundWolfFXClip("Attacco_morso", transform);  //AudioManager.Instance.PlayWolfSFX("Attacco_morso");
        }
        // Imposta la destinazione dell'agente alla posizione obiettivo
        agent.SetDestination(agent.transform.position);
    }

    // Metodo chiamato dall'animazione dell'attacco al termine
    public void AttackAnimationFinished()
    {
        isAttacking = false;
    }
    public void AttackAnimationStarted()
    {
        isAttacking = true;
    }

    private void EnableAttack()
    {
        boxColliderTesta.enabled = true;
    }

    private void DisableAttack()
    {
        boxColliderTesta.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("COLLISIONE CON PLAYER");
            CorpoLupo.enabled = false;
            puntoEsclamativo.SetActive(false);
        }
    }
    private void OnPlayerCollision()
    {
        // Resetta la posizione del nemico o esegui altre azioni necessarie
        ResetPosition();
    }
}
