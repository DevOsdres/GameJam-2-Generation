using UnityEngine;

public class Cactus : MonoBehaviour
{   
    // para ambos cactus y hongo
    public GameObject player;
    public float speed = 5f;  // Velocidad de movimiento del cactus

    Animator animatorCactus;
    bool isColliding = false;
    private void Start()
    {
        // Buscar al jugador por su tag
        player = GameObject.FindGameObjectWithTag("Player");
        animatorCactus = GetComponent<Animator>();

    }

    private void Update()
    {
        // Verificar la distancia al jugador
        if (DistanceToPlayer() < 5f)
        {
            MoveTowardsPlayer();
            if (isColliding)
            {

            }
        }
    }

    private void OnTriggerStay(Collider collision) {   //mientras lo detecta ejecuta abimacion ataque
        if (collision.gameObject.CompareTag("Player"))
        {   
            isColliding = true;
            animatorCactus.SetBool("Attack", true);
            
        }
    }

    private void OnTriggerExit(Collider collision){    //deja de c
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
            animatorCactus.SetBool("Attack", false);
        }

    }
    private float DistanceToPlayer()
    {
        // Calcular la distancia al jugador
        if (player != null)
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }
        return Mathf.Infinity; // Retorna infinito si el jugador no está disponible
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Rotar suavemente hacia el jugador
            transform.rotation = Quaternion.LookRotation(direction);

            // Mover hacia el jugador
            //transform.position += direction * speed * Time.deltaTime;
        }
    }
}
