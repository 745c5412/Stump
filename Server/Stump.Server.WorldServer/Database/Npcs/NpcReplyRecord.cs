using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Npcs.Replies;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Conditions;

namespace Stump.Server.WorldServer.Database.Npcs
{
    public class NpcReplyRecordRelator
    {
        public static string FetchQuery = "SELECT * FROM npcs_replies";
    }

    [TableName("npcs_replies")]
    public class NpcReplyRecord : ParameterizableRecord, IAutoGeneratedRecord
    {
        private string m_criteria;
        private ConditionExpression m_criteriaExpression;
        private NpcMessage m_message;

        public int Id
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public int ReplyId
        {
            get;
            set;
        }

        public int MessageId
        {
            get;
            set;
        }

        [NullString]
        public string Criteria
        {
            get { return m_criteria; }
            set
            {
                m_criteria = value;
                m_criteriaExpression = null;
            }
        }

        [Ignore]
        public ConditionExpression CriteriaExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Criteria) || Criteria == "null")
                    return null;

                return m_criteriaExpression ?? (m_criteriaExpression = ConditionExpression.Parse(Criteria));
            }
            set
            {
                m_criteriaExpression = value;
                Criteria = value.ToString();
            }
        }

        public NpcMessage Message
        {
            get { return m_message ?? (m_message = NpcManager.Instance.GetNpcMessage(MessageId)); }
            set
            {
                m_message = value;
                MessageId = value.Id;
            }
        }

        public NpcReply GenerateReply()
        {
            return DiscriminatorManager<NpcReply>.Instance.Generate(Type, this);
        }
    }
}