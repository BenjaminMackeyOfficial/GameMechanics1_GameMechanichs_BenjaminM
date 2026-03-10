using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TeleportAbility", menuName = "AbilityScriptableObjects/TeleportAbility")]
public class Teleporter : Ability
{
    private GameObject playerPredict; 
    private Material material;
    private bool canTele = false;
    [SerializeField] GameObject capsulePrefab;

    private Color red;
    private Color green;

    private PlayerController playerController;
    private GameObject parentObj;
    private GameObject parentCamera;

    private Vector3 pos;

    private GameObject MakePlayerPer()
    {
        if(capsulePrefab == null) return new GameObject(); 
        return Instantiate(capsulePrefab);
    }
    public override void Initialize(GameObject parent)
    {
        Debug.Log("ability started");
        if(playerPredict == null) playerPredict = MakePlayerPer();
        if(parentObj != parent)
        {
            parentObj = parent;
            parentCamera = parentObj.transform.Find("Camera").gameObject;
            playerController = parent.GetComponent<PlayerController>();
            material = playerPredict.GetComponent<Renderer>().material;
        }

        red = new Color(255,0,0,60) /255;
        green = new Color(0,255,0,60) /255;

    }
    public override void Update()
    {
        if(parentObj == null) return;
        RaycastHit hit1;

        if(!Physics.Raycast(parentObj.transform.position, parentCamera.transform.forward, out hit1, 1000f, ~(1 << 2))) 
        {
            canTele = false;
            playerPredict.transform.position = Vector3.up * int.MaxValue;
            return;
        }
        
        Debug.Log(hit1.collider.gameObject);
        playerPredict.transform.position = hit1.point + hit1.normal *1.2f;

        if(Physics.OverlapCapsule(
                playerPredict.transform.position + Vector3.up * 0.5f,
                playerPredict.transform.position - Vector3.up * 0.5f,
                0.5f, ~(1 << 2)).Length >0)
        {
            material.color = red;
            canTele = false;
        }
        else
        {
            material.color = green;
            pos = playerPredict.transform.position;
            canTele = true;
        }
        
    }
    public override void Excecute()
    {
        if(!canTele) return;
        //any visuals

        //

        playerController.Teleport(pos);
    }
}
