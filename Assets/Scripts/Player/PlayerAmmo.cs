using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{

    public Transform startPosition;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("explosiveAmmo"))
        {
            GameManager.instance.explosiveAmmo += other.gameObject.GetComponent<AmmoBox>().ammo;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Botiquin"))
        {
            GameManager.instance.HealthGain(other.gameObject.GetComponent<Botiquin>().health);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("DeathFloor"))
        {
            Debug.Log("Toco muerte");
            GameManager.instance.LoseHealth(50);

            GetComponent<CharacterController>().enabled = false; //se hace esto porque el character controller tiene unos lios con la posicion, entonces se desactiva para facilitar el teletransporte 
            gameObject.transform.position = startPosition.position;
            GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.instance.LoseHealth(10);
        }
    }
}
