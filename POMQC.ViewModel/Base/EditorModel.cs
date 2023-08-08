using System;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Base
{
    public class EditorModel
    {
        public EditorModel()
        {
            From = new DatePickerViewModel { Date = DateTime.Now.AddMonths(-1), Function = Function };
            To = new DatePickerViewModel { Date = DateTime.Now.AddMonths(1), Function = Function };
        }

        public DatePickerViewModel From { get; set; }

        public DatePickerViewModel To { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}