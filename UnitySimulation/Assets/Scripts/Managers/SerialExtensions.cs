public static class SerialExtensions
{
    //"[000;000;000;000] => int[]{0,0,0,0}
    public static int[] ExtractMotorValues(this string message)
    {
        if (message.Length < 17)
        {
            Logger.Log.Error("Recieved serial message is too short!");
            return null;
        }
        message = message.Remove(0, 1);
        int index = 1;
        int[] res = new int[4];
        for (int i = 0; i < 4; i++)
        {
            string val = $"{message[index]}{message[index + 1]}{message[index + 2]}";
            index += 4;

            if(!int.TryParse(val, out res[i]))
            {
                Logger.Log.Error("Failed to convert the value: {val}");
                return null;
            }
        }
        return res;
    }

    //"int[]{0,0,0,0} => [000;000;000;000;] 
    public static string BuildMovementCommand(this int[] values)
    {

        if (values.Length != 4)
        {
            Logger.Log.Error("More or less values were sent to build the movement command!");
            return null;
        }
        string message = "[";

        for (int i = 0; i < 4; i++)
        {
            message += values[i].ToString("000");
            message += ";";
        }

        return message += "]";
    }
}
