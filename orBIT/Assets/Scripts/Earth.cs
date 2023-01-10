using UnityEngine;

public class Earth : MonoBehaviour
{

    private void Update()
    {
        var rotationVector = new Vector3(Difficulty.Instance.EarthSpeed * -1, 0, 0);
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
