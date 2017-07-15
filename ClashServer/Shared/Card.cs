using System;

public class Card
{
    private Card()
    {

    }

    public static Card GetOne()
    {
        Card card = new Card();
        return card;
    }
}

