using UnityEngine;

public class FaceCamera : MonoBehaviour
{

	void Update () {
        transform.rotation = Camera.main.transform.rotation;
    }
}
