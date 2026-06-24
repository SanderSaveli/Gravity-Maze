using SanderSaveli.GravityMaze;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(PortalNormal))]
public class PortalClipController : MonoBehaviour
{
    [SerializeField] private PortalController _portalController;
    [Header("Affected Renderers")]
    public SpriteRenderer[] affectedRenderers;

    [Header("Portal Reference")]
    public PortalNormal portal;

    [Header("Params")]
    [SerializeField] private bool _isDebugMode;
    static readonly int PortalPosID = Shader.PropertyToID("_PortalPos");
    static readonly int PortalNormalID = Shader.PropertyToID("_PortalNormal");
    static readonly int ClipEnabledID = Shader.PropertyToID("_ClipEnabled");

    private bool _isActive;

    private void Reset()
    {
        portal = GetComponent<PortalNormal>();
        _portalController = GetComponent<PortalController>();
    }

    private void OnEnable()
    {
        _portalController.OnPrepareForTeleport += SetActive;
    }

    private void OnDisable()
    {
        _portalController.OnPrepareForTeleport -= SetActive;
    }

    private void SetActive(bool active)
    {
        _isActive = active;
        foreach (SpriteRenderer r in affectedRenderers)
        {
            if (r == null) continue;

            Material mat = r.sharedMaterial;
            mat.SetFloat(ClipEnabledID, _isActive || _isDebugMode ? 1f : 0f);
            Debug.Log("Set clip enable: " + (_isActive || _isDebugMode));
        }
    }

    void Update()
    {
        if (!_isActive && !_isDebugMode)
        {
            return;
        }
        if (portal == null || affectedRenderers == null) return;

        Vector2 normal = portal.PortalNormalVector.normalized;
        Vector2 planePos = portal.PlanePosition;


        foreach (SpriteRenderer r in affectedRenderers)
        {
            if (r == null) continue;

            Material mat = r.sharedMaterial;
            mat.SetVector(PortalPosID, planePos);
            mat.SetVector(PortalNormalID, normal);
        }
    }
}