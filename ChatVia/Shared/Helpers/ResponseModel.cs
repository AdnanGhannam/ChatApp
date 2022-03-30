using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatVia.Shared.Helpers
{
    public class ResponseModel<TData, TErrors>
    {
        public TData? Data { get; set; }
        public List<TErrors> Errors { get; set; } = new();
        public bool Succeeded { get; set; }

        [JsonConstructor]
        public ResponseModel() { }

        public ResponseModel(TData? data, TErrors? error)
        {
            Data = data;

            if(error != null)
                Errors.Add(error);

            Succeeded = !Equals(data, default(TData));
        }

        public ResponseModel(TData? data, List<TErrors> errors)
        {
            Data = data;
            Errors = errors;
            Succeeded = !Equals(data, default(TData));
        }
    }

    public class ResponseModel<TData> : ResponseModel<TData, ErrorModel>
    {
        [JsonConstructor]
        public ResponseModel(): base() { }

        public ResponseModel(TData? data, ErrorModel? error)
            : base(data, error) { }

        public ResponseModel(TData? data, List<ErrorModel> errors)
            : base(data, errors) { }
    }
}
