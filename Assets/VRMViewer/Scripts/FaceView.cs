using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class FaceView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _faceCamera;

        [SerializeField]
        private GameObject _referenceObject;

        [SerializeField]
        private Toggle _faceCameraToggle;

        [SerializeField]
        private Slider _faceCameraDistance;

        private GameObject _vrmModel = null;
        private float _distanceFromFace = 0.5f;

        public GameObject VrmModel { set { _vrmModel = value; } }

        private void LateUpdate()
        {
            if (_vrmModel != null)
            {
                var frontfacePos = _vrmModel.GetComponent<VRMLookAtHead>().Head.position;
                var faceSurfaceNormal = _vrmModel.GetComponent<VRMLookAtHead>().Head.forward;
                var distanceFromFace = _faceCameraDistance.value + _distanceFromFace;
                transform.localPosition = new Vector3(
                    frontfacePos.x + faceSurfaceNormal.x * distanceFromFace + Time.deltaTime * 0.001f, 
                    frontfacePos.y + faceSurfaceNormal.y * distanceFromFace, 
                    frontfacePos.z + faceSurfaceNormal.z * distanceFromFace);
            }
            else
            {
                transform.localPosition = new Vector3(0, 1.2f, 0);
            }

            if (_vrmModel != null)
            {
                var tLookAt = _vrmModel.GetComponent<VRMLookAtHead>().Head.position;
                transform.LookAt(new Vector3(tLookAt.x, tLookAt.y, tLookAt.z));
            }
            else
            {
                transform.LookAt(new Vector3(0, 1.2f, 0));
            }

            // Pass the rotation and translation to the face camera
            if (_faceCameraToggle.isOn)
            {
                _faceCamera.transform.position = new Vector3(
                    _referenceObject.transform.position.x, 
                    _referenceObject.transform.position.y, 
                    _referenceObject.transform.position.z);
                _faceCamera.transform.rotation = new Quaternion(
                    _referenceObject.transform.rotation.x,
                    _referenceObject.transform.rotation.y, 
                    _referenceObject.transform.rotation.z, 
                    _referenceObject.transform.rotation.w);
            }
        }

    }
}
