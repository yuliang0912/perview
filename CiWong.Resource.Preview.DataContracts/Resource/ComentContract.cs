using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiWong.Resource.Preview.DataContracts.Resource
{
    public class CwComent
    {
        public int page_index { get; set; }
        public int page_size { get; set; }
        public int record_count { get; set; }
        public string msg { get; set; }
        public List<CwCommentDTO> record_list { get; set; }
    }

    public class CwCommentDTO
    {
        public string com_content { get; set; }

        public int com_status { get; set; }

        public long com_subject_id { get; set; }

        public int com_user_id { get; set; }

        public string com_user_name { get; set; }
        public string com_userphoto { get; set; }

        public long comment_date { get; set; }

        public long comment_id { get; set; }

        public int comment_type { get; set; }

        public string comment_type_name { get; set; }

        public IEnumerable<CwReplyDTO> replys { get; set; }

        public int son_category { get; set; }
    }

    public class CwReplyDTO
    {
        public long comment_id { get; set; }

        public long parend_id { get; set; }

        public string reply_content { get; set; }

        public long reply_date { get; set; }

        public long reply_id { get; set; }

        public int reply_user_id { get; set; }

        public string reply_user_name { get; set; }

        public string reply_userphoto { get; set; }
    }
}
