namespace HeartOfTheMachine
{

    public class InvalidInputException : System.Exception
    {

        public InvalidInputException()
        {
            
        }
        
        public InvalidInputException(string msg) : base(msg)
        {
            
        }
    }
    
}