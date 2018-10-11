#pragma warning disable 0414
using System;
using System.IO;
using UnityEngine;
#if (NET_4_6 && UNITY_2017_1_OR_NEWER)
using System.Threading.Tasks;
#endif


namespace VRM
{
    public class VRMRuntimeLoaderNet4 : MonoBehaviour
    {
        [SerializeField, Header("GUI")]
        CanvasManager m_canvas;

        [SerializeField]
        LookTarget m_faceCamera;

        [SerializeField, Header("loader")]
        UniHumanoid.HumanPoseTransfer m_source;

        [SerializeField]
        UniHumanoid.HumanPoseTransfer m_target;

        [SerializeField, Header("runtime")]
        VRMFirstPerson m_firstPerson;

#if (NET_4_6 && UNITY_2017_1_OR_NEWER)
        VRMBlendShapeProxy m_blendShape;

        void SetupTarget()
        {
            if (m_target != null)
            {
                m_target.Source = m_source;
                m_target.SourceType = UniHumanoid.HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;

                m_blendShape = m_target.GetComponent<VRMBlendShapeProxy>();

                m_firstPerson = m_target.GetComponent<VRMFirstPerson>();

                var animator = m_target.GetComponent<Animator>();
                if (animator != null)
                {
                    m_firstPerson.Setup();

                    if (m_faceCamera != null)
                    {
                        m_faceCamera.Target = animator.GetBoneTransform(HumanBodyBones.Head);
                    }
                }
            }
        }

        private void Awake()
        {
            SetupTarget();
        }

        private void Start()
        {
            if (m_canvas == null)
            {
                Debug.LogWarning("no canvas");
                return;
            }

            m_canvas.LoadVRMButton.onClick.AddListener(LoadVRMClicked);
            m_canvas.LoadBVHButton.onClick.AddListener(LoadBVHClicked);
        }

        // Byte列を得る
        async static Task<Byte[]> ReadBytesAsync(string path)
        {
            return File.ReadAllBytes(path);
        }

        async static Task<GameObject> LoadAsync(Byte[] bytes)
        {
            var context = new VRMImporterContext();

            // GLB形式でJSONを取得しParseします
            context.ParseGlb(bytes);

            try
            {
                // ParseしたJSONをシーンオブジェクトに変換していく
                await context.LoadAsyncTask();

                // T-Poseのモデルを表示したくない場合、ShowMeshesする前に準備する
                // ロード後に表示する
                context.ShowMeshes();

                return context.Root;
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
                // 関連するリソースを破棄する
                context.Destroy(true);
                throw;
            }
        }

        /// <summary>
        /// Taskで非同期にロードする例
        /// </summary>
        async void LoadVRMClicked()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open VRM", ".vrm");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var bytes = await ReadBytesAsync(path);

            var go = await LoadAsync(bytes);

            OnLoaded(go);
        }

        void LoadBVHClicked()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open BVH", ".bvh");
            if (!string.IsNullOrEmpty(path))
            {
                LoadBvh(path);
            }
#else
            LoadBvh(Application.dataPath + "/default.bvh");
#endif
        }

        void OnLoaded(GameObject root)
        {
            root.transform.SetParent(transform, false);

            // add motion
            var humanPoseTransfer = root.AddComponent<UniHumanoid.HumanPoseTransfer>();
            if (m_target != null)
            {
                GameObject.Destroy(m_target.gameObject);
            }
            m_target = humanPoseTransfer;
            SetupTarget();
        }

        void LoadBvh(string path)
        {
            Debug.LogFormat("ImportBvh: {0}", path);
            var context = new UniHumanoid.ImporterContext
            {
                Path = path
            };
            context.Load();

            if (m_source != null)
            {
                GameObject.Destroy(m_source.gameObject);
            }
            m_source = context.Root.GetComponent<UniHumanoid.HumanPoseTransfer>();

            SetupTarget();
        }
#endif
    }
}
