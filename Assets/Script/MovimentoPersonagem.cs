using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Personagem : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float velocidadeGiro = 720f;

    [Header("Pulo e Gravidade")]
    public float alturaPulo = 1.5f;
    public float gravidade = -20f;

    private CharacterController controller;
    private float velocidadeY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Entrada do jogador
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direcao = new Vector3(h, 0f, v);

        // Evita velocidade maior na diagonal
        if (direcao.magnitude > 1f)
        {
            direcao.Normalize();
        }

        // 2. Rotação do personagem
        if (direcao != Vector3.zero)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcao);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotacaoAlvo,
                velocidadeGiro * Time.deltaTime
            );
        }

        // 3. Pulo e gravidade
        if (controller.isGrounded)
        {
            // Mantém o personagem "colado" ao chão
            if (velocidadeY < 0f)
            {
                velocidadeY = -2f;
            }

            // Pulo
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            {
                velocidadeY = Mathf.Sqrt(
                    alturaPulo * -2f * gravidade
                );
            }
        }

        // Aplica gravidade
        velocidadeY += gravidade * Time.deltaTime;

        // 4. Movimento final
        Vector3 movimentoFinal = direcao * velocidade;
        movimentoFinal.y = velocidadeY;

        controller.Move(movimentoFinal * Time.deltaTime);
    }

    // Detecta quando o personagem entra em um Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se possui a tag "maça" ou "maca"
        bool temTagMaca =
            other.CompareTag("maça") ||
            other.CompareTag("maca");

        // Verifica se o nome contém "maça" ou "maca"
        string nomeObjeto = other.gameObject.name.ToLower();

        bool temNomeMaca =
            nomeObjeto.Contains("maça") ||
            nomeObjeto.Contains("maca");

        // Se for uma maçã, faz o item desaparecer
        if (temTagMaca || temNomeMaca)
        {
            other.gameObject.SetActive(false);
        }
    }
}

