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
        castFrom = transform.position - Vector3.up * (0.5f * transform.localScale.y);
    }

    public void checkGround() //must be called externally
    {
        RaycastHit hit;
        grounded = Physics.Raycast(castFrom, Vector3.down, out hit, castDist);


        if (grounded) groundUp = hit.normal;
        else groundUp = Vector3.up;

    }
   
}
