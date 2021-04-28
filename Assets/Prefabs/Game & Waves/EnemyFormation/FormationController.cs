using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5;
    public bool moveLeftRight = true;
    public bool moveBotTop = false;
    public float delayBetweenEnemySpawn = 0.5f;
    public bool instantSpawnAtStart = false;
    public bool multipleEnemySpawn = false;
    public float multipleEnemyNumber = 3;
    public bool endlessSpawn = false;
    public bool instantSpawnEndless = false;

    private bool movingRight = true;
    private bool movingTop = true;
    private PlayerController playerController;
    private float xmin;
    private float xmax;

    private bool spawnInProgress = false;
    private bool startSpawnEnded = false;
    private int childIndex = 0;
    private int childIndexContinue = 0;
    // private bool indexIncrementEN = false;

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Use this for initialization
    void Start()
    {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xmin = leftEdge.x;
        xmax = rightEdge.x;

        playerController = FindObjectOfType<PlayerController>();

        if (instantSpawnAtStart)
        {
            SpawnEnemies();
        }
        else
        {
            SpawnUntilFull();
        }
    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeftRight)
        {
            MoveLeftRight();
        }
        
        if (moveBotTop)
        {
            MoveBotTop();
        }

        // Check if the formation is going outside the playspace...
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xmin)
        {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xmax)
        {
            movingRight = false;
        }

        // Check if all enemies are dead.
        if(AllMembersDead())
        {
            if (!endlessSpawn)
            {
                WaveController.formationCleared++;
                Object.Destroy(gameObject);
            }
            else
            {
                if (instantSpawnEndless)
                {
                    SpawnEnemies();
                }
                else
                {
                    SpawnUntilFull();
                }
            }
        }
        if (multipleEnemySpawn && startSpawnEnded)
        {
            print("Entree dans condition multiple");
            SpawnContinue();
        }

        // If the player is defeated, destroy the formation
        if(playerController.Health <= 0)
        {
            Invoke("DestroyObject", 1.95f);
        }
    }

    bool AllMembersDead()
    {
        foreach (Transform position in transform)
        {
            if ((position.childCount > 0) || spawnInProgress)
            {
                return false;
            }
        }
        childIndex = 0;
        return true;
    }

    Transform NextFreePosition()
    {
        int positionIndex = 0;

        // indexIncrementEN = !indexIncrementEN;        Variable à utiliser si on appelle la fonction pour des vérifications 1 fois sur 2, ainsi on active l'index uniquement 1 fois sur 2

        foreach (Transform position in transform)   // Pour chaque child de la formation, on vérifie si le child est vide. Si il est vide la position est libre.
        {
            if (position.childCount == 0)
            {
                if (positionIndex >= childIndex)
                {
                    // if (indexIncrementEN)
                    // {                                               
                        childIndex++;
                    // }                                             
                    return position;
                }
            }
            positionIndex++;
        }
        return null;
    }

    Transform NextFreePositionContinue()
    {
        int positionIndex = 0;

        // indexIncrementEN = !indexIncrementEN;        Variable à utiliser si on appelle la fonction pour des vérifications 1 fois sur 2, ainsi on active l'index uniquement 1 fois sur 2

        foreach (Transform position in transform)   // Pour chaque child de la formation, on vérifie si le child est vide. Si il est vide la position est libre.
        {
            if (position.childCount == 0)
            {
                if (positionIndex <= multipleEnemyNumber)
                {
                    // if (indexIncrementEN)
                    // {                                               
                    childIndexContinue++;
                    // }                                             
                    return position;
                }
            }
            positionIndex++;
        }
        return null;
    }

    void SpawnEnemies()
    {
        foreach (Transform child in transform) // Pour chaque child de la formation, on instantie un ennemi et on lui donne comme parent la position.
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;

        }
        startSpawnEnded = true;
        print("FinSpawn " + startSpawnEnded);
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition(); // On crée une position et on récupère le "transform" d'une position libre pour y faire spawn les ennemis.

        if (freePosition)
        {
            // If the first ennemy is destroyed and AllMembersDead is looping, he will return true so spawnInProgress help for that case.
            spawnInProgress = true;

            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            Invoke("SpawnUntilFull", delayBetweenEnemySpawn);
        }
        else
        {
            spawnInProgress = false;
            startSpawnEnded = true;
            print("FinSpawn " + startSpawnEnded);
        }
    }

    void SpawnContinue()
    {
        Transform freePosition = NextFreePositionContinue(); // On crée une position et on récupère le "transform" d'une position libre pour y faire spawn les ennemis.

        if (freePosition && (multipleEnemyNumber > 0))
        {
            // If the first ennemy is destroyed and AllMembersDead is looping, he will return true so spawnInProgress help for that case.
            spawnInProgress = true;

            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            Invoke("SpawnContinue", delayBetweenEnemySpawn);

            multipleEnemyNumber--;
        }
        else
        {
            spawnInProgress = false;
        }
    }


    void MoveLeftRight()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    void MoveBotTop()
    {
        if (movingTop)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }
}
 