using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class MultipleLanguageSupport : MonoBehaviour
    {
        [SerializeField]
        private GameObject _canvasRoot;

        [SerializeField]
        private GameObject _errorMessage;

        [SerializeField]
        private GameObject _pauseMessage;

        [SerializeField]
        private Button _japaneseButton;

        [SerializeField]
        private Button _englishButton;

        [SerializeField]
        private Button _schineseButton;

        [SerializeField]
        private Button _tchineseButton;

        [SerializeField]
        private Button _koreanButton;

        private GameObject _vrmModel = null;

        public GameObject VrmModel { set { _vrmModel = value; } }
        public static string VrmLoadErrorMessage = "Failed to load VRM, please check your file again";
        public static string BvhLoadErrorMessage = "Failed to load BVH, press R to refresh if the model freezes";

        private static IEnumerable<Transform> Traverse(Transform t)
        {
            yield return t;
            foreach (Transform child in t)
            {
                foreach (var x in Traverse(child))
                {
                    yield return x;
                }
            }
        }

        private void Start()
        {
            // Language option
            _japaneseButton.onClick.AddListener(Show_Japnaese_Text);
            _englishButton.onClick.AddListener(Show_English_Text);
            _schineseButton.onClick.AddListener(Show_sChinese_Text);
            _tchineseButton.onClick.AddListener(Show_tChinese_Text);
            _koreanButton.onClick.AddListener(Show_Korean_Text);
        }

        private void Show_Japnaese_Text()
        {
#if true
            if (_vrmModel == null)
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.Japanese); }

                    var initialLocalization = t.GetComponent<InitialLocalization>();
                    if (initialLocalization != null) { initialLocalization.SetLanguage(LANGUAGES.Japanese); }
                }
            }
            else
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.Japanese); }
                }
            }
#else
            // 1. Main Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MainPanelTitle").GetComponent<Text>().text = "メインメニュー";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenVRMText").GetComponent<Text>().text = "VRMモデル";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenBVHText").GetComponent<Text>().text = "BVHデータ";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlText").GetComponent<Text>().text = "モーション";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionText").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraViewText").GetComponent<Text>().text = "視点";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseInformationText").GetComponent<Text>().text = "モデル情報";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LanguageTitle").GetComponent<Text>().text = "言語";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text = "TabキーでUIを\n非表示・表示にできます";

            // 2. Motion control Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlTitle").GetComponent<Text>().text = "モーション";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BodyPoseText").GetComponent<Text>().text = "ボディポーズ";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BVHText").GetComponent<Text>().text = "BVHモーション";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TPoseText").GetComponent<Text>().text = "T-ポーズ";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EyeControlText").GetComponent<Text>().text = "視線制御";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookStraightAheadText").GetComponent<Text>().text = "目線操作オフ";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtCameraText").GetComponent<Text>().text = "カメラ目線";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtSphereText").GetComponent<Text>().text = "球体を見る";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "HideSphereText").GetComponent<Text>().text = "球体を隠す";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OrbitalText").GetComponent<Text>().text = "軌道半径";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VerticalPositionText").GetComponent<Text>().text = "垂直位置";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BlendShapeText").GetComponent<Text>().text = "BlendShapeテスト";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableAutoBlinkText").GetComponent<Text>().text = "自動まばたき";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableLipSyncText").GetComponent<Text>().text = "口パク: あいうえお";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlBackButtonText").GetComponent<Text>().text = "戻る";

            // 3. Expression panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionTitle").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionBackButtonText").GetComponent<Text>().text = "戻る";

            // 4. Viewpoint panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointTitle").GetComponent<Text>().text = "視点";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FreeViewpointText").GetComponent<Text>().text = "自由視点";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FirstPersonModeText").GetComponent<Text>().text = "一人称描画";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text = "マウス:\n左クリック・右クリック: 回転\nホイール: 平行移動";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text = "キーボード:\nW: 前へ\nS: 後ろへ\nA: 左へ\nD: 右へ\nP: ポーズ\nO: 原点に戻る\nTab: UIを非表示・表示する";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AutoChangedViewpointText").GetComponent<Text>().text = "自動視点変更";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FaceCameraText").GetComponent<Text>().text = "フェイスクローズアップ";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "DistanceFromFace").GetComponent<Text>().text = "距離";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointBackButtonText").GetComponent<Text>().text = "戻る";

            // 5. License panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTitle").GetComponent<Text>().text = "モデル情報";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Title").GetComponent<Text>().text = "タイトル";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Version").GetComponent<Text>().text = "バージョン";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Author").GetComponent<Text>().text = "作者";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Contact").GetComponent<Text>().text = "連絡先";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Reference").GetComponent<Text>().text = "参照";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AvatarPermission").GetComponent<Text>().text = "アバターの人格に関する許諾範囲";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUser").GetComponent<Text>().text = "許諾範囲";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Violent").GetComponent<Text>().text = "暴力表現";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sexual").GetComponent<Text>().text = "性的表現";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Commercial").GetComponent<Text>().text = "商用利用";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Other").GetComponent<Text>().text = "その他の条件";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseRules").GetComponent<Text>().text = "再配布・改変に関する許諾範囲";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseType").GetComponent<Text>().text = "ライセンス";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicense").GetComponent<Text>().text = "その他の条件";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseBackButtonText").GetComponent<Text>().text = "戻る";

            if (_vrmModel == null)
            {
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TitleDynamicText").GetComponent<Text>().text = "タイトル";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VersionDynamicText").GetComponent<Text>().text = "バージョン";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AuthorDynamicText").GetComponent<Text>().text = "作者";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ContactDynamicText").GetComponent<Text>().text = "連絡先";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ReferenceDynamicText").GetComponent<Text>().text = "参照";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUserDynamicText").GetComponent<Text>().text = "許諾範囲";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViolentDynamicText").GetComponent<Text>().text = "暴力表現";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SexualDynamicText").GetComponent<Text>().text = "性的表現";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CommercialDynamicText").GetComponent<Text>().text = "商用利用";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherDynamicText").GetComponent<Text>().text = "その他の条件";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTypeDynamicText").GetComponent<Text>().text = "ライセンス";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicenseDynamicText").GetComponent<Text>().text = "その他の条件";
            }
