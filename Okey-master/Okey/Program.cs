using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey
{
    class Program
    {
        static KeyValuePair<string, int> indicatorStone;
        static KeyValuePair<string, int> okeyStone;
        static List<KeyValuePair<string, int>> okeyStonesMain;
        static List<KeyValuePair<string, int>>[] playerList = new List<KeyValuePair<string, int>>[4];
        static List<KeyValuePair<T, U>> ReplaceIndicatorStone<T, U>(List<KeyValuePair<T, U>> list, KeyValuePair<T, U> item) where T : IEquatable<T>
        {
            var target_idx = list.FindIndex(n => n.Value.Equals(0));
            while (target_idx != -1)
            {
                list[target_idx] = item;
                target_idx = list.FindIndex(n => n.Key.Equals(0));
            }
            return list;
        }
        static string[] color = new string[] { "sarı", "mavi", "siyah", "kırmızı" };
        static void Main(string[] args)
        {
            Okey ok = new Okey();
            okeyStonesMain = ok.GetOkeyStoneList();
            ///////////////////Sahte okey belirlenip çıkarılması////////////////////
            Random random = new Random();
            int indicatorStoneIndex = random.Next(okeyStonesMain.Count);
            indicatorStone = okeyStonesMain[indicatorStoneIndex];
            while (indicatorStone.Key.Equals("sahte-okey") || indicatorStone.Value == 13)
            {
                indicatorStoneIndex = random.Next(okeyStonesMain.Count);
                indicatorStone = okeyStonesMain[indicatorStoneIndex];
            }
            string _key = indicatorStone.Key;
            int _value = indicatorStone.Value;
            okeyStone = new KeyValuePair<string, int>(_key, (_value + 1));
            okeyStonesMain = okeyStonesMain.Where((v, i) => i != indicatorStoneIndex).ToList(); // Gösterge listeden çıkartılır
            okeyStonesMain = ReplaceIndicatorStone(okeyStonesMain, new KeyValuePair<string, int>("sahte-okey", okeyStone.Value));
            Console.WriteLine("Sahte-Okey : " + indicatorStone);
            Console.WriteLine("Okey : " + okeyStone);
            playerList[0] = new List<KeyValuePair<string, int>>();
            playerList[1] = new List<KeyValuePair<string, int>>();
            playerList[2] = new List<KeyValuePair<string, int>>();
            playerList[3] = new List<KeyValuePair<string, int>>();
            /////////////////////////////////Dağıtım////////////////////////////////
            int firstPersonIndex = random.Next(4);
            for (int n = 0; n <= 14; n++)
            {
                playerList[firstPersonIndex].Add(okeyStonesMain[n]);
            }
            Queue<int> personList = new Queue<int>();
            int count = 0, temp = firstPersonIndex;
            while (count < 3)
            {
                temp++;
                personList.Enqueue(temp % 4);
                count++;
            }
            int j = 15;
            for (int i = 0; i < 3; i++)
            {
                temp = personList.Dequeue();
                for (int k = 1; k <= 14; k++)
                {
                    playerList[temp].Add(okeyStonesMain[j]);
                    j++;
                }
            }
            ///////////////////////////En iyi elin belirlenmesi/////////////////////
            int min = 15; // min işe yaramaz taş sayısı.
            int minIndex = firstPersonIndex; // min işe yaramaz taş sayısı olan index.
            int unHelpfulStoneCount = 0;
            int okCount = 0;
            KeyValuePair<string, int> tempStone;
            List<KeyValuePair<string, int>> okList = new List<KeyValuePair<string, int>>{ okeyStone };
            for (int l = 0; l < 4; l++)
            {
                Console.WriteLine("---------Kişi " + (l + 1) + ": ");
                playerList[l] = playerList[l].OrderBy(o => o.Value).ToList();
                unHelpfulStoneCount = 0;
                okCount = 0;
                for (int item = 0; item < playerList[l].Count; item++)
                {
                    tempStone = playerList[l][item];
                    bool isOkeyStone = (tempStone.Key == okeyStone.Key && tempStone.Value == tempStone.Value);
                    Console.WriteLine(tempStone.Key + " " + tempStone.Value);
                    if (tempStone.Key.Equals("sahte-okey"))
                    {
                        tempStone = indicatorStone;
                    }
                    if(!(okeyStone.Equals(tempStone)))
                    {
                        if (!(StoneOk(playerList[l].Except(okList).ToList(), tempStone))) // işe yaramaz bir taşsa.
                        {
                            unHelpfulStoneCount++;
                        }
                        unHelpfulStoneCount = unHelpfulStoneCount - okCount;
                        min = (unHelpfulStoneCount < min) ? unHelpfulStoneCount : min;
                        minIndex = (min == unHelpfulStoneCount) ? l : minIndex;
                    }
                    else
                    {
                        okCount++;
                    }

                }

            }
            Console.WriteLine("\nİlk Dağıtılan Kişi : " + (firstPersonIndex + 1) + ". Kişi");
            Console.WriteLine("Eli bitmeye en yakın kişi : " + (minIndex + 1) + ". Kişi");
            Console.WriteLine("\nEli:: ");
            foreach (var item in playerList[minIndex])
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }
            ////////////////////////////////////////////////////////////////////////
            Console.ReadKey();
        }


        public static bool StoneOk(List<KeyValuePair<string,int>> olist,KeyValuePair<string,int> val)
        {
            int _hasCount = 0;
            string _key = val.Key;
            int _val = val.Value;
            var result = olist.Find(x => x.Key == _key && x.Value == (val.Value+1));
            while (!(result.Equals(default(KeyValuePair<string, int>)))) // listede aynı rengin bir fazlası yok ise(null ise).
            {
                _hasCount++;
                if(_val==13)
                {
                    _val = 1;
                    result = olist.Find(x => x.Key == val.Key && x.Value == (_val));
                    if(!(result.Equals(default(KeyValuePair<string, int>))))
                    {
                        _hasCount++;
                        break;
                    }
                }
                else
                {
                    _val++;
                    result = olist.Find(x => x.Key == val.Key && x.Value == (_val));
                }
            }
            if (_hasCount < 3)
            {
                int _hasCount2 = 0;
                List<string> _key2 = color.Where((v, i) => v != val.Key).ToList();
                int _val2 = val.Value;
                var result2 = olist.Find(x => _key2.All(x.Key.Contains) && x.Value == (_val2));
                while (!(result.Equals(default(KeyValuePair<string, int>)))) // listede aynı rengin bir fazlası yok ise(null ise).
                {
                    _hasCount++;
                    _key2 = _key2.Where((v, i) => v != result.Key).ToList();
                    result2 = olist.Find(x => _key2.All(x.Key.Contains) && x.Value == (_val2));
                }
                if(_hasCount2<3)
                {
                    return false;
                }
                else
                {

                    return true;
                }
            }
            else
            {

                return true;
            }
        }
    }
}
