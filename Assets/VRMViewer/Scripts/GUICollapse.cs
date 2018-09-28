using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class GUICollapse : MonoBehaviour
    {
        private GameObject _vrmModel = null;

        public GameObject VrmModel { set { _vrmModel = value; } }

        #region Sphere
        [SerializeField]
        private GameObject _targetSphere;

        [SerializeField]
        private GameObject _hideSphere;

        [SerializeField]
        private GameObject _orbitalRadiusText;

        [SerializeField]
        private GameObject _orbitalRadiusSlider;

        [SerializeField]
        private GameObject _vertialPositionText;

        [SerializeField]
        private GameObject _vertialPositionSlider;

        [SerializeField]
        private GameObject _sphereMakeSpace;

        public void DisableSphereSliders()
        {
            _targetSphere.GetComponent<MeshRenderer>().enabled = false;
            _orbitalRadiusText.SetActive(false);
            _orbitalRadiusSlider.SetActive(false);
            _vertialPositionText.SetActive(false);
            _vertialPositionSlider.SetActive(false);
            _sphereMakeSpace.SetActive(false);
        }
       
        public void DisableSphere()
        {
            _targetSphere.GetComponent<MeshRenderer>().enabled = false;
            _hideSphere.SetActive(false);
            _orbitalRadiusText.SetActive(false);
            _orbitalRadiusSlider.SetActive(false);
            _vertialPositionText.SetActive(false);
            _vertialPositionSlider.SetActive(false);
            _sphereMakeSpace.SetActive(false);
        }

        public void EnableSphereSliders()
        {
            _targetSphere.GetComponent<MeshRenderer>().enabled = true;
            _orbitalRadiusText.SetActive(true);
            _orbitalRadiusSlider.SetActive(true);
            _vertialPositionText.SetActive(true);
            _vertialPositionSlider.SetActive(true);
            _sphereMakeSpace.SetActive(true);
        }

        public void EnableSphere()
        {
            if (_hideSphere.GetComponent<Toggle>().isOn)
            {
                _hideSphere.SetActive(true);
            }
            else
            {
                _targetSphere.GetComponent<MeshRenderer>().enabled = true;
                _hideSphere.SetActive(true);
                _orbitalRadiusText.SetActive(true);
                _orbitalRadiusSlider.SetActive(true);
                _vertialPositionText.SetActive(true);
                _vertialPositionSlider.SetActive(true);
                _sphereMakeSpace.SetActive(true);
            }
        }
        #endregion

        #region Face Camera
        [SerializeField]
        private GameObject _faceCameraView;

        [SerializeField]
        private GameObject _faceCmaeraDistance;

        [SerializeField]
        private GameObject _faceCameraMakeSpace;

        public void FaceCameraPropertyControl(Toggle ViewpointPanelFaceCamera)
        {
            if (ViewpointPanelFaceCamera.isOn && _vrmModel != null)
            {
                _faceCameraView.SetActive(true);
                _faceCmaeraDistance.SetActive(true);
                _faceCameraMakeSpace.SetActive(true);
            }
            else if (ViewpointPanelFaceCamera.isOn)
            {
                _faceCameraView.SetActive(true);
            }
            else
            {
                _faceCameraView.SetActive(false);
                _faceCmaeraDistance.SetActive(false);
                _faceCameraMakeSpace.SetActive(false);
            }
        }

        public void FaceCameraPropertyActivateVRM()
        {
            _faceCmaeraDistance.SetActive(true);
            _faceCameraMakeSpace.SetActive(true);
        }
        #endregion

        #region First-person mode
        [SerializeField]
        private GameObject _firstPersonMode;

        public void DisableFirstPersonModeOption()
        {
            _firstPersonMode.SetActive(false);
        }

        public void EnableFirstPersonModeOption()
        {
            _firstPersonMode.SetActive(true);
        }
        #endregion

        #region Collapse everything except free viewpoint mode
        [SerializeField]
        private GameObject _autoChangedViewpointMode;

        [SerializeField]
        private GameObject _faceCameraToggleGameObject;

        [SerializeField]
        private GameObject _viewpointPanelBackButtonGameObject;

        [SerializeField]
        private Toggle _hideSphereToggle;

        [SerializeField]
        private Toggle _faceCameraToggle;

        public void DisableOtherViewpointModes()
        {
            _faceCameraView.SetActive(false);
            _autoChangedViewpointMode.SetActive(false);
            _faceCameraToggleGameObject.SetActive(false);
            _faceCmaeraDistance.SetActive(false);
            _viewpointPanelBackButtonGameObject.SetActive(false);
            _targetSphere.GetComponent<MeshRenderer>().enabled = false;
        }

        public void EnableOtherViewpointModes()
        {
            _autoChangedViewpointMode.SetActive(true);
            _faceCameraToggleGameObject.SetActive(true);
            _viewpointPanelBackButtonGameObject.SetActive(true);

            // If toggles are acitve, maintain the original status
            if (_faceCameraToggle.isOn)
            {
                _faceCameraView.SetActive(true);
                _faceCmaeraDistance.SetActive(true);
                _faceCameraMakeSpace.SetActive(true);
            }

            if (_hideSphereToggle.isOn == false)
            {
                _targetSphere.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        #endregion
    }
}