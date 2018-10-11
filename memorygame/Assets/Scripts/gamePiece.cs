// Esse script definirá e controlará uma peça individual, que por vez será controlada em um "tabuleiro geral" junto com todas as outras por meio de outro script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//dropdown list para selecionar qual tipo de peça será dentro do inspector da unity.
public enum activeTile
{
    elephant,
    giraffe,
    hippo,
    monkey,
    panda,
    parrot,
    penguin,
    pig,
    rabbit,
    snake
}

public class gamePiece : MonoBehaviour {

    //ativa a dropdown list dentro do inspector
    public activeTile activeTile;

    //Game Object contendo o script do tabuleiro geral
    public GameObject gameField;

    //valor da peça, o qual definirá se a peça combinará com outra ou não
    [HideInInspector]
    public int currentTileValue;

    //checa se a peça foi virada
    [HideInInspector]
    public bool tileIsActive;

    //checa se a peça combinou com outra
    [HideInInspector]
    public bool tilesMatched;

    //materiais para criar diferentes peças. público para facilitar a modificação
    [Header("Tile Types")]
    public Material elephantTile;
    public Material giraffeTile;
    public Material hippoTile;
    public Material monkeyTile;
    public Material pandaTile;
    public Material parrotTile;
    public Material penguinTile;
    public Material pigTile;
    public Material rabbitTile;
    public Material snakeTile;

    //componente animator
    Animator anim;

    //componente colider
    Collider col;

    void Start ()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        //mostra as peças no início do nível para serem memorizadas
        col.enabled = false;
        anim.SetBool("isShowing", true);
        StartCoroutine(initialShow());
    }
	
	void Update ()
    {
        //instancia a dropdown list no inspector
        InstantiatePrimitive(activeTile);
    }

    //define o comportamento da peça
    private void OnMouseDown()
    {
        //comportamento esperado caso as peças ativas nunca combinem
        if (!tilesMatched)
        {
            //definirá quais peças estão ativas, ou seja, com quais peças poderemos interagir. a checagem é realizada aqui e o comportamento é definido no script do tabuleiro geral
            if (gameField.GetComponent<gameField>().tile1IsRevealed == false)
            {
                anim.SetBool("isShowing", true);
                gameField.GetComponent<gameField>().tile1IsRevealed = true;
                gameField.GetComponent<gameField>().tile1Value = currentTileValue;
                //Debug.Log("A peça 1 foi revelada");
                tileIsActive = true;
            }
            else if ((gameField.GetComponent<gameField>().tile1IsRevealed) && (gameField.GetComponent<gameField>().tile2IsRevealed == false))
            {
                if (tileIsActive == true)
                {
                    //Debug.Log("Essa peça já foi revelada");
                }
                else
                {
                    anim.SetBool("isShowing", true);
                    gameField.GetComponent<gameField>().tile2IsRevealed = true;
                    gameField.GetComponent<gameField>().tile2Value = currentTileValue;
                    //Debug.Log("A peça 2 foi revelada");
                    tileIsActive = true;
                }
            }
            StartCoroutine(waitASec());
        }
        //porém se elas combinarem o comportamento acima é ignorado.
        else
        {
            //Debug.Log("A peça já foi combinada");
        }
        //chama as funções presentes no controlador geral
        gameField.GetComponent<gameField>().checkTiles();
        gameField.GetComponent<gameField>().compareTiles();
    }

    //CoRoutine que impede que mais peças que o desejado sejam viradas ao mesmo tempo.
    IEnumerator waitASec()
    {
        foreach (Collider c in gameField.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        yield return new WaitForSeconds(1f);
        foreach (Collider c in gameField.GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
    }

    //concede um tempo para o jogador memorizar as peças e então as retorna ao estado inicial
    IEnumerator initialShow()
    {
        yield return new WaitForSeconds(gameField.GetComponent<gameField>().showTime);
        anim.SetBool("isShowing", false);
        col.enabled = true;
    }

    //função que atribui comportamentos aos elementos da dropdown list de diferentes peças
    void InstantiatePrimitive(activeTile at)
    {
        switch (at)
        {
            case activeTile.elephant:
                GetComponent<MeshRenderer>().material = elephantTile;
                currentTileValue = 0;
                break;

            case activeTile.giraffe:
                GetComponent<MeshRenderer>().material = giraffeTile;
                currentTileValue = 1;
                break;

            case activeTile.hippo:
                GetComponent<MeshRenderer>().material = hippoTile;
                currentTileValue = 2;
                break;

            case activeTile.monkey:
                GetComponent<MeshRenderer>().material = monkeyTile;
                currentTileValue = 3;
                break;

            case activeTile.panda:
                GetComponent<MeshRenderer>().material = pandaTile;
                currentTileValue = 4;
                break;
            case activeTile.parrot:
                GetComponent<MeshRenderer>().material = parrotTile;
                currentTileValue = 5;
                break;

            case activeTile.penguin:
                GetComponent<MeshRenderer>().material = penguinTile;
                currentTileValue = 6;
                break;

            case activeTile.pig:
                GetComponent<MeshRenderer>().material = pigTile;
                currentTileValue = 7;
                break;

            case activeTile.rabbit:
                GetComponent<MeshRenderer>().material = rabbitTile;
                currentTileValue = 8;
                break;

            case activeTile.snake:
                GetComponent<MeshRenderer>().material = snakeTile;
                currentTileValue = 9;
                break;
        }
    }
}
