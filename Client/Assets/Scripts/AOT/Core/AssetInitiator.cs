using System.Collections;

namespace GameFramework.Core 
{
    public class AssetInitiator : IEnumerator
    {
        private IEnumerator enumerator;

        public object Current => enumerator;

        public AssetInitiator(IEnumerator enumerator) 
        {
            this.enumerator = enumerator;
        }

        public bool MoveNext()
        {
            if (enumerator == null) 
            {
                return true;          
            }
            else
            {
                return enumerator.MoveNext();                
            }
        }

        public void Reset()
        {
            enumerator = null;
        }
    }
}