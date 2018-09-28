using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace VRMViewer
{
    public class InformationUpdate : MonoBehaviour
    {
        [SerializeField]
        private RokuroCamera _rokuroCamera;

        [SerializeField]
        private TargetMover _targetMover;

        [SerializeField]
        private EyeControlCamera _eyeControlCamera;

        [SerializeField]
        private FaceView _faceView;

        [SerializeField]
        private FacialExpressionPanel _facialExpressionPanel;

        [SerializeField]
        private FlyThroughCameraView _flyThroughCameraView;

        [SerializeField]
        private ViewpointPanel _viewpointPanel;

        [SerializeField]
        private MultipleLanguageSupport _multiLanguageSupport;

        [SerializeField]
        private GUICollapse _closeGameObject;

        public void SetVRM(GameObject VRM, Transform leftEyeTransform, Transform rightEyeTransform)
        {
            _targetMover.VrmModel = VRM; _targetMover.LeftEye = leftEyeTransform; _targetMover.RightEye = rightEyeTransform;
            _eyeControlCamera.VrmModel = VRM; _eyeControlCamera.LeftEye = leftEyeTransform; _eyeControlCamera.RightEye = rightEyeTransform;
            _faceView.VrmModel = VRM;
            _facialExpressionPanel.VrmModel = VRM;
            _flyThroughCameraView.VrmModel = VRM;
            _multiLanguageSupport.VrmModel = VRM;
            _viewpointPanel.VrmModel = VRM;
            _closeGameObject.VrmModel = VRM;
        }

        public void SetBVH(GameObject BVH)
        {
            _targetMover.BvhGameObject = BVH;
            _flyThroughCameraView.BvhGameObject = BVH;
        }

        public void SetExpression(List<GameObject> objs, int validExpNum)
        {
            _rokuroCamera.Objs = objs;
            _rokuroCamera.ValidExpNum = validExpNum;
        }

    }
}
