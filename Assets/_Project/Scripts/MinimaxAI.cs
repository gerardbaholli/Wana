public static class MinimaxAI
{
    private const int Infinity = int.MaxValue;

    public static (int bestScore, Move bestMove) Minimax(Board board, int player, int maxDepth, int currentDepth = 0)
    {
        // Controlla se il gioco è terminato o se abbiamo raggiunto la profondità massima
        if (board.IsGameOver() || currentDepth == maxDepth)
        {
            return (board.Evaluate(player), null);
        }

        Move bestMove = null;
        int bestScore;

        // Determina se stiamo massimizzando o minimizzando
        if (board.CurrentPlayer == player)
        {
            bestScore = -Infinity; // Massimizzare
            foreach (var move in board.GetMoves())
            {
                Board newBoard = board.MakeMove(move);
                (int currentScore, _) = Minimax(newBoard, player, maxDepth, currentDepth + 1);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = move;
                }
            }
        }
        else
        {
            bestScore = Infinity; // Minimizzare
            foreach (var move in board.GetMoves())
            {
                Board newBoard = board.MakeMove(move);
                (int currentScore, _) = Minimax(newBoard, player, maxDepth, currentDepth + 1);

                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                    bestMove = move;
                }
            }
        }

        // Restituisci il punteggio migliore e la mossa corrispondente
        return (bestScore, bestMove);
    }
}