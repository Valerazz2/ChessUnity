using System.Collections;
using System.Collections.Generic;
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
      private SpriteRenderer[,] Tiles = new SpriteRenderer[8, 8];
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
         if (Input.GetMouseButtonDown(0))
         {
            var figure = GetTileByMousePos().Piece;
            if (Desk.CurrentPiece == null)
            {
               if (figure != null && Desk.Move == figure.color)
               {
                  Desk.CurrentPiece = figure;
                  MarkOwnTileYellow(figure);
               
                  MarkAllAbleMoveTiles(figure);
               }
               return;
            }

            var target = GetTileByMousePos();
            if (target != null && !(target.Piece != null && target.Piece.color == Desk.Move))
            {
               if (Desk.CurrentPiece.AbleMoveTo(target))
               {
                  Desk.MoveTo(Desk.CurrentPiece, target);
                  ChessColor prevMoveColor = Desk.Move == ChessColor.White ? ChessColor.Black : ChessColor.White;
                  if (Desk.IsMateFor(Desk.FindKing(Desk.Move)))
                  { 
                     Debug.Log(prevMoveColor + " Wins");
                     StartCoroutine(ReloadSceneIn(3));
                  }
               }
               Desk.CurrentPiece = null;
               ClearAllColoredTiles();
               return;
            }
        
            if (figure != null && figure.color == Desk.Move)
            {
               ClearAllColoredTiles();
               Desk.CurrentPiece = figure;
               MarkOwnTileYellow(figure);
            
               MarkAllAbleMoveTiles(figure);
            }
         }
      }

      private void CreatePhysicsMap()
      {
         for (int x = 0; x < 8; x++)
         {
            for (int y = 0; y < 8; y++)
            {
               var tileInst =  Instantiate(tile).GetComponent<SpriteRenderer>();
               Tiles[x, y] = tileInst;
               tileInst.color = Desk.desk[x, y].Color == ChessColor.Black? Color.green: Color.white;
               tileInst.transform.position = new Vector3(x, y, 1);

               if (Desk.desk[x,y].Piece != null)
               {
                  var inst = Instantiate(figureSprite).GetComponent<PersonalVisualiser>();
                  inst.transform.position = new Vector3(x, y);
                  inst.OwnPiece = Desk.desk[x, y].Piece;
                  inst.Desk = Desk;
               
                  var figure = Desk.desk[x, y].Piece;
                  Sprite sprite = Resources.Load<Sprite>("pieces/" + figure.GetFigureType() + "_" + figure.color);
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
            return Desk.desk[(int)pos.x, (int)pos.y];
         }
         return null;
      }

      private void MarkAllAbleMoveTiles(Piece piece)
      {
         ColoredTiles = new List<SpriteRenderer>();
         foreach (var tile in piece.AbleMoveTiles())
         {
            SpriteRenderer tileSprite = Tiles[tile.Pos.X, tile.Pos.Y];
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
            coloredTile.color = Desk.desk[(int) pos.x, (int) pos.y].Color == ChessColor.White ? Color.white : Color.green;
         }
      }

      IEnumerator ReloadSceneIn(int seconds)
      {
         yield return new WaitForSeconds(seconds);
         SceneManager.LoadScene("SampleScene");
      }
   }
}
