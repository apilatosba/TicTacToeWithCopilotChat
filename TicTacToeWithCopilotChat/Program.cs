using Raylib_cs;
using System.Numerics;



namespace TicTacToeWithCopilotChat {
   internal class Program {
      const int screenWidth = 800;
      const int screenHeight = 450;

      const int cellSize = 100;
      const int boardPositionX = 100;
      const int boardPositionY = 50;

      const int boardSize = 3;
      static Token[,] board = new Token[boardSize, boardSize];

      static bool isXTurn = true;
      static bool isGameOver = false;

      static readonly Vector2 gameEndTextPosition = new Vector2(screenWidth / 2 + 100, 40);

      static GameEndState gameEndState = GameEndState.NotOver;

      static void Main(string[] args) {
         Raylib.InitWindow(screenWidth, screenHeight, "Tic Tac Toe");

         // Game loop
         while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);


            // Draw game board here
            DrawBoard(board);
            RenderGameEndText();

            Raylib.EndDrawing();
         }

         Raylib.CloseWindow();
      }

      static void DrawBoard(Token[,] board) {

         for (int row = 0; row < board.GetLength(0); row++) {
            for (int col = 0; col < board.GetLength(1); col++) {
               Rectangle cellRect = new Rectangle(boardPositionX + col * cellSize, boardPositionY + row * cellSize, cellSize, cellSize);

               switch (board[row, col]) {
                  case Token.Empty:
                     break;
                  case Token.X:
                     int offsetToCenter = cellSize / 2;
                     Vector2 centerOfCell = new Vector2(cellRect.x + offsetToCenter, cellRect.y + offsetToCenter);
                     DrawX(centerOfCell, 10, Color.RED); // Draw X
                     break;
                  case Token.O:
                     DrawO(cellRect.x + 50, cellRect.y + 50, 30, 5, Color.GREEN);
                     break;
                  default:
                     break;
               }

               Raylib.DrawRectangleLinesEx(cellRect, 4, Color.BLACK);

               HandleClick(row, col, cellRect);
            }
         }
      }

      static void HandleClick(int row, int col, Rectangle cellRect) {
         // If game is over then do not handle clicks
         if (isGameOver) return;

         // Detect if cursor is over the current cell
         if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), cellRect)) {

            // A valid move has been made
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) &&
                     board[row, col] == Token.Empty) {
               board[row, col] = isXTurn ? Token.X : Token.O;
               isXTurn = !isXTurn;

               gameEndState = IsGameOver(board);

               switch (gameEndState) {
                  case GameEndState.NotOver:
                     // Ignore
                     break;
                  case GameEndState.Tie:
                     isGameOver = true;
                     break;
                  case GameEndState.XWins:
                     isGameOver = true;
                     break;
                  case GameEndState.OWins:
                     isGameOver = true;
                     break;
               }
            }
         }
      }



      static void DrawO(int centerX, int centerY, float radius, float thickness, Color color) {
         Raylib.DrawCircle(centerX, centerY, radius, color);
         Raylib.DrawCircle(centerX, centerY, radius - thickness, Color.WHITE);
      }

      static void DrawO(float centerX, float centerY, float radius, float thickness, Color color) {
         DrawO((int)centerX, (int)centerY, radius, thickness, color);
      }

      static void DrawX(Vector2 center, float thickness, Color color) {
         Vector2 offset = new Vector2(25, 25);

         Raylib.DrawLineEx(center - offset, center + offset, thickness, color); // Draw X
         Raylib.DrawLineEx(center + new Vector2(-offset.X, offset.Y), center + new Vector2(offset.X, -offset.Y), thickness, Color.RED); // Draw X
      }



      static GameEndState IsGameOver(Token[,] board) {
         // Check rows
         for (int row = 0; row < board.GetLength(0); row++) {
            if (board[row, 0] != Token.Empty && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2]) {
               return board[row, 0] == Token.X ? GameEndState.XWins : GameEndState.OWins;
            }
         }

         // Check columns
         for (int col = 0; col < board.GetLength(1); col++) {
            if (board[0, col] != Token.Empty && board[0, col] == board[1, col] && board[1, col] == board[2, col]) {
               return board[0, col] == Token.X ? GameEndState.XWins : GameEndState.OWins;
            }
         }

         // Check diagonals
         if (board[0, 0] != Token.Empty && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) {
            return board[0, 0] == Token.X ? GameEndState.XWins : GameEndState.OWins;
         }
         if (board[0, 2] != Token.Empty && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) {
            return board[0, 2] == Token.X ? GameEndState.XWins : GameEndState.OWins;
         }

         // Check for tie
         for (int row = 0; row < board.GetLength(0); row++) {
            for (int col = 0; col < board.GetLength(1); col++) {
               if (board[row, col] == Token.Empty) {
                  return GameEndState.NotOver;
               }
            }
         }
         return GameEndState.Tie;
      }

      static void RenderGameEndText(int fontSize = 50) {
         switch (gameEndState) {
            case GameEndState.NotOver:
               break;
            case GameEndState.Tie:
               Raylib.DrawText("Tie Game!", (int)gameEndTextPosition.X, (int)gameEndTextPosition.Y, fontSize, Color.VIOLET);
               break;
            case GameEndState.XWins:
               Raylib.DrawText("X won!", (int)gameEndTextPosition.X, (int)gameEndTextPosition.Y, fontSize, Color.VIOLET);
               break;
            case GameEndState.OWins:
               Raylib.DrawText("O won!", (int)gameEndTextPosition.X, (int)gameEndTextPosition.Y, fontSize, Color.VIOLET);
               break;
         }

         if (isGameOver) {
            if (Raylib_CsLo.RayGui.GuiButton(new Raylib_CsLo.Rectangle(gameEndTextPosition.X + 25, gameEndTextPosition.Y + 70, 80, 30), "Restart")) {
               // Reset the board and game state
               board = new Token[3, 3];
               isXTurn = true;
               isGameOver = false;
               gameEndState = GameEndState.NotOver;
            }
         }
      }
   }

   enum Token {
      Empty,
      X,
      O,
   }

   enum GameEndState {
      NotOver,
      Tie,
      XWins,
      OWins,
   }
}