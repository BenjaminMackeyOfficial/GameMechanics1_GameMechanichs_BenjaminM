using System.Collections;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] Transform[] positions;

    private Rigidbody rb;
    private GameObject movingPlatform;



    private int index = -1;
    private void StartMoveToTarget()
    {
        if (index == positions.Length - 1) index = 0;
        else index += 1;

        startPosition = targetPosition;
        targetPosition = positions[index].position;


        distMult = 1 /  Mathf.Clamp(Mathf.Abs((targetPosition - startPosition).magnitude),0.000001f, float.MaxValue);//keeps speed the same for distances

        progress = 0f;

        StartCoroutine(Animator());
    }

    //
    private bool _animating = false;
    private float speed =5f;

    private float progress = 0f;

    private Vector3 targetPosition = Vector3.zero; //temp backup instead of a null check
    private Vector3 startPosition;

    private float distMult = 1f; // changes via script
    //
    private IEnumerator Animator()
    {
        _animating = true;
        while (progress < 1) 
        {
            progress += Time.deltaTime * speed * distMult;

            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, progress));
            yield return null;
        }
        _animating = false;
    }
    void Update()
    {
        if(!_animating) StartMoveToTarget();
    }
    private void Start()
    {
        movingPlatform = transform.Find("Platform").gameObject;
        rb = movingPlatform.GetComponent<Rigidbody>();

        startPosition = movingPlatform.transform.position;
        targetPosition = movingPlatform.transform.position;
    }
}
