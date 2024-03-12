using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Associated.Application.Common.Model.Response
{
    public class ResponseModel<T>
    {
        public T Result { get; set; }
        public int StatusCode { get; set; } = 400;
        public Dictionary<string, List<string>> Errors{ get; set; } = new Dictionary<string, List<string>>();

        public ResponseModel()
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                Result = default!;
            }
        }
        public void AddError(string key, string error)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors[key] = new List<string>();
            }

            Errors[key].Add(error);
        }
    }
}
