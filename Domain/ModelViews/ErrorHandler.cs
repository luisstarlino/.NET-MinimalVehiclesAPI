namespace _NET_MinimalAPI.Domain.ModelViews
{
    public struct ErrorHandler
    {
        public ErrorHandler() {
            this.Messages = new List<string>();
        }
        public List<string> Messages { get; set;  }
        
    }
}
