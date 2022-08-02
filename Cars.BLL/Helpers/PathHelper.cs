namespace Cars.BLL.Helpers
{
    public static class PathHelper
    {
        public static string GetPath(string url)
        {
            var index = GetIndex(url, ':', 2);
            var result = url.Substring(index - 5);
            return result;
        }

        private static int GetIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
