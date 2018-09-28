using UnityEngine;
using UnityEngine.UI;
using VRM;

namespace VRMViewer
{
    public class MessagePanel : MonoBehaviour
    {
        [SerializeField]
        private Text _messageText;

        [SerializeField]
        private Button _closeButton;

        private void Start()
        {
            _closeButton.onClick.AddListener(OnClose);
            this.gameObject.SetActive(false);
            
        }

        private void OnClose()
        {
            this.gameObject.SetActive(false);
        }

        public void SetMessage(string message)
        {
            this.gameObject.SetActive(true);
            _messageText.text = message;
        }
    }
}
