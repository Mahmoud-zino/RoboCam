using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    //"[000][000][000][000] => int[]{0,0,0,0}
    public static int[] ExtractMotorValues(this string message)
    {
        if (message.Length < 19)
        {
            Debug.LogError("message contained less information as needed!");
            return null;
        }

        int index = 1;
        int[] res = new int[4];
        for (int i = 0; i < 4; i++)
        {
            string val = $"{message[index]}{message[index + 1]}{message[index + 2]}";
            index += 5;

            if(!int.TryParse(val, out res[i]))
            {
                Debug.LogError($"Failed to convert the value: {val}");
                return null;
            }
        }
        return res;
    }

    public static string BuildMovementCommand(this int[] values)
    {

        if (values.Length != 4)
        {
            Debug.LogError("More or less values were sent to build the movement command");
            return null;
        }
        string message = "[";

        for (int i = 0; i < 4; i++)
        {
            message += values[i].ToString("000");
            message += "][";
        }
        message = message.Remove(message.Length - 1);
        return message;
    }
}
