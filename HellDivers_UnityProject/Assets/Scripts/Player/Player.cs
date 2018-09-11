using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HELLDIVERS
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        public PlayerInfo Info { get; private set; }
        public GameObject Display { get; private set; }
        public Animator Anima { get; private set; }

        private void Awake()
        {
            Display = Resources.Load("Characters/Ch01/ch01") as GameObject;
            Display = Instantiate(Display, this.transform);
            Display.transform.position -= Vector3.up;
            Anima = Display.GetComponent<Animator>();
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}