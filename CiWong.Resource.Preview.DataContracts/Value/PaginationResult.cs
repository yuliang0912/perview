using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 分页结果集
    /// </summary>
    [Serializable, DataContract(Name = "pagination_result")]
    public class PaginationResult<T>
    {
        public PaginationResult()
        {
            this.Result = Enumerable.Empty<T>();
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember(Name = "page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// 页索引，从0开始
        /// </summary>
        [DataMember(Name = "page_index")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 结果集
        /// </summary>
        [DataMember(Name = "result")]
        public IEnumerable<T> Result { get; set; }
    }
}
