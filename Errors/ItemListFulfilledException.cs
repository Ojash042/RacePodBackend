namespace RacePodBackend.Errors;

[Serializable]
public class ItemListFulfilledException: Exception{
    public ItemListFulfilledException() : base(){}
    public ItemListFulfilledException(string message) : base(message){}
    public ItemListFulfilledException(String message, Exception inner): base(message,inner){}
}