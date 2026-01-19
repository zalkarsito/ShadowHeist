using UnityEngine;

public class WIN : MonoBehaviour
{
    public GameObject win;
    private AI ai;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ai.lifes <= 0)
        {
            win.SetActive(true);
        }
    }
}
