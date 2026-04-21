using UnityEngine;
using UnityEngine.UI;
using LottiePlugin;

[RequireComponent(typeof(RawImage))]
public class LottiePlayer : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private TextAsset animationJson;
    [SerializeField] private uint textureWidth = 256;
    [SerializeField] private uint textureHeight = 256;

    [Header("Playback")]
    [SerializeField] private bool loop = true;
    [SerializeField] private float speed = 1f;

    public int CurrentFrame => _animation?.CurrentFrame ?? 0;
    public int TotalFrames => _animation != null ? (int)_animation.TotalFramesCount : 0;
    public float Duration => _animation != null ? (float)_animation.DurationSeconds : 0f;
    public bool IsPlaying => _animation != null && _animation.IsPlaying;

    private RawImage _rawImage;
    private LottieAnimation _animation;
    private bool _isPlaying;
    private int _previousFrame;

    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        LoadAnimation();
    }

    private void OnDestroy()
    {
        _animation?.Dispose();
        _animation = null;
    }

    private void Update()
    {
        if (_animation == null || !_isPlaying)
            return;

        _animation.Update(speed);

        if ((!loop && CurrentFrame >= TotalFrames - 1) || _previousFrame > CurrentFrame)
        {
            _animation.DrawOneFrame(_previousFrame);
            Debug.Log("Draw One Frame! " + _previousFrame);
            _previousFrame = CurrentFrame;
            Stop();
            return;
        }

        if (_previousFrame == CurrentFrame)
        {
            return;
        }
        
        _previousFrame = CurrentFrame;
        _animation.DrawOneFrame(CurrentFrame);
    }

    public void Pause()
    {
        if (_animation == null) return;
        _isPlaying = false;
        _animation.Pause();
    }

    public void Stop()
    {
        if (_animation == null) return;
        _isPlaying = false;
        _animation.Stop();
        _previousFrame = 0;
    }

    public void PlayFromFrame(int frame)
    {
        if (_animation == null) return;

        frame = Mathf.Clamp(frame, 0, TotalFrames - 1);
        _animation.Pause();
        _animation.DrawOneFrame(frame);
        _animation.Play();
        _isPlaying = true;
        _previousFrame = frame;
    }

    public void SetNormalizedTime(float t)
    {
        if (_animation == null) return;

        t = Mathf.Clamp01(t);
        int frame = Mathf.RoundToInt(t * (TotalFrames - 1));
        _animation.DrawOneFrame(frame);
    }

    private void LoadAnimation()
    {
        if (animationJson == null)
            return;

        _animation?.Dispose();

        _animation = LottieAnimation.LoadFromJsonData(
            animationJson.text,
            string.Empty,
            textureWidth,
            textureHeight
        );

        _rawImage.texture = _animation.Texture;
        _animation.DrawOneFrame(0);
    }


    public void Play()
    {
        if (_animation == null) return;
        _animation.Play();
        _isPlaying = true;
        _previousFrame = 0;
    }

    public void LoadAnimationFromTextAsset(TextAsset asset)
    {
        if (asset == null) return;

        _animation?.Dispose();

        _animation = LottieAnimation.LoadFromJsonData(
            asset.text,
            string.Empty,
            textureWidth,
            textureHeight
        );

        if (_rawImage != null)
            _rawImage.texture = _animation.Texture;
    }

}
