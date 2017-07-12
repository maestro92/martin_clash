using System;

public class ClientPlayerState
{
    public Enums.Team teamId;

    private ClientPlayerState()
    {

    }

    public static ClientPlayerState GetOne()
    {
        ClientPlayerState state = new ClientPlayerState();
        return state;
    }
}

