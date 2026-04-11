using UnityEngine;

[CreateAssetMenu(fileName = "GravityBombAbility", menuName = "AbilityScriptableObjects/GravityBombAbility")]
public class GravityBomb : Ability
{
    private GameObject ballPredict;
    private Material material;

    [SerializeField] GameObject capsulePrefab;

    private Color blue;
    

    private PlayerController playerController;
    private GameObject parentObj;
    private GameObject parentCamera;

    private Vector3 pos;

    private GameObject MakePlayerPer()
    {
        if (capsulePrefab == null) return new GameObject();
        return Instantiate(capsulePrefab);
    }
    public override void Initialize(GameObject parent)
    {
        Debug.Log("ability started");
        if (ballPredict == null) ballPredict = MakePlayerPer();
        if (parentObj != parent)
        {
            parentObj = parent;
            parentCamera = parentObj.transform.Find("Camera").gameObject;
            playerController = parent.GetComponent<PlayerController>();
            material = ballPredict.GetComponent<Renderer>().material;
        }

        blue = new Color(0, 0, 255, 60) / 255;
        ballPredict.GetComponent<Renderer>().material.color = blue;

    }
    private bool _aiming = false;
    public override void Update()
    {
        if (parentObj == null) return;

        if (!_aiming) ballPredict.SetActive(false);
        else ballPredict.SetActive(true);

        RaycastHit hit1;

        if (!Physics.Raycast(parentObj.transform.position, parentCamera.transform.forward, out hit1, 1000f, ~(1 << 2)))
        {
            ballPredict.transform.position = Vector3.up * int.MaxValue;
            return;
        }

        ballPredict.transform.position = hit1.point;
        pos = hit1.point;

        
       

    }
    public override void Excecute()
    {
        if (!_aiming) return;
        //any visuals
        //
        ApplyForce();
    }

    public float Radius;
    public float Force;
    private void ApplyForce()
    {
        Collider[] nearby = Physics.OverlapSphere(
            pos,
            Radius);
        Debug.Log(nearby.Length);
        foreach (Collider item in nearby)
        {
            Debug.Log(item.name);
            Rigidbody rb = item.gameObject.GetComponent<Rigidbody>();
            if (rb == null) continue; 
            Vector3 force = pos - item.transform.position;
            float dist = Mathf.Clamp(force.magnitude, 0.001f, float.MaxValue);

            rb.AddForce((force * Force) / dist, ForceMode.VelocityChange);
            Debug.Log("easea");
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
        ballPredict.SetActive(false);
    }
}
