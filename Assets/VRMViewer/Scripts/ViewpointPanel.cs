using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class ViewpointPanel : MonoBehaviour
    {
        [SerializeField]
        private GUICollapse _closeGameObject;

        [SerializeField]
        private GameObject _mainCameraGameObject;

        [SerializeField]
        private GameObject _autoChangedCameraMode;

        [SerializeField]
        private Toggle _cameraFreeViewpoint;

        [SerializeField]
        private Toggle _firstPersonMode;

        [SerializeField]
        private Toggle _cameraAutoChangedViewpoint;

        [SerializeField]
        private Toggle _faceView;

        private GameObject _vrmModel = null;

        public GameObject VrmModel { set { _vrmModel = value; } }

        private void Start()
        {
            // Add listener to each viewpoint mode
            _cameraFreeViewpoint.onValueChanged.AddListener(FreeViewpointValueChanged);
            _firstPersonMode.onValueChanged.AddListener(FirstPersonModeValueChanged);
            _cameraAutoChangedViewpoint.onValueChanged.AddListener(AutoChangedViewpointValueChanged);
            _faceView.onValueChanged.AddListener(FaceCloseUpValueChanged);
        }

        private void FreeViewpointValueChanged(bool _)
        {
            Toggle freeViewpoint = _cameraFreeViewpoint;

            if (freeViewpoint.isOn) // Free viewpoint
            {
                _autoChangedCameraMode.SetActive(false);
            }

            // Display the toggle first person mode
            if (_vrmModel != null)
            {
                _closeGameObject.EnableFirstPersonModeOption();
            }
        }

        private void FirstPersonModeValueChanged(bool _)
        {
            Toggle firstPersonMode = _firstPersonMode;

            if (firstPersonMode.isOn)
            {
                // Disable other viewpoint modes
                _closeGameObject.DisableOtherViewpointModes();

                // Set the model in first-person (Render everything but layer 10)
                _mainCameraGameObject.GetComponent<Camera>().cullingMask = ~(1 << VRMFirstPerson.THIRDPERSON_ONLY_LAYER);
            }
            else
            {
                // Enable other viewpoint modes
                _closeGameObject.EnableOtherViewpointModes();

                // Back to the original status (Switch on layer 10, leave others as they are)
                _mainCameraGameObject.GetComponent<Camera>().cullingMask |= (1 << VRMFirstPerson.THIRDPERSON_ONLY_LAYER);
            }
        }

        private void AutoChangedViewpointValueChanged(bool _)
        {
            Toggle autoChangedViewpoint = _cameraAutoChangedViewpoint;

            // Undisplay the toggle first person mode
            if (_vrmModel != null)
            {
                _closeGameObject.DisableFirstPersonModeOption();
            }

            if (autoChangedViewpoint.isOn) // Fly-thorugh viewpoint
            {
                if (_autoChangedCameraMode.activeSelf == false)
                {
                    _autoChangedCameraMode.SetActive(!_autoChangedCameraMode.activeSelf);
                }
            }

        }

        private void FaceCloseUpValueChanged(bool _)
        {
            Toggle faceCloseUp = _faceView;

            _closeGameObject.FaceCameraPropertyControl(faceCloseUp);
        }

    }
}
