using UnityEngine;

[CreateAssetMenu(fileName = "MakeBallAbility", menuName = "AbilityScriptableObjects/MakeBallAbility")]
public class MakeBall : Ability
{
    private GameObject ballPredict;
    private Material material;
    private bool canTele = false;
    [SerializeField] GameObject capsulePrefab;
    [SerializeField] GameObject ballToSpawnPrefab;

    private Color red;
    private Color green;

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

        red = new Color(255, 0, 0, 60) / 255;
        green = new Color(0, 255, 0, 60) / 255;

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
            canTele = false;
            ballPredict.transform.position = Vector3.up * int.MaxValue;
            return;
        }

        ballPredict.transform.position = hit1.point + hit1.normal * 1.2f;

        if (Physics.OverlapSphere(
                ballPredict.transform.position + Vector3.up * 0.5f,
                0.5f, ~(1 << 2)).Length > 0)
        {
            material.color = red;
            canTele = false;
        }
        else
        {
            material.color = green;
            pos = ballPredict.transform.position;
            canTele = true;
        }

    }
    public override void Excecute()
    {
        if (!_aiming) return;
        if (!canTele) return;
        //any visuals

        //

        SpawnBall();
    }
    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballToSpawnPrefab);
        ball.transform.position = pos;
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
