using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VecchiCodici : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //private void Update()
    //{
    //    if (canSeePlayer)
    //    {
    //        timeLastSeen = Time.time; // Memorizza il tempo attuale quando il giocatore è stato visto
    //        timeSinceLastSeen = 0f; // Resettiamo il tempo trascorso dall'ultimo avvistamento
    //    }
    //    else if (timeLastSeen > 0f) // Se canSeePlayer è stato vero almeno una volta e ora non lo è più
    //    {
    //        timeSinceLastSeen += Time.deltaTime; // Conta il tempo trascorso dall'ultimo avvistamento

    //        if (timeSinceLastSeen < 4f) // Se il tempo trascorso è minore di 4 secondi
    //        {
    //            playerSeen = true; // Imposta playerSeen a true
    //        }
    //        else
    //        {
    //            playerSeen = false; // Altrimenti, reimposta playerSeen a false
    //            timeLastSeen = 0f; // Resetta il tempo dell'ultimo avvistamento
    //        }
    //    }
    //}
    //    using System.Collections;
    //using System.Net.Http.Headers;
    //using UnityEngine;
    //using UnityEngine.AI;

    //public class Enemy : MonoBehaviour
    //{
    //    public GameObject playerObject; // Oggetto del giocatore
    //    NavMeshAgent agent;
    //    [SerializeField] LayerMask groundLayer;

    //    Animator animator;
    //    public BoxCollider boxColliderTesta;
    //    bool isAttacking = false;

    //    //patrol
    //    Vector3 destPoint;
    //    bool walkpointSet = false;
    //    [SerializeField] float walkrange;
    //    public float runSpeed = 8f; // Nuova velocità desiderata per l'agente
    //    private float walkSpeed; //Velocità iniziale
    //    //attack

    //    //state change
    //    //float detectionRadius = 10f; // Raggio di rilevamento del giocatore
    //    float attackRange = 1f; //Range di attacco del nemico

    //    //Field of view
    //    public float radius;
    //    [Range(0, 360)]
    //    public float angle;
    //    //public float height = 1.0f;
    //    //public Color meshColor = Color.red;
    //    private GameObject fovVisualInstance; // Istanza del FOV visivo

    //    public LayerMask targetMask;
    //    public LayerMask obstructionMask;

    //    public bool canSeePlayer;
    //    private float timeSinceLastSeen = 0f;
    //    private float timeLastSeen = 0f;
    //    private bool playerSeen = false;// Memorizza l'ultimo tempo in cui il giocatore è stato visto

    //    private void Start()
    //    {
    //        agent = GetComponent<NavMeshAgent>();
    //        playerObject = GameObject.FindGameObjectWithTag("Player"); // Trova l'oggetto del giocatore
    //        animator = GetComponent<Animator>();
    //        StartCoroutine(FOVRoutine());
    //        StartCoroutine(EnemyRoutine());
    //        walkSpeed = agent.speed;

    //        // Crea e istanzia il FOV visivo utilizzando una mesh a forma di cuneo
    //        fovVisualInstance = CreateWedgeVisual();
    //    }

    //    private void Update()
    //    {
    //        // Aggiorna la posizione e la rotazione del FOV visivo
    //        UpdateFovVisualTransform();
    //    }

    //    private IEnumerator EnemyRoutine()
    //    {
    //        float delay = 0.2f;
    //        WaitForSeconds wait = new WaitForSeconds(delay);

    //        while (true)
    //        {
    //            yield return wait;
    //            EnemyRoute();
    //        }
    //    }

    //    private void EnemyRoute()
    //    {
    //        if (playerObject == null)
    //            return;

    //        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

    //        // Controlla se il giocatore � nel raggio di rilevamento
    //        if (playerSeen)
    //        {
    //            // Se il giocatore � anche nell'intervallo di attacco, attacca senza inseguirlo
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
    //    private IEnumerator FOVRoutine()
    //    {
    //        float delay = 0.2f;
    //        WaitForSeconds wait = new WaitForSeconds(delay);

    //        while (true)
    //        {
    //            yield return wait;
    //            FieldOfViewCheck();
    //        }
    //    }

    //    private void FieldOfViewCheck()
    //    {
    //        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

    //        if (rangeChecks.Length != 0)
    //        {
    //            Transform target = rangeChecks[0].transform;
    //            Vector3 directionToTarget = (target.position - transform.position).normalized;

    //            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
    //            {
    //                float distanceToTarget = Vector3.Distance(transform.position, target.position);
    //                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
    //                {
    //                    canSeePlayer = true;
    //                    playerSeen = true;
    //                }
    //                else
    //                    canSeePlayer = false;
    //            }
    //            else
    //                canSeePlayer = false;
    //        }
    //        else if (canSeePlayer)
    //            canSeePlayer = false;
    //    }
    //    void Patrol()
    //    {
    //        agent.speed = walkSpeed;
    //        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
    //        {
    //            animator.SetTrigger("Walk");
    //        }
    //        // Se l'Enemy non può vedere il giocatore, mantieni il colore predefinito del FOV visivo
    //        Renderer fovRenderer = fovVisualInstance.GetComponent<Renderer>();
    //        if (fovRenderer != null)
    //        {
    //            fovRenderer.material.color = Color.yellow; // Mantieni il colore bianco quando non vede il giocatore
    //        }

    //        // Se non abbiamo un punto di destinazione attuale, cerchiamo uno
    //        if (!walkpointSet)
    //        {
    //            SearchForDest();
    //        }

    //        // Imposta la destinazione dell'agente alla posizione obiettivo se il punto di destinazione è stato trovato
    //        if (walkpointSet)
    //        {
    //            // Se l'agente non è in movimento, imposta la destinazione
    //            if (!agent.pathPending && !agent.hasPath)
    //            {
    //                agent.SetDestination(destPoint);
    //            }

    //            // Controlla se l'agente ha raggiunto il punto di destinazione
    //            if (!agent.pathPending && agent.remainingDistance < 0.1f)
    //            {
    //                // Resetta il flag per cercare una nuova destinazione
    //                walkpointSet = false;
    //            }
    //        }
    //    }



    //    void SearchForDest()
    //    {
    //        // Calcolare la posizione del prossimo punto di destinazione basato sulla direzione del movimento

    //        Vector3 backwardPoint = transform.position - transform.forward * walkrange;
    //        destPoint = backwardPoint;

    //        walkpointSet = true; // Assumiamo che il punto sia sempre raggiungibile su NavMesh con questa logica
    //    }



    //void Chase()
    //{
    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
    //    {
    //        animator.SetTrigger("Run");
    //    }
    //    //Se vede il player disattiva il fov
    //    Renderer fovRenderer = fovVisualInstance.GetComponent<Renderer>();
    //    fovVisualInstance.SetActive(false);
    //    // Imposta la destinazione dell'agente alla posizione obiettivo
    //    agent.SetDestination(playerObject.transform.position);
    //    agent.speed = runSpeed;
    //}
    // Disegna un gizmo visivo per mostrare il raggio di rilevamento quando l'oggetto � selezionato
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}


    //    public void DamageAnimFinished()
    //    {
    //        if (isAttacking) Attack();
    //    }

    //private void Attack()
    //{
    //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
    //    {
    //        animator.SetTrigger("Attack");

    //        // Calcola la posizione obiettivo leggermente spostata rispetto al giocatore
    //        //Vector3 targetPosition = transform.position + transform.forward * 1.2f;

    //        // Imposta la destinazione dell'agente alla posizione obiettivo
    //        agent.SetDestination(transform.position);

    //        // Imposta il flag di attacco su true
    //        isAttacking = true;
    //    }
    //}

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

    //    //Mesh CreateWedgeMesh()
    //    //{
    //    //    Mesh mesh = new Mesh();

    //    //    int numTriangles = 8;
    //    //    int numVertices = numTriangles * 3;

    //    //    Vector3[] vertices = new Vector3[numVertices];

    //    //}
    //    private GameObject CreateWedgeVisual()
    //    {
    //        // Creazione della mesh a forma di cuneo
    //        Mesh mesh = new Mesh();

    //        int numTriangles = 8;
    //        int numVertices = numTriangles * 3;

    //        Vector3[] vertices = new Vector3[numVertices];

    //        // Angolo tra ogni triangolo
    //        float angleBetweenTriangles = angle / numTriangles;

    //        // Punto centrale del cuneo
    //        Vector3 centerPoint = Vector3.zero;

    //        // Aggiungi i vertici del cuneo
    //        for (int i = 0; i < numTriangles; i++)
    //        {
    //            // Vertice centrale
    //            vertices[i * 3] = centerPoint;

    //            // Vertici per il bordo del cuneo
    //            float angleOffset1 = angleBetweenTriangles * i;
    //            float angleOffset2 = angleBetweenTriangles * (i + 1);
    //            vertices[i * 3 + 1] = Quaternion.Euler(0, -angle / 2 + angleOffset1, 0) * Vector3.forward * radius;
    //            vertices[i * 3 + 2] = Quaternion.Euler(0, -angle / 2 + angleOffset2, 0) * Vector3.forward * radius;
    //        }

    //        // Imposta gli indici
    //        int[] indices = new int[numVertices];
    //        for (int i = 0; i < numVertices; i++)
    //        {
    //            indices[i] = i;
    //        }

    //        // Assegna i vertici e gli indici alla mesh
    //        mesh.vertices = vertices;
    //        mesh.triangles = indices;

    //        // Normals
    //        Vector3[] normals = new Vector3[numVertices];
    //        for (int i = 0; i < numVertices; i++)
    //        {
    //            normals[i] = Vector3.up;
    //        }
    //        mesh.normals = normals;

    //        // Crea il GameObject per il FOV visivo
    //        GameObject visualObject = new GameObject("FOVVisual");
    //        visualObject.transform.position = transform.position;
    //        visualObject.transform.rotation = transform.rotation;

    //        // Aggiungi il componente MeshFilter e MeshRenderer al GameObject
    //        MeshFilter meshFilter = visualObject.AddComponent<MeshFilter>();
    //        MeshRenderer meshRenderer = visualObject.AddComponent<MeshRenderer>();

    //        // Assegna la mesh creata al MeshFilter
    //        meshFilter.mesh = mesh;

    //        // Assegna un materiale al MeshRenderer (puoi personalizzare questo materiale come preferisci)
    //        meshRenderer.material = new Material(Shader.Find("Standard"));

    //        // Restituisci il GameObject del FOV visivo
    //        return visualObject;
    //    }
    //    private void UpdateFovVisualTransform()
    //    {
    //        // Assicura che il FOV visivo segua l'Enemy nella posizione e nella rotazione
    //        if (fovVisualInstance != null)
    //        {
    //            fovVisualInstance.transform.position = transform.position;
    //            fovVisualInstance.transform.rotation = transform.rotation;
    //        }
    //    }
    //}
}
