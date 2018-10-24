using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class EyeControlCamera : MonoBehaviour
    {
        [SerializeField]
        private GUICollapse _closeGameObject;

        [SerializeField]
        private GameObject _mainCamera;

        [SerializeField]
        private GameObject _targetSphere;

        [SerializeField]
        private GameObject _targetCamera;

        [SerializeField]
        private GameObject _flyThroughCameraView;

        [SerializeField]
        private Toggle _lookAtCameraToggle;

        [SerializeField]
        private Toggle _cameraAutoChangedViewpoint;

        private GameObject _vrmModel = null;
        private Transform _leftEye;
        private Transform _rightEye;
        private bool _lookAtBone;

        public GameObject VrmModel { set { _vrmModel = value; } }
        public Transform LeftEye { set { _leftEye = value; } }
        public Transform RightEye { set { _rightEye = value; } }
        public bool LookAtBone { set { _lookAtBone = value; } }

        private void Start()
        {
            // Add listener to eye operation mode
            _lookAtCameraToggle.onValueChanged.AddListener(EyeLookAtCameraValueChanged);
        }

        private void LateUpdate()
        {
            if(_lookAtCameraToggle.isOn)
            {
                if (_cameraAutoChangedViewpoint.isOn)
                {
                    var pos = _flyThroughCameraView.transform.position;
                    transform.localPosition = new Vector3(pos.x + Time.deltaTime * 0.001f, pos.y, pos.z);
                }
                else
                {
                    var pos = _mainCamera.transform.position;
                    transform.localPosition = new Vector3(pos.x + Time.deltaTime * 0.001f, pos.y, pos.z);
                }
            }
        }

        private void EyeLookAtCameraValueChanged(bool _)
        {
            Toggle eyeOperationModeLookAtCamera = _lookAtCameraToggle;

            if (eyeOperationModeLookAtCamera.isOn)
            {
                if (_vrmModel != null && _vrmModel.GetComponent<VRMLookAtHead>().Target == _targetSphere.transform)
                {
                    _vrmModel.GetComponent<VRMLookAtHead>().Target = _targetCamera.transform;
                }

                if (_lookAtBone)
                {
                    if (_vrmModel != null && (_vrmModel.GetComponent<VRMLookAtBoneApplyer>().LeftEye.Transform == null || _vrmModel.GetComponent<VRMLookAtBoneApplyer>().RightEye.Transform == null))
                    {
                        _vrmModel.GetComponent<VRMLookAtBoneApplyer>().LeftEye.Transform = _leftEye;
                        _vrmModel.GetComponent<VRMLookAtBoneApplyer>().RightEye.Transform = _rightEye;
                    }
                }
                else
                {
                    if (_vrmModel != null && _vrmModel.GetComponent<VRMLookAtBlendShapeApplyer>().m_notSetValueApply)
                    {
                        _vrmModel.GetComponent<VRMLookAtBlendShapeApplyer>().m_notSetValueApply = false;
                    }
                }

                _closeGameObject.DisableSphere();
            }
        }

    }
}
