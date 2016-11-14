using System;
using System.Text;

public class PasswordGenerator
{
    private char[] characterArray;
    private int passwordLength = 6;
    private Random randNum;

    public PasswordGenerator()
    {
        characterArray = "abcdefghijklmnopqrstuvwxyz!@$#ABCDEFGHIJKLMNOPQRSTUVWXYZ#!@$0123456789".ToCharArray();
        randNum = new Random();
    }

    public string Generate()
    {
        StringBuilder sb = new StringBuilder();
        sb.Capacity = passwordLength;
        string appendForPasswordValidity = "A@1z";
        for (int count = 0; count <= passwordLength; count++)
        {
            sb.Append(GetRandomCharacter());
        }
        if ((sb != null))
        {
            sb.Append(appendForPasswordValidity);
            return sb.ToString();
        }
        return string.Empty;
    }

    //Gets Random Character
    private char GetRandomCharacter()
    {
        return this.characterArray[(int)((this.characterArray.GetUpperBound(0) + 1) * randNum.NextDouble())];
    }
}
