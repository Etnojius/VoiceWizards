using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class WandController : StorageItem
{
    public float damageMultiplier;
    public float speedMultiplier;
    public float sizeMultiplier;

    private void Start()
    {
        CustomStart();
        
    }

    public void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
            puPC.CastSpellServerRpc(args.text, transform.GetChild(0).position, transform.GetChild(0).rotation, damageMultiplier, speedMultiplier, sizeMultiplier);
    }
}