#endif

            // Pause Message
            _pauseMessage.GetComponent<Text>().text = "プログラムを一時停止しています";
        }

        private void Show_English_Text()
        {
#if true
            if (_vrmModel == null)
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.English); }

                    var initialLocalization = t.GetComponent<InitialLocalization>();
                    if (initialLocalization != null) { initialLocalization.SetLanguage(LANGUAGES.English); }
                }
            }
            else
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.English); }
                }
            }
#else
            // 1. Main Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MainPanelTitle").GetComponent<Text>().text = "Main Menu";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenVRMText").GetComponent<Text>().text = "VRM Model";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenBVHText").GetComponent<Text>().text = "BVH Data";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlText").GetComponent<Text>().text = "Motion";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionText").GetComponent<Text>().text = "Expression";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraViewText").GetComponent<Text>().text = "Viewpoint";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseInformationText").GetComponent<Text>().text = "Model Information";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LanguageTitle").GetComponent<Text>().text = "Language";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text = "Press Tab key to\nundisplay / display panel";

            // 2. Motion control Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlTitle").GetComponent<Text>().text = "Motion";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BodyPoseText").GetComponent<Text>().text = "Body Pose";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BVHText").GetComponent<Text>().text = "BVH motion";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TPoseText").GetComponent<Text>().text = "T-Pose";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EyeControlText").GetComponent<Text>().text = "Eye Control";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookStraightAheadText").GetComponent<Text>().text = "Look straight ahead";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtCameraText").GetComponent<Text>().text = "Look at camera";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtSphereText").GetComponent<Text>().text = "Look at sphere";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "HideSphereText").GetComponent<Text>().text = "Hide sphere";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OrbitalText").GetComponent<Text>().text = "Orbital radius";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VerticalPositionText").GetComponent<Text>().text = "Vertical position";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BlendShapeText").GetComponent<Text>().text = "BlendShape Test";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableAutoBlinkText").GetComponent<Text>().text = "Auto-blink";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableLipSyncText").GetComponent<Text>().text = "Lip-sync: aa-ih-ou-E-oh";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlBackButtonText").GetComponent<Text>().text = "Back";

            // 3. Expression panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionTitle").GetComponent<Text>().text = "Expression";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionBackButtonText").GetComponent<Text>().text = "Back";

            // 4. Viewpoint panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointTitle").GetComponent<Text>().text = "Camera Viewpoint";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FreeViewpointText").GetComponent<Text>().text = "Free viewpoint";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FirstPersonModeText").GetComponent<Text>().text = "Render model in first-person";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text = "Mouse:\nLeft / right button: rotation\nWheel: translation";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text = "Keyboard:\nW: forward\nS: backward\nA: left\nD: right\nP: pause\nO: original position\nTab: undisplay / display panel";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AutoChangedViewpointText").GetComponent<Text>().text = "Auto-change viewpoint";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FaceCameraText").GetComponent<Text>().text = "Face close-up";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "DistanceFromFace").GetComponent<Text>().text = "Distance";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointBackButtonText").GetComponent<Text>().text = "Back";

            // 5. License panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTitle").GetComponent<Text>().text = "Model Information";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Title").GetComponent<Text>().text = "Title";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Version").GetComponent<Text>().text = "Version";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Author").GetComponent<Text>().text = "Author";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Contact").GetComponent<Text>().text = "Contact";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Reference").GetComponent<Text>().text = "Reference";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AvatarPermission").GetComponent<Text>().text = "Personation / Characterization Permission";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUser").GetComponent<Text>().text = "Allowed user";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Violent").GetComponent<Text>().text = "Violent acts";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sexual").GetComponent<Text>().text = "Sexual acts";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Commercial").GetComponent<Text>().text = "Commercial use";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Other").GetComponent<Text>().text = "Other";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseRules").GetComponent<Text>().text = "Redistribution / Modifications License";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseType").GetComponent<Text>().text = "License";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicense").GetComponent<Text>().text = "Other license";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseBackButtonText").GetComponent<Text>().text = "Back";

            if(_vrmModel == null)
            {
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TitleDynamicText").GetComponent<Text>().text = "Title";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VersionDynamicText").GetComponent<Text>().text = "Version";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AuthorDynamicText").GetComponent<Text>().text = "Author";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ContactDynamicText").GetComponent<Text>().text = "Contact information";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ReferenceDynamicText").GetComponent<Text>().text = "Reference";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUserDynamicText").GetComponent<Text>().text = "Allowed user";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViolentDynamicText").GetComponent<Text>().text = "Violent acts";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SexualDynamicText").GetComponent<Text>().text = "Sexual acts";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CommercialDynamicText").GetComponent<Text>().text = "Commercial use";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherDynamicText").GetComponent<Text>().text = "Other";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTypeDynamicText").GetComponent<Text>().text = "License";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicenseDynamicText").GetComponent<Text>().text = "Other license url";
            }
