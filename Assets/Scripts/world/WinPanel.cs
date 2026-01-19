using UnityEngine;

public class WinPanel : MonoBehaviour
{

    public GameObject winPanel;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            winPanel.SetActive(true);
        }
        
    }
}
