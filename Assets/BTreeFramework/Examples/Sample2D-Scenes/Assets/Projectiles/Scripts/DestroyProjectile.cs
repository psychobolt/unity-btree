using UnityEngine;

namespace Sample2D
{
    public class DestroyProjectile : MonoBehaviour {

	    // Use this for initialization
	    void Start () {
            Destroy(gameObject, 3.0f);
        }
    }
}
