using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStateManager : MonoBehaviour
{

    //states nd whatnot---------
    public bool grounded;
    public bool sprinting;

    public Vector3 groundUp;
    //--------------------------


    //utils
    private Vector3 castFrom;
    [SerializeField] float castDist;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (castDist <= 0.001f) castDist = 0.52f;
    }

    public Vector3 checkGround(float maxSlope) //must be called externally
    {
        RaycastHit hit;
        grounded = Physics.SphereCast(transform.position, transform.localScale.x/2,Vector3.down, out hit, castDist, ~(1 << 2));

        
        //Debug.Log(hit.collider.gameObject.name);
        if (grounded) groundUp = hit.normal;
        else groundUp = Vector3.up;

        return hit.point;
    }
   
}
