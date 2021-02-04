using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class TargetMover : MonoBehaviour
    {
        [SerializeField]
        private GUICollapse _closeGameObject;

        [SerializeField]
        private GameObject _targetSphere;

        [SerializeField]
        private GameObject _targetCamera;

        [SerializeField]
        private Toggle _lookStraightAheadToggle;

        [SerializeField]
        private Toggle _lookAtSphereToggle;

        [SerializeField]
        private Slider _orbitalRadius;

        [SerializeField]
        private Slider _verticalPosition;

        [SerializeField]
        private float _sphereOrbitalRadius = 0.0f;

        [SerializeField]
        private float _angluarVelocity = -70.0f;

        [SerializeField]
        private float _sphereInitialHeight = 1.0f;

        [SerializeField]
        private float _sphereMovableRangeInVertical = 0.8f;

        [SerializeField]
        private float _currentAngle = 0.0f;

        private GameObject _vrmModel = null;
        private GameObject _bvhGameObject = null;
        private Transform _leftEye;
        private Transform _rightEye;
        private bool _lookAtBone;

        public GameObject VrmModel { set { _vrmModel = value; } }
        public GameObject BvhGameObject { set { _bvhGameObject = value; } }
        public Transform LeftEye { set { _leftEye = value; } }
        public Transform RightEye { set { _rightEye = value; } }
        public bool LookAtBone { set { _lookAtBone = value; } }

        private void Start()
        {
            // Add listener to eye operation mode
            _lookStraightAheadToggle.onValueChanged.AddListener(EyeLookStraightAheadValueChanged);
            _lookAtSphereToggle.onValueChanged.AddListener(EyeLookAtSphereValueChanged);
        }

        private void LateUpdate()
        {
            _currentAngle += _angluarVelocity * Time.deltaTime * Mathf.Deg2Rad;

            var x = Mathf.Cos(_currentAngle) * (_sphereOrbitalRadius + _orbitalRadius.value);
            var z = Mathf.Sin(_currentAngle) * (_sphereOrbitalRadius + _orbitalRadius.value);
            var y = (_sphereInitialHeight + _verticalPosition.value) + _sphereMovableRangeInVertical * Mathf.Cos(_currentAngle / 3);

            if (_lookStraightAheadToggle.isOn)
            {
                if (_vrmModel != null)
                {
                    // Make eyes static
                    if (_lookAtBone)
                    {
                        _vrmModel.GetComponent<VRMLookAtBoneApplyer>().LeftEye.Transform = null;
                        _vrmModel.GetComponent<VRMLookAtBoneApplyer>().RightEye.Transform = null;
                    }
                    else
                    {
                        _vrmModel.GetComponent<VRMLookAtBlendShapeApplyer>().m_notSetValueApply = true;

                        var blednShapeProxy = _vrmModel.GetComponent<VRMBlendShapeProxy>();
                        blednShapeProxy.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.LookUp), 0.0f);
                        blednShapeProxy.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.LookDown), 0.0f);
                        blednShapeProxy.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.LookLeft), 0.0f);
                        blednShapeProxy.ImmediatelySetValue(BlendShapeKey.CreateFromPreset(BlendShapePreset.LookRight), 0.0f);
                    }
                }
            }

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

        } // update

        private void EyeLookStraightAheadValueChanged(bool _)
        {
            Toggle eyeOperationModeLookStraightAhead = _lookStraightAheadToggle;

            if (eyeOperationModeLookStraightAhead.isOn)
            {
                _closeGameObject.DisableSphere();
            }
        }

        private void EyeLookAtSphereValueChanged(bool _)
        {
            Toggle eyeOperationModeLookAtSphere = _lookAtSphereToggle;

            if (eyeOperationModeLookAtSphere.isOn)
            {
                if (_vrmModel != null && _vrmModel.GetComponent<VRMLookAtHead>().Target == _targetCamera.transform)
                {
                    _vrmModel.GetComponent<VRMLookAtHead>().Target = _targetSphere.transform;
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

                _closeGameObject.EnableSphere();
            }
        }

    } // class
} // namespace