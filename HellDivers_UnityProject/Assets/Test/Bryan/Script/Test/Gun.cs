using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bryan.Test
{
    public class Gun : MonoBehaviour
    {
        public GameObject m_Bullet;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bullet = Instantiate(m_Bullet);
                bullet.transform.position = this.transform.position;
            }
        }
    }
}