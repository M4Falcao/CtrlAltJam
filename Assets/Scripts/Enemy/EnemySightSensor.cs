using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Enemy
{
    public class EnemySightSensor : MonoBehaviour
    {
        public Transform Player { get; private set; }

        [SerializeField] private LayerMask _ignoreMask;

        private Ray _ray;

        public float viewAngle = 90f; // Ângulo de visão do objeto
        public float viewRadius = 100f; // Raio de visão do objeto

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public bool Ping()
        {
        
            if (Player == null)
            {
                // Jogador não atribuído, portanto, não está visível
                return false;
            }

            Vector3 directionToPlayer = Player.position - transform.position;
            float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
            Debug.Log(angleToPlayer);

            // Verifica se o jogador está dentro do ângulo de visão do objeto
            if (angleToPlayer <= viewAngle / 2f)
            {
                // Verifica se há obstáculos entre o objeto e o jogador
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewRadius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        // O jogador está dentro do campo de visão e não há obstáculos
                        return true;
                    }
                }
            }

            // O jogador não está visível no campo de visão
            return false;

            /*
            if (Player == null)
                return false;

            _ray = new Ray(this.transform.position, Player.position-this.transform.position);

            var dir = new Vector3(_ray.direction.x, 0, _ray.direction.z);

            var angle = Vector3.Angle(dir, this.transform.forward);

            if (angle > 90)
            {
                Debug.Log("angulo");
                Debug.Log(angle);
                return false;
            }
                

            if (!Physics.Raycast(_ray, out var hit, 100, ~_ignoreMask))
            {
                Debug.Log("o outro");
                return false;
            }
            
            if(hit.collider.tag == "Player")
            {
                Debug.Log("Achou");
                Console.WriteLine("Achou");
                return true;
            }
            Debug.Log("nenhum");
            return false;*/
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * 100);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 100);
        }
    }
}
