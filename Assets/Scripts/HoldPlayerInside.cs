using System.Collections.Generic;
using UnityEngine;

public class HoldPlayerInside : MonoBehaviour
{
    private Vector3 lastPos = Vector3.zero;

    private Vector3 forceToApply = Vector3.zero;

    private List<Rigidbody> stuffInBox = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == transform.parent) return;
        Rigidbody rb;
        if(other.gameObject.TryGetComponent(out rb) && !stuffInBox.Contains(rb)) stuffInBox.Add(rb);
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb;
        if (other.gameObject.TryGetComponent(out rb) && stuffInBox.Contains(rb)) stuffInBox.Remove(rb);
    }
    private void UpdateForces()
    {
        forceToApply = (transform.position - lastPos) * 2f;
        lastPos = transform.position;
    }
    private void ApplyForces()
    {
        foreach (Rigidbody item in stuffInBox)
        {
            Debug.Log(item.gameObject.name);
            item.AddForce(forceToApply, ForceMode.VelocityChange);
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        UpdateForces();
        ApplyForces();
    }
}
