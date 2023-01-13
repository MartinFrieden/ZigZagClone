using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Toggle autoRun;
    
        void Start()
        {
            autoRun.isOn = PlayerPrefs.GetInt("AutoRun", 0) != 0;
        }

        public void SwitchAutorun()
        {
            if (autoRun.isOn)
            {
                PlayerPrefs.SetInt("AutoRun", 1);
            }

            else
            {
                PlayerPrefs.SetInt("AutoRun", 0);
            }
            
        }
    
    }
}
