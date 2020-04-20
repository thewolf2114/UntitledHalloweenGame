using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensures the camera doesn't clip through a wall.
/// This script can be found on this website: https://sharpcoderblog.com/blog/third-person-camera-in-unity-3d
/// and is not an original script created by me.
/// </summary>
public class CameraCollision : MonoBehaviour
{
    public Transform referenceTransform;
    public float collisionOffset = 0.2f; //To prevent Camera from clipping through Objects

    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;

    LayerMask notThese;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);

        notThese = ~((1 << 9) | (1 << 18) | (1 << 16) | (1 << 17) | (1 << 0) | (1 << 11));
    }

    // FixedUpdate for physics calculations
    void FixedUpdate()
    {
        Vector3 currentPos = defaultPos;
        RaycastHit hit;
        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance, notThese))
        {
            currentPos = (directionNormalized * (hit.distance - collisionOffset));
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 15f);
    }
}
