namespace Openfox.Foxnet.Service
{
    public class FoxnetServiceManager
    {
        private static readonly Lazy<FoxnetServiceManager> _instance = new Lazy<FoxnetServiceManager>(() => new FoxnetServiceManager());

        public static FoxnetServiceManager Instance 
        {
            get => _instance.Value;
        }

        private FoxnetServiceManager()
        {
            
        }
    }
}