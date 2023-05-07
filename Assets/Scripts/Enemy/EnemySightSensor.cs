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

        public float viewAngle = 90f; // �ngulo de vis�o do objeto
        public float viewRadius = 100f; // Raio de vis�o do objeto

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public bool Ping()
        {
        
            if (Player == null)
            {
                // Jogador n�o atribu�do, portanto, n�o est� vis�vel
                return false;
            }

            Vector3 directionToPlayer = Player.position - transform.position;
            float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
            Debug.Log(angleToPlayer);

            // Verifica se o jogador est� dentro do �ngulo de vis�o do objeto
            if (angleToPlayer <= viewAngle / 2f)
            {
                // Verifica se h� obst�culos entre o objeto e o jogador
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewRadius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        // O jogador est� dentro do campo de vis�o e n�o h� obst�culos
                        return true;
                    }
                }
            }

            // O jogador n�o est� vis�vel no campo de vis�o
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
