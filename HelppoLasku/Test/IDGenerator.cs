using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.Test
{
    public class IDGenerator
    {
        public IDGenerator(int length)
        {
            idLength = length;
            usedIds = new List<string>();
            random = new Random();
        }

        Random random;

        string baseChars = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        int idLength;

        List<string> usedIds;

        public string Next()
        {
            string id = "";
            for (int i = 0; i < idLength; i++)
            {
                id += baseChars[random.Next(baseChars.Length)];
            }

            if (usedIds.Contains(id))
            {
                return Next();
            }

            return id;
        }

        public void AddID(string id)
        {
            if (!isValid(id))
                throw new ArgumentException("id");

            usedIds.Add(id);
        }

        bool isValid(string id)
        {
            if (id.Length == idLength && !usedIds.Contains(id))
            {
                foreach (char c in id)
                {
                    if (!baseChars.Contains(c))
                        return false;
                }
            }
            return true;
        }
    }
}
