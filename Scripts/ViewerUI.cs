using System;
using System.IO;
using System.Linq;
using UniHumanoid;
using UnityEngine;
using UnityEngine.UI;


namespace VRM
{
    public class ViewerUI : MonoBehaviour
    {
        #region UI
        [SerializeField]
        Text m_version;

        [SerializeField]
        Button m_open;

        [SerializeField]
        Toggle m_enableLipSync;

        [SerializeField]
        Toggle m_enableAutoBlink;
        #endregion

        [SerializeField]
        HumanPoseTransfer m_src;

        [SerializeField]
        GameObject m_target;

        [SerializeField]
        GameObject Root;

        [Serializable]
        struct TextFields
        {
            [SerializeField, Header("Info")]
            Text m_textModelTitle;
            [SerializeField]
            Text m_textModelVersion;
            [SerializeField]
            Text m_textModelAuthor;
            [SerializeField]
            Text m_textModelContact;
            [SerializeField]
            Text m_textModelReference;
            [SerializeField]
            RawImage m_thumbnail;

            [SerializeField, Header("CharacterPermission")]
            Text m_textPermissionAllowed;
            [SerializeField]
            Text m_textPermissionViolent;
            [SerializeField]
            Text m_textPermissionSexual;
            [SerializeField]
            Text m_textPermissionCommercial;
            [SerializeField]
            Text m_textPermissionOther;

            [SerializeField, Header("DistributionLicense")]
            Text m_textDistributionLicense;
            [SerializeField]
            Text m_textDistributionOther;

            public void Start()
            {
                m_textModelTitle.text = "";
                m_textModelVersion.text = "";
                m_textModelAuthor.text = "";
                m_textModelContact.text = "";
                m_textModelReference.text = "";

                m_textPermissionAllowed.text = "";
                m_textPermissionViolent.text = "";
                m_textPermissionSexual.text = "";
                m_textPermissionCommercial.text = "";
                m_textPermissionOther.text = "";

                m_textDistributionLicense.text = "";
                m_textDistributionOther.text = "";
            }

            public void Update(VRMImporterContext context)
            {
#if false
                var meta = context.VRM.extensions.VRM.meta;
                m_textModelTitle.text = meta.title;
                m_textModelVersion.text = meta.version;
                m_textModelAuthor.text = meta.author;
                m_textModelContact.text = meta.contactInformation;
                m_textModelReference.text = meta.reference;

                m_textPermissionAllowed.text = meta.allowedUser.ToString();
                m_textPermissionViolent.text = meta.violentUssage.ToString();
                m_textPermissionSexual.text = meta.sexualUssage.ToString();
                m_textPermissionCommercial.text = meta.commercialUssage.ToString();
                m_textPermissionOther.text = meta.otherPermissionUrl;

                m_textDistributionLicense.text = meta.licenseType.ToString();
                m_textDistributionOther.text = meta.otherLicenseUrl;
#else
                var meta = context.ReadMeta(true);

                m_textModelTitle.text = meta.Title;
                m_textModelVersion.text = meta.Version;
                m_textModelAuthor.text = meta.Author;
                m_textModelContact.text = meta.ContactInformation;
                m_textModelReference.text = meta.Reference;

                m_textPermissionAllowed.text = meta.AllowedUser.ToString();
                m_textPermissionViolent.text = meta.ViolentUssage.ToString();
                m_textPermissionSexual.text = meta.SexualUssage.ToString();
                m_textPermissionCommercial.text = meta.CommercialUssage.ToString();
                m_textPermissionOther.text = meta.OtherPermissionUrl;

                m_textDistributionLicense.text = meta.LicenseType.ToString();
                m_textDistributionOther.text = meta.OtherLicenseUrl;

                m_thumbnail.texture = meta.Thumbnail;
#endif
            }
        }
        [SerializeField]
        TextFields m_texts;

        [Serializable]
        struct UIFields
        {
            [SerializeField]
            Toggle ToggleMotionTPose;

            [SerializeField]
            Toggle ToggleMotionBVH;

            [SerializeField]
            ToggleGroup ToggleMotion;

            Toggle m_activeToggleMotion;

