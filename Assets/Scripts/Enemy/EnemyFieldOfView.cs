using UnityEngine;
using System;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    //public GameObject fovVisualInstance; // Istanza del FOV visivo
    //private GameObject circleVisual;
    private MeshRenderer meshRenderer;
    private Color initialColor = new Color(1f, 1f, 1f, 0.1f); // Giallo semitrasparente
    private Color finalColor = Color.red;
    private float fillDuration = 0.3f; // Tempo per il riempimento del FOV
    private float fillTimer = 0f; // Timer per il riempimento del FOV
    private bool filling = false; // Indica se il FOV è in fase di riempimento
    public bool playerSeen = false;
    private bool playerNear = false;
    public bool PlayerNear { get { return playerNear; } }

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public event Action OnPlayerSeen;
    private Color currentColor;

    public CapsuleCollider spotCollider;
    public CapsuleCollider spotCollider2;

    public GameObject CurrentWolf;
    private PlayerController playerController;

    //private float tempoTrascorso;
    SpotController spotController;
    private bool isPlayerVisible = false;
    public bool IsPlayerVisible { get { return isPlayerVisible; } }

    private void Awake()
    {
        spotController = GetComponentInChildren<SpotController>();
    }
    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        //    // Crea e istanzia il FOV visivo utilizzando una mesh a forma di cuneo
        //    if (fovVisualInstance == null)
        //        fovVisualInstance = CreateWedgeVisual(angle, radius);
        //    // Crea e istanzia il cerchio visivo intorno al personaggio
        //    circleVisual = CreateCircleVisual(spotCollider.radius);
        //meshRenderer = fovVisualInstance.GetComponent<MeshRenderer>();
        //meshRenderer.material.color = initialColor;
        //currentColor = initialColor;
    }

    private void Update()
    {
        //// Aggiorna la posizione e la rotazione del FOV visivo
        //UpdateFovVisualTransform();

        // Controlla il FOV per vedere se il giocatore è visibile
        FieldOfViewCheck();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2 && !playerController.isInBushes)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, Vector3.Distance(transform.position, target.position), obstructionMask))
                    isPlayerVisible = true;
                else isPlayerVisible = false;
            }
            else isPlayerVisible = false;
        }
        else isPlayerVisible = false;

        if (isPlayerVisible || playerNear)
        {
            if (!filling)
            {
                fillTimer += Time.deltaTime;
                if (fillTimer >= fillDuration)
                {
                    fillTimer = fillDuration;
                    filling = true;
                }
            }

            float fillAmount = fillTimer / fillDuration;
            //Color lerpedColor = Color.Lerp(initialColor, finalColor, fillAmount);
            //meshRenderer.material.color = lerpedColor;
            //currentColor = lerpedColor;

            if (filling && fillAmount >= 1f)
            {
                if (!playerSeen) AudioManager.Instance.PlaySoundWolfFXClip("Wolfbark", transform); //if (!playerSeen) AudioManager.Instance.PlayWolfSFX("Wolfbark");
                playerSeen = true;
                OnPlayerSeen?.Invoke();
                //fovVisualInstance.SetActive(false);
            }
        }
        else
        {
            if (filling)
            {
                fillTimer -= Time.deltaTime;
                if (fillTimer <= fillDuration)
                {
                    fillTimer = fillDuration;
                    filling = false;
                }
            }
            //// Riempi gradualmente il colore attuale del FOV con il colore iniziale
            //currentColor = Color.Lerp(currentColor, initialColor, Time.deltaTime * fillSpeed);
            //meshRenderer.material.color = currentColor;

            fillTimer = 0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("PLAYER IN THE NEXT CONTROL !!!");
            if (other.GetComponent<PlayerController>().isInBushes)
            {
                playerNear = false;
            }
            else
                playerNear = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().isInBushes)
            {
                playerNear = false;
            }
            else
                playerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerNear = false;
    }
    //private GameObject CreateWedgeVisual(float angle, float radius)
    //{
    //    // Creazione della mesh a forma di cuneo
    //    Mesh mesh = new Mesh();

    //    int numTriangles = 8;
    //    int numVertices = numTriangles * 3;

    //    Vector3[] vertices = new Vector3[numVertices];

    //    // Angolo tra ogni triangolo
    //    float angleBetweenTriangles = angle / numTriangles;

    //    // Punto centrale del cuneo
    //    Vector3 centerPoint = Vector3.zero;

    //    // Aggiungi i vertici del cuneo
    //    for (int i = 0; i < numTriangles; i++)
    //    {
    //        // Vertice centrale
    //        vertices[i * 3] = centerPoint;

    //        // Vertici per il bordo del cuneo
    //        float angleOffset1 = angleBetweenTriangles * i;
    //        float angleOffset2 = angleBetweenTriangles * (i + 1);
    //        vertices[i * 3 + 1] = Quaternion.Euler(0, -angle / 2 + angleOffset1, 0) * Vector3.forward * radius;
    //        vertices[i * 3 + 2] = Quaternion.Euler(0, -angle / 2 + angleOffset2, 0) * Vector3.forward * radius;
    //    }

    //    // Imposta gli indici
    //    int[] indices = new int[numVertices];
    //    for (int i = 0; i < numVertices; i++)
    //    {
    //        indices[i] = i;
    //    }

    //    // Assegna i vertici e gli indici alla mesh
    //    mesh.vertices = vertices;
    //    mesh.triangles = indices;

    //    // Normals
    //    Vector3[] normals = new Vector3[numVertices];
    //    for (int i = 0; i < numVertices; i++)
    //    {
    //        normals[i] = Vector3.up;
    //    }
    //    mesh.normals = normals;

    //    // Crea il GameObject per il FOV visivo
    //    GameObject visualObject = new GameObject("FOVVisual");
    //    visualObject.transform.position = transform.position;
    //    visualObject.transform.rotation = transform.rotation;
    //    visualObject.tag = "FOV";
    //    visualObject.transform.SetParent(CurrentWolf.transform);


    //    // Aggiungi il componente MeshFilter e MeshRenderer al GameObject
    //    MeshFilter meshFilter = visualObject.AddComponent<MeshFilter>();
    //    MeshRenderer meshRenderer = visualObject.AddComponent<MeshRenderer>();

    //    // Creazione del materiale semitrasparente
    //    Material material = new Material(Shader.Find("Standard"));
    //    material.color = initialColor; // Imposta il colore con un valore alfa di 0.5 per renderlo semitrasparente
    //    material.SetFloat("_Mode", 2); // Imposta il rendering mode su Transparent
    //    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //    material.SetInt("_ZWrite", 0);
    //    material.DisableKeyword("_ALPHATEST_ON");
    //    material.EnableKeyword("_ALPHABLEND_ON");
    //    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

    //    // Assegna il materiale alla mesh renderer
    //    meshRenderer.material = material;

    //    // Assegna la mesh creata al MeshFilter
    //    meshFilter.mesh = mesh;

    //    // Restituisci il GameObject del FOV visivo
    //    return visualObject;
    //}

    //private GameObject CreateCircleVisual(float radius)
    //{
    //    // Numero di segmenti per il cerchio
    //    int numSegments = 32;

    //    // Creazione della mesh per il cerchio
    //    Mesh circleMesh = new Mesh();
    //    Vector3[] vertices = new Vector3[numSegments + 1];
    //    int[] triangles = new int[numSegments * 3];

    //    // Calcola i vertici del cerchio
    //    for (int i = 0; i <= numSegments; i++)
    //    {
    //        float angle = Mathf.Deg2Rad * (360f / numSegments) * i;
    //        vertices[i] = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
    //    }

    //    // Assegna gli indici dei triangoli
    //    for (int i = 0; i < numSegments; i++)
    //    {
    //        triangles[i * 3] = 0;
    //        triangles[i * 3 + 1] = i + 1;
    //        triangles[i * 3 + 2] = i + 2;
    //    }

    //    // L'ultimo triangolo deve collegare l'ultimo vertice al primo
    //    triangles[numSegments * 3 - 3] = 0;
    //    triangles[numSegments * 3 - 2] = numSegments;
    //    triangles[numSegments * 3 - 1] = 1;

    //    // Assegna i vertici e i triangoli alla mesh
    //    circleMesh.vertices = vertices;
    //    circleMesh.triangles = triangles;

    //    // Crea il GameObject per il cerchio visivo
    //    GameObject circleObject = new GameObject("CircleVisual");
    //    circleObject.transform.position = transform.position;
    //    circleObject.transform.rotation = transform.rotation;
    //    circleObject.transform.SetParent(CurrentWolf.transform);

    //    // Aggiungi il componente MeshFilter e MeshRenderer al GameObject
    //    MeshFilter meshFilter = circleObject.AddComponent<MeshFilter>();
    //    MeshRenderer meshRenderer = circleObject.AddComponent<MeshRenderer>();

    //    // Creazione del materiale semitrasparente
    //    Material material = new Material(Shader.Find("Standard"));
    //    material.color = initialColor; // Imposta il colore con un valore alfa di 0.5 per renderlo semitrasparente
    //    material.SetFloat("_Mode", 2); // Imposta il rendering mode su Transparent
    //    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //    material.SetInt("_ZWrite", 0);
    //    material.DisableKeyword("_ALPHATEST_ON");
    //    material.EnableKeyword("_ALPHABLEND_ON");
    //    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

    //    // Assegna il materiale alla mesh renderer
    //    meshRenderer.material = material;

    //    // Assegna la mesh creata al MeshFilter
    //    meshFilter.mesh = circleMesh;

    //    // Restituisci il GameObject del cerchio visivo
    //    return circleObject;
    //}


    //private void UpdateFovVisualTransform()
    //{
    //    // Assicura che il FOV visivo segua l'Enemy nella posizione e nella rotazione
    //    if (fovVisualInstance != null)
    //    {
    //        fovVisualInstance.transform.position = transform.position;
    //        fovVisualInstance.transform.rotation = transform.rotation;
    //    }
    //    circleVisual.transform.position = transform.position;
    //    circleVisual.transform.rotation = transform.rotation;
    //}


}
