using UnityEngine;

namespace Jobs
{
    public class CubeController : MonoBehaviour
    {
        public Vector3 FirstClosest { get; set; }
        public Vector3 SecondClosest { get; set; }
        public Vector3 ThirdClosest { get; set; }
        public Vector3 Farthest { get; set; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, FirstClosest);
            Gizmos.DrawLine(transform.position, SecondClosest);
            Gizmos.DrawLine(transform.position, ThirdClosest);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Farthest);
        }
    }
}