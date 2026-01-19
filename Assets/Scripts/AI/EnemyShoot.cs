using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;

    public Transform spawnBulletPoint;

    private Transform playerPosition;

    public float bulletVelocity = 100;
    void Start()
    {
        playerPosition = FindAnyObjectByType<PlayerMovement>().transform; //encontramos la posicion del jugador
        Invoke("ShootPlayer", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShootPlayer()
    {
         Vector3 playerDirection = playerPosition.position - transform.position; //buscar donde esta el jugador

        GameObject newBullet;

        newBullet = Instantiate(enemyBullet, spawnBulletPoint.position, spawnBulletPoint.rotation); //instancia la bala

        newBullet.GetComponent<Rigidbody>().AddForce(playerDirection * bulletVelocity, ForceMode.Force); // añadir impulso a la bala

        Invoke("ShootPlayer", 3);
    }
}
