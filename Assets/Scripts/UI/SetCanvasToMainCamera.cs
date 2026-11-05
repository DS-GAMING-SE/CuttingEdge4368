using UnityEngine;

public class SetCanvasToMainCamera : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    public void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        if (!canvas.worldCamera)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
