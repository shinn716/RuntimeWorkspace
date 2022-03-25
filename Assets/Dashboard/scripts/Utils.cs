using System;

public static class Utils
{
    public static string CreateUUID()
    {
        return Guid.NewGuid().ToString("N");
    }
}
