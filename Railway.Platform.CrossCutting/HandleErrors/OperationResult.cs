namespace Railway.Platform.CrossCutting.HandleErrors
{
    public class OperationResult<T>
    {
        public OperationResult() 
        {
            Errors = [];    
        }

        public OperationResult(T value)
        {
            Result = value;
            Errors = [];
        }

        public OperationResult(List<Error> errors)
        {
            Errors = errors;
        }

        public OperationResult(Error error)
        {
            Errors = [error];
        }

        public T? Result { get; private set; }

        public List<Error>? Errors { get; private set; }

        public bool HasErrors() => Errors != null && Errors.Count != 0;
        public OperationResult<T> AddErrors(List<Error> errors) => new(errors);
        public OperationResult<T> AddError(Error error) => new(error);
        public OperationResult<T> AddResult(T result) => new(result);
    }
}
