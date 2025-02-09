using System;
using System.Collections.Generic;

public class Board
{
    public int CurrentPlayer { get; private set; } // Giocatore corrente (1 o 2)
    
    // Controlla se il gioco è terminato
    public bool IsGameOver()
    {
        // Implementa la logica per verificare se il gioco è finito
        return false;
    }

    // Valuta lo stato del tabellone dal punto di vista di un giocatore
    public int Evaluate(int player)
    {
        // Implementa la logica per calcolare il punteggio del tabellone
        return 0;
    }

    // Ottieni tutte le mosse valide
    public List<Move> GetMoves()
    {
        // Implementa la logica per generare tutte le mosse valide
        return new List<Move>();
    }

    // Simula una mossa e restituisce una nuova istanza del tabellone
    public Board MakeMove(Move move)
    {
        // Implementa la logica per creare un nuovo stato del tabellone
        return new Board();
    }

    // public Move GetBestMove()
    // {
    //     // Ottieni il risultato dell'esecuzione del minimax e restituisci la mossa
    //     var result = MinimaxAI.Minimax(board, currentPlayer, maxDepth, 0);
    //     return result.Move;
    // }

}