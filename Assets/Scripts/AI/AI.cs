using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform[] destinations;

    private int destinPoint = 0;

    public float distanceToFollowPath = 2;

    [Header("FollowPlayer")]
    private GameObject player;

    public bool followPlayer;
    private float distanceToPlayer;
    public float distanceToFollow = 5;

    public int lifes = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (destinations == null || destinations.Length == 0)
        {
            transform.gameObject.GetComponent<AI>().enabled = false;
        }
        else
        {
            agent.destination = destinations[0].transform.position;

            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= distanceToFollow)
        {
            FollowPlayer();
        }

        else
        {
            EnemyPath();
        }

    }

    public void EnemyPath()
    {
        agent.destination = destinations[destinPoint].position;

        if (Vector3.Distance(transform.position, destinations[destinPoint].position)<= distanceToFollowPath)
        {
            if (destinations[destinPoint] != destinations[destinations.Length - 1])
            {
                destinPoint++;
            }
            else
            {
                destinPoint = 0;
            }
        }
    }

    public void FollowPlayer()
    {
        agent.destination = player.transform.position;
    }

    public void ExplosionImpact()
    {
        Destroy(gameObject);
    }

    public void LooseLife(int lifesToLoose)
    {
        lifes = lifes - lifesToLoose;

        if (lifes <= 0)
        {
            Destroy(gameObject );
        }
    }
}
