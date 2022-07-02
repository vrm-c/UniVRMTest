using System;
using System.Collections;
using System.IO;
using UniHumanoid;
using UnityEngine;
using UnityEngine.UI;
using VRM;
using SimpleFileBrowser;
using UniGLTF;

namespace VRMViewer
{
    public class ViewerUI : MonoBehaviour
    {
        #region UI
        [SerializeField]
        private LicensePanel _licensePanel;

        [SerializeField]
        private MotionControlPanel _motionControlPanel;

        [SerializeField]
        private FacialExpressionPanel _facialExpressionPanel;

        [SerializeField]
        private InformationUpdate _informationUpdate;

        [SerializeField]
        private GUICollapse _closeGameObject;

        [SerializeField]
        private MessagePanel _errorMessagePanel;

        [SerializeField]
        private MessagePanel _pauseMessagePanel;

        [SerializeField]
        private GameObject _targetSphere;

        [SerializeField]
        private GameObject _targetCamera;

        [SerializeField]
        private GameObject _canvasRoot;

        [SerializeField]
        private Text _version;

        [SerializeField]
        private Button _openVRM;

        [SerializeField]
        private Button _openBVH;

        [SerializeField]
        private Toggle _toggleMotionBVH;

        [SerializeField]
        private Toggle _toggleMotionTPose;

        [SerializeField]
        private Toggle _lookAtCamera;

        [SerializeField]
        private Toggle _lookAtSphere;

        [SerializeField]
        private Toggle _freeViewpointToggle;

        [SerializeField]
        private Toggle _faceViewToggle;

        [SerializeField]
        private HumanPoseClip _avatarTPose;

        private string _vrmPath = null;
        private string _bvhPath = null;

        private HumanPoseTransfer _bvhSource;
        private HumanPoseTransfer _loadedBvhSourceOnAvatar;
        private BvhImporterContext _bvhMotion;
        private VRMBlendShapeProxy _proxy;

        // GLTFからモデルのオブジェクト
        private GameObject _vrmModel = null;
        private Transform _leftEyeSaved;
        private Transform _rightEyeSaved;

        // BVHのオブジェクト
        private string _bvhPathLocal = null;
        private string _bvhPathSaved = null;
        // Pause the scene
        private bool _pause;
        // Initial_BVH_Crush_flag
        private bool _bvhLoadingTrigger = false;
        // VRMLookAtBone flag
        private bool _lookAtBoneFlag = false;
        #endregion

        private void Start()
        {
            _version.text = string.Format("UniVRM - {0}.{1}", VRMVersion.MAJOR, VRMVersion.MINOR);

            _pause = false;
            _openVRM.onClick.AddListener(OnOpenClickedVRM);
            _openBVH.onClick.AddListener(OnOpenClickedBVH);

            // Load initial motion
            string path = Application.streamingAssetsPath + "/test.txt";

            if (File.Exists(path))
            {
                LoadMotion(path);
                _bvhPathSaved = path;
            }

            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length > 1)
            {
                LoadModel(cmds[1]);
            }
        }

        private void LoadMotion(string path)
        {
            try
            {
                // Trigger BVH
                _bvhLoadingTrigger = true;
                // Save current path
                _bvhPathLocal = path;
                var previous_motion = _bvhMotion;
                if (previous_motion != null) { Destroy(previous_motion.Root); }

                var context = new UniHumanoid.BvhImporterContext();
                _bvhMotion = context;
                context.Parse(path);
                context.Load();
                if (context.Avatar == null || context.Avatar.isValid == false)
                {
                    if (context.Root != null) { Destroy(context.Root); }
                    throw new Exception("BVH importer failed");
                }

                // Send BVH 
                _informationUpdate.SetBVH(_bvhMotion.Root);

                SetMotion(context.Root.GetComponent<HumanPoseTransfer>());
            }
            catch (Exception e)
            {
                if (_bvhMotion.Root == true) { Destroy(_bvhMotion.Root); }
                _errorMessagePanel.SetMessage(MultipleLanguageSupport.BvhLoadErrorMessage + "\nError message: " + e.Message);
                throw;
            }

        }

        private void Update()
        {
            UIOperation();

            if (_proxy != null)
            {
                _proxy.Apply();
            }
        }

