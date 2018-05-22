using System;

namespace StockTicker.Soap.Models
{

    [Serializable]
    public class ResultModel<T>
    {
        private string _error;

        private T _t;

        public ResultModel()
        {

        }

        public bool Success { get; set; }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;

                Success = false;
            }
        }

        public T Result
        {
            get { return _t; }
            set
            {
                _t = value;

                Success = true;
            }
        }
         
        public static ResultModel<T> CreateInstance(T t)
        {
            return new ResultModel<T>
            {
                Result = t
            };
        }

        public static ResultModel<T> ThrowIfError(string error)
        {
            return new ResultModel<T>
            {
                Error = error
            };
        }
    }
}