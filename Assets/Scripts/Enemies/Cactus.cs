using UnityEngine;

public class Cactus : MonoBehaviour
{   
    // para ambos cactus y hongo
    public GameObject player;
    public float speed = 5f;  // Velocidad de movimiento del cactus

    Animator animatorCactus;
    bool isColliding = false;

    bool isDead = false;
    [SerializeField] AudioClip atackSound; 
    private void Start()
    {
        // Buscar al jugador por su tag
        player = GameObject.FindGameObjectWithTag("Player");
        animatorCactus = GetComponent<Animator>();

    }

    private void Update()
    {
        // Verificar la distancia al jugador
        //isDead = true;

        if (!isDead)
        {
            float probabilidad = Random.Range(1,100);
            float ProbabilidadSuceder = 8;

            if (probabilidad <= ProbabilidadSuceder)
            {   
                animatorCactus.SetBool("Mov", true);
            }
            else
            {
                animatorCactus.SetBool("Mov", false);
            }
            if (DistanceToPlayer() < 5f)
            {
                MoveTowardsPlayer();
                if (isColliding)
                {

                }
            }
        }else
        {
           
            animatorCactus.SetBool("dead", true);
        }
    }

    private void OnTriggerStay(Collider collision) {   //mientras lo detecta ejecuta abimacion ataque
        if (collision.gameObject.CompareTag("Player"))
        {   
            isColliding = true;
            animatorCactus.SetBool("Attack", true);
            AudioManager2.Instance.PlaySFXOne(atackSound);
            
        }
    }

    private void OnTriggerExit(Collider collision){    //deja de detectar, no ejecuta mas animacion ataque
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

        void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que choca es el AttackDetection
        if (other.gameObject.CompareTag("AttackDetection"))
        {
            // Lógica cuando el enemigo choca con el trigger
            dead();

            
        }
    }

    private void dead()
    {
        animatorCactus.SetBool("dead", true);
        isDead = true;
    }
}
