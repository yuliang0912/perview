using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// webApi契约实体
    /// </summary>
    [DataContract(Name = "user")]
    public class UserContract
    {
        public UserContract(long id = 0, string name = "")
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
