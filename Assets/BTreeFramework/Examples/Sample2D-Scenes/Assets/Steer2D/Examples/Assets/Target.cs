using UnityEngine;
using System.Collections.Generic;

public class Target : MonoBehaviour {

	public Steer2D.Seek AgentSeek;
    public Steer2D.Arrive AgentArrive;

    void Start()
    {
        if (AgentSeek != null)
            AgentSeek.TargetPoint = transform.position;

        if (AgentArrive != null)
            AgentArrive.TargetPoint = transform.position;
    }

	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            transform.position = position;

            if (AgentSeek != null)
                AgentSeek.TargetPoint = position;

            if (AgentArrive != null)
                AgentArrive.TargetPoint = position;
        }
    }
}
