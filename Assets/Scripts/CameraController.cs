using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;

    [SerializeField]
    private float m_camMaxDist = 10.0f;
    [SerializeField]
    private float m_camMinDist = 2.0f;
    [SerializeField]
    private float scrollSpeed = 4.0f; // Scroll speed

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();
    }

    private void FixedUpdate()
    {
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
    }

    private void UpdateVerticalMovements()
    {
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        float eulersAngleX = transform.rotation.eulerAngles.x;

        float comparisonAngle = eulersAngleX + currentAngleY;

        comparisonAngle = ClampAngle(comparisonAngle);

        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }
        transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    }

    //private void UpdateCameraScroll()
    //{
    //    if (Input.mouseScrollDelta.y != 0)
    //    {
    //        //TODO: Faire une v�rification selon la distance la plus proche ou la plus �loign�e
    //            //Que je souhaite entre ma cam�ra et mon objet

    //        //TODO: Lerp plut�t que d'effectuer imm�diatement la translation de la cam�ra
    //        transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
    //    }
    //}

    private void UpdateCameraScroll()
    {

        if (Input.mouseScrollDelta.y != 0)
        {
            //TODO: Faire une vérification selon la distance la plus proche ou la plus éloignée
            //Que je souhaite entre ma caméra et mon objet
            Vector3 planeOnFloor = Vector3.ProjectOnPlane(transform.position, Vector3.up);
            float currDist = Vector3.Distance(m_objectToLookAt.transform.position, planeOnFloor);
            if (m_camMinDist <= currDist && currDist <= m_camMaxDist)
            {
                Vector3 targetPos = planeOnFloor + Vector3.forward * Input.mouseScrollDelta.y;
                //Debug.Log("targetPos: " + targetPos);

                //targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.up);
                //Debug.Log("after: " + targetPos);
                float newDistance = Vector3.Distance(m_objectToLookAt.transform.position, targetPos);
                Debug.Log("newDistance: " + newDistance);

                if (newDistance >= m_camMaxDist)
                {
                    Debug.Log("MAXDistance: " + newDistance);
                    return;
                }
                else if (newDistance <= m_camMinDist)
                {
                    Debug.Log("MINDistance: " + newDistance);
                    return;
                }
                if (newDistance > m_camMinDist && newDistance < m_camMaxDist)
                {
                    Vector3 newTargetPos = new Vector3(0, transform.position.y, targetPos.z);
                    transform.position = Vector3.Lerp(transform.position, newTargetPos, scrollSpeed * Time.deltaTime);
                }
            }
        }
    }
    private void MoveCameraInFrontOfObstructionsFUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;

        var vecteurDiff = transform.position - m_objectToLookAt.position;
        var distance = vecteurDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vecteurDiff, out hit, distance, layerMask))
        {
            //J'ai un objet entre mon focus et ma cam�ra
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff.normalized * hit.distance, Color.yellow);
            transform.SetPositionAndRotation(hit.point, transform.rotation);
        }
        else
        {
            //Je n'en ai pas
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff, Color.white);
        }
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
