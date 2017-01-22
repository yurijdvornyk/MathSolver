namespace ProblemSdk.Error
{
    public class DataItemTypeMismatchException : ProblemException
    {
        public DataItemTypeMismatchException(string name, string validType, string invalidType, string message) :
            base(string.Format(Messages.DATA_ITEM_TYPE_MISMATCH, name, validType, invalidType), message)
        { }
    }
}
