using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject trigger;
    private GameObject movingPart;

    private List<GameObject> objectsInZone = new List<GameObject>();
    public void ObjectEntered(GameObject obj)
    {
        if(!objectsInZone.Contains(obj)) objectsInZone.Add(obj);
        InZoneAdjust();
    }
    public void ObjectExited(GameObject obj)
    {
        if (objectsInZone.Contains(obj)) objectsInZone.Remove(obj);
        InZoneAdjust();
    }
    private void InZoneAdjust()
    {
        if (objectsInZone.Count == 0) CloseDoor();
        else OpenDoor();
    }

    private void OpenDoor()
    {
        targetForDoor = 1f;
        if (!animating) StartCoroutine(DoorAnimator());
    }
    private void CloseDoor()
    {
        targetForDoor = 0f;
        if (!animating) StartCoroutine(DoorAnimator());
    }
    //variables for controlling the door
    private float openAmmount = 0f; //1 is fully opened, 0 is closed
    private float targetForDoor = 0f; //what the door lerp targets

    private Vector3 openPosition;
    private Vector3 closePosition;

    private float animateSpeed = 4f;
    private bool animating = false;
    //
    private IEnumerator DoorAnimator()
    {
        animating = true;
        while(openAmmount != targetForDoor)
        {
            openAmmount = math.lerp(openAmmount, targetForDoor, animateSpeed * Time.deltaTime);

            movingPart.transform.position = Vector3.Lerp(closePosition, openPosition, openAmmount);

            yield return null;
        }
        animating = false;
    }

    private void Start()
    {
        movingPart = transform.Find("MovingPart").gameObject;
        openPosition = movingPart.transform.position + Vector3.up * 4;
        closePosition = movingPart.transform.position;
    }
}
