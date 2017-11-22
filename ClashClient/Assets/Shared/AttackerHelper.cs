using System;




public class AttackerHelper
{
    private AttackerHelper()
    {

    }

    public static AttackerHelper GetOne()
    {
        AttackerHelper helper = new AttackerHelper();
        return helper;
    }
}
