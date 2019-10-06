using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Rigidbody2D rigidbody;

    public void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var playerpos = GameManager.Instance.Player.transform.position;
        var mobpos = transform.position;
        if((playerpos - mobpos).sqrMagnitude < 4 && (playerpos - mobpos).sqrMagnitude > 1)
        {
            rigidbody.MovePosition(((playerpos - mobpos).normalized * speed * Time.deltaTime) + transform.localPosition);
        }
    }

}
