using System;
using POMQC.Data.Filter;
using POMQC.ViewModels.Filter;
using System.Collections.Generic;
using POMQC.ViewModels.Base;

namespace POMQC.Services.Filter
{
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _repository = new FilterRepository();

        #region IFilterService Members

        public FilterViewModel Filters(bool showAllLabel, bool showNewButton, bool showViewButton, FunctionViewModel function, bool showDateRange = false, bool showStatus = true, bool showFactory = true)
        {
            var model = new FilterViewModel()
            {
                ShowAllLabel = showAllLabel,
                ShowNewButton = showNewButton,
                ShowViewButton = showViewButton,
                Function = function,
                ShowDateRange = showDateRange,
                ShowStatus = showStatus,
                ShowFactory = showFactory,
                DateRange = new EditorModel
                {
                    Function = function,
                    From = new DatePickerViewModel
                    {
                        Date = DateTime.Now.AddMonths(-1),
                        Function = function
                    },
                    To = new DatePickerViewModel
                    {
                        Date = DateTime.Now.AddMonths(1),
                        Function = function
                    }
                }
            };

            var customers = _repository.Customers();
            var custPOs = _repository.CustPOs();
            var aiglPOs = _repository.AIGLPOs();
            
            foreach (var item in customers)
            {
                model.Customers.Add(new CustomerViewModel { CustId = item.CustId, CustName = item.CustName, Parent = item.Parent });
            }

            foreach (var item in custPOs)
            {
                model.CustPOs.Add(new POViewModel { Parent = item.Parent, PO = item.PO });
            }

            foreach (var item in aiglPOs)
            {
                model.AIGLPOs.Add(new POViewModel { Parent = item.Parent, PO = item.PO });
            }

            model.Factories = Factories();

            return model;
        }

        public IList<FactoryBase> Factories()
        {
            var model = new List<FactoryBase>();
            var result = _repository.Factories();

            foreach (var factory in result)
            {
                model.Add(new FactoryBase
                {
                    FactoryId = factory.FactoryId,
                    FactoryName = factory.FactoryName
                });
            }

            return model;
        }

        #endregion
    }
}