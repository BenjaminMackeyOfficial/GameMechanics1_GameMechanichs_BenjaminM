using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStateManager stateManager;
    private GameObject CurrentCheckPoint = null;

    private Color green;
    private Color blue;

    public void HitCheckpoint(GameObject chkpoint)
    {
        if (chkpoint == CurrentCheckPoint) return;
        if(CurrentCheckPoint != null) CurrentCheckPoint.GetComponent<Renderer>().material.color = blue ;
        CurrentCheckPoint = chkpoint;
        CurrentCheckPoint.GetComponent <Renderer>().material.color = green ;
    }
    public void KillPlayer()
    {
        Vector3 respawnPos;
        if (CurrentCheckPoint == null) respawnPos = Vector3.up * 3;
        else respawnPos = CurrentCheckPoint.transform.position + Vector3.up;

        controller.Teleport(respawnPos);
    }
    void Start()
    {
        controller = GetComponent<PlayerController>();
        stateManager = GetComponent<PlayerStateManager>();

        green = new Color(0, 255, 0, 195) / 255f;
        blue = new Color(0, 0, 255, 195) / 255f;
    }
    private void OnTriggerEnter(Collider other)
    {
        Collectible collectible; //collectible and zone could probably be one class... oh well
        EffectZone zone;
        if (other.gameObject.TryGetComponent(out collectible))
        {
            collectible.Collect(); //all this does is dissable the collectible
        }
        else if(other.gameObject.TryGetComponent(out zone))
        {
            zone.Effect(this);
        }
        
        
    }
   
}
