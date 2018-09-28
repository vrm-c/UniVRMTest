using UnityEngine;
using VRM;

namespace VRMViewer
{
    public class FlyThroughCameraView : MonoBehaviour
    {
        [SerializeField]
        private float _flyThroughCameraOrbitalRadius = 3.0f;

        [SerializeField]
        private float _angluarVelocity = 40.0f;

        [SerializeField]
        private float _flyThroughCameraInitialHeight = 1.5f;

        [SerializeField]
        private float _flyThroughCameraMovableRangeInVertical = 1.0f;

        private GameObject _vrmModel = null;
        private GameObject _bvhGameObject = null;
        private float _angle = 0.0f;

        public GameObject VrmModel { set { _vrmModel = value; } }
        public GameObject BvhGameObject { set { _bvhGameObject = value; } }

        private void LateUpdate()
        {
            _angle += _angluarVelocity * Time.deltaTime * Mathf.Deg2Rad;

            var x = Mathf.Cos(_angle) * _flyThroughCameraOrbitalRadius;
            var z = Mathf.Sin(_angle) * _flyThroughCameraOrbitalRadius;
            var y = _flyThroughCameraInitialHeight + _flyThroughCameraMovableRangeInVertical * Mathf.Cos(_angle / 3);
            transform.localPosition = new Vector3(x, y, z);

            // Fly-through viewpoint (virtual camera)
            if (_bvhGameObject != null)
            {
                if (_vrmModel != null)
                {
                    var tLookAt = _vrmModel.GetComponent<VRMLookAtHead>().Head.position;
                    transform.LookAt(new Vector3(tLookAt.x, tLookAt.y, tLookAt.z));
                }
                else
                {
                    transform.LookAt(new Vector3(0.0f, 1.2f, 0.0f));
                }

            }
            else
            {
                transform.LookAt(new Vector3(0.0f, 1.2f, 0.0f));
            }
        }

    }
}
