using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour {

	void Update ()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(this.gameObject);
        }
	}
}
