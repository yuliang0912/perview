using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 作业内容资源
    /// </summary>
    [Serializable, DataContract(Name = "work_resource")]
    public class WorkResourceContract
    {
        public WorkResourceContract()
        {
            this.resourceParts = Enumerable.Empty<ResourcePartsContract>();
        }

        public WorkResourceContract(long recordId, long packageId, string taskId, int moduleId, long versionId, string resourceName, string resourceType, string requirementContent, bool IsFull, IEnumerable<ResourcePartsContract> resourceParts)
        {
            this.RecordId = recordId;
            this.PackageId = packageId;
            this.TaskId = taskId;
            this.ModuleId = moduleId;
            this.VersionId = versionId;
            this.ResourceName = resourceName;
            this.ResourceType = resourceType;
            this.RequirementContent = requirementContent;
            this.IsFull = IsFull;
            this.resourceParts = resourceParts;
        }

        /// <summary>
        /// 单元布置内容ID
        /// </summary>
        [DataMember(Name = "content_id")]
        public long ContentId { get; set; }

        /// <summary>
        /// 作业记录ID
        /// </summary>
        [DataMember(Name = "record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// 资源包ID
        /// </summary>
        [DataMember(Name = "package_id")]
        public long PackageId { get; set; }

        /// <summary>
        /// 资源目录层级ID
        /// </summary>
        [DataMember(Name = "task_id")]
        public string TaskId { get; set; }

        /// <summary>
        /// 目录模块ID
        /// </summary>
        [DataMember(Name = "module_id")]
        public int ModuleId { get; set; }

        /// <summary>
        /// 资源版本ID
        /// </summary>
        [DataMember(Name = "version_id")]
        public long VersionId { get; set; }

		/// <summary>
		/// 资源关联路径格式为:根版本ID/父版本ID/当前版本ID
		/// </summary>
		[DataMember(Name = "relation_path")]
		public string RelationPath { get; set; }

        /// <summary>
        /// 子模块ID(适用于子集没有版本ID的资源,例如跟读中的单词组)
        /// </summary>
        [DataMember(Name = "son_module_id")]
        public string SonModuleId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [DataMember(Name = "resource_name")]
        public string ResourceName { get; set; }

        /// <summary>
        /// 基础资源类型ID
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }

        /// <summary>
        /// 作业要求
        /// </summary>
        [DataMember(Name = "requirement_content")]
        public string RequirementContent { get; set; }

        /// <summary>
        /// 支持选择子资源
        /// </summary>
        [DataMember(Name = "is_full")]
        public bool IsFull { get; set; }

        /// <summary>
        /// 子资源集合
        /// </summary>
        [DataMember(Name = "resourceparts")]
        public IEnumerable<ResourcePartsContract> resourceParts { get; set; }
    }


    /// <summary>
    /// 资源子集(筛选)
    /// </summary>
    [Serializable, DataContract(Name = "resource_parts")]
    public class ResourcePartsContract
    {

        public ResourcePartsContract()
        {

        }

        public ResourcePartsContract(long ContentId, long versionId, string resourceType)
        {
            this.ContentId = ContentId;
            this.VersionId = versionId;
            this.ResourceType = resourceType;
        }

        /// <summary>
        /// 父内容ID
        /// </summary>
        [DataMember(Name = "content_id")]
        public long ContentId { get; set; }

        /// <summary>
        /// 子资源版本ID
        /// </summary>
        [DataMember(Name = "version_id")]
        public long VersionId { get; set; }

        /// <summary>
        /// 子资源类型ID
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }

    }
}
