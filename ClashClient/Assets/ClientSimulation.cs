using System;


public class ClientSimulation
{
    public ClientPlayerState state;
    public Simulation simulation;

    public ClientSimulation()
    {

    }

    public void Init()
    {
        simulation = new Simulation();


    }

    public void Tick()
    {
        simulation.Update();
    }
}

