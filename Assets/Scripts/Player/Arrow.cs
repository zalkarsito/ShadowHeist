using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
public enum ArrowType
{
    Normal,
    Explosive
}
public class Arrow : MonoBehaviour
{
    public ArrowType type;
    private Rigidbody rb;
    private int arrowDamage = 25;

    private bool isStuck = false; // Flag to track if the arrow is stuck
    private GameObject playerBody;

    [Header("Explosive Arrow")]
    [SerializeField] private float radius = 5;
    [SerializeField] private float explosionForce = 70;
    public GameObject explosionEffect;
    private bool exploded = false;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip explosion;
    public AudioClip ArrowHitSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 30f); // Destroy the arrow after 5 seconds if it doesn't hit anything
        playerBody = GameObject.FindGameObjectWithTag("Player");
        Collider arrowCollider = GetComponent<Collider>();
        Collider bodyCollider = playerBody.transform.Find("Body").GetComponent<Collider>();
        Collider playerCollider = playerBody.GetComponent<Collider>();

        if (arrowCollider != null && bodyCollider != null)
        {
            Physics.IgnoreCollision(arrowCollider, playerCollider);
            Physics.IgnoreCollision(arrowCollider, bodyCollider);
        }
    }

    // This method is called when the arrow hits a collider
    private void OnCollisionEnter(Collision collision)
    {
        if (type == ArrowType.Normal)
        {
            if (collision.gameObject.CompareTag("Enemy")) //mata al enemigo
            {
                Debug.Log("hit");
                audioSource.PlayOneShot(ArrowHitSound); //sound
                collision.gameObject.GetComponent<AI>().LooseLife(1);
                Destroy(gameObject);
            }
            // Check if the arrow is not already stuck
            if (!isStuck && !collision.transform.CompareTag("Player")) //hace que no se choque con el gugador
            {
                isStuck = true; // Mark the arrow as stuck

                // Stop the movement by setting the Rigidbody's velocity and angular velocity to zero
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Optionally, freeze the Rigidbody's movement and rotation completely
                rb.isKinematic = true;

                Debug.Log("Arrow stuck! :" + collision.transform.name);
            }
        }

        if (type == ArrowType.Explosive)
        {
            
            
            if (exploded == false)
             {
                AudioSource.PlayClipAtPoint(explosion, transform.position);

                Exploded();
                exploded = true;
             }
            
        }
        

        
    }

    private void Exploded()
    {
        


        Debug.Log("Exploded() fue llamado");
        //Para ver con que objetos a colisionado el radio de la esfera
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        Debug.Log(" Objetos en el radio: " + colliders.Length);
        foreach (var rangeObjects  in colliders)
        {
            Debug.Log(" Detectado: " + rangeObjects.name);

            AI ai = rangeObjects.GetComponent<AI>();
            if (ai != null)
            {
                ai.ExplosionImpact(); //mata al enemigo
            }
            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>(); //tomamos todo los rigidbody de el rango
            if (rb != null)
            {
                Debug.Log(" Tiene Rigidbody: " + rangeObjects.name);
                rb.AddExplosionForce(explosionForce * 10, transform.position, radius); //addexplosionfprce ya nos calcula la explosion copn los datos que nosotros le damos
            }
        }
        
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
