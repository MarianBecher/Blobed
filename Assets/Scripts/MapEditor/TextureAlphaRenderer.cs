using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class TextureAlphaRenderer : MonoBehaviour {
    [SerializeField]
    private RawImage targetImage;
    [SerializeField, Range(0,1)]
    private float targetAlpha;

    public float TargetAlpha { get { return targetAlpha; } set { targetAlpha = value; targetImage.color = new Color(1, 1, 1, targetAlpha); } }

    void Awake()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        renderTexture.isPowerOfTwo = false;
        this.GetComponent<Camera>().targetTexture = renderTexture;
        targetImage.texture = renderTexture;
        TargetAlpha = targetAlpha;
    }

    void OnValidate()
    {
        this.TargetAlpha = targetAlpha;
    }
}
