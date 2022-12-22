namespace UP08.Test
{
    using System.Collections;
    using UnityEngine;

    public class NewBehaviourScript : MonoBehaviour
    {
        public class A
        {
            public float a;
            public override string ToString()
            {
                return $"{a:F1}";
            }
        }
        CustomLinkedList<A> list = new CustomLinkedList<A>();

        IEnumerator Start()
        {
            var a1 = new A() { a = 1F };
            var a15 = new A() { a = 1.5f };
            var a2 = new A() { a = 2F };
            var a3 = new A() { a = 3F };

            Debug.Log($"#1 {list.ToString()}");

            list.AddLast(a1);
            list.AddLast(a2);
            list.AddLast(a3);
            Debug.Log($"#2 {list.ToString()}");

            list.Add(a15);
            Debug.Log($"#3 {list.ToString()}");

            list.Remove(a2);
            Debug.Log($"#3 {list.ToString()}");

            list.Remove(a1);
            Debug.Log($"#4 {list.ToString()}");

            list.Remove(a15);
            list.Remove(a3);
            Debug.Log($"#5 {list.ToString()}");

            Debug.Log("-----");


            list.AddLast(a1);
            list.AddLast(a15);
            list.AddLast(a2);
            list.AddLast(a3);

            for (var iter = list.Begin; iter != null; ++iter)
            {
                Debug.Log(iter.Current.Data.ToString());
            }


            Debug.Log("-----");

            SceneController.Open("MainScene");
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Open("SubScene");
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Open("AdditiveScene1", SceneController.LoadSceneMode.Additive);
            SceneController.Open("AdditiveScene2", SceneController.LoadSceneMode.Additive);
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Close("AdditiveScene1");
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Open("AdditiveScene1", SceneController.LoadSceneMode.Additive);
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Open("MainScene");
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            SceneController.Open("AdditiveScene1", SceneController.LoadSceneMode.Additive);
            SceneController.Open("AdditiveScene2", SceneController.LoadSceneMode.Additive);
            Debug.Log($"Current Scene: {SceneController.Current.Name}");

            yield return null;
            while (!Input.GetKeyUp(KeyCode.Return)) yield return null;

            Debug.Log(SceneController.Addition.ToString());

            SceneController.Close();
        }
    }
}