using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bryan.Test
{
    public class Character : MonoBehaviour
    {
        public bool IsDead { get { return Hp <= 0; } }

        public float Hp
        {
            get { return m_Hp; }
            set
            {
                if (value < 0) m_Hp = 0;
                else if (value > m_MaxHp) m_Hp = m_MaxHp;
                else m_Hp = value;
            }
        }

        [SerializeField] private float m_Hp;
        [SerializeField] private float m_MaxHp;
    }

    public interface IDamager
    {
        float DmgAmount();
    }

    public interface IDamageable
    {
        float Hp { get; set; }

        bool IsDead { get; }

        void OnHurt();

        void OnDeath();
    }

    public class BattleBehavior2
    {
        public static void TakeDamage(IDamager attacker, IDamageable target)
        {
            if (target.IsDead) return;

            if (target.IsDead)
                target.OnDeath();
            else
                target.OnHurt();
        }
    }

    public class BattleBehavior
    {
        public IDamageable m_Ch;

        public BattleBehavior(IDamageable ch)
        {
            m_Ch = ch;
        }

        public bool IsDead
        {
            get
            {
                return m_Ch.IsDead;
            }
        }

        public void TakeDamage(float amount)
        {
            m_Ch.Hp -= amount;
            if (IsDead)
            {
                m_Ch.OnDeath();
            }
            else
            {
                m_Ch.OnHurt();
            }
        }
    }

    public class Mob : Character, IDamageable
    {
        public BattleBehavior Battle { get { return m_Battle; } }
        protected BattleBehavior m_Battle;

        // Use this for initialization
        private void Start()
        {
            m_Battle = new BattleBehavior(this);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnHurt()
        {
        }

        public void OnDeath()
        {
            Destroy(this.gameObject);
        }

        private void OnDrawGizmos()
        {
            Handles.Label(this.transform.position + Vector3.up, Hp.ToString());
        }
    }
}