using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP04 
{
    public class CoroutineTest : MonoBehaviour
    {
        private SmartCoroutine coroutine1, coroutine2;

        private void Start() 
        {
            coroutine1 = SmartCoroutine.Create(CoCoroutine1)
                            .OnAborted(() => { Debug.Log("Coroutine1 - Aborted"); })
                            .Start();
            coroutine2 = SmartCoroutine.Create(CoCoroutine2(10));

            SmartCoroutine.AfterDelay(2F, () => { Debug.Log("Delayed coroutine"); });
        }

        private IEnumerator CoCoroutine1() 
        {
            Debug.Log("Coroutine1 - Start");
            yield return new WaitForSeconds(1F);
            Debug.Log("Coroutine1 - End");
        }

        private IEnumerator CoCoroutine2(int arg) 
        {
            Debug.Log($"Coroutine2 {arg} - Start");
            yield return new WaitForSeconds(0.5f);

            coroutine1.Stop();
            
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Coroutine2 {arg} - End");
        }
    }
}