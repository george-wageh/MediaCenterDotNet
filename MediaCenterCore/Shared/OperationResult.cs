
namespace MediaCenterCore.Shared
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; } = false;
        public T? Object { get; set; }
        public string Message { get; set; }
    }

}