        private void UIOperation()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) // hide the panel
            {
                if (_canvasRoot != null) { _canvasRoot.SetActive(!_canvasRoot.activeSelf); }
            }
            // Pause the rendering scene
            if (Input.GetKeyDown(KeyCode.P))
            {
                _pause = !_pause;
                _pauseMessagePanel.gameObject.SetActive(_pause);
                Time.timeScale = _pause ? 0 : 1;
            }
            // Resume the normal activity
            if (Input.GetKeyDown(KeyCode.R) && _errorMessagePanel.gameObject.activeSelf == true)
            {
                _errorMessagePanel.gameObject.SetActive(false);
                LoadMotion(_bvhPathSaved);
            }
        }

        private void OnOpenClickedVRM()
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("VRM File", ".vrm", ".VRM"));
            FileBrowser.SetDefaultFilter(".vrm");
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);
            StartCoroutine(ShowLoadDialogModel());
            if (string.IsNullOrEmpty(_vrmPath)) return;
            _errorMessagePanel.gameObject.SetActive(false);
        }

        private IEnumerator ShowLoadDialogModel()
	    {
		    yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files);
            //Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
            if (FileBrowser.Success)
            {
                _vrmPath = FileBrowser.Result[0];
                LoadModel(_vrmPath);
            }
            else
            {
                _vrmPath = null;
            }
        }

        private void OnOpenClickedBVH()
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("BVH File", ".bvh", ".BVH", ".txt"));
            FileBrowser.SetDefaultFilter(".bvh");
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);
            StartCoroutine(ShowLoadDialogMotion());
            if (string.IsNullOrEmpty(_bvhPath)) return;
            _errorMessagePanel.gameObject.SetActive(false);
        }

        private IEnumerator ShowLoadDialogMotion()
        {
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files);
            //Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
            if (FileBrowser.Success)
            {
                _bvhPath = FileBrowser.Result[0];
                LoadMotion(_bvhPath);
            }
            else
            {
                _bvhPath = null;
            }
        }

        private void LoadModel(string path)
        {
            try
            {
                // If BVH trigger is still on
                if (_bvhLoadingTrigger == true) { LoadMotion(_bvhPathSaved); }

                if (!File.Exists(path)) { return; }

                Debug.LogFormat("{0}", path);
                var parser = new GlbFileParser(path);
                var vrmData = new VRMData(parser.Parse());
                var context = new VRMImporterContext(vrmData);

                // GLTFにアクセスできます
                Debug.LogFormat("{0}", context.GLTF);

                // Call License Update function
                _licensePanel.LicenseUpdatefunc(context);

                // GLTFからモデルを生成します
                try
                {
                    var result = context.Load();
                    result.ShowMeshes();
                    result.EnableUpdateWhenOffscreen();
                    result.ShowMeshes();
                    _vrmModel = result.Root;
                    Debug.LogFormat("loaded {0}", _vrmModel.name);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }

                // Set up Model
                SetModel(_vrmModel);
            }
            catch (Exception e)
            {
                _errorMessagePanel.SetMessage(MultipleLanguageSupport.VrmLoadErrorMessage + "\nError message: " + e.Message);
                throw;
            }

        }

        private void SetModel(GameObject vrmModel)
        {
            // cleanup
            var loaded = _loadedBvhSourceOnAvatar;
            _loadedBvhSourceOnAvatar = null;

            if (loaded != null)
            {
                Debug.LogFormat("destroy {0}", loaded);
                Destroy(loaded.gameObject);
            }

            if (vrmModel != null)
            {
                // Set up expressions
                _facialExpressionPanel.CreateDynamicObject(_vrmModel);
                _informationUpdate.SetVRM(vrmModel);
                _proxy = vrmModel.GetComponent<VRMBlendShapeProxy>();

                // Set up LookAt
                var lookAt = vrmModel.GetComponent<VRMLookAtHead>();
                if (lookAt != null)
                {
                    _loadedBvhSourceOnAvatar = vrmModel.AddComponent<HumanPoseTransfer>();

                    _loadedBvhSourceOnAvatar.Source = _bvhSource;
                    _motionControlPanel.LoadedBvhSourceOnAvatar = _loadedBvhSourceOnAvatar;

                    if (_toggleMotionBVH.isOn)
                    {
                        _loadedBvhSourceOnAvatar.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
                    }
                    if (_toggleMotionTPose.isOn)
                    {
                        _loadedBvhSourceOnAvatar.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
                    }
                    if (_faceViewToggle.isOn)
                    {
                        _closeGameObject.FaceCameraPropertyActivateVRM();
                    }

                    _motionControlPanel.AssignAutoPlay(vrmModel);

                    if (_lookAtSphere.isOn)
                    {
                        lookAt.Target = _targetSphere.transform;
                    }
                    else if (_lookAtCamera.isOn)
                    {
                        lookAt.Target = _targetCamera.transform;
                    }
                    else
                    {
                        lookAt.Target = _targetSphere.transform;
                    }

                    lookAt.UpdateType = UpdateType.LateUpdate; // after HumanPoseTransfer's setPose

                    var animator = _vrmModel.GetComponent<Animator>();
                    var leftEye = OffsetOnTransform.Create(animator.GetBoneTransform(HumanBodyBones.LeftEye)).Transform;
                    var rightEye = OffsetOnTransform.Create(animator.GetBoneTransform(HumanBodyBones.RightEye)).Transform;

                    if (!(leftEye == null || rightEye == null))
                    {
                        _leftEyeSaved = leftEye;
                        _rightEyeSaved = rightEye;
                        _lookAtBoneFlag = true;

                        // Send information
                        _informationUpdate.SetVRM(_vrmModel);
                        _informationUpdate.SetBoneEyeTransform(_leftEyeSaved, _rightEyeSaved);
                        _informationUpdate.SetLookAtType(_lookAtBoneFlag);

                    }
                    else
                    {
                        if (_vrmModel.GetComponent<VRMLookAtBoneApplyer>() != null)
                        {
                            Destroy(_vrmModel.GetComponent<VRMLookAtBoneApplyer>());
                            _vrmModel.AddComponent<VRMLookAtBlendShapeApplyer>();
                        }

                        _lookAtBoneFlag = false;

                        // Send information
                        _informationUpdate.SetVRM(_vrmModel);
                        _informationUpdate.SetLookAtType(_lookAtBoneFlag);
                    }
                }
           
                // Set up FirstPerson
                var m_firstPerson = _vrmModel.GetComponent<VRMFirstPerson>();
                if (m_firstPerson != null) { m_firstPerson.Setup(); }
                if (_freeViewpointToggle.isOn) { _closeGameObject.EnableFirstPersonModeOption(); }
            }
        }

        private void SetMotion(HumanPoseTransfer src)
        {
            if (src.Avatar.isValid)  // check whether the source is valid
            {
                _bvhSource = src;
                src.GetComponent<Renderer>().enabled = false;
                _motionControlPanel.BvhSource = _bvhSource;
                _bvhLoadingTrigger = false;
                _bvhPathSaved = _bvhPathLocal;

                _motionControlPanel.EnableBvh();
                _toggleMotionBVH.isOn = true;
                _toggleMotionTPose.isOn = false;
            }
        }

    }
}
