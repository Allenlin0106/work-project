using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class AuditLogViewModel : ViewModelBase
    {
        private readonly IAuditService _auditService;
        private string _entityType;
        private string _entityId;
        private string _action;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private AuditLogDto _selected;
        private int _totalCount;

        public AuditLogViewModel(IAuditService auditService)
        {
            _auditService = auditService;
            Items = new ObservableCollection<AuditLogDto>();
            SearchCommand = new RelayCommand(_ => Search());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            Search();
        }

        public ObservableCollection<AuditLogDto> Items { get; }
        public string EntityType { get => _entityType; set => SetField(ref _entityType, value); }
        public string EntityId { get => _entityId; set => SetField(ref _entityId, value); }
        public string Action { get => _action; set => SetField(ref _action, value); }
        public DateTime? FromDate { get => _fromDate; set => SetField(ref _fromDate, value); }
        public DateTime? ToDate { get => _toDate; set => SetField(ref _toDate, value); }
        public AuditLogDto Selected { get => _selected; set => SetField(ref _selected, value); }
        public int TotalCount { get => _totalCount; set => SetField(ref _totalCount, value); }

        public ICommand SearchCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        private void Search()
        {
            var query = new AuditQueryDto
            {
                EntityType = EntityType,
                EntityId = EntityId,
                Action = Action,
                FromUtc = FromDate,
                ToUtc = ToDate,
                Page = 1,
                PageSize = 200
            };
            var page = _auditService.Query(query);
            Items.Clear();
            foreach (var item in page.Items) Items.Add(item);
            TotalCount = page.TotalCount;
        }

        private void ClearFilters()
        {
            EntityType = null;
            EntityId = null;
            Action = null;
            FromDate = null;
            ToDate = null;
            Search();
        }
    }
}
