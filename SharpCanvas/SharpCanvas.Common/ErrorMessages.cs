namespace SharpCanvas.Common
{
    public static class ErrorMessages
    {
        public const string CANVAS_STACK_UNDERFLOW =
            "restore() caused a a buffer underflow: There are no more saved states available to restore.";
        public const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";
        public const string NOT_SUPPORTED_ERR = "Some of the paramters are invalid";
        public const string TYPE_MISTMATCH_ERR = "Type mistmatch error";
    }
}
