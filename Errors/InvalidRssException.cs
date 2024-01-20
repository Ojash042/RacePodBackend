namespace RacePodBackend.Errors; 

[Serializable]
public class InvalidRssException : Exception {
	public InvalidRssException() : base(){}
	public InvalidRssException(string message): base(message){}
	public InvalidRssException(string message, Exception inner): base(message, inner){}
}