#endif

            // Pause Message
            _pauseMessage.GetComponent<Text>().text = "The program is paused";
        }

        private void Show_sChinese_Text()
        {
#if true
            if (_vrmModel == null)
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.SimplifiedChinese); }

                    var initialLocalization = t.GetComponent<InitialLocalization>();
                    if (initialLocalization != null) { initialLocalization.SetLanguage(LANGUAGES.SimplifiedChinese); }
                }
            }
            else
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.SimplifiedChinese); }
                }
            }
#else
            // 1. Main Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MainPanelTitle").GetComponent<Text>().text = "主菜单";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenVRMText").GetComponent<Text>().text = "VRM模型";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenBVHText").GetComponent<Text>().text = "BVH数据";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlText").GetComponent<Text>().text = "动作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionText").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraViewText").GetComponent<Text>().text = "视线";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseInformationText").GetComponent<Text>().text = "模型数据";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LanguageTitle").GetComponent<Text>().text = "语言";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text = "按Tab键\n隐藏或显示介面";

            // 2. Motion control Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlTitle").GetComponent<Text>().text = "动作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BodyPoseText").GetComponent<Text>().text = "身体姿势";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BVHText").GetComponent<Text>().text = "BVH动作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TPoseText").GetComponent<Text>().text = "T-Pose";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EyeControlText").GetComponent<Text>().text = "视线控制";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookStraightAheadText").GetComponent<Text>().text = "看向前方";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtCameraText").GetComponent<Text>().text = "看向相机";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtSphereText").GetComponent<Text>().text = "看向球体";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "HideSphereText").GetComponent<Text>().text = "隐藏球体";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OrbitalText").GetComponent<Text>().text = "环绕半径";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VerticalPositionText").GetComponent<Text>().text = "垂直高度";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BlendShapeText").GetComponent<Text>().text = "BlendShape测试";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableAutoBlinkText").GetComponent<Text>().text = "自动眨眼";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableLipSyncText").GetComponent<Text>().text = "嘴形同步: aa-ih-ou-E-oh";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlBackButtonText").GetComponent<Text>().text = "返回";

            // 3. Expression panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionTitle").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionBackButtonText").GetComponent<Text>().text = "返回";

            // 4. Viewpoint panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointTitle").GetComponent<Text>().text = "视线";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FreeViewpointText").GetComponent<Text>().text = "自由视线";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FirstPersonModeText").GetComponent<Text>().text = "以第一人称渲染模型";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text = "鼠标:\n左键或右键: 旋转\n滚轮: 平移";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text = "键盘:\nW: 前\nS: 后\nA: 左\nD: 右\nP: 暂停\nO: 回到原位\nTab: 隐藏或显示介面";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AutoChangedViewpointText").GetComponent<Text>().text = "自動切换视线";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FaceCameraText").GetComponent<Text>().text = "脸部特写";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "DistanceFromFace").GetComponent<Text>().text = "距离";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointBackButtonText").GetComponent<Text>().text = "返回";

            // 5. License panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTitle").GetComponent<Text>().text = "模型数据";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Title").GetComponent<Text>().text = "标题";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Version").GetComponent<Text>().text = "版本";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Author").GetComponent<Text>().text = "作者";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Contact").GetComponent<Text>().text = "联系方式";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Reference").GetComponent<Text>().text = "参考";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AvatarPermission").GetComponent<Text>().text = "人物模型权限";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUser").GetComponent<Text>().text = "允许的使用者";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Violent").GetComponent<Text>().text = "暴力表现";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sexual").GetComponent<Text>().text = "性表现";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Commercial").GetComponent<Text>().text = "商业利用";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Other").GetComponent<Text>().text = "其他条件";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseRules").GetComponent<Text>().text = "二次配布 / 修改许可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseType").GetComponent<Text>().text = "许可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicense").GetComponent<Text>().text = "其他许可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseBackButtonText").GetComponent<Text>().text = "返回";

            if (_vrmModel == null)
            {
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TitleDynamicText").GetComponent<Text>().text = "标题";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VersionDynamicText").GetComponent<Text>().text = "版本";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AuthorDynamicText").GetComponent<Text>().text = "作者";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ContactDynamicText").GetComponent<Text>().text = "联系方式";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ReferenceDynamicText").GetComponent<Text>().text = "参考";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUserDynamicText").GetComponent<Text>().text = "允许的使用者";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViolentDynamicText").GetComponent<Text>().text = "暴力表现";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SexualDynamicText").GetComponent<Text>().text = "性表现";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CommercialDynamicText").GetComponent<Text>().text = "商业利用";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherDynamicText").GetComponent<Text>().text = "其他条件";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTypeDynamicText").GetComponent<Text>().text = "许可";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicenseDynamicText").GetComponent<Text>().text = "其他许可";
            }
