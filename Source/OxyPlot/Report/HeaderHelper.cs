namespace OxyReporter
{
    public class HeaderHelper
    {
        public HeaderHelper()
        {
            
        }

        int[] headerLevel = new int[10];

        public string GetHeader(int level)
        {
            for (int i = level - 1; i > 0; i--)
                if (headerLevel[i] == 0)
                    headerLevel[i] = 1;

                headerLevel[level]++;
            for (int i = level + 1; i < 10; i++)
                headerLevel[i] = 0;
            string levelString = "";
            for (int i = 1; i <= level; i++)
            {
                if (i > 1)
                    levelString += ".";
                levelString += headerLevel[i];
            }
            return levelString;
        }
    }

 }
