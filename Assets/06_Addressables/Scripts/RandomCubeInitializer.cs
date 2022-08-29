namespace UP06
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RandomCubeInitializer : MonoBehaviour
    {
        [SerializeField]
        private Vector2 sizeRange;

        public void SetRandom()
        {
            transform.position = Vector3.up * 5F;
            transform.localEulerAngles = Random.insideUnitSphere * 360F;
            transform.localScale = new Vector3(GetRandomSize(), GetRandomSize(), GetRandomSize());

            Destroy(gameObject, 5F);

            float GetRandomSize()
            {
                return Random.Range(sizeRange.x, sizeRange.y);
            }
        }
    }
}