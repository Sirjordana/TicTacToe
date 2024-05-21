using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public bool isCubeTurn = true;
    public TextMeshProUGUI label;
    public Image labelBg;
    public Cell[] cells;
    public GameObject restartButton;
    public GameObject backToMenuButton;
    // Start is called before the first frame update
    public AudioClip clipWin;
    public AudioClip clipDraw;
    private bool modeAI;
    public float timer = 0;
    public bool gameEnd = false;

    void Start()
    {
        isCubeTurn = true;
        restartButton.SetActive(false);
        backToMenuButton.SetActive(false);
        int flag = PlayerPrefs.GetInt("AI", 1);
        modeAI = flag == 1;
        ChangeTurn();

    }

    // Suponiendo que las cells se disponen de la siguiente forma:
    // 0 | 1 | 2
    // 3 | 4 | 5
    // 6 | 7 | 8

    public void CheckWinner()
    {
        bool isDraw = true;

        // Revisa las filas
        for (int i = 0; i < 9; i += 3)
        {
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 1].status && cells[i + 1].status == cells[i + 2].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
            if (cells[i].status == CellType.EMPTY || cells[i + 1].status == CellType.EMPTY || cells[i + 2].status == CellType.EMPTY) isDraw = false;
        }

        // Revisa las columnas
        for (int i = 0; i < 3; i++)
        {
            if (cells[i].status != 0 && cells[i].status == cells[i + 3].status && cells[i + 3].status == cells[i + 6].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
        }

        // Revisa las diagonales
        if (cells[0].status != 0 && cells[0].status == cells[4].status && cells[4].status == cells[8].status)
        {
            DeclareWinner(cells[0].status);
            return;
        }

        if (cells[2].status != 0 && cells[2].status == cells[4].status && cells[4].status == cells[6].status)
        {
            DeclareWinner(cells[2].status);
            return;
        }

        // Si todas las celdas están llenas y no hay ganador, entonces es un empate.
        if (isDraw)
        {
            label.text = "It's a draw!";
            labelBg.color = Color.black;
            gameEnd = true;
            SetupGameFinished(false);
            
        }
    }

    public void ChangeTurn()
    {
        isCubeTurn = !isCubeTurn;
        if (isCubeTurn == true)
        {
            if (modeAI)
            {
                label.text = "Cube's turn(AI)\n...";
                labelBg.color = Color.yellow;
            }
            else
            {
                label.text = "Cube's turn(Player 2)\n...";
                labelBg.color = Color.yellow;
            }
        }
        else
        {
            if (modeAI)
            {
                label.text = "Sphere's turn(Player)\n...";
                labelBg.color = Color.cyan;
            }
            else
            {
                label.text = "Sphere's turn(Player 1)\n...";
                labelBg.color = Color.cyan;
            }
        }
    }

    void DeclareWinner(CellType status)
    {
        if (status == CellType.SPHERE)
        {
            label.text = "Sphere is the winner";
            labelBg.color = Color.cyan;
        }
        else
        {
            label.text = "Cube is the winner";
            labelBg.color = Color.yellow;
        }

        gameEnd = true;
        SetupGameFinished(true);
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (modeAI && isCubeTurn && gameEnd == false)
        {
            if (timer >= 3f)
            {
                foreach (Cell cell in cells)
                {
                    if (cell.status == CellType.EMPTY)
                    {
                        cell.onClick();
                        break;
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("RESTART");
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetupGameFinished(bool winner)
    {
        restartButton.SetActive(true);
        backToMenuButton.SetActive(true);
        if (winner)
        {
            GetComponent<AudioSource>().PlayOneShot(clipWin);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(clipDraw);    
        }
        
    }

    public bool getModeAI()
    {
        return modeAI;
    }
}
