using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;
using System.Linq.Dynamic;

namespace CRM.Code.Utils.Ordering
{
    public interface ListOrderItem
    {
        int OrderNo { get; set; }
        int ID { get; set; }
    }
    
    public class Ordering
    {
        public Ordering() { }

        public static int GetNextOrderID(IEnumerable data)
        {
            var data2 = data.Cast<ListOrderItem>();

            int order = 1;

            if (data2.Any())
            {
                order = data2.OrderByDescending(l => l.OrderNo).First().OrderNo + 1;
            }

            return order;
        }

        public static void ChangeOrder(IEnumerable data, ListOrderItem target, bool increase)
        {
            var data2 = data.Cast<ListOrderItem>();

            if (increase)
            {
                var upper = data2.Where(p => p.OrderNo > target.OrderNo).OrderBy(p => p.OrderNo);

                if (upper.Any())
                {
                    var upperItem = upper.First();

                    int tempOrder = target.OrderNo;

                    target.OrderNo = upperItem.OrderNo;
                    upperItem.OrderNo = tempOrder;
                }
            }
            else
            {
                var lower = data2.Where(p => p.OrderNo < target.OrderNo).OrderByDescending(p => p.OrderNo);

                if (lower.Any())
                {
                    var lowerItem = lower.First();

                    int tempOrder = target.OrderNo;

                    target.OrderNo = lowerItem.OrderNo;
                    lowerItem.OrderNo = tempOrder;
                }
            }
        }

        public static void MoveToTop(IEnumerable data, ListOrderItem target)
        {
            var data2 = data.Cast<ListOrderItem>();
            int orderNo = 2;
            foreach (ListOrderItem item in data2.Where(p => p.OrderNo <= target.OrderNo).OrderBy(a => a.OrderNo).ToArray())
            {
                item.OrderNo = orderNo++;
            }
            target.OrderNo = 1;
        }

        public static void MoveToBottom(IEnumerable data, ListOrderItem target)
        {
            var data2 = data.Cast<ListOrderItem>();
            int orderNo = target.OrderNo;
            foreach (ListOrderItem item in data2.Where(p => p.OrderNo >= target.OrderNo).OrderBy(a => a.OrderNo).ToArray())
            {
                item.OrderNo = orderNo++;
            }
            target.OrderNo = orderNo;
        }
    }
}