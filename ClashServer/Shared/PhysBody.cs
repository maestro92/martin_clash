using System;

public class PhysBody
{
	private PhysBody()
	{

    }

    public static PhysBody GetOne()
    {
        PhysBody physBody = new PhysBody();
        return physBody;
    }
}
