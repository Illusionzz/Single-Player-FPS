using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    private float checkRate = 0.05f;
    public float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;
    public Animator animator;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate) {
            lastCheckTime = Time.time;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) {
                if (hit.collider.gameObject != curInteractGameObject) {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                }
            }
        }
    }

    void OpenDoor() 
    {
        if (Input.GetKeyDown(KeyCode.E))
            animator.SetBool("Open Door", true);
    }
}

public interface IInteractable 
{
    string GetInteractPrompt();
    void OnInteract();
}