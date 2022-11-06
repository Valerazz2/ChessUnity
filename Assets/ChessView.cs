using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;
using UnityEngine.SceneManagement;

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
         var figure = GetTileByMousePos().currentFigure;
         if (Desk.currentFigure == null)
         {
            if (figure != null && Desk.Move == figure.color)
            {
               Desk.currentFigure = figure;
               MarkOwnTileYellow(figure);
               
               MarkAllAbleMoveTiles(figure);
            }
            return;
         }

         var target = GetTileByMousePos();
         if (target != null && !(target.currentFigure != null && target.currentFigure.color == Desk.Move))
         {
            if (Desk.currentFigure.AbleMoveTo(target))
            {
               Desk.MoveTo(Desk.currentFigure, target);
               FigureColor prevMoveColor = Desk.Move == FigureColor.White ? FigureColor.Black : FigureColor.White;
               if (Desk.IsMateFor(Desk.FindKing(Desk.Move)))
               { 
                  Debug.Log(prevMoveColor + " Wins");
                  StartCoroutine(ReloadSceneIn(3));
               }
            }
            Desk.currentFigure = null;
            ClearAllColoredTiles();
            return;
         }
        
         if (figure != null && figure.color == Desk.Move)
         {
            ClearAllColoredTiles();
            Desk.currentFigure = figure;
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
            tileInst.color = Desk.desk[x, y].color == FigureColor.Black? Color.green: Color.white;
            tileInst.transform.position = new Vector3(x, y, 1);

            if (Desk.desk[x,y].currentFigure != null)
            {
               var inst = Instantiate(figureSprite).GetComponent<PersonalVisualiser>();
               inst.transform.position = new Vector3(x, y);
               inst.ownFigure = Desk.desk[x, y].currentFigure;
               inst.Desk = Desk;
               
               var figure = Desk.desk[x, y].currentFigure;
               Sprite sprite = Resources.Load<Sprite>("pieces/" + figure.GetFigureType() + "_" + figure.color);
               inst.GetComponent<SpriteRenderer>().sprite = sprite;
            }
         }
      }
   }

   private Tile GetTileByMousePos()
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

   private void MarkAllAbleMoveTiles(Figure figure)
   {
      ColoredTiles = new List<SpriteRenderer>();
      foreach (var tile in figure.AbleMoveTiles())
      {
         SpriteRenderer tileSprite = Tiles[tile.pos.X, tile.pos.Y];
         tileSprite.color = Color.yellow;
         ColoredTiles.Add(tileSprite);
      }
   }

   private void MarkOwnTileYellow(Figure figure)
   {
      SpriteRenderer tileSprite = Tiles[figure.ownTile.pos.X, figure.ownTile.pos.Y];
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
         coloredTile.color = Desk.desk[(int) pos.x, (int) pos.y].color == FigureColor.White ? Color.white : Color.green;
      }
   }

   IEnumerator ReloadSceneIn(int seconds)
   {
      yield return new WaitForSeconds(seconds);
      SceneManager.LoadScene("SampleScene");
   }
}
