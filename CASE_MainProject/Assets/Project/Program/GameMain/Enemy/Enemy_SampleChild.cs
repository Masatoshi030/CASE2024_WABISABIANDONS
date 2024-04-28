using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SampleChild : Enemy_Parent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    // ƒLƒƒƒ‰‚É‚æ‚Á‚Ä”j‰óŒã‚Ì‹““®‚ª•Ï‚í‚é‚Æv‚¤‚Ì‚Åoverride‚Å‚æ‚ë‚µ‚­‚Å‚·
    public override void DestroyFunc()
    {
        Debug.Log("”j‰óŠÖ”");
    }
}
