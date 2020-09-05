using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager s_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (s_Instance == null)
                s_Instance = FindObjectOfType<EffectManager>();
            return s_Instance;
        }
    }

    public enum EffectType
    {
        Common,
        Flesh
    }

    public ParticleSystem commonHitEffectPrefab;
    public ParticleSystem fleshHitEffectPrefab;

    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.Common)
    {
        ParticleSystem targetPrefab = commonHitEffectPrefab;
        if (effectType == EffectType.Flesh)
            targetPrefab = fleshHitEffectPrefab;

        ParticleSystem effect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal));
        if (parent != null)
            effect.transform.SetParent(parent);

        effect.Play();
    }
}