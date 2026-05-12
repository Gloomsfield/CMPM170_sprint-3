using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ResultMessenger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textElement;

        private void Start()
        {
            textElement.text = GameManager.Instance.playtestResultMessage;
        }
    }
}