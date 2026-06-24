using UnityEngine;

[ExecuteAlways]
public class PortalMaterialSync : MonoBehaviour
{
    [Header("Materials")]
    public SpriteRenderer clipRenderrer;     // материал с обычным клиппингом
    public SpriteRenderer inverseRenderrer;  // материал с инверсным клиппингом

    private void LateUpdate()
    {
        if (clipRenderrer == null || inverseRenderrer == null)
            return;
        Material inverseMaterial = inverseRenderrer.sharedMaterial;
        Material clipMaterial = clipRenderrer.sharedMaterial;
        // Синхронизация трёх параметров
        inverseMaterial.SetVector("_PortalPos", clipMaterial.GetVector("_PortalPos"));
        inverseMaterial.SetVector("_PortalNormal", clipMaterial.GetVector("_PortalNormal"));

        if(clipMaterial.GetFloat("_ClipEnabled") == 0)
        {
            inverseRenderrer.enabled = false;
        }
        else
        {
            inverseRenderrer.enabled = true;
        }
    }
}