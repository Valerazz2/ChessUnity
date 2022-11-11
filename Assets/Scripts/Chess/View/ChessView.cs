using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess.View
{
   public class ChessView : MonoBehaviour
   {
      [SerializeField] private GameObject tile;
      [SerializeField] private GameObject figureSprite;
      private Desk Desk;
      private SpriteRenderer[,] Tiles = new SpriteRenderer[Desk.DeskSizeX, Desk.DeskSizeY];
      private SpriteRenderer choosedTile;
      private Color choosedTileColor;
      private List<SpriteRenderer> ColoredTiles = new List<SpriteRenderer>();

      private void Start()
      {
         Desk = new Desk();
         Desk.CreateMap();
         CreatePhysicsMap();
      }

      private void Update()
      {
         if (!Input.GetMouseButtonDown(0)) return;
         
         var figure = GetTileByMousePos().Piece;
         if (Desk.CurrentPiece == null)
         {
            if (figure != null && Desk.Move == figure.Color)
            {
               Desk.CurrentPiece = figure;
               MarkOwnTileYellow(figure);
               
               MarkAllAbleMoveTiles(figure);
            }
            return;
         }

         var target = GetTileByMousePos();
         if (target != null && !(target.Piece != null && target.Piece.Color == Desk.Move))
         {
            if (Desk.CurrentPiece.AbleMoveTo(target))
            {
               Desk.MoveTo(Desk.CurrentPiece, target);
               ChessColor prevMoveColor = Desk.Move.Invert();
               if (Desk.MateFor(Desk.FindKing(Desk.Move)))
               { 
                  Debug.Log(prevMoveColor + " Wins");
                  StartCoroutine(ReloadSceneIn(3));
               }

               if (Desk.StaleMateFor(Desk.Move))
               {
                  Debug.Log("StaleMate");
                  StartCoroutine(ReloadSceneIn(3));
               }
            }
            Desk.CurrentPiece = null;
            ClearAllColoredTiles();
            return;
         }
        
         if (figure != null && figure.Color == Desk.Move)
         {
            ClearAllColoredTiles();
            Desk.CurrentPiece = figure;
            MarkOwnTileYellow(figure);
            MarkAllAbleMoveTiles(figure);
         }
      }

      private void CreatePhysicsMap()
      {
         for (int x = 0; x < Desk.DeskSizeX; x++)
         {
            for (int y = 0; y < Desk.DeskSizeY; y++)
            {
               var tileInst =  Instantiate(tile).GetComponent<SpriteRenderer>();
               Tiles[x, y] = tileInst;
               tileInst.color = Desk.Squares[x, y].Color == ChessColor.Black? Color.green: Color.white;
               tileInst.transform.position = new Vector3(x, y, 1);

               if (Desk.Squares[x,y].Piece != null)
               {
                  var figure = Desk.Squares[x, y].Piece;
                  var inst = Instantiate(figureSprite).GetComponent<PersonalVisualiser>();
                  inst.transform.position = new Vector3(x, y);
                  inst.OwnPiece = figure;
                  inst.Desk = Desk;
                  
                  Sprite sprite = Resources.Load<Sprite>("pieces/" + figure.GetPieceType() + "_" + figure.Color);
                  inst.GetComponent<SpriteRenderer>().sprite = sprite;
               }
            }
         }
      }

      private Square GetTileByMousePos()
      {
         Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         RaycastHit2D hit2D = Physics2D.Raycast(mousePos, transform.forward, 1000);
         if (hit2D && hit2D.collider.gameObject.CompareTag("Tile"))
         {
            Vector2 pos =hit2D.transform.position;
            return Desk.Squares[(int)pos.x, (int)pos.y];
         }
         return null;
      }

      private void MarkAllAbleMoveTiles(Piece piece)
      {
         ColoredTiles = new List<SpriteRenderer>();
         foreach (var square in piece.AbleMoveTiles())
         {
            SpriteRenderer tileSprite = Tiles[square.Pos.X, square.Pos.Y];
            tileSprite.color = Color.yellow;
            ColoredTiles.Add(tileSprite);
         }
      }

      private void MarkOwnTileYellow(Piece piece)
      {
         SpriteRenderer tileSprite = Tiles[piece.OwnSquare.Pos.X, piece.OwnSquare.Pos.Y];
         choosedTileColor = tileSprite.color;
         tileSprite.color = Color.yellow;
         choosedTile = tileSprite;
      }

      private void ClearAllColoredTiles()
      {
         choosedTile.color = choosedTileColor;
         foreach (var coloredTile in ColoredTiles)
         {
            Vector3 pos = coloredTile.transform.position;
            coloredTile.color = Desk.Squares[(int) pos.x, (int) pos.y].Color == ChessColor.White ? Color.white : Color.green;
         }
      }

      IEnumerator ReloadSceneIn(int seconds)
      {
         yield return new WaitForSeconds(seconds);
         SceneManager.LoadScene("SampleScene");
      }
   }
}