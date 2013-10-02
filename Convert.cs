using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Number
{
    public class Convert
    {
        public string ToWords(string number)
        {
            var numToWordsDict = new Dictionary<decimal, string>
            {
                {0, ""},
                {1, " один"},
                {2, " два"},
                {3," три"},
                {4," четыре"},
                {5," пять"},
                {6," шесть"},
                {7," семь"},
                {8," восемь"},
                {9," девять"},
                {10," десять"},
                {11," одиннадцать"},
                {12," двенадцать"},
                {13," тринадцать"},
                {14," четырнадцать"},
                {15," пятнадцать"},
                {16," шестнадцать"},
                {17," семнадцать"},
                {18," восемнадцать"},
                {19," девятнадцать"},
                {20," двадцать"},
                {30," тридцать"},
                {40," сорок"},
                {50," пятьдесят"},
                {60," шестьдесят"},
                {70," семьдесят"},
                {80," восемьдесят"},
                {90," девяносто"},
                {100, " сто"},
                {200, " двести"},
                {300, " триста"},
                {400," четыреста"},
                {500," пятьсот"},
                {600," шестьсот"},
                {700," семьсот"},
                {800," восемьсот"},
                {900," девятьсот"}
            };
            
            string[] sum = number.Split(',');
            int sumCount = sum[0].Count();
            int firstPart = Convert.ToInt32(sum[0]);

            List<string> firstPartInWords = Regex.Matches(sum[0], "(.{0,3})", RegexOptions.RightToLeft)
                                        .Cast<Match>()
                                        .Select(m => m.Groups[0].Value)
                                        .ToList();
            string inWords = "";
            int innerCounter = 0;
            foreach (string innerPart in firstPartInWords)
            {
                if (!String.IsNullOrEmpty(innerPart))
                {
                    if (innerCounter == 1)
                        inWords = inWords.Insert(0, NumDeclension((int)Char.GetNumericValue(innerPart[innerPart.Count()-1])));
                    if (innerCounter == 2)// > 2
                    {
                        if(innerPart.Count() == 1)
                            inWords = inWords.Insert(0, MillionAndAboveDecl("0"+innerPart));
                        else if(innerPart.Count() == 2)
                            inWords = inWords.Insert(0, MillionAndAboveDecl(innerPart));
                        else
                            inWords = inWords.Insert(0, MillionAndAboveDecl((innerPart[1] + innerPart[2]).ToString()));
                    }
                    if (innerCounter > 2)
                        inWords = "Больше миллиарда? Серьезно?";

                    switch (innerPart.Count())
                    {
                        case 1:
                            inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == Convert.ToInt32(innerPart)).Value);
                            break;
                        case 2:
                            if (firstPart < 20)
                                inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == firstPart).Value);
                            else
                            {
                                inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == (int)Char.GetNumericValue(innerPart[1])).Value);
                                inWords = inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == ((int)Char.GetNumericValue(innerPart[0])) * 10).Value);
                            }
                            break;
                        case 3:
                            if ((firstPart - (int)Char.GetNumericValue(innerPart[0]) * 100) < 20)
                                inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == firstPart).Value);
                            else
                            {
                                inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == (int)Char.GetNumericValue(innerPart[2])).Value);
                                inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == ((int)Char.GetNumericValue(innerPart[1])) * 10).Value);
                            }
                            inWords = inWords.Insert(0, numToWordsDict.Single(w => w.Key == ((int)Char.GetNumericValue(innerPart[0]) * 100)).Value);
                            break;
                    }
                    innerCounter++;
                }
            }
            inWords += LVDeclension((int)Char.GetNumericValue(sum[0][(sum[0].Count()-1)]));

            var otherPart = inWords.Remove(0,2);
            inWords = inWords[1].ToString().ToUpper() + otherPart + " "+ sum[1] + " копеек.";
            return inWords;
        }

        private string MillionAndAboveDecl(string num)
        {
            string val = " миллион";//можно добавить миллиарды и тпх
            int numLast = (int)Char.GetNumericValue(num[1]);
            if (numLast > 1 && numLast < 5 && Convert.ToInt32(num) < 10)
                val += "а";
            else if (numLast > 5 && Convert.ToInt32(num) < 21 || (Convert.ToInt32(num) % 10) == 0)
                val += "ов";
            return val;
        }

        private string NumDeclension(int num)
        {
            string decl = null;
            if (num == 1)
                decl = " тысяча";
            else if (num > 1 && num < 5)
                decl = " тысячи";
            else
                decl = " тысяч";
            return decl;
        }

        private string LVDeclension(int num)
        {
            string decl = null;
            if (num != 0 && num < 2)
                decl = " рубль";
            else if (num != 0 && num < 5)
                decl = " рубля";
            else
                decl = " рублей";
            return decl;
        }
    }
}
