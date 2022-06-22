using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP04 
{
    public partial class SmartCoroutine
    {
        protected static SmartCoroutineInstance instance = null;


        private Coroutine coroutine;

        public bool IsRunning { get; protected set; }

        public System.Func<IEnumerator> Body { protected get; set; }

        public delegate void SmartCoroutineDelegate();
        public SmartCoroutineDelegate onFinished, onAborted;


        public SmartCoroutine()
        {
            if (instance == null)
            {
                instance = new GameObject("[SmartCoroutine]").AddComponent<SmartCoroutineInstance>();
                GameObject.DontDestroyOnLoad(instance.gameObject);
            }
        }

        public static SmartCoroutine Create(System.Func<IEnumerator> body = null)
        {
            return new SmartCoroutine() { Body = body };
        }

        public static SmartCoroutine Create(IEnumerator body)
        {
            return new SmartCoroutine().StartImmediate(body);
        }

        public SmartCoroutine OnFinished(SmartCoroutineDelegate callback)
        {
            onFinished += callback;
            return this;
        }
        public SmartCoroutine OnAborted(SmartCoroutineDelegate callback)
        {
            onAborted += callback;
            return this;
        }


        public virtual SmartCoroutine Start()
        {
            if (Body == null) throw new NoCoroutineBodyException();

            Stop();
            coroutine = instance.StartCoroutine(CoStart());

            return this;

            IEnumerator CoStart()
            {
                IsRunning = true;

                yield return Body();

                // Aborted
                if (!IsRunning) yield break;

                IsRunning = false;
                onFinished?.Invoke();
            }
        }
        public SmartCoroutine StartImmediate(IEnumerator body)
        {
            Stop();
            coroutine = instance.StartCoroutine(CoStart());

            return this;

            IEnumerator CoStart()
            {
                IsRunning = true;

                yield return body;

                // Aborted
                if (!IsRunning) yield break;

                IsRunning = false;
                onFinished?.Invoke();
            }
        }

        public virtual void Stop()
        {
            if (coroutine == null) return;
            if (!IsRunning) return;

            instance.StopCoroutine(coroutine);

            IsRunning = false;
            onAborted?.Invoke();
        }
    }
}