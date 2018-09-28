using System;
using System.Linq;
using UniHumanoid;
using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class MotionControlPanel : MonoBehaviour
    {
        [SerializeField]
        private GUICollapse _closeSphereSliders;

        [SerializeField]
        private Toggle _enableAutoBlink;

        [SerializeField]
        private Toggle _enableLipSync;

        [SerializeField]
        private Toggle _hideSphere;

        [SerializeField]
        private HumanPoseClip _avatarTPose;

        private HumanPoseTransfer _bvhSource;
        private HumanPoseTransfer _loadedBvhSourceOnAvatar;
        private AiueoViewer _lipSync;
        private BlinkerViewer _blink;

        public HumanPoseTransfer BvhSource { set { _bvhSource = value; } }
        public HumanPoseTransfer LoadedBvhSourceOnAvatar { set { _loadedBvhSourceOnAvatar = value; } }

        [Serializable]
        private struct UIFields
        {
            [SerializeField]
            private Toggle _toggleMotionBVH;

            [SerializeField]
            private Toggle _toggleMotionTPose;

            [SerializeField]
            private ToggleGroup _toggleMotion;

            private Toggle _activeToggleMotion;

            public void UpdateTogle(Action onBvh, Action onTPose)
            {
                var value = _toggleMotion.ActiveToggles().FirstOrDefault();
                if (value == _activeToggleMotion)
                    return;

                _activeToggleMotion = value;
                if (value == _toggleMotionTPose)
                {
                    onTPose();
                }
                else if (value == _toggleMotionBVH)
                {
                    onBvh();
                }
                else
                {
                    Debug.Log("motion: no toggle");
                }
            }
        }
        [SerializeField]
        private UIFields _ui;

        private bool _enableLipSyncValue;
        private bool EnableLipSyncValue
        {
            set
            {
                if (_enableLipSyncValue == value) return;
                _enableLipSyncValue = value;
                if (_lipSync != null)
                {
                    _lipSync.enabled = _enableLipSyncValue;
                }
            }
        }

        private bool _enableBlinkValue;
        private bool EnableBlinkValue
        {
            set
            {
                if (_enableBlinkValue == value) return;
                _enableBlinkValue = value;
                if (_blink != null)
                {
                    _blink.enabled = _enableBlinkValue;
                }
            }
        }

        private void Start()
        {
            // Add listener
            _hideSphere.onValueChanged.AddListener(SphereHideModeValueChanged);
        }

        private void Update()
        {
            EnableLipSyncValue = _enableLipSync.isOn;
            EnableBlinkValue = _enableAutoBlink.isOn;

            _ui.UpdateTogle(EnableBvh, EnableTPose);
        }

        private void SphereHideModeValueChanged(bool _)
        {
            Toggle sphereHideMode = _hideSphere;

            if (sphereHideMode.isOn)
            {
                _closeSphereSliders.DisableSphereSliders();
            }
            else
            {
                _closeSphereSliders.EnableSphereSliders();
            }
        }

        public void EnableBvh()
        {
            if (_loadedBvhSourceOnAvatar != null)
            {
                _loadedBvhSourceOnAvatar.Source = _bvhSource;
                _loadedBvhSourceOnAvatar.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
            }
        }

        public void EnableTPose()
        {
            if (_loadedBvhSourceOnAvatar != null)
            {
                _loadedBvhSourceOnAvatar.PoseClip = _avatarTPose;
                _loadedBvhSourceOnAvatar.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
            }
        }

        public void AssignAutoPlay(GameObject vrmModel)
        {
            _lipSync = vrmModel.AddComponent<AiueoViewer>();
            _blink = vrmModel.AddComponent<BlinkerViewer>();
        }

    }
}
