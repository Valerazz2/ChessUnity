using System.Collections;
using Chess.Model;
using Chess.View;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeskView : AbstractView<Desk>
{
    [SerializeField] private SquareView squareViewPrefab;
    [SerializeField] private PieceView pieceViewPrefab;

    private SquareView choosedSquare;
    
    private void Start()
    {
        var desk = new Desk();
        desk.CreateMap();
        Bind(desk);
        //ChessEx chessEx = new ChessEx(desk);
        //StartCoroutine(chessEx.a());
    }

    protected override void OnBind()
    {
        CreateViews(model.ISquares, squareViewPrefab);
        CreateViews(model.GetAllPiece(), pieceViewPrefab);
        
        model.OnPieceAdd += CreateView2;
    }

    private void CreateView2(Piece p)
    {
        CreateView(p, pieceViewPrefab);
    }
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var target = GetSquareByMousePos(); 
        target?.Select();
    }
  
    private Square GetSquareByMousePos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit2D = Physics2D.Raycast(mousePos, transform.forward, 1000);
        if (hit2D && hit2D.collider.gameObject.GetComponent<SquareView>())
        {
            return hit2D.transform.gameObject.GetComponent<SquareView>().model;
        }
        return null;
    }

    private IEnumerator ReloadSceneIn(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("SampleScene");
    }
}
