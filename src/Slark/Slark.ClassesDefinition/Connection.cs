using System;
namespace Slark.ClassesDefinition
{
    public class SlarkConnection<T> where T : IAttachable
    {
        public SlarkConnection()
        {
            
        }

        public T TheOtherSide { get; set; }
    }
}
