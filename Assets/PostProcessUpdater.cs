using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessUpdater : MonoBehaviour
{

    public Volume volume;
    private LiftGammaGain gamma;


    public void UpdateGamma()
    {
        VolumeProfile profile = volume.sharedProfile;


        volume.profile.TryGet(out gamma);
        gamma.gamma.value = new Vector4(1f,1f,1f, PlayerPrefs.GetFloat("BRIGHT"));
        //gamma.gamma.value = new Vector4(1f, 1f, 1f, 2);
    }

    private void OnEnable()
    {
        UpdateGamma();
    }


}



