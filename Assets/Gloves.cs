using UnityEngine;
//using UIButton = UnityEngine.UI.Button;
using Bhaptics.SDK2;
using System.Diagnostics;

namespace DefaultCompany
{
    public class Gloves : MonoBehaviour
    {

        public void TestHaptic()
        {
            BhapticsLibrary.Play("left_hand");
            BhapticsLibrary.Play("right_hand");
            

            /*BhapticsLibrary.PlayParam("left_hand",
                intensity: 1f,  // The value multiplied by the original value
                duration: 2f,   // The value multiplied by the original value
                angleX: 20f,    // The value that rotates around global Vector3.up(0~360f)
                offsetY: 0.3f   // The value to move up and down(-0.5~0.5)
                );

            BhapticsLibrary.PlayParam("right_hand",
                intensity: 1f,  // The value multiplied by the original value
                duration: 2f,   // The value multiplied by the original value
                angleX: 20f,    // The value that rotates around global Vector3.up(0~360f)
                offsetY: 0.3f   // The value to move up and down(-0.5~0.5)
                ); */

            //BhapticsLibrary.StopAll();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Avatar"))
            {
                Start();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            TestHaptic();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
