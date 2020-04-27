using System;
using UnityEngine;

namespace Orbox.Utils
{
    public struct TwoChars
    {
        public readonly char First;
        public readonly char Second;

        public TwoChars(char first, char second)
        {
            First = first;
            Second = second;
        }
    }

    public struct ThreeChars
    {
        public readonly char First;
        public readonly char Second;
        public readonly char Third;

        public ThreeChars(char first, char second, char third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }


    public static class NumberText
    {
        const int TwoDigitMax = 99;
        const int ThreeDigitMax = 999;

        public static TwoChars TwoDigitIntToChar(int number)
        {
            if (number > TwoDigitMax)
            {
                return new TwoChars('-','-');
            }             


            int firstDigit = 0;
            int secondDigit = 0;

            if (number >= 10)
            {
                firstDigit = number / 10;
                secondDigit = number % 10;
            }
            else
            {
                secondDigit = number;
            }
            
            char first = IntToChar(firstDigit);
            char second = IntToChar(secondDigit);
            
            var result = new TwoChars(first, second);
            return result;
        }

        public static ThreeChars ThreeDigitIntToChar(int number)
        {
            if (number > TwoDigitMax)
            {
                return new ThreeChars('-', '-', '-');
            }             

            int firstDigit = 0;
            int secondDigit = 0;
            int thirdDigit = 0;

            if (number >= 100)
            {
                firstDigit = number / 100;
                secondDigit = number % 100;
                thirdDigit = number % 10;
            }

            if (number >= 10 && number < 100)
            {
                secondDigit = number / 10;
                thirdDigit = number % 10;
            }

            if( number >= 0 && number < 10)
            {
                thirdDigit = number;
            }

            char first = IntToChar(firstDigit);
            char second = IntToChar(secondDigit);
            char third = IntToChar(thirdDigit);

            var result = new ThreeChars(first, second, third);
            return result;
        }

        public static char IntToChar(int digit)
        {
            char result = '0';

            switch (digit)
            {
                case 1: result = '1'; break;
                case 2: result = '2'; break;
                case 3: result = '3'; break;
                case 4: result = '4'; break;
                case 5: result = '5'; break;
                case 6: result = '6'; break;
                case 7: result = '7'; break;
                case 8: result = '8'; break;
                case 9: result = '9'; break;
            }

            return result;
        }

        //orbox: TODO: óáğà ïîòğåáëåíèå ïàìÿòè â êó÷å
        public static string FormatTime(float time)
        {
            if (time < 0)
                return "--:--:--";

            int minutes = (int)(time / 60);
            int seconds = (int)(time) % 60;
            int mseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100f);

            return string.Format("{0,2:00}:{1,2:00}:{2,2:00}", minutes, seconds, mseconds);
        }

        public static string GetEnding(int dight)
        {
            if (dight == 1) return "st";
            if (dight == 2) return "nd";
            if (dight == 3) return "rd";
            if (dight > 3) return "th";

            return "0";
        }

    }
}