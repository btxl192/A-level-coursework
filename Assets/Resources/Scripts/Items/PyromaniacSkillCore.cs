using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyromaniacSkillCore : SkillCore {

	void Start ()
    {
        _Skills.Add(new ChainLightning(3));
	}

}
