using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey
{
    class Okey
    {
        List<KeyValuePair<string, int>> okeyStones;
        string[] color = new string[]{ "sarı","mavi","siyah","kırmızı","sahte-okey" };
        private Random rng = new Random();

        public Okey()
        {
            okeyStones = new List<KeyValuePair<string, int>>();
            okeyStones = CreateOkeyStones(CreateOkeyStones(okeyStones));
            Shuffle();
        }

        public List<KeyValuePair<string, int>> GetOkeyStoneList()
        {
            return okeyStones;
        }

        public List<KeyValuePair<string, int>> CreateOkeyStones(List<KeyValuePair<string, int>> list)
        {
            for (int i = 0; i < color.Length-1; i++)
            {
                for (int j = 1; j<=13; j++)
                {
                    list.Add(new KeyValuePair<string,int>(color[i], j ));
                }
            }
            list.Add(new KeyValuePair<string, int>(color[color.Length - 1], 0));
            return list;
        }

        public void Shuffle()
        {
            int n = okeyStones.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                KeyValuePair<string, int> value = okeyStones[k];
                okeyStones[k] = okeyStones[n];
                okeyStones[n] = value;
            }
        }
    }
}
