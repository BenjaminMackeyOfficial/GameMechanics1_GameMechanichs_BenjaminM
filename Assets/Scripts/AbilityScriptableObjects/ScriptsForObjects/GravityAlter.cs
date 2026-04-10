
using UnityEngine;

[CreateAssetMenu(fileName = "GravityOffAbility", menuName = "AbilityScriptableObjects/GravityOffAbility")]
public class GravityAlter : Ability
{

    private GameObject highlightedObj;
    [SerializeField] Material defaultMaterial;
    private Color heldMaterial; //the color the highlighted object used to be, so i can set it back after un-highlighting
    private Color purple;
    

    private PlayerController playerController;
    private GameObject parentObj;
    private GameObject parentCamera;

    private Vector3 pos;

  
    public override void Initialize(GameObject parent)
    {
        Debug.Log("ability started");

        if (parentObj != parent)
        {
            parentObj = parent;
            parentCamera = parentObj.transform.Find("Camera").gameObject;
  
            //material = playerPredict.GetComponent<Renderer>().material;
        }

        purple = new Color(193, 64, 248, 10) / 255;
        

    }
    private bool _aiming = false;
    public override void Update()
    {
        GameObject tempHold = null;

        if (parentObj == null) return;

        if (!_aiming)
        {
            if (highlightedObj != null) highlightedObj.GetComponent<Renderer>().material.color = heldMaterial;
            highlightedObj = null;
            return;
        }
        
        RaycastHit hit1;

        if (!Physics.Raycast(parentObj.transform.position, parentCamera.transform.forward, out hit1, 1000f, ~(1 << 2)))
        {
            if(highlightedObj != null) highlightedObj.GetComponent<Renderer>().material.color = heldMaterial;
            highlightedObj = null;
        }
        else if(hit1.collider.gameObject == highlightedObj)
        {
            FlashPurple(highlightedObj);
        }
        else if (!hit1.collider.gameObject.CompareTag("Floor"))
        {
            Debug.Log("yuh");
            if(highlightedObj != null)
            {
                highlightedObj.GetComponent<Renderer>().material.color = heldMaterial;   
            }

            tempHold = hit1.collider.gameObject;

            heldMaterial = tempHold.GetComponent<Renderer>().material.color;

            FlashPurple(tempHold);

            highlightedObj = tempHold;
        }
        
        if(tempHold != null)
        {
            highlightedObj = tempHold;  
        }
    }
    private void FlashPurple(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = purple * (Mathf.Sin(Time.time * 10) * 0.5f + 1);
    }
    private Rigidbody GiveObjectRB(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) rb = obj.AddComponent<Rigidbody>();

        return rb;  
    }
    
    public override void Excecute()
    {
        if (highlightedObj == null) return;
        

        Rigidbody rb;
        rb = GiveObjectRB(highlightedObj);
        if (rb.isKinematic == false && rb.useGravity == false)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.AddForce(Vector3.up * 100, ForceMode.Force);
        }
        
    }

    public override void Abort()
    {
        _aiming = false;
    }
    public override void Aim()
    {
        _aiming = true;
    }
    public override void AbortAll()
    {
        
    }
}
