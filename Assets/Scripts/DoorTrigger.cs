using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController controller;
    private void OnTriggerEnter(Collider other)
    {
        controller.ObjectEntered(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        controller.ObjectExited(other.gameObject);
    }
    private void Start()
    {
        controller = transform.parent.gameObject.GetComponent<DoorController>();
    }
    
}
