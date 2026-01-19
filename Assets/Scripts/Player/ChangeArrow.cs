using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeArrow : MonoBehaviour
{

    public GameObject[] arrow;

    public int selectedArrow = 0;

    public string arrowPrefabPath = "Arrow";
    public string explosivePrefabPath = "Explosive";

    private GameObject arrowPrefab;

    private Arrow arrowScript;
    void Start()
    {
        arrowPrefab = Resources.Load<GameObject>(arrowPrefabPath);
        arrowScript = arrowPrefab.GetComponent<Arrow>();
        SelectArrow();
        ActualizeArrow();
    }

    
    void Update()
    {
        int preiousArrow = selectedArrow;
        

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedArrow >= arrow.Length - 1)
            {
                selectedArrow = 0;
            }
            else
            {
                selectedArrow++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedArrow <= 0)
            {
                selectedArrow = arrow.Length - 1;
            }
            else
            {
                selectedArrow--;
            }
        }

        if (preiousArrow != selectedArrow)
        {
            SelectArrow();
            ActualizeArrow();
        }

    }

    void ActualizeArrow()
    {
        if (selectedArrow == 0)
        {
            Debug.Log("Flecha normal seleccionada");
            arrowScript.type = ArrowType.Normal;
            Debug.Log(arrowScript.type);
             
        }
        else if (selectedArrow == 1)
        {
            Debug.Log("Flecha explosiva seleccionada");
            arrowScript.type = ArrowType.Explosive;
            Debug.Log(arrowScript.type);
        }
    }

    void SelectArrow()
    {
        int i = 0;
        foreach( Transform arrow in transform)
        {
            if (arrow.gameObject.layer == LayerMask.NameToLayer("Ammo"))
            {
                if ( i == selectedArrow)
                {
                    arrow.gameObject.SetActive(true);
                }
                else
                {
                    arrow.gameObject.SetActive(false);
                }
                i++;
            }
        }



    }
}
