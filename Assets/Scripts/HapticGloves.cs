using UnityEngine;
//using UIButton = UnityEngine.UI.Button;
using Bhaptics.SDK2;
namespace DefaultCompany
{
    public class HapticGloves : MonoBehaviour
    {
        
        public void TestHaptic()
        {
            BhapticsLibrary.PlayParam(BhapticsEvent.PUNCH,
                intensity: 0.3f,  // The value multiplied by the original value
                duration: 2f,   // The value multiplied by the original value
                angleX: 20f,    // The value that rotates around global Vector3.up(0~360f)
                offsetY: 0.3f   // The value to move up and down(-0.5~0.5)
                );

            //BhapticsLibrary.StopAll();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.name == "Cube")
            {
                Debug.LogFormat("<color=yellow>touching cube</color>");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.name == "Cube")
            {
                Debug.LogFormat("<color=orange>not touching cube</color>");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //TestHaptic();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