#endif

            // Pause Message
            _pauseMessage.GetComponent<Text>().text = "程式暫停中";
        }

        private void Show_tChinese_Text()
        {
#if true
            if (_vrmModel == null)
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.TraditionalChinese); }

                    var initialLocalization = t.GetComponent<InitialLocalization>();
                    if (initialLocalization != null) { initialLocalization.SetLanguage(LANGUAGES.TraditionalChinese); }
                }
            }
            else
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.TraditionalChinese); }
                }
            }
#else
            // 1. Main Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MainPanelTitle").GetComponent<Text>().text = "主選單";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenVRMText").GetComponent<Text>().text = "VRM模型";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenBVHText").GetComponent<Text>().text = "BVH數據";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlText").GetComponent<Text>().text = "動作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionText").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraViewText").GetComponent<Text>().text = "視點";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseInformationText").GetComponent<Text>().text = "模型資訊";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LanguageTitle").GetComponent<Text>().text = "語言";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text = "按Tab鍵\n隱藏或顯示介面";

            // 2. Motion control Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlTitle").GetComponent<Text>().text = "動作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BodyPoseText").GetComponent<Text>().text = "身體姿勢";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BVHText").GetComponent<Text>().text = "BVH動作";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TPoseText").GetComponent<Text>().text = "T-姿勢";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EyeControlText").GetComponent<Text>().text = "視線控制";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookStraightAheadText").GetComponent<Text>().text = "看正前方";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtCameraText").GetComponent<Text>().text = "看相機";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtSphereText").GetComponent<Text>().text = "看球體";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "HideSphereText").GetComponent<Text>().text = "隱藏球體";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OrbitalText").GetComponent<Text>().text = "環繞半徑";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VerticalPositionText").GetComponent<Text>().text = "垂直高度";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BlendShapeText").GetComponent<Text>().text = "BlendShape測試";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableAutoBlinkText").GetComponent<Text>().text = "自動眨眼";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableLipSyncText").GetComponent<Text>().text = "對嘴: aa-ih-ou-E-oh";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlBackButtonText").GetComponent<Text>().text = "返回";

            // 3. Expression panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionTitle").GetComponent<Text>().text = "表情";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionBackButtonText").GetComponent<Text>().text = "返回";

            // 4. Viewpoint panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointTitle").GetComponent<Text>().text = "視點";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FreeViewpointText").GetComponent<Text>().text = "自由視點";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FirstPersonModeText").GetComponent<Text>().text = "以第一人稱渲染模型";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text = "滑鼠:\n左鍵或右鍵: 旋轉\n滾輪: 平移";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text = "鍵盤:\nW: 往前\nS: 往後\nA: 往左\nD: 往右\nP: 暫停\nO: 回到原點\nTab: 隱藏或顯示介面";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AutoChangedViewpointText").GetComponent<Text>().text = "自動變換視點";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FaceCameraText").GetComponent<Text>().text = "臉部特寫";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "DistanceFromFace").GetComponent<Text>().text = "距離";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointBackButtonText").GetComponent<Text>().text = "返回";

            // 5. License panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTitle").GetComponent<Text>().text = "模型資訊";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Title").GetComponent<Text>().text = "標題";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Version").GetComponent<Text>().text = "版本";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Author").GetComponent<Text>().text = "作者";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Contact").GetComponent<Text>().text = "聯絡資訊";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Reference").GetComponent<Text>().text = "参考";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AvatarPermission").GetComponent<Text>().text = "阿凡達的人格許可範圍";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUser").GetComponent<Text>().text = "允許的使用者";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Violent").GetComponent<Text>().text = "暴力表現";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sexual").GetComponent<Text>().text = "性的表現";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Commercial").GetComponent<Text>().text = "商業利用";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Other").GetComponent<Text>().text = "其他條件";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseRules").GetComponent<Text>().text = "再散佈 / 修改許可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseType").GetComponent<Text>().text = "許可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicense").GetComponent<Text>().text = "其他許可";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseBackButtonText").GetComponent<Text>().text = "返回";

            if (_vrmModel == null)
            {
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TitleDynamicText").GetComponent<Text>().text = "標題";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VersionDynamicText").GetComponent<Text>().text = "版本";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AuthorDynamicText").GetComponent<Text>().text = "作者";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ContactDynamicText").GetComponent<Text>().text = "聯絡資訊";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ReferenceDynamicText").GetComponent<Text>().text = "参考";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUserDynamicText").GetComponent<Text>().text = "允許的使用者";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViolentDynamicText").GetComponent<Text>().text = "暴力表現";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SexualDynamicText").GetComponent<Text>().text = "性的表現";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CommercialDynamicText").GetComponent<Text>().text = "商業利用";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherDynamicText").GetComponent<Text>().text = "其他條件";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTypeDynamicText").GetComponent<Text>().text = "許可";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicenseDynamicText").GetComponent<Text>().text = "其他許可";
            }
