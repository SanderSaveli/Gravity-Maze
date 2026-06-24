using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class PortalSystem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PortalController _firstPortal;
        [SerializeField] private PortalController _secondPortal;
        private SpriteRenderer[] _affectedRenderers;

        [Header("Parameters")]
        [SerializeField] private RenderTexture _firstTexture;
        [SerializeField] private RenderTexture _secondTexture;
        private ILevelProvider _levelProvider;

        private void OnEnable()
        {
            _levelProvider = GetComponentInParent<LevelProvider>();
            _affectedRenderers = new SpriteRenderer[1];
            _affectedRenderers[0] = _levelProvider.Player.GetComponent<SpriteRenderer>();

            _firstPortal.CanvasRender.SetRender(_firstTexture, _secondTexture);
            _secondPortal.CanvasRender.SetRender(_secondTexture, _firstTexture);

            _firstPortal.ClipController.affectedRenderers = _affectedRenderers;
            _secondPortal.ClipController.affectedRenderers = _affectedRenderers;

            _firstPortal.PortalTeleportator.LinkedPortal = _secondPortal.PortalTeleportator;
            _secondPortal.PortalTeleportator.LinkedPortal = _firstPortal.PortalTeleportator;
        }
    }
}
