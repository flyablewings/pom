using System.Collections.Generic;

namespace POMQC.Entities.Base
{
    public class DefectCodeMultiColumns
    {
        public DefectCodeMultiColumns()
        {
            data = new List<DefectData>();
        }

        public IList<DefectData> data { get; set; }

        public IList<DefectField> fields 
        { 
            get 
            {
                return new List<DefectField>
                {
                    new DefectField { name = "value", text = "Id" },
                    new DefectField { name = "category", text = "Defect Category" },
                    new DefectField { name = "text", text = "Defect Code" },
                    new DefectField { name = "description", text = "Defect Description" }
                };
            } 
        }

        public bool autoOpen { get { return true; } }

        public bool headShow { get { return true; } }

        public string fieldText { get { return "description"; } }

        public string fieldValue { get { return "value"; } }
    }

    public class DefectData
    {
        public string value { get; set; }

        public string category { get; set; }

        public string text { get; set; }

        public string description { get; set; }
    }

    public class DefectField
    {
        public string name { get; set; }

        public string text { get; set; }
    }
}