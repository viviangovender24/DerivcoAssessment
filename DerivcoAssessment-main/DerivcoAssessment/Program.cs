using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DerivcoAssessment
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /*
             Create a CSV File of names and genders
             Group by gender
             No dublicates allowed in a set (Used hashset)
             Take two names from the sets and format it to the input string
             Run the program and print results in a textfile
             */

            var time = new System.Diagnostics.Stopwatch(); //time

            //Group of players
            HashSet<String> femalePlayers = new HashSet<string>();
            HashSet<String> malePlayers = new HashSet<string>();
            Dictionary<string, int> outputDic = new Dictionary<string, int>();
            time.Start();
            
            //Read CSV
            string path = "playerNames.txt";
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                var values = line.Split(',');

                if (values[1] == "f") //Check Gender
                {
                    if (values[0].All(c => char.IsLetter(c))) //Each letter in each name is an alphabet
                    {
                        femalePlayers.Add(values[0]);
                    }
                }
                else if (values[1] == "m")
                {
                    if (values[0].All(c => char.IsLetter(c)))
                    {
                        malePlayers.Add(values[0]);
                    }
                }
            }

            foreach (var fp in femalePlayers)
            {
                foreach (var mc in malePlayers)
                {
                    string input = $"{fp} matches {mc}";

                    //Remove spaces and trim

                    string Name = input.Replace(" ", "");

                    //Assume the program is not case sensitive
                    Name = Name.Trim().ToLower();

                    //Convert string to character array
                    var chars = Name.ToCharArray();


                    Dictionary<char, int> charMap = new Dictionary<char, int>();

                    /*
                     Character inserted as keys in K/V pair
                     Count is inserted as Values
                     If the dictionary contains the character then increase its value by 1
                     */

                    //Build charMap
                    foreach (var ch in chars)
                    {
                        if (charMap.ContainsKey(ch))
                        {
                            charMap[ch] = charMap[ch] + 1;
                        }
                        else
                        {
                            charMap.Add(ch, 1);
                        }
                    }


                    //Loop through dictionary for only keys
                    var keys = new HashSet<char>(charMap.Keys); //Stores unique keys

                    string number = "";

                    foreach (var ch in keys)
                    {
                        number += charMap[ch].ToString();
                    }

                  
                    
                    
                    //Console.WriteLine(number);

                    //Calculate percentage
                    /*
                    Input a number e.g. 22111, 222222
                     If number is even length -> Split in half:
                     reverse one number then add together
                    If number is odd length ->store last digit of larger dividend into temp, then add it to the sum at end
                     */

                    do
                    {
                        string leftNum = number.Substring(0, (number.Length / 2));
                        string rightNum = number.Substring((number.Length / 2), number.Length - (number.Length / 2));

                        var leftNumArr = leftNum.ToCharArray();
                        var rightNumArr = rightNum.ToCharArray();

                        Array.Reverse(rightNumArr);

                        int[] LArr = Array.ConvertAll(leftNumArr, c => (int)Char.GetNumericValue(c));
                        int[] RArr = Array.ConvertAll(rightNumArr, c => (int)Char.GetNumericValue(c));

                        List<int> sumArr = new List<int>();

                        for (int i = 0; i < LArr.Length; i++)
                        {
                            int tempInt = (LArr[i]) + (RArr[i]);
                            sumArr.Add(tempInt);
                        }

                        //Add the last digit from the larger array
                        if (LArr.Length > RArr.Length)
                        {
                            sumArr.Add(LArr[LArr.Length - 1]);
                        }
                        else if (RArr.Length > LArr.Length)
                        {
                            sumArr.Add(RArr[RArr.Length - 1]);
                        }

                        number = "";

                        foreach (var num in sumArr)
                        {
                            string temp = num.ToString();
                            number += temp;
                        }
                    } while (number.Length > 2);

                    if (Convert.ToInt32(number) > 80)
                    {
                        outputDic.Add($"{input} {number}%, good match", Convert.ToInt32(number));
                    }
                    else
                    {
                        outputDic.Add($"{input} {number}%", Convert.ToInt32(number));
                    }
                }
            }

            
            
            //Sorts dictionary according to % match, then alphabetically due to matching %
            outputDic = outputDic.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (var value in outputDic)
            {
                Console.WriteLine(value.Key);
            }

            File.WriteAllLines("output.txt", outputDic.Keys);
            time.Stop();
            Console.WriteLine($"Time Taken To Run Program {time.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }
    }
}