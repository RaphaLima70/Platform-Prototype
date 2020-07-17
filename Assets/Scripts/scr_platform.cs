using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_platform : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator anim;

    float velocidade = 5;
    float jumpForce = 6;
    float inputX;

    public Transform pePos;
    public LayerMask layerChao;
    public bool noChao;
    public float raioPe;

    public float puloFantasma;
    public float tempoCombo;
    public int ataqueAtual;

    public bool pulando;
    public bool atacando;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        //verifica se o personagem está pisando em um objeto com layer chão através da posição de um outro objeto posicionado nos pés
        noChao = Physics2D.OverlapCircle(pePos.position, raioPe, layerChao);

        //Entrada de movimentação horizontal
        inputX = Input.GetAxisRaw("Horizontal");

        //Entrada do pulo e verificando o pulo fantasma como condição de pulo
        if (Input.GetButtonDown("Jump") && puloFantasma > 0)
        {
            pulando = true;
        }

        //Controlador do pulo fantasma
        if (noChao)
        {
            puloFantasma = 0.1f;
        }
        else
        {
            puloFantasma -= Time.deltaTime;
            if (puloFantasma <= 0)
            {
                puloFantasma = 0;
            }
        }

        //verica se o personagem não está atacando, possibilitando ações como andar e virar a imagem do personagem
        if (!atacando)
        {
            if (inputX < 0)
            {
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);

            }
            if (inputX > 0)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);

            }
            if (inputX != 0)
            {
                velocidade = 15;

                if (velocidade >= 8)
                {
                    velocidade = 8;
                }
            }
        }

        //Entrada de ataque
        if (Input.GetKeyDown(KeyCode.Z) && ataqueAtual < 3)
        {
            ataqueAtual++;
            anim.SetInteger("Attack", ataqueAtual);
            atacando = true;
            tempoCombo = 0.37f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (atacando && noChao)
        {
            inputX = 0;
        }

        if (ataqueAtual > 0)
        {
            tempoCombo -= Time.deltaTime;
        }
        if(tempoCombo < 0)
        {
            fimCombo();
        }
    }

    private void FixedUpdate()
    {
        //executa o movimento horizontal baseado nas entradas
        rigid.velocity = new Vector2(inputX * velocidade, rigid.velocity.y);

        //executa o pulo baseado nas entradas
        if (pulando)
        {
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            pulando = false;
        }
    }

    public void fimCombo()
    {
        ataqueAtual = 0;
        atacando = false;
        anim.SetInteger("Attack", 0);
    }

    public void Attack(string botao)
    {
        switch (ataqueAtual)
        {
            case 1:
                anim.SetBool("Attack1" + botao, true);

                break;
            case 2:
                anim.SetBool("Attack2" + botao, true);

                break;
            case 3:
                anim.SetBool("Attack3" + botao, true);

                break;
        }

    }


}
