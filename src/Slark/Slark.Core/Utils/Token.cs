using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slark.Core
{
    public class SlarkToken
    {
        public SlarkToken(int expiredHours)
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            Token = Convert.ToBase64String(time.Concat(key).ToArray());
            ExpiredHours = expiredHours;
        }

        public string Token { get; set; }
        public bool IsExpired
        {
            get
            {
                byte[] data = Convert.FromBase64String(Token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                return when < DateTime.UtcNow.AddHours(-ExpiredHours);
            }
        }

        private int ExpiredHours
        {
            get; set;
        } = 24;

        public static SlarkToken NewToken(int expiredHours = 24)
        {
            return new SlarkToken(expiredHours);
        }
    }
}
