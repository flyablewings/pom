using System;

namespace POMQC.Entities.Base
{
    public class EntityBase
    {
        public long InspectionId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public int AgentId { get; set; }

        public long CustId { get; set; }

        public int FactoryId { get; set; }

        public string CustPO { get; set; }

        public string AIGLPO { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CreatedUser { get; set; }

        public string UpdatedUser { get; set; }

        public string UpdateDate { get; set; }

        public string AgentName { get; set; }

        public string FactoryName { get; set; }

        public string CustName { get; set; }

        public string Style { get; set; }

        public string Brand { get; set; }

        public int OrderQty { get; set; }

        public int InspectedQty { get; set; }
    }
}