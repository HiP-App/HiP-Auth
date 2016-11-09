using System;
using System.Text;

public class PasswordGenerator
{
    private char[] characterArray;
    private Int32 passwordLength = 8;
    private Random randNum;

    public PasswordGenerator()
    {
        characterArray = "abcdefghijklmnopqrstuvwxyz!@$ABCDEFGHIJKLMNOPQRSTUVWXYZ!@$0123456789".ToCharArray();
        randNum = new Random();
    }

    public string Generate()
    {
        StringBuilder sb = new StringBuilder();
        sb.Capacity = passwordLength;
        for (int count = 0; count <= passwordLength - 1; count++)
        {
            sb.Append(GetRandomCharacter());
        }
        if ((sb != null))
        {
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