            public void UpdateTogle(Action onBvh, Action onTPose)
            {
                var value = ToggleMotion.ActiveToggles().FirstOrDefault();
                if (value == m_activeToggleMotion)
                    return;

                m_activeToggleMotion = value;
                if (value == ToggleMotionTPose)
                {
                    onTPose();
                }
                else if (value == ToggleMotionBVH)
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
        UIFields m_ui;

        [SerializeField]
        HumanPoseClip m_pose;

        private void Reset()
        {
            var buttons = GameObject.FindObjectsOfType<Button>();
            m_open = buttons.First(x => x.name == "Open");

            var toggles = GameObject.FindObjectsOfType<Toggle>();
            m_enableLipSync = toggles.First(x => x.name == "EnableLipSync");
            m_enableAutoBlink = toggles.First(x => x.name == "EnableAutoBlink");

            var texts = GameObject.FindObjectsOfType<Text>();
            m_version = texts.First(x => x.name == "Version");

            m_src = GameObject.FindObjectOfType<HumanPoseTransfer>();

            m_target = GameObject.FindObjectOfType<TargetMover>().gameObject;
        }

        HumanPoseTransfer m_loaded;

        AIUEO m_lipSync;
        bool m_enableLipSyncValue;
        bool EnableLipSyncValue
        {
            set
            {
                if (m_enableLipSyncValue == value) return;
                m_enableLipSyncValue = value;
                if (m_lipSync != null)
                {
                    m_lipSync.enabled = m_enableLipSyncValue;
                }
            }
        }

        Blinker m_blink;
        bool m_enableBlinkValue;
        bool EnableBlinkValue
        {
            set
            {
                if (m_blink == value) return;
                m_enableBlinkValue = value;
                if (m_blink != null)
                {
                    m_blink.enabled = m_enableBlinkValue;
                }
            }
        }

        private void Start()
        {
            m_version.text = string.Format("VRMViewer {0}.{1}",
                VRMVersion.MAJOR, VRMVersion.MINOR);
            m_open.onClick.AddListener(OnOpenClicked);

            // load initial bvh
            LoadMotion(Application.streamingAssetsPath + "/test.txt");

            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length > 1)
            {
                LoadModel(cmds[1]);
            }

            m_texts.Start();
        }

        private void LoadMotion(string path)
        {
            var context = new UniHumanoid.ImporterContext
            {
                Path = path
            };
            UniHumanoid.BvhImporter.Import(context);
            SetMotion(context.Root.GetComponent<HumanPoseTransfer>());
        }

        private void Update()
        {
            EnableLipSyncValue = m_enableLipSync.isOn;
            EnableBlinkValue = m_enableAutoBlink.isOn;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Root != null) Root.SetActive(!Root.activeSelf);
            }

            m_ui.UpdateTogle(EnableBvh, EnableTPose);
        }

        void EnableBvh()
        {
            if (m_loaded != null)
            {
                m_loaded.Source = m_src;
                m_loaded.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
            }
        }

        void EnableTPose()
        {
            if (m_loaded != null)
            {
                m_loaded.PoseClip = m_pose;
                m_loaded.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
            }
        }

        void OnOpenClicked()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open vrm", "vrm");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            LoadModel(path);
        }

        void LoadModel(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            Debug.LogFormat("{0}", path);
            var bytes = File.ReadAllBytes(path);

            var context = new VRMImporterContext(path);

            // GLB形式でJSONを取得しParseします
            context.ParseGlb(bytes);

            // GLTFにアクセスできます
            Debug.LogFormat("{0}", context.GLTF);
            m_texts.Update(context);

            // GLTFからモデルを生成します
            GameObject go = null;

            try
            {
                VRMImporter.LoadFromBytes(context);
                go = context.Root;
                Debug.LogFormat("loaded {0}", go.name);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }

            SetModel(go);
        }

        void SetModel(GameObject go)
        {
            // cleanup
            var loaded = m_loaded;
            m_loaded = null;

            if (loaded != null)
            {
                Debug.LogFormat("destroy {0}", loaded);
                GameObject.Destroy(loaded.gameObject);
            }

            if (go != null)
            {
                m_loaded = go.AddComponent<HumanPoseTransfer>();

                m_loaded.Source = m_src;
                m_loaded.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;

                m_lipSync = go.AddComponent<AIUEO>();
                m_blink = go.AddComponent<Blinker>();

                var lookAt = go.GetComponent<VRMLookAtHead>();
                lookAt.Target = m_target.transform;
                lookAt.UpdateType = UpdateType.LateUpdate; // after HumanPoseTransfer's setPose
            }
        }

        void SetMotion(HumanPoseTransfer src)
        {
            m_src = src;
            src.GetComponent<Renderer>().enabled = false;

            EnableBvh();
        }
    }
}
