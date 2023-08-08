using System;

namespace POMQC.ViewModels.Base
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public int AgentId { get; set; }

        public long CustId { get; set; }

        public int FactoryId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string CreatedUser { get; set; }

        public string UpdatedUser { get; set; }

        public string CreateDate { get; set; }

        public string UpdateDate { get; set; }

        public string CustPO { get; set; }

        public string AIGLPO { get; set; }       
    }
}