#endif

            // Pause Message
            _pauseMessage.GetComponent<Text>().text = "程式暫停中";
        }

        private void Show_Korean_Text()
        {
#if true
            if (_vrmModel == null)
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.Korean); }

                    var initialLocalization = t.GetComponent<InitialLocalization>();
                    if (initialLocalization != null) { initialLocalization.SetLanguage(LANGUAGES.Korean); }
                }
            }
            else
            {
                foreach (var t in Traverse(_canvasRoot.transform))
                {
                    var localization = t.GetComponent<Localization>();
                    if (localization != null) { localization.SetLanguage(LANGUAGES.Korean); }
                }
            }
#else
            // 1. Main Panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MainPanelTitle").GetComponent<Text>().text = "메인메뉴";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenVRMText").GetComponent<Text>().text = "VRM 모델";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OpenBVHText").GetComponent<Text>().text = "BVH 모션데이터";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlText").GetComponent<Text>().text = "모션";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionText").GetComponent<Text>().text = "표정";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraViewText").GetComponent<Text>().text = "시점";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseInformationText").GetComponent<Text>().text = "모델정보";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LanguageTitle").GetComponent<Text>().text = "언어";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "PanelDisaplyText").GetComponent<Text>().text = "Tab키를 눌러서 \n패널을 숨기거나 보이게 할 수 있습니다";

            // 2. Motion control 
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlTitle").GetComponent<Text>().text = "모션";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BodyPoseText").GetComponent<Text>().text = "몸 포즈";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BVHText").GetComponent<Text>().text = "BVH 모션";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TPoseText").GetComponent<Text>().text = "T-포즈";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EyeControlText").GetComponent<Text>().text = "눈 움직임";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookStraightAheadText").GetComponent<Text>().text = "정면 쳐다보기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtCameraText").GetComponent<Text>().text = "카메라 쳐다보기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LookAtSphereText").GetComponent<Text>().text = "떠다니는 구슬 쳐다보기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "HideSphereText").GetComponent<Text>().text = "구슬 숨기기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OrbitalText").GetComponent<Text>().text = "궤도 반경";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VerticalPositionText").GetComponent<Text>().text = "세로 위치";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "BlendShapeText").GetComponent<Text>().text = "BlendShape 테스트";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableAutoBlinkText").GetComponent<Text>().text = "자동 눈 깜빡이기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "EnableLipSyncText").GetComponent<Text>().text = "입모양: 아이우에오";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MotionControlBackButtonText").GetComponent<Text>().text = "뒤로";

            // 3. Expression panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionTitle").GetComponent<Text>().text = "표정";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FacialExpressionBackButtonText").GetComponent<Text>().text = "뒤로";

            // 4. Viewpoint panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointTitle").GetComponent<Text>().text = "카메라 시점";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FreeViewpointText").GetComponent<Text>().text = "자유시점";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FirstPersonModeText").GetComponent<Text>().text = "1인칭으로 모델 렌더링";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "MouseOperationText").GetComponent<Text>().text = "마우스:\n왼쪽 / 오른쪽버튼: 회전\n휠버튼: 직선이동";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text.Replace("\\n", "\n");
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "KeyboardOperationText").GetComponent<Text>().text = "키보드:\nW: 앞으로\nS: 뒤로\nA: 왼쪽\nD: 오른쪽\nP: 일시정지\nO: 원 위치\nTab: 패널 숨기기";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AutoChangedViewpointText").GetComponent<Text>().text = "카메라 시점 자동 전환";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "FaceCameraText").GetComponent<Text>().text = "얼굴 클로즈업";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "DistanceFromFace").GetComponent<Text>().text = "얼굴과의 거리";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViewpointBackButtonText").GetComponent<Text>().text = "뒤로";

            // 5. License panel
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTitle").GetComponent<Text>().text = "모델정보";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Title").GetComponent<Text>().text = "이름";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Version").GetComponent<Text>().text = "버전";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Author").GetComponent<Text>().text = "제작자";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Contact").GetComponent<Text>().text = "연락처";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Reference").GetComponent<Text>().text = "참조";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AvatarPermission").GetComponent<Text>().text = "표현에 대한 허가";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUser").GetComponent<Text>().text = "사용허가된 자";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Violent").GetComponent<Text>().text = "폭력묘사";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Sexual").GetComponent<Text>().text = "성적묘사";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Commercial").GetComponent<Text>().text = "상업적이용";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Other").GetComponent<Text>().text = "기타";

            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseRules").GetComponent<Text>().text = "재배포 · 변경에 관한 허가";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseType").GetComponent<Text>().text = "라이센스";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicense").GetComponent<Text>().text = "기타 라이센스";
            _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseBackButtonText").GetComponent<Text>().text = "뒤로";

            if(_vrmModel == null)
            {
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "TitleDynamicText").GetComponent<Text>().text = "이름";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "VersionDynamicText").GetComponent<Text>().text = "버전";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AuthorDynamicText").GetComponent<Text>().text = "제작자";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ContactDynamicText").GetComponent<Text>().text = "연락처";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ReferenceDynamicText").GetComponent<Text>().text = "참조";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "AllowedUserDynamicText").GetComponent<Text>().text = "사용허가된 자";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "ViolentDynamicText").GetComponent<Text>().text = "폭력묘사";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SexualDynamicText").GetComponent<Text>().text = "성적묘사";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CommercialDynamicText").GetComponent<Text>().text = "상업적이용";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherDynamicText").GetComponent<Text>().text = "기타";

                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LicenseTypeDynamicText").GetComponent<Text>().text = "라이센스";
                _canvasRoot.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "OtherLicenseDynamicText").GetComponent<Text>().text = "기타 라이센스 URL";
            }
#endif

            // Pause Message
            _pauseMessage.GetComponent<Text>().text = "일시정지 중입니다";
        }

    }
}