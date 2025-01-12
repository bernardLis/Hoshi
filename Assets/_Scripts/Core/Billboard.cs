using UnityEngine;

namespace BountyBalance.Core
{
    public class Billboard : MonoBehaviour
    {
        Camera _cam;

        // Start is called before the first frame update
        void Start()
        {
            _cam = Camera.main;
        }

        void LateUpdate()
        {
            Vector3 lookRotation =
                Quaternion.LookRotation(transform.position - _cam.transform.position).eulerAngles;
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = Quaternion.Euler(lookRotation);
        }
    }
}