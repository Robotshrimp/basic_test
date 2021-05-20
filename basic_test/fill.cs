using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic_test
{
    class fill
    {
        public void f_fill(ref List<List<int>> list, int size_x, int size_y)
        {
            if (list.Capacity < size_y)
                list.Capacity = size_y;
            for (int i = list.Count; i < list.Capacity; i++)
            {
                list.Add(new List<int>());
            }
            for (int y = 0; y < size_y; y++)
            {
                if (list[y].Capacity < size_x)
                    list[y].Capacity = size_x;
                for (int i = list[y].Count; i < list[y].Capacity; i++)
                {
                    list[y].Add(0);
                }
            }
        }
    }
}
