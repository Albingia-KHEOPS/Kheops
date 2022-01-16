using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools {
    public class Paging {
        public Paging() {
            Size = ConfigurationManager.AppSettings.Get("PaginationSize").ParseInt(20).Value;
        }
        public Paging(int size) {
            Size = size;
        }
        public int Size { get; set; }
        public int CurrentPage { get; set; }
        public int CurrentOffset => (CurrentPage - 1) * Size;
        public int GetStart(int count, int number = 1) {
            CurrentPage = number < 1 ? 1 : number;
            int result = 1;
            if (count >= 2 && count > Size) {
                var realPageCount = (decimal)count / Size;
                if (CurrentPage > realPageCount) {
                    CurrentPage = count % Size == 0 ? (int)realPageCount : ((int)realPageCount + 1);
                }
                result = ((CurrentPage - 1) * Size) + 1;
            }
            return result;
        }
        public int GetEnd(int count, int number = 1) {
            int result = GetStart(count, number) + Size - 1;
            if (result > count) {
                result = count;
            }
            return result;
        }
        public (int minLine, int maxLine) Init(int count, int number = 1) {
            int min = GetStart(count, number);
            int max = min + Size - 1;
            if (max > count) { max = count; }
            return (min, max);
        }
        public int GetMaxPage(int count) {
            if (Size == 1) {
                return count;
            }
            return (Size / count) + (Size % count == 0 ? 0 : 1);
        }
    }
}
