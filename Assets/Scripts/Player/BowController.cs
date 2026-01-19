using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Animator bowAnimator; // Reference to the Animator
    //public InventorySystem inventory; // Reference to your inventory system
    public string arrowItemName = "Arrow"; // Name of the arrow item in the inventory
    private bool isDrawing = false;

    public string arrowPrefabPath = "Arrow"; // Path in the Resources folder (without file extension)

    private GameObject arrowPrefab; // The loaded arrow prefab
    public Transform spawnPosition;

    public float shootingForce = 100; //The force of the arrow

    private Arrow arrowScript; //to get the arrow script

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip arrowSound;
    public AudioClip stringMissSound;

    private void Start()
    {
        bowAnimator = GetComponent<Animator>();
        LoadArrowPrefab(); //load the arrow before we use it
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.DrawArrow.started += OnDrawStarted;
        inputActions.Player.DrawArrow.canceled += OnDrawCanceled;

        inputActions.Player.ReleaseArrow.performed += OnReleaseArrow;
    }

    private void OnDisable()
    {
        inputActions.Player.DrawArrow.started -= OnDrawStarted;
        inputActions.Player.DrawArrow.canceled -= OnDrawCanceled;

        inputActions.Player.ReleaseArrow.performed -= OnReleaseArrow;

        inputActions.Player.Disable();
    }
    private void LoadArrowPrefab()
    {
        // Load the arrow prefab from the specified path in the Resources folder
        arrowPrefab = Resources.Load<GameObject>(arrowPrefabPath);
        arrowScript = arrowPrefab.GetComponent<Arrow>();

        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow prefab not found at path: " + arrowPrefabPath);
        }
    }

    void Update()
    {
        if (true) // LAter Check if there are arrows in the inventory, true makes infinite arrows posible
        {
            //HandleBowDrawing(); //Check if we have enoughf arrows
        }
        else
        {
            if (isDrawing) CancelDraw();
        }
    }

    private void OnDrawStarted(InputAction.CallbackContext context)
    {
        StartDraw();
        isDrawing = true;
    }

    private void OnDrawCanceled(InputAction.CallbackContext context)
    {
        if (isDrawing)
        {
            CancelDraw();
            isDrawing = false;
            audioSource.PlayOneShot(stringMissSound); //sound
        }
    }

    private void OnReleaseArrow(InputAction.CallbackContext context)
    {
        if (isDrawing)
        {
            ReleaseArrow();
            isDrawing = false;
        }
    }


    private void StartDraw() //start charging arrow animation
    {
        isDrawing = true;
        bowAnimator.SetBool("IsDrawing", true); // Set the Animator parameter
    }

    private void CancelDraw()
    {
        isDrawing = false;
        bowAnimator.SetBool("IsDrawing", false); // Reset the Animator parameter
    }

    private void ReleaseArrow() //we shoot an arrow
    {
        if (!isDrawing) return;

        isDrawing = false;
        bowAnimator.SetBool("IsDrawing", false);

        // Reduce the arrow count in the inventory
        // inventory.RemoveItem(arrowItemName, 1);

        // Call your shooting logic here
        ShootArrow();
    }


    private void ShootArrow() //si es tipo normal se queda igual si es tipo explosive que detecte si tiene balas, RECUERDA QUE YA ESTA EL PREFAB DE ARROW AQUI  
    {
        audioSource.PlayOneShot(arrowSound);//sound
        if (arrowScript.type == ArrowType.Normal)
        {
            Vector3 shootingDirection = CalculateDirection().normalized;

            // Instantiate the bullet
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition.position, Quaternion.identity); //instantiate the prefab of the arrow
            arrow.transform.SetParent(null);

            // Poiting the bullet to face the shooting direction
            arrow.transform.forward = shootingDirection;

            // Shoot the bullet
            arrow.GetComponent<Rigidbody>().AddForce(shootingDirection * shootingForce, ForceMode.Impulse);
        }
        else if (arrowScript.type == ArrowType.Explosive)
        {
            if(GameManager.instance.explosiveAmmo > 0)
            {
                Vector3 shootingDirection = CalculateDirection().normalized;

                // Instantiate the bullet
                GameObject arrow = Instantiate(arrowPrefab, spawnPosition.position, Quaternion.identity); //instantiate the prefab of the arrow
                arrow.transform.SetParent(null);

                // Poiting the bullet to face the shooting direction
                arrow.transform.forward = shootingDirection;

                // Shoot the bullet
                arrow.GetComponent<Rigidbody>().AddForce(shootingDirection * shootingForce, ForceMode.Impulse);

                GameManager.instance.explosiveAmmo--;
            }
            else 
            {
                Debug.Log("No balas explosivas");
            }
        }
        
    }


    public Vector3 CalculateDirection()
    {
        // Shooting from the middle of the screen to check where are we pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // Hitting Something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        // Returning the shooting direction and spread
        return targetPoint - spawnPosition.position;
    }


}