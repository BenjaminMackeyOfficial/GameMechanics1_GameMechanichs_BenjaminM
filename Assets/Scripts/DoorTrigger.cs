using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController controller;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("s");
        controller.ObjectEntered(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("e");
        controller.ObjectExited(other.gameObject);
    }
    private void Start()
    {
        controller = transform.parent.gameObject.GetComponent<DoorController>();
    }
    
}
