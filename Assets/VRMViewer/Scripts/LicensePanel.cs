using System;
using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class LicensePanel : MonoBehaviour
    {
        [Serializable]
        public struct TextFields
        {
            [SerializeField, Header("Info")]
            Text _textModelTitle;
            [SerializeField]
            Text _textModelVersion;
            [SerializeField]
            Text _textModelAuthor;
            [SerializeField]
            Text _textModelContact;
            [SerializeField]
            Text _textModelReference;
            [SerializeField]
            RawImage _thumbnail;

            [SerializeField, Header("CharacterPermission")]
            Text _textPermissionAllowed;
            [SerializeField]
            Text _textPermissionViolent;
            [SerializeField]
            Text _textPermissionSexual;
            [SerializeField]
            Text _textPermissionCommercial;
            [SerializeField]
            Text _textPermissionOther;

            [SerializeField, Header("DistributionLicense")]
            Text _textDistributionLicense;
            [SerializeField]
            Text _textDistributionOther;

            public void LicenseUpdate(VRMImporterContext context)
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

                _textModelTitle.text = meta.Title;
                _textModelVersion.text = meta.Version;
                _textModelAuthor.text = meta.Author;
                _textModelContact.text = meta.ContactInformation;
                _textModelReference.text = meta.Reference;

                _textPermissionAllowed.text = meta.AllowedUser.ToString();
                _textPermissionViolent.text = meta.ViolentUssage.ToString();
                _textPermissionSexual.text = meta.SexualUssage.ToString();
                _textPermissionCommercial.text = meta.CommercialUssage.ToString();
                _textPermissionOther.text = meta.OtherPermissionUrl;

                _textDistributionLicense.text = meta.LicenseType.ToString();
                _textDistributionOther.text = meta.OtherLicenseUrl;

                _thumbnail.texture = meta.Thumbnail;
#endif
            }
        }

        [SerializeField]
        private TextFields _texts;

        public void LicenseUpdatefunc(VRMImporterContext context)
        {
            _texts.LicenseUpdate(context);
        }
    }

}
