using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class Localization : MonoBehaviour
    {
        [SerializeField]
        [TextArea]
        public string Japnaese = "日本語";

        [SerializeField]
        [TextArea]
        public string English = "English";

        [SerializeField]
        [TextArea]
        public string SimplifiedChinese = "简体中文";

        [SerializeField]
        [TextArea]
        public string TraditionalChinese = "繁体中文";

        [SerializeField]
        [TextArea]
        public string Korean = "한국어";

        [SerializeField]
        private Text _text;

        private void Reset()
        {
            _text = GetComponent<Text>();
        }

        public void SetLanguage(LANGUAGES languages)
        {
            switch (languages)
            {
                case LANGUAGES.Japanese:
                    _text.text = Japnaese;
                    break;
                case LANGUAGES.English:
                    _text.text = English;
                    break;
                case LANGUAGES.SimplifiedChinese:
                    _text.text = SimplifiedChinese;
                    break;
                case LANGUAGES.TraditionalChinese:
                    _text.text = TraditionalChinese;
                    break;
                case LANGUAGES.Korean:
                    _text.text = Korean;
                    break;
            }
        }
    }
}