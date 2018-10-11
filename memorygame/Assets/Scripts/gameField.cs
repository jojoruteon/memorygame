//script que controlará as peças do jogo como um todo, e não individualmente
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameField : MonoBehaviour {

    //tempo de mostragem inicial das peças, público para poder ser modificado de acordo com a dificuldade do nível
    public int showTime = 1;

    //array dinâmico contendo todas as peças em jogo
    GameObject[] gameTiles;
    //array de componentes que chamará os animators dos filhos do Game Object contendo esse script
    Animator[] anim;

    //checa se a primeira peça foi revelada
    [HideInInspector]
    public bool tile1IsRevealed;

    //checa se a segunda peça foi revelada
    [HideInInspector]
    public bool tile2IsRevealed;

    //checa se o valor das peças já podem ser comparados
    [HideInInspector]
    public bool canCompare;

    //atribui um valor mutável à primeira peça e um valor definitivo para ser chamado quando necessário
    [HideInInspector]
    public int tile1Value;
    int tile1DefValue = 999;

    //atribui um valor mutável à segunda peça e um valor definitivo para ser chamado quando necessário
    [HideInInspector]
    public int tile2Value;
    int tile2DefValue = 998;

    //Game Objects públicos para atribuição de valor à UI
    public GameObject acertosUI;
    public GameObject errosUI;

    //verifica se a jogada foi um acerto ou um erro
    bool missed = true;

    void Start ()
    {
        //atribui todos os Game Objects com a tag "memoryTile" ao array que conterá todas as peças em jogo
        gameTiles = GameObject.FindGameObjectsWithTag("memoryTile");

        //atribui os respectivos componentes animator ao array de animators anim
        anim = GetComponentsInChildren<Animator>();

        //é necessário definir o valor definitivo às peças no Start para não ocorrer uma comparação de valores não intencional
        tile1Value = tile1DefValue;
        tile2Value = tile2DefValue;
    }

    void Update()
    {
        endLevel();
    }

    //função para checar se as peças estão viradas e podem ser comparadas
    public void checkTiles()
    {
        if (tile1IsRevealed && tile2IsRevealed)
        {
            canCompare = true;
        }
    }

    //função que compara as duas peças
    public void compareTiles()
    {
        if (canCompare)
        {
            //checa cada peça em jogo uma por uma
            for (int i = 0; i < gameTiles.Length; i++)
            {
                //checa se as peças combinaram, o que alterará o comportamento das peças no script de peça individual
                if ((gameTiles[i].GetComponent<gamePiece>().currentTileValue == tile1Value) && (gameTiles[i].GetComponent<gamePiece>().currentTileValue == tile2Value))
                {
                    Debug.Log("As peças são iguais");
                    gameTiles[i].GetComponent<gamePiece>().tilesMatched = true;
                    missed = false;
                    canCompare = false;
                }
                //caso não combinem as peças apenas voltam ao seu estado inicial
                else if (tile1Value != tile2Value)
                {
                    Debug.Log("As peças são diferentes");
                    canCompare = false;
                    StartCoroutine(waitASec());
                }
                //torna o tileIsActive falso para "desvirar" a peça
                gameTiles[i].GetComponent<gamePiece>().tileIsActive = false;
            }
            //adiciona os respectivos pontos na UI, deve ser chamado aqui para não repetir dentro do foreach e adicionar pontos inexistentes
            if (missed)
            {
                errosUI.GetComponent<score>().addToScore();
            }
            else
            {
                acertosUI.GetComponent<score>().addToScore();
            }

            //reseta os valores atuais das peças viradas para que outras comparações possam ocorrer
            missed = true;
            tile1Value = tile1DefValue;
            tile2Value = tile2DefValue;

            tile1IsRevealed = false;
            tile2IsRevealed = false;
        }
    }


    //verifica se todas as peças foram viradas para terminar o nível e grava a pontuação final no playerprefs (que sempre regravará numa nova partida)
    void endLevel()
    {
        bool allMatched = true;

        for (int i = 0; i < gameTiles.Length; i++)
        {
            if (gameTiles[i].GetComponent<gamePiece>().tilesMatched == false)
            {
                allMatched = false; break;
            }
        }

        if (allMatched)
        {
            PlayerPrefs.SetInt("Acertos", acertosUI.GetComponent<score>().add);
            PlayerPrefs.SetInt("Erros", errosUI.GetComponent<score>().add);
            StartCoroutine(waitToEnd());
        }
    }

    //CoRoutine para realizar a animação das peças desvirando
    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1f);

        foreach (Animator anim in anim)
        {
            for (int i = 0; i < gameTiles.Length; i++)
            {
                if (gameTiles[i].GetComponent<gamePiece>().tilesMatched == false)
                {
                    gameTiles[i].GetComponent<Animator>().SetBool("isShowing", false);
                }
            }
        }
    }

    //espera para mostrar o resultado final
    IEnumerator waitToEnd()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("score_scene");
    }
}
