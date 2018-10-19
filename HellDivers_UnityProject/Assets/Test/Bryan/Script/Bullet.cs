using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bryan.Test
{
    public class Bullet : MonoBehaviour
    {
        public float m_Damage = 10;
        public float m_Speed = 10;
        private float m_LiveTime = 2;
        private float m_StartTime;
        private float DuringTime { get { return Time.realtimeSinceStartup - m_StartTime; } }

        // Use this for initialization
        private void Start()
        {
            m_StartTime = Time.realtimeSinceStartup;
        }

        // Update is called once per frame
        private void Update()
        {
            this.transform.position += this.transform.forward * m_Speed * Time.deltaTime;
            if (DuringTime >= m_LiveTime) DestroyImmediate(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable target = other.transform.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                target.Battle.TakeDamage(m_Damage);
            }
        }
    }
}